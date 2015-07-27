using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using Android.Media;
using Android.Locations;

using System;
using System.Collections.Generic;

namespace DMSvStandard
{

	/// <summary>
	///  This is a singleton class which provides all necessary actions which should be done by application 
	/// </summary>
	class ApplicationActions
    {

		System.Timers.Timer inboxTimer;
		System.Timers.Timer outboxTimer;
		System.Timers.Timer gpsTimer;

        private static ApplicationActions instance;


       
		bool m_bStarted = false;


		Location _previousLocation;

		bool m_bStartTrip;
		int currentActivityID=0;
		DtmdLocationManager gpsLocationManager=null;
		DtmdLocationManager networkLocationManager = null;

        private ApplicationActions()
        {            
			ApplicationData.Instance.getClientMessageQueue().OnAdd += new EventHandler(ReceiveClientMessage); 
        }

        public static ApplicationActions Instance
        {
            get
            {
                if (instance == null)
                {  
                    instance = new ApplicationActions();
                }
                return instance;
            }
        }

		public void initTimers ()
		{
			if (m_bStarted)
				return;
			if (player == null) {
				player = new MediaPlayer ();
			}
			sendRemainingMessages ();
			if (ApplicationData.Instance.getConfigurationModel ().getInboxUpdateInterval () > 0) {
				inboxTimer = new System.Timers.Timer ();
				inboxTimer.Elapsed += new System.Timers.ElapsedEventHandler (OnInboxTimerHandler);
				inboxTimer.Interval = ApplicationData.Instance.getConfigurationModel ().getInboxUpdateInterval () * 60000;
				inboxTimer.Enabled = true;
				inboxTimer.Start ();
			}
			if (ApplicationData.Instance.getConfigurationModel ().getOutboxUpdateInterval () > 0) {
				outboxTimer = new System.Timers.Timer ();
				outboxTimer.Elapsed += new System.Timers.ElapsedEventHandler (OnOutboxTimerHandler);
				outboxTimer.Interval = ApplicationData.Instance.getConfigurationModel ().getOutboxUpdateInterval () * 60000;
				outboxTimer.Enabled = true;
				outboxTimer.Start ();
			}
			m_bStarted = true;
		}

		/// <summary>
		/// Sends the remaining messages from queue to the execution.
		/// </summary>
		public void sendRemainingMessages()
		{
			List<TextMessage> msgs = loadMessages (TextMessage.MSG_OUTBOX);

			foreach (TextMessage oneMsg in msgs)
			{
				if (oneMsg.Status == TextMessage.STATUS_TOBESENT) {
					DealtisMessage msg = new DealtisMessage();

					msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
					msg.setActionCode(oneMsg.Id.ToString());
					msg.setActionPriority(3);
					msg.setActionStatus(DealtisMessage.STATUS_NEW);
					msg.setActionType(DealtisMessage.ACTION_SENDMSG);
					msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
					msg.setComments("");
					msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
					msg.setMessageDate(oneMsg.ActionDate);
					msg.getParameters().Add("TEXT_MSG", oneMsg.Message);
					msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
					msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
					msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
					msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());

					ApplicationData.Instance.getServerMessageQueue().Add(msg);
				}
			}
		}

		/// <summary>
		/// Restarts inbox,outbox and GPS possition update timers
		/// </summary>
		public void restartTimers()
		{
			if (inboxTimer != null)
				inboxTimer.Stop ();
			if (outboxTimer != null)
				outboxTimer.Stop ();
			if (gpsTimer != null)
				gpsTimer.Stop ();

			if (ApplicationData.Instance.getConfigurationModel().getInboxUpdateInterval()>0)
			{
				inboxTimer = new System.Timers.Timer();
				inboxTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnInboxTimerHandler);
				inboxTimer.Interval = ApplicationData.Instance.getConfigurationModel().getInboxUpdateInterval()*60000;
				inboxTimer.Enabled = true;
				inboxTimer.Start();

			}

			if (ApplicationData.Instance.getConfigurationModel().getOutboxUpdateInterval()>0)
			{
				outboxTimer = new System.Timers.Timer();
				outboxTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnOutboxTimerHandler);
				outboxTimer.Interval = ApplicationData.Instance.getConfigurationModel().getOutboxUpdateInterval()*60000;
				outboxTimer.Enabled = true;
				outboxTimer.Start();

			}

			if ((m_bStartTrip)&&(ApplicationData.Instance.getConfigurationModel().getGspPositionSending()>0))
			{
				gpsTimer = new System.Timers.Timer ();
				gpsTimer.Elapsed += new System.Timers.ElapsedEventHandler (OnGpsTimerHandler);
				gpsTimer.Interval = ApplicationData.Instance.getConfigurationModel().getGspPositionSending()*60000;
				gpsTimer.Enabled = true;
				gpsTimer.Start ();

			}
		}

		public void Destory()
		{
			player.Release ();

			if (m_bStarted) {
				if (gpsLocationManager!=null)
					gpsLocationManager.stopLocation ();
				if (networkLocationManager != null)
					networkLocationManager.stopLocation ();
			}

		}

		/// <summary>
		/// Raises the inbox timer handler event and send request to load inbox messages from Transics
		/// </summary>
		private void OnInboxTimerHandler(object source, System.Timers.ElapsedEventArgs args)
		{
			ApplicationActions.Instance.loadInboxMessages();

		}

		/// <summary>
		/// Raises the outbox timer handler event and send request to load outbox messages from Transics
		/// </summary>
		private void OnOutboxTimerHandler(object source, System.Timers.ElapsedEventArgs args)
		{
			ApplicationActions.Instance.loadOutboxMessages();

		}

		protected MediaPlayer player;
		public void alert()
		{
			player.Reset();
			player.SetDataSource(MainActivity.getContext().Assets.OpenFd("sound/beep2.mp3").FileDescriptor);
			player.Prepare(); 
			player.Start(); 

		}


		/// <summary>
		/// Gets the best location from GPS provider.
		/// </summary>
		/// <returns>The best location.</returns>
		private Location getBestLocation()
		{

			 Location meilleurlocation = null;
			if ((gpsLocationManager == null) && (networkLocationManager == null)) {
				return null;
				meilleurlocation = null;
			} else {
				if (gpsLocationManager != null) {
					if (gpsLocationManager.isLocationEnabled ())
						meilleurlocation = gpsLocationManager.getCurrentLocation ();
					
								
				}

				if (networkLocationManager != null) {
					if (networkLocationManager.isLocationEnabled ())
						meilleurlocation = networkLocationManager.getCurrentLocation ();
						//return networkLocationManager.getCurrentLocation ();
					
				}


			}
				
			return meilleurlocation;
		}
        
		/// <summary>
		/// Raises the gps timer handler event and send users current GPS possition
		/// </summary>
		private void OnGpsTimerHandler(object source, System.Timers.ElapsedEventArgs args)
		{
			Location _currentLocation = getBestLocation();
			//
			if (_currentLocation != null) {


				if (_previousLocation == null) {
					ApplicationActions.Instance.StartActivity (ApplicationData.Instance.getConfigurationModel ().getActivityDriving (), 0, _currentLocation.Latitude, _currentLocation.Longitude);
					_previousLocation = _currentLocation;
					currentActivityID = ApplicationData.Instance.getConfigurationModel ().getActivityDriving ();

				} else {
					if ((distance (_currentLocation.Latitude, _currentLocation.Longitude, _previousLocation.Latitude, _previousLocation.Longitude) < 150) /*(_currentLocation.Latitude == _previousLocation.Latitude) && (_currentLocation.Longitude == _previousLocation.Longitude)*/) {
						if (currentActivityID != ApplicationData.Instance.getConfigurationModel ().getActivityWaiting ()) {
//							ApplicationActions.Instance.StartActivity (ApplicationData.Instance.getConfigurationModel ().getActivityWaiting (), 0, _currentLocation.Latitude, _currentLocation.Longitude);
//							_previousLocation = _currentLocation;
//							currentActivityID = ApplicationData.Instance.getConfigurationModel ().getActivityWaiting ();
							Console.Out.Write(">>>>>POSITION < 150 SET WAITING"+DateTime.Now.Minute);
							ApplicationActions.Instance.StartActivity (ApplicationData.Instance.getConfigurationModel ().getActivityWaiting (), 0, _currentLocation.Latitude, _currentLocation.Longitude);
							_previousLocation = _currentLocation;
							currentActivityID = ApplicationData.Instance.getConfigurationModel ().getActivityWaiting ();  
						}else{
							Console.Out.Write(">>>>>POSITION < 150 SET WAITING AFTER WAITING"+DateTime.Now.Minute);
							ApplicationActions.Instance.StartActivity (ApplicationData.Instance.getConfigurationModel ().getActivityWaiting (), 0, _currentLocation.Latitude, _currentLocation.Longitude);
							_previousLocation = _currentLocation;
							currentActivityID = ApplicationData.Instance.getConfigurationModel ().getActivityWaiting ();  
						}

					} else {
						ApplicationActions.Instance.StartActivity (ApplicationData.Instance.getConfigurationModel ().getActivityDriving (), 0, _currentLocation.Latitude, _currentLocation.Longitude);
						_previousLocation = _currentLocation;
						Console.Out.Write(">>>>>SET DRIVING"+DateTime.Now.Minute);
						currentActivityID = ApplicationData.Instance.getConfigurationModel ().getActivityDriving ();
					}

				}

			} 
		}

		public bool isTripStarted()
		{
			return m_bStartTrip;
		}

		public void setTripStarted(bool _b)
		{
			m_bStartTrip = _b;
		}

		/// <summary>
		/// Changes the state of the trip depending from its current state.
		/// if trip is started then it stops trip, otherwise it start trip.
		/// </summary>
		public void ChangeTripState()
		{

			if (!m_bStartTrip) {
				m_bStartTrip = true;
			} else {
				m_bStartTrip = false;
			}

			if (m_bStartTrip) {
				_previousLocation = null;
				currentActivityID = 0;

				gpsLocationManager = new DtmdLocationManager (0);
				networkLocationManager = new DtmdLocationManager (1);
				//60 de 60 000
				gpsLocationManager.startLocation (ApplicationData.Instance.getConfigurationModel ().getGspPositionSending () * 60000 / 3);
				networkLocationManager.startLocation (ApplicationData.Instance.getConfigurationModel ().getGspPositionSending () * 60000 / 3);

				if (ApplicationData.Instance.getConfigurationModel().getGspPositionSending()>0)
				{
					gpsTimer = new System.Timers.Timer ();
					gpsTimer.Elapsed += new System.Timers.ElapsedEventHandler (OnGpsTimerHandler);
					gpsTimer.Interval = ApplicationData.Instance.getConfigurationModel().getGspPositionSending()*60000;
					gpsTimer.Enabled = true;
					gpsTimer.Start ();

				}
			} else {
				gpsTimer.Stop ();
				gpsLocationManager.stopLocation ();
				networkLocationManager.stopLocation ();
				_previousLocation = null;
				currentActivityID = 0;
			}
		}


        
		/// <summary>
		/// This is a listener for receiving messages from the server
		/// </summary>
		private void ReceiveClientMessage(object sender, EventArgs e)
        {
            if (ApplicationData.Instance.getClientMessageQueue() == null)
                return;

  
			DealtisMessage newMsg = ApplicationData.Instance.getClientMessageQueue()[ApplicationData.Instance.getClientMessageQueue().Count-1];
                if (newMsg != null)
                {
                    if (newMsg.getActionType() == DealtisMessage.ACTION_OUTBOX)
                    {
                        List<TextMessage> outboxMessages = new List<TextMessage>();
                        foreach (KeyValuePair<string, string> kvp in newMsg.getParameters())
                        {                            
                                TextMessage oneMsg = new TextMessage();
                                String value = kvp.Value;
                                String[] splitedValues = Regex.Split(value, DealtisMessage.MSG_SEPARATOR);
                                   
                                oneMsg.Message = splitedValues[0];
                                oneMsg.ArrivalDate = DateTime.Parse(splitedValues[1]);
                                if (splitedValues[2] == "0")
                                    oneMsg.Status = TextMessage.STATUS_SENT;
                                else if (splitedValues[2] == "1")
                                    oneMsg.Status = TextMessage.STATUS_TREATED;
                                outboxMessages.Add(oneMsg);
                        }

                        if (outboxMessages.Count > 0)
                        {
                            List<TextMessage> existingMessages = loadMessages(TextMessage.MSG_OUTBOX);
                            existingMessages = updateOutboxMessageList(existingMessages, outboxMessages);
							saveMessages(existingMessages, TextMessage.MSG_OUTBOX);
                           
                        }  
                    }
                    else if (newMsg.getActionType() == DealtisMessage.ACTION_INBOX)
                    {
                        List<TextMessage> inboxMessages = new List<TextMessage>();
                        foreach (KeyValuePair<string, string> kvp in newMsg.getParameters())
                        {
                            TextMessage oneMsg = new TextMessage();
                            String value = kvp.Value;
                            String[] splitedValues = Regex.Split(value, DealtisMessage.MSG_SEPARATOR);

                            oneMsg.Message = splitedValues[0];
                            oneMsg.Id = int.Parse(splitedValues[1]);
                            oneMsg.ArrivalDate = DateTime.Now;
                            oneMsg.Status = TextMessage.STATUS_UNREAD;
                            inboxMessages.Add(oneMsg);
                        }

                        if (inboxMessages.Count > 0)
                        {
							List<TextMessage> existingMessages = loadMessages(TextMessage.MSG_INBOX);
                            List<TextMessage> allMessages = updateInboxMessageList(existingMessages, inboxMessages);

							saveMessages(allMessages, TextMessage.MSG_INBOX);
                           
                        }
                    }
                    else if ((newMsg.getActionType() == DealtisMessage.ACTION_SETREAD)&&(newMsg.getActionStatus() == DealtisMessage.STATUS_EXECUTED))
                    {

                        if (newMsg.getParameters().ContainsKey("MSGID"))
                        {
                            String textMsgID = newMsg.getParameters()["MSGID"];
                            
							List<TextMessage> existingMessages = loadMessages(TextMessage.MSG_INBOX);
                            setInboxMessagesRead(int.Parse(textMsgID), existingMessages);
							saveMessages(existingMessages, TextMessage.MSG_INBOX);
                            ApplicationData.Instance.setInboxIndicator(ApplicationData.Instance.getInboxIndicator() - 1);
							
                        }
					System.Diagnostics.Debug.WriteLine("End SETREAD - " + ApplicationData.Instance.getOutboxIndicator().ToString());
                    }
					else if (newMsg.getActionType() == DealtisMessage.ACTION_SENDMSG)
					{
					System.Diagnostics.Debug.WriteLine("Start SEND MESSAGE - " + ApplicationData.Instance.getOutboxIndicator().ToString());
						if (newMsg.getParameters().ContainsKey("MSGID"))
						{
							String textMsgID = newMsg.getParameters()["MSGID"];
						
							List<TextMessage> existingMessages = loadMessages(TextMessage.MSG_OUTBOX);
							setOutboxMessagesSend(int.Parse(textMsgID), existingMessages);
							saveMessages(existingMessages, TextMessage.MSG_OUTBOX);
							ApplicationData.Instance.setOutboxIndicator(ApplicationData.Instance.getOutboxIndicator() - 1);
							
						}
					System.Diagnostics.Debug.WriteLine("End SEND MESSAGE - " + ApplicationData.Instance.getOutboxIndicator().ToString());
					}                    
                    else if (newMsg.getActionType() == DealtisMessage.ACTION_LOADTRIP)
                    {
                        Trip newTrip = new Trip();
                        newTrip.TripState = Trip.STATE_STOP;
                        foreach (KeyValuePair<string, string> kvp in newMsg.getParameters())
                        {
                            
                            
                            String str;
                            String id = kvp.Key;
                            if (id == "TRIP_ID")
                                newTrip.TripID = kvp.Value;
                            else if (id == "TRIP_ACT_START")
                                newTrip.TripStartActivityID = Convert.ToInt32(kvp.Value);
                            else if (id == "TRIP_ACT_END")
                                newTrip.TripEndActivityID = Convert.ToInt32(kvp.Value);
                            else if (id == "TRIP_NAME")
                                newTrip.TripName = kvp.Value;
                            else if (id == "TRIP_STATUS")
                            {
                                if (kvp.Value == "NOT_EXECUTED")
                                    newTrip.TripState = 7;
                                else if (kvp.Value == "BUSY")
                                    newTrip.TripState = 5;
                                else if (kvp.Value == "FINISHED")
                                    newTrip.TripState = 2;
 

                            }
                             else if (id == "TRIP_DATE")
                                newTrip.TripExecutionDate = Convert.ToDateTime(kvp.Value);
                          
                        }
                        if ((newTrip.TripID!=null)&&(newTrip.TripID!= ""))
                            ApplicationData.Instance.setCurrentTrip(newTrip);
                    }
                    else if (newMsg.getActionType() == DealtisMessage.ACTION_STARTTRIP)
                    {
                        if (newMsg.getParameters()["RESULT"] == "1")
                        {
                            if (ApplicationData.Instance.getCurrentTrip() != null)
                            {
                                ApplicationData.Instance.getCurrentTrip().TripState = Trip.STATE_START;
                                ApplicationData.Instance.getCurrentTrip().TripActionResult = 1;
                            }
                        }
                        else if (newMsg.getParameters()["RESULT"] == "-1")
                        {
                            if (ApplicationData.Instance.getCurrentTrip() != null)
                                ApplicationData.Instance.getCurrentTrip().TripActionResult = -1;
                        }
                    }
                    else if (newMsg.getActionType() == DealtisMessage.ACTION_STOPTRIP)
                    {
                        if (newMsg.getParameters()["RESULT"] == "1")
                        {
                            if (ApplicationData.Instance.getCurrentTrip() != null)
                            {
                                ApplicationData.Instance.getCurrentTrip().TripState = Trip.STATE_STOP;
                                ApplicationData.Instance.getCurrentTrip().TripActionResult = 1;
                            }
                        }
                        else if (newMsg.getParameters()["RESULT"] == "-1")
                        {
                            if (ApplicationData.Instance.getCurrentTrip() != null)
                                ApplicationData.Instance.getCurrentTrip().TripActionResult = -1;
                        } 
                    }
                }
				removeMessageByID(newMsg.getActionCode());
          
        }

		private void removeMessageByID (String _msgID)
		{
			for (int i=0; i<ApplicationData.Instance.getClientMessageQueue().Count; i++) {
				if (ApplicationData.Instance.getClientMessageQueue()[i].getActionCode()==_msgID)
				{
					ApplicationData.Instance.getClientMessageQueue().RemoveAt(i);
					return;
				}
			}
		}

		/// <summary>
		/// Parse messages from XML and create DealtisMessage object
		/// </summary>
        private DealtisMessage parseMessage(String strMsg)
        {
            DealtisMessage msg = new DealtisMessage();


            String strCurrentNode = "";
            String strParamName = "";

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
                                    msg.setMessageDate(DateTime.Parse(reader.Value.Trim()));
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

        

        public String getDateTime()
        {
            DateTime currentDate = DateTime.Now;
            String strDate = currentDate.ToShortDateString();

            String strTime = currentDate.ToShortTimeString();

            return strDate + " " + strTime;
        }

       

		public bool isExternalStorageAvailable()
		{
			String state = Android.OS.Environment.ExternalStorageState;

			if (Android.OS.Environment.MediaMounted.Equals(state)) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// Saves the message into corresonding XML file depending on message type.
		/// </summary>
		/// <param name="_msg"> instance of TextMessage</param>
		/// <param name="_nMessageType">Type of message: 1 - sent message, 2 - received message, 3 - draft message</param>
        public void saveMessage(TextMessage _msg, int _nMessageType)
        {
			System.IO.Stream fs = null;


			String fileName = "sent_messages.xml";

			if (_nMessageType == 2)
				fileName = "received_messages.xml";
			else if (_nMessageType == 3)
				fileName = "draft_messages.xml";



			if (isExternalStorageAvailable ()) {
				String storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

				if (!Directory.Exists (storagePath + "/DTMD"))
					Directory.CreateDirectory (storagePath + "/DTMD");					 

				fileName = storagePath + "/DTMD/" + fileName;

				if (!File.Exists (fileName)) {                
					TextWriter tw = new StreamWriter (fileName);                
					tw.WriteLine ("<?xml version=\"1.0\"?>");
					tw.WriteLine ("<Messages>");
					tw.WriteLine ("</Messages>");                
					tw.Close ();
				}

				fs = new FileStream (fileName, FileMode.Open);
			} else {
				try
				{
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
				catch (Exception e)
				{ 
					fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
					TextWriter tw = new StreamWriter(fs);                
					tw.WriteLine("<?xml version=\"1.0\"?>");
					tw.WriteLine("<Messages>");
					tw.WriteLine("</Messages>");                
					tw.Close();
					fs.Close();
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
			}


		
            XmlDocument doc = new XmlDocument();


			doc.Load(fs);

            XmlNode node = doc.SelectSingleNode("Messages");
            
            
            XmlNode newSub = doc.CreateNode(XmlNodeType.Element, "Message", null);
            XmlAttribute xa = doc.CreateAttribute("id");
            xa.Value = _msg.Id.ToString();
            newSub.Attributes.Append(xa);
            node.AppendChild(newSub);


            XmlNode newSubValue = doc.CreateNode(XmlNodeType.Element, "Value", null);
            newSubValue.InnerText = _msg.Message;
            newSub.AppendChild(newSubValue);
          

            XmlNode newSubSender = doc.CreateNode(XmlNodeType.Element, "Sender", null);
            newSubSender.InnerText = _msg.Sender;
            newSub.AppendChild(newSubSender);
           

            XmlNode newSubDateTime = doc.CreateNode(XmlNodeType.Element, "DateTime", null);
            newSubDateTime.InnerText = _msg.ActionDate.ToString("G");
            newSub.AppendChild(newSubDateTime);
           

            

            XmlNode newSubStatus = doc.CreateNode(XmlNodeType.Element, "Status", null);
            newSubStatus.InnerText = _msg.Status.ToString();
            newSub.AppendChild(newSubStatus);

            XmlNode newArrivalDate = doc.CreateNode(XmlNodeType.Element, "ArrivalDate", null);
            newArrivalDate.InnerText = _msg.ArrivalDate.ToString("G");
            newSub.AppendChild(newArrivalDate);
           
			fs.Close ();

			if (isExternalStorageAvailable ()) {
				File.Delete (fileName);

            
				XmlTextWriter writer = new XmlTextWriter (fileName, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();                     
			} else {
				fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
				XmlTextWriter writer = new XmlTextWriter (fs, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();   
				fs.Close ();  
			}
        }

		/// <summary>
		/// Saves list message into corresonding XML file depending on message type.
		/// </summary>
		/// <param name="_msg"> instance of TextMessage</param>
		/// <param name="_nMessageType">Type of message: 1 - sent message, 2 - received message, 3 - draft message</param>
        public void saveMessages(List<TextMessage> _msgs, int _nMessageType)
        {
			System.IO.Stream fs = null;


			String fileName = "sent_messages.xml";

			if (_nMessageType == 2)
				fileName = "received_messages.xml";
			else if (_nMessageType == 3)
				fileName = "draft_messages.xml";

			if (isExternalStorageAvailable ()) {
				String storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

				if (!Directory.Exists (storagePath + "/DTMD"))
					Directory.CreateDirectory (storagePath + "/DTMD");					 

				fileName = storagePath + "/DTMD/" + fileName;
				File.Delete(fileName);
				              
				TextWriter tw = new StreamWriter (fileName);                
				tw.WriteLine ("<?xml version=\"1.0\"?>");
				tw.WriteLine ("<Messages>");
				tw.WriteLine ("</Messages>");                
				tw.Close ();


				fs = new FileStream (fileName, FileMode.Open);
			} else {
				try
				{
					fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
					TextWriter tw = new StreamWriter(fs);                
					tw.WriteLine("<?xml version=\"1.0\"?>");
					tw.WriteLine("<Messages>");
					tw.WriteLine("</Messages>");                
					tw.Close();
					fs.Close();
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
				catch (Exception e)
				{ 

				}
			}


            XmlDocument doc = new XmlDocument();
            


			doc.Load(fs);
            XmlNode node = doc.SelectSingleNode("Messages");

            for (int i = 0; i < _msgs.Count; i++)
            {
                TextMessage _msg = _msgs[i];
                
                XmlNode newSub = doc.CreateNode(XmlNodeType.Element, "Message", null);
                XmlAttribute xa = doc.CreateAttribute("id");
                xa.Value = _msg.Id.ToString();
                newSub.Attributes.Append(xa);
                node.AppendChild(newSub);


                XmlNode newSubValue = doc.CreateNode(XmlNodeType.Element, "Value", null);
                newSubValue.InnerText = _msg.Message;
                newSub.AppendChild(newSubValue);


                XmlNode newSubSender = doc.CreateNode(XmlNodeType.Element, "Sender", null);
                newSubSender.InnerText = _msg.Sender;
                newSub.AppendChild(newSubSender);


                XmlNode newSubDateTime = doc.CreateNode(XmlNodeType.Element, "DateTime", null);
                newSubDateTime.InnerText = _msg.ActionDate.ToString("G");
                newSub.AppendChild(newSubDateTime);


              

                XmlNode newSubStatus = doc.CreateNode(XmlNodeType.Element, "Status", null);
                newSubStatus.InnerText = _msg.Status.ToString();
                newSub.AppendChild(newSubStatus);

                XmlNode newArrivalDate = doc.CreateNode(XmlNodeType.Element, "ArrivalDate", null);
                newArrivalDate.InnerText = _msg.ArrivalDate.ToString("G");
                newSub.AppendChild(newArrivalDate);              
            }

			fs.Close ();

			if (isExternalStorageAvailable ()) {
				File.Delete (fileName);


				XmlTextWriter writer = new XmlTextWriter (fileName, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();                     
			} else {
				fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
				XmlTextWriter writer = new XmlTextWriter (fs, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();   
				fs.Close ();  
			}


        
        }


		/// <summary>
		/// Update the message in corresonding XML file depending on message type.
		/// </summary>
		/// <param name="_msg"> instance of TextMessage</param>
		/// <param name="_nMessageType">Type of message: 1 - sent message, 2 - received message, 3 - draft message</param>
        public void updateMessage(TextMessage _msg, int _nMessageType)
        {

			System.IO.Stream fs = null;


			String fileName = "sent_messages.xml";

			if (_nMessageType == 2)
				fileName = "received_messages.xml";
			else if (_nMessageType == 3)
				fileName = "draft_messages.xml";



			if (isExternalStorageAvailable ()) {
				String storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

				if (!Directory.Exists (storagePath + "/DTMD"))
					Directory.CreateDirectory (storagePath + "/DTMD");					 

				fileName = storagePath + "/DTMD/" + fileName;

				if (!File.Exists (fileName)) {                
					TextWriter tw = new StreamWriter (fileName);                
					tw.WriteLine ("<?xml version=\"1.0\"?>");
					tw.WriteLine ("<Messages>");
					tw.WriteLine ("</Messages>");                
					tw.Close ();
				}

				fs = new FileStream (fileName, FileMode.Open);
			} else {
				try
				{
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
				catch (Exception e)
				{ 
					fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
					TextWriter tw = new StreamWriter(fs);                
					tw.WriteLine("<?xml version=\"1.0\"?>");
					tw.WriteLine("<Messages>");
					tw.WriteLine("</Messages>");                
					tw.Close();
					fs.Close();
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
			}


            XmlDocument doc = new XmlDocument();

            
			doc.Load(fs);

            XmlNode node = doc.SelectSingleNode("Messages");

            XmlNode nodeToUpdate = node.SelectSingleNode("descendant::Message[@id='" + _msg.Id.ToString() + "']");
            if (nodeToUpdate != null)
                node.RemoveChild(nodeToUpdate);

            XmlNode newSub = doc.CreateNode(XmlNodeType.Element, "Message", null);
            XmlAttribute xa = doc.CreateAttribute("id");
            xa.Value = _msg.Id.ToString();
            newSub.Attributes.Append(xa);
            node.AppendChild(newSub);


            XmlNode newSubValue = doc.CreateNode(XmlNodeType.Element, "Value", null);
            newSubValue.InnerText = _msg.Message;
            newSub.AppendChild(newSubValue);


            XmlNode newSubSender = doc.CreateNode(XmlNodeType.Element, "Sender", null);
            newSubSender.InnerText = _msg.Sender;
            newSub.AppendChild(newSubSender);


            XmlNode newSubDateTime = doc.CreateNode(XmlNodeType.Element, "DateTime", null);
            newSubDateTime.InnerText = _msg.ActionDate.ToString("G");
            newSub.AppendChild(newSubDateTime);


           

            XmlNode newSubStatus = doc.CreateNode(XmlNodeType.Element, "Status", null);
            newSubStatus.InnerText = _msg.Status.ToString();
            newSub.AppendChild(newSubStatus);

            XmlNode newArrivalDate = doc.CreateNode(XmlNodeType.Element, "ArrivalDate", null);
            newArrivalDate.InnerText = _msg.ArrivalDate.ToString("G");
            newSub.AppendChild(newArrivalDate);


			fs.Close ();

			if (isExternalStorageAvailable ()) {
				File.Delete (fileName);


				XmlTextWriter writer = new XmlTextWriter (fileName, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();                     
			} else {
				fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
				XmlTextWriter writer = new XmlTextWriter (fs, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();   
				fs.Close ();  
			}


        }

		/// <summary>
		/// Delete message from corresonding XML file depending on message type.
		/// </summary>
		/// <param name="_msg"> instance of TextMessage</param>
		/// <param name="_nMessageType">Type of message: 1 - sent message, 2 - received message, 3 - draft message</param>
        public void deleteMessage(TextMessage _msg, int _nMessageType)
        {
			System.IO.Stream fs = null;


			String fileName = "sent_messages.xml";

			if (_nMessageType == 2)
				fileName = "received_messages.xml";
			else if (_nMessageType == 3)
				fileName = "draft_messages.xml";



			if (isExternalStorageAvailable ()) {
				String storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

				if (!Directory.Exists (storagePath + "/DTMD"))
					Directory.CreateDirectory (storagePath + "/DTMD");					 

				fileName = storagePath + "/DTMD/" + fileName;

				if (!File.Exists (fileName)) {                
					TextWriter tw = new StreamWriter (fileName);                
					tw.WriteLine ("<?xml version=\"1.0\"?>");
					tw.WriteLine ("<Messages>");
					tw.WriteLine ("</Messages>");                
					tw.Close ();
				}

				fs = new FileStream (fileName, FileMode.Open);
			} else {
				try
				{
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
				catch (Exception e)
				{ 
					fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
					TextWriter tw = new StreamWriter(fs);                
					tw.WriteLine("<?xml version=\"1.0\"?>");
					tw.WriteLine("<Messages>");
					tw.WriteLine("</Messages>");                
					tw.Close();
					fs.Close();
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
			}

            XmlDocument doc = new XmlDocument();


			doc.Load(fs);

            XmlNode node = doc.SelectSingleNode("Messages");

            XmlNode nodeToUpdate = node.SelectSingleNode("descendant::Message[@id='" + _msg.Id.ToString() + "']");
            if (nodeToUpdate != null)
                node.RemoveChild(nodeToUpdate);            

			fs.Close ();

			if (isExternalStorageAvailable ()) {
				File.Delete (fileName);


				XmlTextWriter writer = new XmlTextWriter (fileName, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();                     
			} else {
				fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
				XmlTextWriter writer = new XmlTextWriter (fs, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();   
				fs.Close ();  
			}

		

            if (_nMessageType == TextMessage.MSG_OUTBOX)
            {
                if (_msg.Status == TextMessage.STATUS_TOBESENT)
				{
                    ApplicationData.Instance.setOutboxIndicator(ApplicationData.Instance.getOutboxIndicator() - 1);

				}
            }
            else if (_nMessageType == TextMessage.MSG_INBOX)
            {
                if (_msg.Status == TextMessage.STATUS_UNREAD)
				{
                    ApplicationData.Instance.setInboxIndicator(ApplicationData.Instance.getInboxIndicator() - 1);

            	}
        	}
		}

        public void deleteAllMessage(int _nMessageType)
        {
			System.IO.Stream fs = null;


			String fileName = "sent_messages.xml";

			if (_nMessageType == 2)
				fileName = "received_messages.xml";
			else if (_nMessageType == 3)
				fileName = "draft_messages.xml";



			if (isExternalStorageAvailable ()) {
				String storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

				if (!Directory.Exists (storagePath + "/DTMD"))
					Directory.CreateDirectory (storagePath + "/DTMD");					 

				fileName = storagePath + "/DTMD/" + fileName;

				if (!File.Exists (fileName)) {                
					TextWriter tw = new StreamWriter (fileName);                
					tw.WriteLine ("<?xml version=\"1.0\"?>");
					tw.WriteLine ("<Messages>");
					tw.WriteLine ("</Messages>");                
					tw.Close ();
				}

				fs = new FileStream (fileName, FileMode.Open);
			} else {
				try
				{
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
				catch (Exception e)
				{ 
					fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
					TextWriter tw = new StreamWriter(fs);                
					tw.WriteLine("<?xml version=\"1.0\"?>");
					tw.WriteLine("<Messages>");
					tw.WriteLine("</Messages>");                
					tw.Close();
					fs.Close();
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
			}
			

            XmlDocument doc = new XmlDocument();
		

			doc.Load(fs);

            XmlNode node = doc.SelectSingleNode("Messages");

            node.RemoveAll();

			fs.Close ();

			if (isExternalStorageAvailable ()) {
				File.Delete (fileName);


				XmlTextWriter writer = new XmlTextWriter (fileName, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();                     
			} else {
				fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
				XmlTextWriter writer = new XmlTextWriter (fs, null);
				writer.Formatting = Formatting.Indented;
				doc.Save (writer);
				writer.Close ();   
				fs.Close ();  
			}

			

            if (_nMessageType == TextMessage.MSG_OUTBOX)
            {
                
                 ApplicationData.Instance.setOutboxIndicator(0);
				
            }
            else if (_nMessageType == TextMessage.MSG_INBOX)
            {
              
                    ApplicationData.Instance.setInboxIndicator(0);
				
            }
        }

			/// <summary>
			/// Loads corresponding type of messages
			/// </summary>
			/// <param name="_nMessageType">Type of message: 1 - sent message, 2 - received message, 3 - draft message</param>
        public List<TextMessage> loadMessages(int _nType)
        {
            List<TextMessage> list = new List<TextMessage>();

			System.IO.Stream fs = null;


			String fileName = "sent_messages.xml";

			if (_nType == 2)
				fileName = "received_messages.xml";
			else if (_nType == 3)
				fileName = "draft_messages.xml";



			if (isExternalStorageAvailable ()) {
				String storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

				if (!Directory.Exists (storagePath + "/DTMD"))
					Directory.CreateDirectory (storagePath + "/DTMD");					 

				fileName = storagePath + "/DTMD/" + fileName;

				if (!File.Exists (fileName)) {                
					TextWriter tw = new StreamWriter (fileName);                
					tw.WriteLine ("<?xml version=\"1.0\"?>");
					tw.WriteLine ("<Messages>");
					tw.WriteLine ("</Messages>");                
					tw.Close ();
				}

				fs = new FileStream (fileName, FileMode.Open);
			} else {
				try
				{
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
				catch (Exception e)
				{ 
					fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
					TextWriter tw = new StreamWriter(fs);                
					tw.WriteLine("<?xml version=\"1.0\"?>");
					tw.WriteLine("<Messages>");
					tw.WriteLine("</Messages>");                
					tw.Close();
					fs.Close();
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
			}

			
            TextMessage oneMessage = null;
            String strCurrentNode = "";

			

            try
            {
				using (XmlReader reader = XmlReader.Create(fs))
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "Message")
                                {
                                    if (oneMessage != null)
                                        list.Add(oneMessage);
     
                                    oneMessage = new TextMessage();
                                    oneMessage.Id = Int32.Parse(reader.GetAttribute("id"));
                                }

                                strCurrentNode = reader.Name;
                                
                                break;
                            case XmlNodeType.Text:
                                if (strCurrentNode == "Value")
                                {
                                    oneMessage.Message = reader.Value.Trim();
                                }
                                else if (strCurrentNode == "Sender")
                                {
                                    oneMessage.Sender = reader.Value.Trim();
                                }
                                else if (strCurrentNode == "DateTime")
                                {
                                    oneMessage.ActionDate = DateTime.Parse(reader.Value.Trim());
                                }                                
                                else if (strCurrentNode == "Status")
                                {
                                    oneMessage.Status = Int32.Parse(reader.Value.Trim());
                                }
                                else if (strCurrentNode == "ArrivalDate")
                                {
                                    oneMessage.ArrivalDate = DateTime.Parse(reader.Value.Trim());
                                }                                

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
            { 
				fs.Close ();
			}
			fs.Close ();

            if (oneMessage != null)
                list.Add(oneMessage);

            return list;
        
        }

        public String getCurrentActionCode()
        {
            String strCode = "";

           
            strCode += DateTime.Now.ToString("yyyyMMddHHmmssfff");

            return strCode;

        }
		
		/// <summary>
			/// Creaye a request to send text message and add it to the message queue.
		/// </summary>
		public void sendTextMessage(TextMessage _textMessage)
        { 
        
            if ((_textMessage != null) && (_textMessage.Message != ""))
            {
                DealtisMessage msg = new DealtisMessage();

                msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
				msg.setActionCode(_textMessage.Id.ToString());
                msg.setActionPriority(3);
                msg.setActionStatus(DealtisMessage.STATUS_NEW);
                msg.setActionType(DealtisMessage.ACTION_SENDMSG);
				msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
                msg.setComments("");
				msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
                msg.setMessageDate(DateTime.Now);
                msg.getParameters().Add("TEXT_MSG", _textMessage.Message);
				msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
				msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
				msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
				msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());
               
				ApplicationData.Instance.getServerMessageQueue().Add(msg);
                

                saveMessage(_textMessage, 1);
            }
        
			}

		/// <summary>
			/// Creaye a request to update received message state into red and add it to the message queue.
		/// </summary>		
        public void setTextMessageRead(int _msgID)
        {
                DealtisMessage msg = new DealtisMessage();

                msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
                msg.setActionCode(getCurrentActionCode());
                msg.setActionPriority(3);
                msg.setActionStatus(DealtisMessage.STATUS_NEW);
                msg.setActionType(DealtisMessage.ACTION_SETREAD);
				msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
                msg.setComments("");
				msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
                msg.setMessageDate(DateTime.Now);
                msg.getParameters().Add("MSGID", _msgID.ToString());
				msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
				msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
				msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
				msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());

				ApplicationData.Instance.getServerMessageQueue().Add(msg);
                
        }
		
		/// <summary>
		/// Creaye a request to update outbox list and add it to the message queue.
		/// </summary>	
        public List<TextMessage> updateOutboxMessageList(List<TextMessage> currentMsgs, List<TextMessage> txMsgs)
        {
            int Indicator = 0;

            for (int i = 0; i < currentMsgs.Count; i++)
            {               
                TextMessage oneMsg = currentMsgs[i];
                bool bFound = false;

                for (int j = 0; j < txMsgs.Count; j++)
                {
                    TextMessage oneTxMsg = txMsgs[j];

                    if (oneMsg.Message == oneTxMsg.Message)
                    {
                        oneMsg.ArrivalDate = oneTxMsg.ArrivalDate;
                        oneMsg.Status = oneTxMsg.Status;
                        bFound = true;
                        break;
                    }
                }
                if ((!bFound)&&(oneMsg.Status == TextMessage.STATUS_SENT))
                    oneMsg.Status = TextMessage.STATUS_TREATED;

                if (oneMsg.Status == TextMessage.STATUS_TOBESENT)
                    Indicator++;
            }

            ApplicationData.Instance.setOutboxIndicator(Indicator);
			

            return currentMsgs;               
        }
		
		/// <summary>
			/// Creaye a request to update inbox list and add it to the message queue.
		/// </summary>	
        public List<TextMessage> updateInboxMessageList(List<TextMessage> currentMsgs, List<TextMessage> txMsgs)
        {
            List<TextMessage> newMessages = new List<TextMessage>();
            for (int i = 0; i < txMsgs.Count; i++)
            {
                TextMessage oneTxMsg = txMsgs[i];
                bool bFound = false;

                for (int j = 0; j < currentMsgs.Count; j++)
                {
                    TextMessage oneMsg = currentMsgs[j];

                    if (oneMsg.Id == oneTxMsg.Id)
                    {
                        oneMsg.Status = oneTxMsg.Status;
                        bFound = true;
                       
                        break;
                    }
                }
                if (!bFound)
                    newMessages.Add(oneTxMsg);
            }

            if (newMessages.Count > 0)
            { 
                for (int j = 0; j < newMessages.Count; j++)
                {
                    currentMsgs.Add(newMessages[j]);
                }
            }

            int Indicator = 0;
            for (int j = 0; j < currentMsgs.Count; j++)
            {
                TextMessage oneMsg = currentMsgs[j];
                if (oneMsg.Status != TextMessage.STATUS_READ)
                    Indicator++;
            }

			if (ApplicationData.Instance.getInboxIndicator () < Indicator)
				alert ();

            ApplicationData.Instance.setInboxIndicator(Indicator);
			

            return currentMsgs;
        }
		
		/// <summary>
		/// Creaye a request to load inbox message list and add it to the message queue.
		/// </summary>	
        public void loadInboxMessages()
        {
            DealtisMessage msg = new DealtisMessage();

            msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setActionCode(getCurrentActionCode());
            msg.setActionPriority(7);
            msg.setActionStatus(DealtisMessage.STATUS_NEW);
            msg.setActionType(DealtisMessage.ACTION_INBOX);
			msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setComments("");
			msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
            msg.setMessageDate(DateTime.Now);
			msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
			msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
			msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
			msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());
           
			ApplicationData.Instance.getServerMessageQueue().Add(msg);
                                         
        
        }

		/// <summary>
		/// Creaye a request to load outbox message list and add it to the message queue.
		/// </summary>	
        public void loadOutboxMessages()
        {
            DealtisMessage msg = new DealtisMessage();

            msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setActionCode(getCurrentActionCode());
            msg.setActionPriority(7);
            msg.setActionStatus(DealtisMessage.STATUS_NEW);
            msg.setActionType(DealtisMessage.ACTION_OUTBOX);
			msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setComments("");
			msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
            msg.setMessageDate(DateTime.Now);
			msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
			msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
			msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
			msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());

			ApplicationData.Instance.getServerMessageQueue().Add (msg);
            
        }


        public void setInboxMessagesRead(int _msgID, List<TextMessage> _inbox)
        {
            for (int j = 0; j < _inbox.Count; j++)
            {
                TextMessage oneMsg = _inbox[j];
                if (oneMsg.Id == _msgID)
                {
                    oneMsg.Status = TextMessage.STATUS_READ;
                    break;
                }
            }
        }

		public void setOutboxMessagesSend(int _msgID, List<TextMessage> _outbox)
		{
			for (int j = 0; j < _outbox.Count; j++)
			{
				TextMessage oneMsg = _outbox[j];
				if (oneMsg.Id == _msgID)
				{
					oneMsg.Status = TextMessage.STATUS_SENT;
					break;
				}
			}
		}

        public void initIndicators()
        {
            List<TextMessage> existingMessages = loadMessages(1);
            int indicator = 0;
            for (int i = 0; i < existingMessages.Count; i++)
            {
                TextMessage msg = existingMessages[i];
                if (msg.Status == TextMessage.STATUS_TOBESENT)
                    indicator++;
            }
            ApplicationData.Instance.setOutboxIndicator(indicator);

            existingMessages = loadMessages(2);
            indicator = 0;
            for (int i = 0; i < existingMessages.Count; i++)
            {
                TextMessage msg = existingMessages[i];
                if (msg.Status != TextMessage.STATUS_READ)
                    indicator++;
            }
            ApplicationData.Instance.setInboxIndicator(indicator);
			//UpdateBadges(this,null);
            
        }

		/// <summary>
			/// Creaye a request to send current GPS possition and add it to the message queue.
		/// </summary>
        public void sendGPSPossition(double _Latitude, double _Longitude)
        {
            DealtisMessage msg = new DealtisMessage();

            msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setActionCode(getCurrentActionCode());
            msg.setActionPriority(6);
            msg.setActionStatus(DealtisMessage.STATUS_NEW);
            msg.setActionType(DealtisMessage.ACTION_SENDPOSSITION);
			msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setComments("");
			msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
            msg.setMessageDate(DateTime.Now);
			msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
			msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
			msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
            msg.getParameters().Add("LATITUDE", _Latitude.ToString());
            msg.getParameters().Add("LONGITUDE", _Longitude.ToString());
			msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());

			ApplicationData.Instance.getServerMessageQueue().Add(msg);
            

        }

        public bool loadMenus()
        {
            ApplicationData.Instance.getMenuList().Clear();


            MenuCode oneMenu = null;
            String strCurrentNode = "";

            try
            {
                using (XmlReader reader = XmlReader.Create(ApplicationData.Instance.getApplicationPath() + "\\code_menu.xml"))
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "MENU")
                                    oneMenu = new MenuCode();

                                strCurrentNode = reader.Name;
                                break;
                            case XmlNodeType.Text:
                                if (strCurrentNode == "CODE_MENU")
                                {
                                    oneMenu.setGodeMenu(reader.Value.Trim());
                                }
                                else if (strCurrentNode == "BARECODE_MENU")
                                {
                                    oneMenu.setBarcodeMenu(reader.Value.Trim());
                                }
                                break;
                            case XmlNodeType.XmlDeclaration:
                            case XmlNodeType.ProcessingInstruction:
                            case XmlNodeType.Comment:
                                break;
                            case XmlNodeType.EndElement:
                                if (reader.Name == "MENU")
                                {
                                    ApplicationData.Instance.getMenuList().Add(oneMenu);
                                }
                                break;
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            { return false; }

            return true;
        }

        public String getMenuCode(String strBarcode)
        {
            MenuCode _menu = ApplicationData.Instance.getMenuList().Find(delegate(MenuCode mc) { return mc.getBarcodeMenu() == strBarcode; });

            if (_menu == null)
                return null;

            return _menu.getCodeMenu();
        }

        

		/// <summary>
		/// Creaye a request to load list of activities and add it to the message queue.
		/// </summary>
        public void loadActivities()
        {
            DealtisMessage msg = new DealtisMessage();

            msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setActionCode(getCurrentActionCode());
            msg.setActionPriority(2);
            msg.setActionStatus(DealtisMessage.STATUS_NEW);
            msg.setActionType(DealtisMessage.ACTION_LOADACTIVITIES);
			msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setComments("");
			msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
            msg.setMessageDate(DateTime.Now);
			msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
			msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
			msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
			msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());
            
			ApplicationData.Instance.getServerMessageQueue().Add(msg);

            
        }
		
		/// <summary>
		/// Creaye a request to start activity and add it to the message queue.
		/// </summary>
        public void StartActivity(int _activityID, int _kilometrs, double _Latitude, double _Longitude)
        {
            DealtisMessage msg = new DealtisMessage();

            msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setActionCode(getCurrentActionCode( ));
            msg.setActionPriority(3);
            msg.setActionStatus(DealtisMessage.STATUS_NEW);
            msg.setActionType(DealtisMessage.ACTION_STARTACTIVITY);
			msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setComments("");
			msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
            msg.setMessageDate(DateTime.Now);
			msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
			msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
			msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
			msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());
            msg.getParameters().Add("ACTIVITY_ID", _activityID.ToString());
            msg.getParameters().Add("KILOMETERS", _kilometrs.ToString());
            msg.getParameters().Add("LATITUDE", _Latitude.ToString());
            msg.getParameters().Add("LONGITUDE", _Longitude.ToString());

			ApplicationData.Instance.getServerMessageQueue().Add(msg);

            
        }
		
		/// <summary>
			/// Creaye a request to stop activity and add it to the message queue.
		/// </summary>
        public void StopActivity(int _activityID, double _Latitude, double _Longitude)
        {
            DealtisMessage msg = new DealtisMessage();

            msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setActionCode(getCurrentActionCode());
            msg.setActionPriority(3);
            msg.setActionStatus(DealtisMessage.STATUS_NEW);
            msg.setActionType(DealtisMessage.ACTION_STOPACTIVITY);
			msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setComments("");
			msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
            msg.setMessageDate(DateTime.Now);
			msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
			msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
			msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
			msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());
            msg.getParameters().Add("ACTIVITY_ID", _activityID.ToString());            
            msg.getParameters().Add("LATITUDE", _Latitude.ToString());
            msg.getParameters().Add("LONGITUDE", _Longitude.ToString());

			ApplicationData.Instance.getServerMessageQueue().Add(msg);
            
        }
		
		/// <summary>
		/// Creaye a request to load trip of day and add it to the message queue.
		/// </summary>
        public void loadTrip()
        {
            DealtisMessage msg = new DealtisMessage();

            msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setActionCode(getCurrentActionCode());
            msg.setActionPriority(2);
            msg.setActionStatus(DealtisMessage.STATUS_NEW);
            msg.setActionType(DealtisMessage.ACTION_LOADTRIP);
			msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setComments("");
			msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
            msg.setMessageDate(DateTime.Now);
			msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
			msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
			msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
			msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());

			ApplicationData.Instance.getServerMessageQueue().Add(msg);

            
        }

		/// <summary>
		/// Creaye a request to start a trip and add it to the message queue.
		/// </summary>
        public void startTrip()
        {
            if (ApplicationData.Instance.getCurrentTrip() == null)
                return;

            DealtisMessage msg = new DealtisMessage();

            msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setActionCode(getCurrentActionCode());
            msg.setActionPriority(3);
            msg.setActionStatus(DealtisMessage.STATUS_NEW);
            msg.setActionType(DealtisMessage.ACTION_STARTTRIP);
			msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setComments("");
			msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
            msg.setMessageDate(DateTime.Now);
			msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
			msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
			msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
			msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());
            msg.getParameters().Add("TRIP_ID", ApplicationData.Instance.getCurrentTrip().TripID);            


			ApplicationData.Instance.getServerMessageQueue().Add(msg);
            
        }

		/// <summary>
			/// Creaye a request to stop a trip and add it to the message queue.
		/// </summary>
        public void stopTrip()
        {
            if (ApplicationData.Instance.getCurrentTrip() == null)
                return;

            DealtisMessage msg = new DealtisMessage();

            msg.setUserCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setActionCode(getCurrentActionCode());
            msg.setActionPriority(3);
            msg.setActionStatus(DealtisMessage.STATUS_NEW);
            msg.setActionType(DealtisMessage.ACTION_STOPTRIP);
			msg.setAgancyCode(ApplicationData.Instance.getConfigurationModel().getUserName());
            msg.setComments("");
			msg.setConnectionType(ApplicationData.Instance.getConfigurationModel().getConnectionType());
            msg.setMessageDate(DateTime.Now);
			msg.getParameters().Add("TX_USER", ApplicationData.Instance.getConfigurationModel().getTxUserName());
			msg.getParameters().Add("TX_PASSWORD", ApplicationData.Instance.getConfigurationModel().getTxUserPassword());
			msg.getParameters().Add("TX_SYSTEMNR", ApplicationData.Instance.getConfigurationModel().getTxSystemNr());
			msg.getParameters().Add("LANGUAGE", ApplicationData.Instance.getConfigurationModel().getLanguage());
            msg.getParameters().Add("TRIP_ID", ApplicationData.Instance.getCurrentTrip().TripID);
            

			ApplicationData.Instance.getServerMessageQueue().Add(msg);

            
        }


		public double distance(double lat1, double lng1, double lat2, double lng2)
		{
			// Store the earth radius in meters
			double earthRadius = 6371 * 1000;
			// Compute and return the distance between the two locations
			double dLat = toRadians(lat2 - lat1);
			double dLng = toRadians(lng2 - lng1);
			double sindLat = Math.Sin(dLat / 2);
			double sindLng = Math.Sin(dLng / 2);
			double a = Math.Pow(sindLat, 2) + Math.Pow(sindLng, 2) * Math.Cos(toRadians(lat1)) * Math.Cos(toRadians(lat2));
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			double dist = earthRadius * c;
			// Return the computed distance
			return dist;
		}

		private double toRadians(double ang)
		{
			return ang * Math.PI / 180;
		}




    }

}
