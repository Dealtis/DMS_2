using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;


namespace DMSvStandard
{
	/// <summary>
	/// This is a singleton class which contains all data necessary for the server.
	/// </summary>
    class ServerData
    {
		private static ServerData instance;

        private List<RequestProcessor> runningThreads;

        public String TestIP = "www.google.com";

        private TransicsService.Service txService = null;


        private String txSessionID;


		private ServerData()
        {
           
            runningThreads = new List<RequestProcessor>();
        }

		public static ServerData Instance
        {
            get
            {
                if (instance == null)
                {
					instance = new ServerData();
                }
                return instance;
            }
        }

        public TransicsService.Service getTransicsService()
        {
            if (txService == null)
                txService = new TransicsService.Service();
			//TransicsService.
            return txService;
        }

        public String getTransicsSessionID(String _user, String _pwd, int _systemNr, String _lang)
        {
            while (isTxLocked())
                Thread.Sleep(300);

            if (((_user == null) || (_user.Length == 0)) ||
                ((_pwd == null) || (_pwd.Length == 0)) ||
                (_systemNr <= 0))
                return "";

            if (((txSessionID == null) || (txSessionID == ""))||
                ((_user != getTxUser()) || (_pwd != getTxPassword()) || (_systemNr != getTxSystemNr()) || (_lang != getTxLanguage())))
            {
                if ((txSessionID!=null)&&(txSessionID.Length > 0))
                    ServerActions.Instance.TransicsLogout(txSessionID);

                setTxUser(_user);
                setTxPassword(_pwd);
                setTxSystemNr(_systemNr);
                setTxLanguage(_lang);
                txSessionID = ServerActions.Instance.TransicsLogin();
            }

            return txSessionID;
        }

        public void removeCurrentSession()
        {
            txSessionID = "";
        }

       

        public string getApplicationPath()
        {
            int nNameLangth = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName).Length;

            String strFullPath = Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            return strFullPath.Substring(0, strFullPath.Length - nNameLangth);
        }

        public List<RequestProcessor> getRunningThreads()
        { return runningThreads; }

        private String txUser;

        public String getTxUser()
        { return txUser; }

        public void setTxUser(String _txUser)
        { txUser = _txUser; }

        private String txLanguage;

        public String getTxLanguage()
        { return txLanguage; }

        public void setTxLanguage(String _txLanguage)
        { txLanguage = _txLanguage; }

        private String txPassword;

        public String getTxPassword()
        { return txPassword; }

        public void setTxPassword(String _txPassword)
        { txPassword = _txPassword; }

        private int txSystemNr;

        public int getTxSystemNr()
        { return txSystemNr; }

        public void setTxSystemNr(int _txSystemNr)
        { txSystemNr = _txSystemNr; }

        public bool showStatusChanges(String _msgAction)
        {
            if (_msgAction == DealtisMessage.ACTION_SENDMSG)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_OUTBOX)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_INBOX)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_SETREAD)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_SENDPOSSITION)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_STARTACTIVITY)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_STOPACTIVITY)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_LOADACTIVITIES)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_LOADTRIP)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_STARTTRIP)
                return false;
            else if (_msgAction == DealtisMessage.ACTION_STOPTRIP)
                return false;

            return true;
        
        }

        private bool tXLock = false;

        public void setTxLock()
        {
            tXLock = true;
        }

        public void releaseTxLock()
        {
            tXLock = false;
        }

        public bool isTxLocked()
        { return tXLock; }

        private bool bDebugMode;

        public void setDebugMode(bool _bDebugMode)
        { bDebugMode = _bDebugMode; }

        public bool isDebugMode()
        { return bDebugMode; }

        private bool bMailNotify;

        public void setMailNotify(bool _bMailNotify)
        { bMailNotify = _bMailNotify; }

        public bool isMailNotify()
        { return bMailNotify; }

        private String strSmtpHost;

        public void setSmtpHost(String _strSmtpHost)
        { strSmtpHost = _strSmtpHost; }

        public String getSmtpHost()
        { return strSmtpHost; }

        private String strSmtpDomain;

        public void setSmtpDomain(String _strSmtpDomain)
        { strSmtpDomain = _strSmtpDomain; }

        public String getSmtpDomain()
        { return strSmtpDomain; }

        private String strSmtpUser;

        public void setSmtpUser(String _strSmtpUser)
        { strSmtpUser = _strSmtpUser; }

        public String getSmtpUser()
        { return strSmtpUser; }

        private String strSmtpPwd;

        public void setSmtpPwd(String _strSmtpPwd)
        { strSmtpPwd = _strSmtpPwd; }

        public String getSmtpPwd()
        { return strSmtpPwd; }

        private int strSmtpPort;

        public void setSmtpPort(int _strSmtpPort)
        { strSmtpPort = _strSmtpPort; }

        public int getSmtpPort()
        { return strSmtpPort; }

        private String strSmtpFrom;

        public void setSmtpFrom(String _strSmtpFrom)
        { strSmtpFrom = _strSmtpFrom; }

        public String getSmtpFrom()
        { return strSmtpFrom; }

        private String strSmtpTo;

        public void setSmtpTo(String _strSmtpTo)
        { strSmtpTo = _strSmtpTo; }

        public String getSmtpTo()
        { return strSmtpTo; }


        
    }
}
