using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
//using System.MessagingCE;
using System.IO;

namespace DMSvStandard
{
	/// <summary>
	/// This is server main thread, which is reponsible for processing all requests comming from application.
	/// </summary>
    class ServerMainThread
    {        
        private bool bStopThread = false;

		public ServerMainThread()
        {           
            bStopThread = false;
        }

        public bool isStopThread()
        { return bStopThread; }

        public void setStopThread(bool bStopThread)
        { this.bStopThread = bStopThread; }

        public void Run()
        {
            processMessages();
        }

        public void log(String szMsg)
        {
            // create a writer and open the file
            TextWriter tw = new StreamWriter(ApplicationData.Instance.getApplicationPath() +  "testlog.txt");

            // write a line of text to the file
            tw.WriteLine(DateTime.Now.ToShortTimeString() + " - " + szMsg);

            // close the stream
            tw.Close();
        
        }

        public bool isSchduleHourOK(DateTime lastDate, int scheduleHour, bool bCheckDate)
        {
            //return true;
            int curHour = DateTime.Now.Hour;

            if ((scheduleHour>=0)&&(curHour < scheduleHour))
                return false;

            if ((bCheckDate) && (lastDate.ToShortDateString() == DateTime.Now.ToShortDateString()))
                return false;

            return true;
                   
        }

		/// <summary>
		/// This method runs forever when the application is started and process requests from server queue depending from their priority and schedule.
		/// </summary>
        public void processMessages()
        {
            try
            {
                while (!bStopThread)
                {
                    //log("Running");
					while ((ApplicationData.Instance.getServerMessageQueue().Count > 0) && (!bStopThread))
                    {
                        // log("Has message");
						ApplicationData.Instance.getServerMessageQueue().Sort(delegate(DealtisMessage p1, DealtisMessage p2) { return p1.getActionPriority().CompareTo(p2.getActionPriority()); });

                        DealtisMessage oneMsg = null;

						for (int i = 0; i < ApplicationData.Instance.getServerMessageQueue().Count; i++)
                        {
							if (ApplicationData.Instance.getServerMessageQueue()[i].getActionStatus() != DealtisMessage.STATUS_INPORGRESS)
                            {
								oneMsg = ApplicationData.Instance.getServerMessageQueue()[i];
                                break;
                            }
                        }

                        if (oneMsg != null)
                        {

                            ServerActions.Instance.log("Select message of type - " + oneMsg.getActionType() + " for processing");


                            bool dailyTask = false;
                            bool scheduleOK = false;
                            if (oneMsg.getParameters().ContainsKey("DAILY"))
                            {
                                dailyTask = true;
                                int schHour = -1;
                                if (oneMsg.getParameters().ContainsKey("HOUR"))
                                {
                                    schHour = Convert.ToInt32(oneMsg.getParameters()["HOUR"]);
                                }

                                if (oneMsg.getActionStatus() == DealtisMessage.STATUS_NEW)
                                {
                                    if (isSchduleHourOK(oneMsg.getMessageDate(), schHour, false))
                                        scheduleOK = true;
                                }
                                else
                                {
                                    if (isSchduleHourOK(oneMsg.getMessageDate(), schHour, true))
                                        scheduleOK = true;
                                }

                            }

                            if ((oneMsg.getActionStatus() == DealtisMessage.STATUS_NEW) && (ServerData.Instance.showStatusChanges(oneMsg.getActionType())))
                            {


                                DealtisMessage response = new DealtisMessage();
                                response.setUserCode(oneMsg.getUserCode());
                                response.setActionCode(oneMsg.getActionCode());
                                response.setActionType(oneMsg.getActionType());
                                response.setActionPriority(oneMsg.getActionPriority());
                                response.setAgancyCode(oneMsg.getAgancyCode());
                                response.setConnectionType(oneMsg.getConnectionType());
                                response.setComments("");
                                response.setActionStatus(DealtisMessage.STATUS_QUEUED);

                                


								ApplicationData.Instance.getClientMessageQueue().AddItem(response);

								oneMsg.setActionStatus(DealtisMessage.STATUS_QUEUED);
                            }


                            if (((!dailyTask) || ((dailyTask) && (scheduleOK))) && (ServerActions.Instance.isConnectionAvailable(oneMsg.getConnectionType())))
                            {
                                ServerActions.Instance.log("Start processing message of type - " + oneMsg.getActionType());

                                if (ServerData.Instance.showStatusChanges(oneMsg.getActionType()))
                                {
                                    DealtisMessage response = new DealtisMessage();
                                    response.setUserCode(oneMsg.getUserCode());
                                    response.setActionCode(oneMsg.getActionCode());
                                    response.setActionType(oneMsg.getActionType());
                                    response.setActionPriority(oneMsg.getActionPriority());
                                    response.setAgancyCode(oneMsg.getAgancyCode());
                                    response.setConnectionType(oneMsg.getConnectionType());
                                    response.setComments("");
                                    response.setActionStatus(DealtisMessage.STATUS_INPORGRESS);

									ApplicationData.Instance.getClientMessageQueue().AddItem(response);
                                }

								oneMsg.setActionStatus(DealtisMessage.STATUS_INPORGRESS);


                                ThreadStartWraper ths = new ThreadStartWraper(oneMsg);
                                Thread th = new Thread(new ThreadStart(ths.WorkerThreadFunction));
								th.Name = "DealtisServer Message Thread Action-" + oneMsg.getActionType();
                                
                                th.Start();
                                
                                removeMessagesFromQueue(oneMsg);

                                if (dailyTask)
                                {
                                    oneMsg.setMessageDate(DateTime.Now);
                                    oneMsg.setActionStatus(DealtisMessage.STATUS_EXECUTED);
									ApplicationData.Instance.getServerMessageQueue().Add(oneMsg);
                                }
                            }
                        }


                        if (!bStopThread)
                            Thread.Sleep(500);
                    }

                    
                    if (!bStopThread)
                        Thread.Sleep(3000);
                }

                bool stillRunning = false;

                while (true)
                {
                    stillRunning = false;
                    foreach (RequestProcessor oneProcess in ServerData.Instance.getRunningThreads())
                    {
                        if (!oneProcess.done)
                        {
                            stillRunning = true;
                            break;
                        }
                    }
                    if (stillRunning)
                        Thread.Sleep(1000);
                    else break;
                }

                ServerData.Instance.getRunningThreads().Clear();
            }
            catch (Exception ex)
            {
                ServerActions.Instance.log("Error in main loop - " + ex.Message);
                
            }

           

        }

        private void removeMessagesFromQueue(DealtisMessage _msg)
        {
            for (int i = 0; i < ApplicationData.Instance.getServerMessageQueue().Count; i++)
            {
				if (ApplicationData.Instance.getServerMessageQueue()[i].getActionCode() == _msg.getActionCode())
                {
					ApplicationData.Instance.getServerMessageQueue().RemoveAt(i);
                    break;
                }
            }
        
        }

    }
}
