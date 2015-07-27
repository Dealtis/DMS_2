using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System.Diagnostics;


namespace DMSvStandard
{
	/// <summary>
	/// This is a singleton class which provides all necessary actions which should be done by application server
	/// </summary>
    class ServerActions
    {
		private static ServerActions instance;
		ServerMainThread th;
		Thread m_MainThread=null;


		private ServerActions()
         {
            
         }

		public static ServerActions Instance
         {
            get
            {
                if (instance == null)
                {
					instance = new ServerActions();
                }
                return instance;
            }
         }


		/// <summary>
		/// Check if the message in server queue
		/// </summary>
        private bool isRequestReceived(DealtisMessage newMsg)
        {
             foreach (DealtisMessage oneMsg in ApplicationData.Instance.getServerMessageQueue())
             {
                 if (oneMsg.getParameters().ContainsKey("UPDATE_NOW"))
                     return true;
             }

            return false;
        
        }

		/// <summary>
		/// Update message status in server queue
		/// </summary>
         private void updateRecivedMessages(DealtisMessage newMsg)
         {
             foreach (DealtisMessage oneMsg in ApplicationData.Instance.getServerMessageQueue())
             {
                 if (oneMsg.getActionCode() == newMsg.getActionCode())
                 {
                     newMsg.setActionStatus(oneMsg.getActionStatus());
                     newMsg.setMessageDate(oneMsg.getMessageDate());
					ApplicationData.Instance.getServerMessageQueue().Remove(oneMsg);
					ApplicationData.Instance.getServerMessageQueue().Add(newMsg);
                     
                     return;
                 }
             }

			ApplicationData.Instance.getServerMessageQueue().Add(newMsg);
         }

		/// <summary>
		/// Parses the message and create object of type DealtisMessage.
		/// </summary>
         private DealtisMessage parseMessage(String strMsg)
         {
             DealtisMessage msg = new DealtisMessage();

             
             String strCurrentNode = "";
             String strParamName="";

             try
             {
                 using (XmlReader reader = XmlReader.Create(new StringReader(strMsg)))
                 {
                     while (reader.Read())
                     {
                         switch (reader.NodeType)
                         {
                             case XmlNodeType.Element:
                                 strCurrentNode = reader.Name;
                                 if ((strCurrentNode == "PARAMETER") && (reader.MoveToNextAttribute()))
                                 {
                                     reader.MoveToNextAttribute();
                                     if (reader.Name.Trim() == "name")
                                         strParamName = reader.Value.Trim();                                 
                                 }
                                 break;
                             case XmlNodeType.Text:
                                 if (strCurrentNode == "USER")
                                 {
                                     msg.setUserCode(reader.Value.Trim());
                                 }
                                 else if (strCurrentNode == "DATE_TIME")
                                 {
                                     msg.setMessageDate(DateTime.Parse( reader.Value.Trim()));
                                 }
                                 else if (strCurrentNode == "TIMEOUT")
                                 {
                                     msg.setTimeout(DateTime.Parse(reader.Value.Trim()));
                                 }
                                 else if (strCurrentNode == "AGENCY_CODE")
                                 {
                                     msg.setAgancyCode(reader.Value.Trim());
                                 }
                                 else if (strCurrentNode == "ACTION_TYPE")
                                 {
                                     msg.setActionType(reader.Value.Trim());
                                 }
                                 else if (strCurrentNode == "ACTION_CODE")
                                 {
                                     msg.setActionCode(reader.Value.Trim());
                                 }
                                 else if (strCurrentNode == "PRIORITY")
                                 {
                                     msg.setActionPriority(Convert.ToInt32(reader.Value.Trim()));
                                 }
                                 else if (strCurrentNode == "CONNECTION")
                                 {
                                     msg.setConnectionType(Convert.ToInt32(reader.Value.Trim()));
                                 }
                                 else if (strCurrentNode == "STATUS")
                                 {
                                     msg.setActionStatus(Convert.ToInt32(reader.Value.Trim()));
                                 }
                                 else if (strCurrentNode == "COMMENTS")
                                 {
                                     msg.setComments(reader.Value.Trim());
                                 }
                                 else if (strCurrentNode == "PARAMETER")
                                 {
                                     if ((strParamName != null) && (strParamName.Length > 0))
                                        msg.getParameters().Add(strParamName, reader.Value.Trim());
                                     strParamName = "";
                                 }
                                 break;
                             case XmlNodeType.Attribute:
                                 strParamName = reader.Value.Trim();
                                 break;
                             case XmlNodeType.XmlDeclaration:
                             case XmlNodeType.ProcessingInstruction:
                             case XmlNodeType.Comment:
                             case XmlNodeType.EndElement:                             
                                 break;
                         }
                     }
                 }
             }
             catch (FileNotFoundException e)
             { return null; }



             return msg;                     
         }

		/// <summary>
		/// Check if the connection to the Internet is available
		/// </summary>
         public bool isConnectionAvailable(int nConnectionType)
         {
             if (testConnection())
             {
                 return true;
             }
			return false;

             bool results = false;
             if (nConnectionType == DealtisMessage.CONNECTION_ANY)
             {
                 if (isUSBConnectionAvailable())
                     results = true;
                 else if (findWirelessAdapter())
                     results = true;
                 else if (isGPRSConnectionAvailable())
                     results = true;
                 else results = false;
             }
             else if (nConnectionType == DealtisMessage.CONNECTION_USB)
             {
                 if (isUSBConnectionAvailable())
                     results = true;
                 else results = false;
             }
             else if (nConnectionType == DealtisMessage.CONNECTION_WIFI)
             {
                 if (findWirelessAdapter())
                     results = true;
                 else results = false;
             }
             else if (nConnectionType == DealtisMessage.CONNECTION_GPRS)
             {
                 if (isGPRSConnectionAvailable())
                     results = true;
                 else results = false;
             }

             if (!results)
                ServerActions.Instance.log("Connection is not available");

             return results;
             

         }

        public bool isUSBConnectionAvailable()
		{ return true;
           
        }

        public bool isWifiConnectionAvailable()
		{ return true;
            
        }

        public bool isGPRSConnectionAvailable()
		{ return true;
           
        }


        


        public bool testConnection()
        {
         
            try
            {
                System.Net.IPHostEntry ipHe =
                    System.Net.Dns.GetHostByName(ServerData.Instance.TestIP);
                return true;
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in testConnection - " + ex.Message);
                

                return false;
            }
        }

        public void log(String szMsg)
        {
            try
            {
                

                String path = ApplicationData.Instance.getApplicationPath() + "DTMServer.log";
               
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(DateTime.Now.ToShortTimeString() + " - " + szMsg);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(DateTime.Now.ToShortTimeString() + " - " + szMsg);
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        public bool findWirelessAdapter()
		{ return true;
            
        }

		/// <summary>
		/// Login into Transics
		/// </summary>
		/// <returns>Session ID</returns>
        public String TransicsLogin()
        {
            try
            {

                ServerData.Instance.setTxLock();

                TransicsService.Login login = new TransicsService.Login();

                login.SystemNr = Convert.ToInt32(ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
				login.PdaID = ApplicationData.Instance.getConfigurationModel().getTxUserPassword();
                login.Vehicle = new TransicsService.IdentifierVehicle();
				login.Vehicle.Id = ApplicationData.Instance.getConfigurationModel().getTxUserName();
				login.Language = ApplicationData.Instance.getConfigurationModel().getLanguage();


				TransicsService.Service webService = ServerData.Instance.getTransicsService();



				TransicsService.LoginResult finalResult = webService.Login(login);

                if ((finalResult.Errors != null) && (finalResult.Errors.Length > 0))
                {
                    String _errMsg = "Error in TransicsLogin:" + Environment.NewLine;

                    for (int err = 0; err < finalResult.Errors.Length; err++)
                    {
                        _errMsg += finalResult.Errors[err].ErrorCode + " - " + finalResult.Errors[err].Value + Environment.NewLine;
                    }

                    ServerActions.Instance.log(_errMsg);
                    
                }
                

                String sessionID = "";

                if (finalResult.Success)
                    sessionID = finalResult.SessionID;


                ServerData.Instance.releaseTxLock();

                return sessionID;
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in TransicsLogin - " + ex.Message);
               
				ServerData.Instance.releaseTxLock();
                return "";
            }
            
        }

		/// <summary>
		/// Logout and close the session with Transics
		/// </summary>
        public bool TransicsLogout(String _txSession)
        {
            try
            {
                ServerData.Instance.setTxLock();

                TransicsService.Service webService = ServerData.Instance.getTransicsService();
                TransicsService.Session session = new TransicsService.Session();
                session.SessionID = _txSession;

                
				TransicsService.ResultInfo finalResult = webService.Logout(session);

                if ((finalResult.Errors != null) && (finalResult.Errors.Length > 0))
                {
                    String _errMsg = "Error in TransicsLogout:" + Environment.NewLine;

                    for (int err = 0; err < finalResult.Errors.Length; err++)
                    {
                        _errMsg += finalResult.Errors[err].ErrorCode + " - " + finalResult.Errors[err].Value + Environment.NewLine;
                    }

                    ServerActions.Instance.log(_errMsg);
                    
                }

                bool logoutResult = false;

                if (finalResult.Success)
                    logoutResult = true;

                ServerData.Instance.releaseTxLock();

                return logoutResult;
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in TransicsLogout - " + ex.Message);
                
				ServerData.Instance.releaseTxLock();
                return false;
            }

        }

        public String getCurrentActionCode()
        {
            String strCode = "";

            
            strCode += DateTime.Now.ToString("yyyyMMddHHmmssfff");

            return strCode;

        }

		/// <summary>
		/// Removes the messages by action type from servere message queue.
		/// </summary>
        public int removeMessagesByActionType(String _actionType)
        {
            int result = 0; /* 0 - nothing to remove; 1 - removed from queue; 2 - remove original*/

            for (int i = 0; i < ApplicationData.Instance.getServerMessageQueue().Count; i++)
            { 
				if ((ApplicationData.Instance.getServerMessageQueue()[i].getActionType() == _actionType)&&
				    ((ApplicationData.Instance.getServerMessageQueue()[i].getActionStatus() == DealtisMessage.STATUS_NEW)||
				 (ApplicationData.Instance.getServerMessageQueue()[i].getActionStatus() == DealtisMessage.STATUS_QUEUED)))
                {
					ApplicationData.Instance.getServerMessageQueue().RemoveAt(i);
                    result = 1;
                    break;
                }
				else if ((ApplicationData.Instance.getServerMessageQueue()[i].getActionType() == _actionType) &&
				         ((ApplicationData.Instance.getServerMessageQueue()[i].getActionStatus() == DealtisMessage.STATUS_NEW) ||
				 (ApplicationData.Instance.getServerMessageQueue()[i].getActionStatus() == DealtisMessage.STATUS_QUEUED)))
                {
                    result = 2;
                    break;
                }            
            }

            return result;
        
        }

		/// <summary>
		/// Starts the server.
		/// </summary>
		public void StartServer()
		{

			m_MainThread = new Thread(new ThreadStart(this.WorkerThreadFunction));
			
			m_MainThread.Name = "DTMDServer Main Thread";
			
			m_MainThread.Start();
			
			
			//base.Start();
		}

		public bool isServerStarted()
		{
			if (m_MainThread != null)
				return true;
			else
				return false;
		}
		
		private void WorkerThreadFunction()
		{
			
			th = new ServerMainThread();
			
			th.Run();
		}

        
    }
}
