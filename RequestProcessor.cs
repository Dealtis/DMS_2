using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace DMSvStandard
{
	/// <summary>
	/// This class is repsonsible for processing all users requests.
	/// Each request is processed in seperate thread.
	/// </summary>
    class RequestProcessor
    {
        private DealtisMessage Msg;        
        public bool done = false;        

        public RequestProcessor(DealtisMessage _msg)
        {
            Msg = _msg;
            done = false;
        }

        public void Run()
        {
            if (Msg.getActionType() == DealtisMessage.ACTION_SENDMSG)
            {
				System.Diagnostics.Debug.WriteLine("Server SEND MESSAGE" );
                if (!sendTransicsMessage())
				{
					System.Diagnostics.Debug.WriteLine("Server SEND MESSAGE" );
                    sendTransicsMessage(); // try again
				}
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_OUTBOX)
            {
                if (!getTransicsOutboxContent())
                    getTransicsOutboxContent(); // try again
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_INBOX)
            {
                if (!getTransicsInboxContent())
                    getTransicsInboxContent(); // try again
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_SETREAD)
            {
                if (!setTransicsMessageRead())
                    setTransicsMessageRead(); // try again
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_SENDPOSSITION)
            {
                if (!sendGPSPossition())
                    sendGPSPossition(); // try again
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_LOADACTIVITIES)
            {
                if (!loadActivities())
                    loadActivities(); // try again
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_STARTACTIVITY)
            {
                if (!startActivity(true))
                    startActivity(true); // try again
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_STOPACTIVITY)
            {
                if (!stopActivity(true))
                    stopActivity(true); // try again
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_LOADTRIP)
            {
                if (!loadTrip())
                    loadTrip(); // try again
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_STARTTRIP)
            {
                if (!startTrip())
                    startTrip(); // try again
                done = true;
            }
            else if (Msg.getActionType() == DealtisMessage.ACTION_STOPTRIP)
            {
                if (!stopTrip())
                    stopTrip(); // try again
                done = true;
            }
        }

		/// <summary>
		///  Send message to the Transics.
		/// </summary>
        private bool sendTransicsMessage()
        {
            try
            {
                TransicsService.Service webService = ServerData.Instance.getTransicsService();

                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;
                

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];


                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;


                if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
                    session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);

                    if (Msg.getParameters().ContainsKey("TEXT_MSG"))
                    {
                        String textMsg = Msg.getParameters()["TEXT_MSG"];

                        TransicsService.TextMessage txMsg = new TransicsService.TextMessage();
                        txMsg.Message = textMsg;

                        
						TransicsService.ResultInfo results = webService.Send_TextMessage(session, txMsg);

                        if ((results.Errors != null) && (results.Errors.Length > 0))
                        {
                            if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                            {
                                ServerData.Instance.removeCurrentSession();
                                return false;
                            }

                            String _errMsg = "Error in sendTransicsMessage:" + Environment.NewLine;

                            for (int err = 0; err < results.Errors.Length; err++)
                            {
                                _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                            }

                            ServerActions.Instance.log(_errMsg);
                            
                        }

                        if (results.Success)
                        {
							System.Diagnostics.Debug.WriteLine("Server SEND MESSAGE" );
							DealtisMessage response = new DealtisMessage();
							
							response.setUserCode(Msg.getUserCode());
							response.setActionCode(Msg.getActionCode());
							response.setActionType(DealtisMessage.ACTION_SENDMSG);
							response.setActionPriority(Msg.getActionPriority());
							response.setAgancyCode(Msg.getAgancyCode());
							response.setConnectionType(Msg.getConnectionType());
							response.setComments("");
							response.setActionStatus(DealtisMessage.STATUS_EXECUTED);
							
							
							response.getParameters().Add("MSGID", Msg.getActionCode());
							
							
							ApplicationData.Instance.getClientMessageQueue().AddItem(response);

                            return true;
                        }
                        else {  return false; }
                    }
                    
                    return false;
                }
                else
                {
                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in sendTransicsMessage - " + ex.Message);
                
                return false;
            }
        }

		/// <summary>
		///  Retreive list of outbox messages from Transics.
		/// </summary>
        private bool getTransicsOutboxContent()
        {
            try
            {
                TransicsService.Service webService = ServerData.Instance.getTransicsService();


                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];


                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;

                if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr,_txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
                    session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr,_txLang);

                    
					TransicsService.GetTextMessagesOutbox results = webService.Get_TextMessagesOutbox(session, false);

                    if ((results.Errors != null) && (results.Errors.Length > 0))
                    {
                        if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                        {
                            ServerData.Instance.removeCurrentSession();
                            
                            return false;
                        }

                        String _errMsg = "Error in getTransicsOutboxContent:" + Environment.NewLine;

                        for (int err = 0; err < results.Errors.Length; err++)
                        {
                            _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                        }

                        ServerActions.Instance.log(_errMsg);
                        
                    }

                    if (results.TextMessages.Length > 0)
                    {
                        DealtisMessage response = new DealtisMessage();

                        response.setUserCode(Msg.getUserCode());
                        response.setActionCode(Msg.getActionCode());
                        response.setActionType(DealtisMessage.ACTION_OUTBOX);
                        response.setActionPriority(Msg.getActionPriority());
                        response.setAgancyCode(Msg.getAgancyCode());
                        response.setConnectionType(Msg.getConnectionType());
                        response.setComments("");
                        response.setActionStatus(DealtisMessage.STATUS_EXECUTED);


                        for (int i = 0; i < results.TextMessages.Length; i++)
                        {
                            TransicsService.TextMessageOutboxResult oneMsg = results.TextMessages[i];

                            String msgID = "MSG_" + i.ToString();
                            String msgText = oneMsg.Message;
                            DateTime arivleDate = (DateTime)oneMsg.ArrivalDate;
                            response.getParameters().Add(msgID, msgText + DealtisMessage.MSG_SEPARATOR + arivleDate.ToString("G") + DealtisMessage.MSG_SEPARATOR + "0");
                        }

						ApplicationData.Instance.getClientMessageQueue().AddItem (response);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in getTransicsOutboxContent - " + ex.Message);               
                return false;
            }
        }

		/// <summary>
		/// Retreive list of inbox messages from Transics.
		/// </summary>
        private bool getTransicsInboxContent()
        {
            try
            {
                TransicsService.Service webService = ServerData.Instance.getTransicsService();


                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];


                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;

                if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr,_txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
                    session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);

                    

					TransicsService.GetTextMessagesInbox results = webService.Get_TextMessagesInbox(session, false);

                    if ((results.Errors != null) && (results.Errors.Length > 0))
                    {
                        if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                        {
                            ServerData.Instance.removeCurrentSession();
                            return false;
                        }

                        String _errMsg = "Error in getTransicsInboxContent:" + Environment.NewLine;

                        for (int err = 0; err < results.Errors.Length; err++)
                        {
                            _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                        }

                        ServerActions.Instance.log(_errMsg);
                        
                    }

                    if (results.TextMessages.Length > 0)
                    {
                        DealtisMessage response = new DealtisMessage();

                        response.setUserCode(Msg.getUserCode());
                        response.setActionCode(Msg.getActionCode());
                        response.setActionType(DealtisMessage.ACTION_INBOX);
                        response.setActionPriority(Msg.getActionPriority());
                        response.setAgancyCode(Msg.getAgancyCode());
                        response.setConnectionType(Msg.getConnectionType());
                        response.setComments("");
                        response.setActionStatus(DealtisMessage.STATUS_EXECUTED);

                        for (int i = 0; i < results.TextMessages.Length; i++)
                        {
                            TransicsService.TextMessageInboxResult oneMsg = results.TextMessages[i];

                            String msgID = "MSG_" + i.ToString();
                            String msgText = oneMsg.Message;
                            long id = oneMsg.TextMessageId;
                            response.getParameters().Add(msgID, msgText + DealtisMessage.MSG_SEPARATOR + id.ToString());
                        }

						ApplicationData.Instance.getClientMessageQueue().AddItem (response);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in getTransicsInboxContent - " + ex.Message);                

                return false;
            }
        }

		/// <summary>
		///  Send to the Transics the information that received message has been red by user.
		/// </summary>
        private bool setTransicsMessageRead()
        {
            try
            {
                TransicsService.Service webService = ServerData.Instance.getTransicsService();

                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];


                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;

                if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
                    session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);

                    if (Msg.getParameters().ContainsKey("MSGID"))
                    {
                        String textMsgID = Msg.getParameters()["MSGID"];

                       

						TransicsService.ResultInfo results = webService.Read_TextMessage(session, int.Parse(textMsgID));

                        if ((results.Errors != null) && (results.Errors.Length > 0))
                        {
                            if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                            {
                                ServerData.Instance.removeCurrentSession();
                                return false;
                            }

                            String _errMsg = "Error in setTransicsMessageRead:" + Environment.NewLine;

                            for (int err = 0; err < results.Errors.Length; err++)
                            {
                                _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                            }

                            ServerActions.Instance.log(_errMsg);
                           
                        }

                        if (results.Success)
                        {
                            DealtisMessage response = new DealtisMessage();

                            response.setUserCode(Msg.getUserCode());
                            response.setActionCode(Msg.getActionCode());
                            response.setActionType(DealtisMessage.ACTION_SETREAD);
                            response.setActionPriority(Msg.getActionPriority());
                            response.setAgancyCode(Msg.getAgancyCode());
                            response.setConnectionType(Msg.getConnectionType());
                            response.setComments("");
                            response.setActionStatus(DealtisMessage.STATUS_EXECUTED);


                            response.getParameters().Add("MSGID", textMsgID);


							ApplicationData.Instance.getClientMessageQueue().AddItem(response);
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in setTransicsMessageRead - " + ex.Message);               
                return false;
            }
        }

		/// <summary>
		/// Send user's GPS possition to Transics.
		/// </summary>
        private bool sendGPSPossition()
        {
            try
            {

                TransicsService.Service webService = ServerData.Instance.getTransicsService();

                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];


                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;


                if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang) != "")
                {


                    TransicsService.Session session = new TransicsService.Session();
                    session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);


                    if ((Msg.getParameters().ContainsKey("LATITUDE")) && (Msg.getParameters().ContainsKey("LONGITUDE")))
                    {
                        double _lat = Convert.ToDouble(Msg.getParameters()["LATITUDE"]);
                        double _long = Convert.ToDouble(Msg.getParameters()["LONGITUDE"]);

                       

                        TransicsService.Activity activity = new TransicsService.Activity();
                        activity.ID = 1048;
                        session.Position = new TransicsService.Position();
                        session.Position.Latitude = _lat;
                        session.Position.Longitude = _long;

                       
						TransicsService.ResultInfo results = webService.Start_Activity(session, activity,null,null,"");


                        if ((results.Errors != null) && (results.Errors.Length > 0))
                        {
                            if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                            {
                                ServerData.Instance.removeCurrentSession();
                                return false;
                            }

                            String _errMsg = "Error in sendGPSPossition:" + Environment.NewLine;

                            for (int err = 0; err < results.Errors.Length; err++)
                            {
                                _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                            }

                            ServerActions.Instance.log(_errMsg);
                           
                        }

                        if (results.Success)
                        {
                            return true;
                        }
                        else return false;
                    }
                    return false;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in sendGPSPossition - " + ex.Message);                
                return false;
            }
            
        }

		/// <summary>
		///  Load list of activities from Transics.
		/// </summary>
        private bool loadActivities()
        {
            try
            {
                TransicsService.Service webService = ServerData.Instance.getTransicsService();

                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];


                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;

                if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
                    session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);

                   
					TransicsService.GetActivityList_V2 results = webService.Get_ActivityList_V2(session, true);

                    if ((results.Errors != null) && (results.Errors.Length > 0))
                    {
                        if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                        {
                            ServerData.Instance.removeCurrentSession();
                            return false;
                        }

                        String _errMsg = "Error in loadActivities:" + Environment.NewLine;

                        for (int err = 0; err < results.Errors.Length; err++)
                        {
                            _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                        }

                        ServerActions.Instance.log(_errMsg);
                       
                    }

                    if ((results.Activities.Length > 0)||(results.AnomalyCodes.Length > 0))
                    {
                        DealtisMessage response = new DealtisMessage();

                        response.setUserCode(Msg.getUserCode());
                        response.setActionCode(Msg.getActionCode());
                        response.setActionType(DealtisMessage.ACTION_LOADACTIVITIES);
                        response.setActionPriority(Msg.getActionPriority());
                        response.setAgancyCode(Msg.getAgancyCode());
                        response.setConnectionType(Msg.getConnectionType());
                        response.setComments("");
                        response.setActionStatus(DealtisMessage.STATUS_EXECUTED);

                        for (int i = 0; i < results.Activities.Length; i++)
                        {
                            TransicsService.ActivityVersionResult_V2 oneActivityList = results.Activities[i];

                            for (int j = 0; j < oneActivityList.Activities.Length; j++)
                            {
                                TransicsService.ActivityInfo_V2 oneActivity = oneActivityList.Activities[j];
                                
                                String msgID = "ACTIVITY_" + i.ToString()+"_"+j.ToString();
                                String msgText = oneActivity.ID.ToString() + DealtisMessage.MSG_SEPARATOR + oneActivity.Name + DealtisMessage.MSG_SEPARATOR;
                                msgText += oneActivity.ActivityType + DealtisMessage.MSG_SEPARATOR + oneActivity.IsPlanning.ToString();
                               
                                response.getParameters().Add(msgID, msgText);
                            }
                        }

                        for (int i = 0; i < results.AnomalyCodes.Length; i++)
                        {
                            TransicsService.CodeInfo anomale = results.AnomalyCodes[i];
                            String msgID = "ANOMALE_" + i.ToString();
                            String msgText = anomale.Code + DealtisMessage.MSG_SEPARATOR + anomale.Description;                            
                            response.getParameters().Add(msgID, msgText);                        
                        }

						ApplicationData.Instance.getClientMessageQueue().AddItem(response);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in loadActivities - " + ex.Message);              
                return false;
            }
        }


		/// <summary>
		///  Send to the Transics the information about activity which has been started by user.
		/// </summary>
        private bool startActivity(bool _shouldRepond)
        {
            try
            {
                TransicsService.Service webService = ServerData.Instance.getTransicsService();

                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];

                 int activityID = 0;
                 int kilometers = 0;
                 double _long = 0;
                 double _lat = 0;

                 if (Msg.getParameters().ContainsKey("ACTIVITY_ID"))
                    activityID = Convert.ToInt32(Msg.getParameters()["ACTIVITY_ID"]);

                if (Msg.getParameters().ContainsKey("KILOMETERS"))
                    kilometers = Convert.ToInt32(Msg.getParameters()["KILOMETERS"]);

                if (Msg.getParameters().ContainsKey("LATITUDE"))
                    _lat = Convert.ToDouble(Msg.getParameters()["LATITUDE"]);

                if (Msg.getParameters().ContainsKey("LONGITUDE"))
                    _long = Convert.ToDouble(Msg.getParameters()["LONGITUDE"]);




                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;

                if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
					session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);
                    session.Position = new TransicsService.Position();
                    session.Position.Latitude = _lat;
                    session.Position.Longitude = _long;
                    
                    TransicsService.Activity activity = new TransicsService.Activity();
                    activity.ID = activityID;

					TransicsService.ResultInfo results;

                    if (kilometers>0)
						results = webService.Start_Activity(session,activity,kilometers, null,"");                     
					else results = webService.Start_Activity(session,activity,kilometers, null,"");
                    

                    if ((results.Errors != null) && (results.Errors.Length > 0))
                    {
                        if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                        {
							ServerData.Instance.removeCurrentSession();
                            return false;
                        }

                        String _errMsg = "Error in startActivity:" + Environment.NewLine;

                        for (int err = 0; err < results.Errors.Length; err++)
                        {
                            _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                        }

                        ServerActions.Instance.log(_errMsg);
                       
                    }

                    if (results.Success)
                    {
                        if (_shouldRepond)
                        {
                            DealtisMessage response = new DealtisMessage();

                            response.setUserCode(Msg.getUserCode());
                            response.setActionCode(Msg.getActionCode());
                            response.setActionType(DealtisMessage.ACTION_STARTACTIVITY);
                            response.setActionPriority(Msg.getActionPriority());
                            response.setAgancyCode(Msg.getAgancyCode());
                            response.setConnectionType(Msg.getConnectionType());
                            response.setComments("");
                            response.setActionStatus(DealtisMessage.STATUS_EXECUTED);

                            response.getParameters().Add("ACTIVITY_ID", activityID.ToString());

							ApplicationData.Instance.getClientMessageQueue().AddItem(response);
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in startActivity - " + ex.Message);
                

                return false;
            }

        }

		/// <summary>
		///  Send to the Transics the information about activity which has been stopped by user.
		/// </summary>
        private bool stopActivity(bool _shouldRepond)
        {
            try
            {
                TransicsService.Service webService =ServerData.Instance.getTransicsService();

                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];

                int activityID = 0;                
                double _long = 0;
                double _lat = 0;

                if (Msg.getParameters().ContainsKey("ACTIVITY_ID"))
                    activityID = Convert.ToInt32(Msg.getParameters()["ACTIVITY_ID"]);

                if (Msg.getParameters().ContainsKey("LATITUDE"))
                    _lat = Convert.ToDouble(Msg.getParameters()["LATITUDE"]);

                if (Msg.getParameters().ContainsKey("LONGITUDE"))
                    _long = Convert.ToDouble(Msg.getParameters()["LONGITUDE"]);




                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;

				if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
					session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);
                    session.Position = new TransicsService.Position();
                    session.Position.Latitude = _lat;
                    session.Position.Longitude = _long;

                    TransicsService.Activity activity = new TransicsService.Activity();
                    activity.ID = activityID;

                   

					TransicsService.ResultInfo results = webService.Stop_Activity(session, activity, null, null);

                    if ((results.Errors != null) && (results.Errors.Length > 0))
                    {
                        if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                        {
							ServerData.Instance.removeCurrentSession();
                            return false;
                        }

                        String _errMsg = "Error in stopActivity:" + Environment.NewLine;

                        for (int err = 0; err < results.Errors.Length; err++)
                        {
                            _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                        }

                        ServerActions.Instance.log(_errMsg);
                        
                    }

                    if (results.Success)
                    {
                        if (_shouldRepond)
                        {
                            DealtisMessage response = new DealtisMessage();

                            response.setUserCode(Msg.getUserCode());
                            response.setActionCode(Msg.getActionCode());
                            response.setActionType(DealtisMessage.ACTION_STOPACTIVITY);
                            response.setActionPriority(Msg.getActionPriority());
                            response.setAgancyCode(Msg.getAgancyCode());
                            response.setConnectionType(Msg.getConnectionType());
                            response.setComments("");
                            response.setActionStatus(DealtisMessage.STATUS_EXECUTED);

                            response.getParameters().Add("ACTIVITY_ID", activityID.ToString());

							ApplicationData.Instance.getClientMessageQueue().AddItem(response);
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in stopActivity - " + ex.Message);
                

                return false;
            }

        }

		/// <summary>
		///  Load trip of current day from Transics.
		/// </summary>
        private bool loadTrip()
        {
            try
            {
                TransicsService.Service webService = ServerData.Instance.getTransicsService();

                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];

                


                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;

				if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
                    session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);


                    TransicsService.PlanningInfo info = new TransicsService.PlanningInfo();
                    info.GetOnlyModifications = true;

                   

					TransicsService.Planning_v2 results = webService.Get_Planning_v2(session, info);

                    if ((results.Errors != null) && (results.Errors.Length > 0))
                    {
                        if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                        {
							ServerData.Instance.removeCurrentSession();
                            return false;
                        }

                        String _errMsg = "Error in loadTrip:" + Environment.NewLine;

                        for (int err = 0; err < results.Errors.Length; err++)
                        {
                            _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                        }

                        ServerActions.Instance.log(_errMsg);
                        
                    }

                    TransicsService.TripResult_v2 selectedTrip = null;

                    if (results.Trips.Length > 0)
                    {
                        
                        for (int i = 0; i < results.Trips.Length; i++)
                        {
                            if (results.Trips[i].Status == TransicsService.enumPlanningStatus.BUSY)
                            {
                                selectedTrip = results.Trips[i];
                                break;
                            }
                        }

                        if (selectedTrip == null)
                        { 
                            TransicsService.TripResult_v2 oneTrip = null;
                            for (int i = 0; i < results.Trips.Length; i++)
                            {
                                if (results.Trips[i].Status != TransicsService.enumPlanningStatus.NOT_EXECUTED)
                                    continue;

                                DateTime plannedDate = (DateTime)results.Trips[i].ExecutionDate;
                                if (plannedDate.ToShortDateString() == DateTime.Now.ToShortDateString())
                                {
                                    if (oneTrip == null)
                                        oneTrip = results.Trips[i];
                                    else
                                    {
                                        DateTime plannedDate2 = (DateTime)oneTrip.ExecutionDate;
                                        if (plannedDate2.Hour > plannedDate.Hour)
                                            oneTrip = results.Trips[i];
                                        else if ((plannedDate2.Hour == plannedDate.Hour)&& (plannedDate2.Minute > plannedDate.Minute))
                                            oneTrip = results.Trips[i];    
                                    }
                                }
                            }

                            selectedTrip = oneTrip;                        
                        }                        
                    }

                    DealtisMessage response = new DealtisMessage();

                    response.setUserCode(Msg.getUserCode());
                    response.setActionCode(Msg.getActionCode());
                    response.setActionType(DealtisMessage.ACTION_LOADTRIP);
                    response.setActionPriority(Msg.getActionPriority());
                    response.setAgancyCode(Msg.getAgancyCode());
                    response.setConnectionType(Msg.getConnectionType());
                    response.setComments("");
                    response.setActionStatus(DealtisMessage.STATUS_EXECUTED);

                    if (selectedTrip != null)
                    {
                        response.getParameters().Add("TRIP_ID", selectedTrip.TripId);
                        response.getParameters().Add("TRIP_ACT_START", selectedTrip.StartTripAct.ID.ToString());
                        response.getParameters().Add("TRIP_ACT_END", selectedTrip.StopTripAct.ID.ToString());
                        response.getParameters().Add("TRIP_NAME", selectedTrip.DriverDisplay);
                        response.getParameters().Add("TRIP_STATUS", selectedTrip.Status.ToString());
                        response.getParameters().Add("TRIP_DATE", selectedTrip.ExecutionDate.ToString());
                    }


					ApplicationData.Instance.getClientMessageQueue().AddItem(response);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in loadTrip - " + ex.Message);
                

                return false;
            }

        }

		/// <summary>
		/// Start loaded trip on Transics.
		/// </summary>
        private bool startTrip()
        {
            try
            {
                TransicsService.Service webService = ServerData.Instance.getTransicsService();

                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];

                String _tripID = "";
                int _activityID = 0;

                if (Msg.getParameters().ContainsKey("TRIP_ID"))
                    _tripID = Msg.getParameters()["TRIP_ID"];
               

                double _long = 0;
                double _lat = 0;
                

                if (Msg.getParameters().ContainsKey("LATITUDE"))
                    _lat = Convert.ToDouble(Msg.getParameters()["LATITUDE"]);

                if (Msg.getParameters().ContainsKey("LONGITUDE"))
                    _long = Convert.ToDouble(Msg.getParameters()["LONGITUDE"]);



                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;

				if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
					session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);
                    session.Position = new TransicsService.Position();
                    session.Position.Longitude = _long;
                    session.Position.Latitude = _lat;


					TransicsService.ResultInfo results = webService.Start_Trip(session, _tripID, null, null);

                    DealtisMessage response = new DealtisMessage();

                    response.setUserCode(Msg.getUserCode());
                    response.setActionCode(Msg.getActionCode());
                    response.setActionType(DealtisMessage.ACTION_STARTTRIP);
                    response.setActionPriority(Msg.getActionPriority());
                    response.setAgancyCode(Msg.getAgancyCode());
                    response.setConnectionType(Msg.getConnectionType());
                    response.setComments("");
                    response.setActionStatus(DealtisMessage.STATUS_EXECUTED);


                    if ((results.Errors != null) && (results.Errors.Length > 0))
                    {
                        if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                        {
                            ServerData.Instance.removeCurrentSession();
                            return false;
                        }

                        String _errMsg = "Error in startTrip:" + Environment.NewLine;

                        for (int err = 0; err < results.Errors.Length; err++)
                        {
                            _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                        }

                        ServerActions.Instance.log(_errMsg);                        
                        response.getParameters().Add("RESULT", "-1");                        
                    }

                    if (results.Success)
                    {                        
                        response.getParameters().Add("RESULT", "1");                                                                                             
                    }
					ApplicationData.Instance.getClientMessageQueue().AddItem(response);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in startTrip - " + ex.Message);               
                return false;
            }

        }

		/// <summary>
		/// Stop loaded trip on Transics.
		/// </summary>
        private bool stopTrip()
        {
            try
            {
                TransicsService.Service webService = ServerData.Instance.getTransicsService();

                String _txUser = "";
                String _txPassword = "";
                int _txSystemNr = -1;

                if (Msg.getParameters().ContainsKey("TX_USER"))
                    _txUser = Msg.getParameters()["TX_USER"];
                if (Msg.getParameters().ContainsKey("TX_PASSWORD"))
                    _txPassword = Msg.getParameters()["TX_PASSWORD"];
                if (Msg.getParameters().ContainsKey("TX_SYSTEMNR"))
                    _txSystemNr = Convert.ToInt32(Msg.getParameters()["TX_SYSTEMNR"]);

                String _txLang = "EN";
                if (Msg.getParameters().ContainsKey("LANGUAGE"))
                    _txLang = Msg.getParameters()["LANGUAGE"];

                String _tripID = "";
                int _activityID = 0;

                if (Msg.getParameters().ContainsKey("TRIP_ID"))
                    _tripID = Msg.getParameters()["TRIP_ID"];
              

                double _long = 0;
                double _lat = 0;
                

                if (Msg.getParameters().ContainsKey("LATITUDE"))
                    _lat = Convert.ToDouble(Msg.getParameters()["LATITUDE"]);

                if (Msg.getParameters().ContainsKey("LONGITUDE"))
                    _long = Convert.ToDouble(Msg.getParameters()["LONGITUDE"]);



                if (((_txUser == null) || (_txUser.Length == 0)) ||
                    ((_txPassword == null) || (_txPassword.Length == 0)) ||
                    (_txSystemNr <= 0))
                    return false;

				if (ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang) != "")
                {

                    TransicsService.Session session = new TransicsService.Session();
					session.SessionID = ServerData.Instance.getTransicsSessionID(_txUser, _txPassword, _txSystemNr, _txLang);
                    session.Position = new TransicsService.Position();
                    session.Position.Longitude = _long;
                    session.Position.Latitude = _lat;


					TransicsService.ResultInfo results = webService.Stop_Trip(session, _tripID, null, null);

                    DealtisMessage response = new DealtisMessage();

                    response.setUserCode(Msg.getUserCode());
                    response.setActionCode(Msg.getActionCode());
                    response.setActionType(DealtisMessage.ACTION_STOPTRIP);
                    response.setActionPriority(Msg.getActionPriority());
                    response.setAgancyCode(Msg.getAgancyCode());
                    response.setConnectionType(Msg.getConnectionType());
                    response.setComments("");
                    response.setActionStatus(DealtisMessage.STATUS_EXECUTED);


                    if ((results.Errors != null) && (results.Errors.Length > 0))
                    {
                        if (results.Errors[0].ErrorCode == "ERROR_SESSION_CLOSED")
                        {
							ServerData.Instance.removeCurrentSession();
                            return false;
                        }

                        String _errMsg = "Error in stopTrip:" + Environment.NewLine;

                        for (int err = 0; err < results.Errors.Length; err++)
                        {
                            _errMsg += results.Errors[err].ErrorCode + " - " + results.Errors[err].Value + Environment.NewLine;
                        }

                        ServerActions.Instance.log(_errMsg);                        

                        response.getParameters().Add("RESULT", "-1");                                                                                             
                    }

                    if (results.Success)
                    {
                        response.getParameters().Add("RESULT", "1");                                                                                             
                    }

					ApplicationData.Instance.getClientMessageQueue().AddItem(response);


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in stopTrip - " + ex.Message);               
                return false;
            }

        }

    }
}
