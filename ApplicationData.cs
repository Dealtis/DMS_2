using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace DMSvStandard
{
	/// <summary>
	///  This is a singleton class which provides all data necessary for the application 
	/// </summary>
    class ApplicationData
    {
        private static ApplicationData instance;
        public static int DIRECTION_DELIVERY=1;
        public static int DIRECTION_PICKUP = 2;
        private String strLang = "";
        private Translator translator;
        public int MSG_TIMEOUT = 3;
        public static readonly Random randomGenerator = new Random();
        public String MENU_DELIVERY = "LIVRAISON";
        public String MENU_PICKUP = "ENLEVEMENT";
        public String MENU_NEWMSG = "NOUVEAU_MESSAGE";        
        public String MENU_INBOX = "RECUS";
        public String MENU_OUTBOX = "ENVOYES";
        public String MENU_DRAFT = "BROUILLONS";
        public String MENU_STATUS = "STATUS";
        public String MENU_CONFIG = "CONFIG";
		public bool userLogin=false;
		private bool adminLogin=false;
		public static string codemissionactive;
		public static string groupagemissionactive;
		public static string datedj;
		public static string mouth;
		public static string day;
		public static string hour;
		public static string minute;
		public static string seconde;
		public static string CR;
		public static bool User;
		public static int ithread;
		public static string dateimport;
		public static string GPS;


		public static string UserAndsoft;
		public static string UserTransic;

        private ApplicationData()
        {            
            translator = null;

            outboxIndicator = 0;
            inboxIndicator = 0;
			livraisonIndicator = 0;
            
        }

        public static ApplicationData Instance
        {
            get
			{
                if (instance == null)
                {
                    instance = new ApplicationData();
                }
                return instance;
            }
        }

		public bool isUserLogin()
		{
			return userLogin;
		}

		public void setUserLogin(bool _login)
		{
			userLogin = _login;
		}

		public bool isAdminLogin()
		{
			return adminLogin;
		}

		public void setAdminLogin(bool _login)
		{
			adminLogin = _login;
		}

		private int messageListOrdering = 0;

		public int getMessageListOrdering()
		{
			return messageListOrdering;
		}

		public void setMessageListOrdering(int _order)
		{
			messageListOrdering = _order;
		}
//
//		private DTMDQueue<DealtisMessage> clientMessageQueue = new DTMDQueue<DealtisMessage>();
//
//		public DTMDQueue<DealtisMessage> getClientMessageQueue ()
//		{
//			return clientMessageQueue;
//		}        
//
//		private List<DealtisMessage> serverMessageQueue = new List<DealtisMessage>();
//		
//		public List<DealtisMessage> getServerMessageQueue ()
//		{
//			return serverMessageQueue;
//		}        


		public Translator getTranslator()
        { return translator; }

        public void setTranslator(String _strLanguage, String strBundle)
        {
            translator = new Translator(_strLanguage, strBundle);
        }

       

        public string getApplicationName()
        {
            return Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
        }

        public string getApplicationPath()
        {
            int nNameLangth = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName).Length;

            String strFullPath = Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            return strFullPath.Substring(0, strFullPath.Length - nNameLangth);
        }

		private ConfigurationModel configurationModel = null;

		public void setConfigurationModel (ConfigurationModel _model)
		{
			configurationModel = _model;
		}

		public ConfigurationModel getConfigurationModel ()
		{
			if (configurationModel == null)
				return new ConfigurationModel();
			else
				return configurationModel;
  		}

        
		private int livraisonIndicator;


		public int getLivraisonIndicator()
		{ return livraisonIndicator; }

		public void setLivraisonIndicator (int _livraisonIndicator)
		{

			livraisonIndicator = _livraisonIndicator;
			System.Diagnostics.Debug.WriteLine("setLivraison - " + livraisonIndicator.ToString());
		}

		private int enlevementIndicator;


		public int getEnlevementIndicator()
		{ return enlevementIndicator; }

		public void setEnlevementIndicator (int _enlevementIndicator)
		{

			enlevementIndicator = _enlevementIndicator;
			System.Diagnostics.Debug.WriteLine("setEnlevement - " + enlevementIndicator.ToString());
		}

        private int inboxIndicator;

        public int getInboxIndicator()
        { return inboxIndicator; }

        public void setInboxIndicator (int _inboxIndicator)
		{
			if (inboxIndicator != _inboxIndicator) {
				inboxIndicator = _inboxIndicator;
				System.Diagnostics.Debug.WriteLine("setInbox - " + inboxIndicator.ToString());

			}
        }

        private int outboxIndicator;

        public int getOutboxIndicator()
        { return outboxIndicator; }

        public void setOutboxIndicator (int _outboxIndicator)
		{
			if (outboxIndicator != _outboxIndicator) {
				System.Diagnostics.Debug.WriteLine("setOutbox - " + _outboxIndicator.ToString());
				outboxIndicator = _outboxIndicator;

			}
        }


        private int direction;

        public int getDirection()
        { return direction; }

        public void setDirection(int _direction)
        {
            direction = _direction;
        }

        private String strBarcode = "";
        public String getBarcode() { return strBarcode; }
        public void setBarcode(String _barcode) { strBarcode = _barcode; }
        public void addBarcode(char _barcode) { strBarcode += _barcode; }

//        private List<MenuCode> listMenu;
//        public List<MenuCode> getMenuList() { return listMenu; }
//
//
//        
//
//        private Trip currentTrip;
//
//        public Trip getCurrentTrip()
//        { return currentTrip; }
//
//        public void setCurrentTrip(Trip _currentTrip)
//        {
//            currentTrip = _currentTrip;
//        }

		private ConfigurationModel tempConfigModel;
		
		public ConfigurationModel getTempConfigModel()
		{ return tempConfigModel; }
		
		public void setTempConfigModel(ConfigurationModel _m)
		{
			tempConfigModel = _m;
		}

//		private TextMessage currentTextMessage;
//		
//		public TextMessage CurrentTextMessage
//		{
//			get { return currentTextMessage; }
//			set { currentTextMessage = value; }
//		}


    }

    


}
