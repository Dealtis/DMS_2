using System;
using System.Xml;
using System.IO;

namespace DMSvStandard
{
	/// <summary>
	///  This class contains the configuration of application. It allows to save configuration into XML and load it from XML.
	/// </summary>
	public class ConfigurationModel
	{
		public int inboxUpdateInterval=0;
		private String strLang = "FR";
		private String strCompanyName = "";
		private int nTimout = 0;     
		private String phoneNumber = "";
		private int connectionType = 0;
		private String strUserName = "";
		private String strUserBarcode = "";
		public int outboxUpdateInterval=0;
		private int gspPositionSending=0;
		private int activiyDriving=0;
		private int activiyWaiting=0;
		private int autoTrip=0;


		public ConfigurationModel ()
		{

		}

		public int getActivityDriving()
		{
			return activiyDriving;
		}
		public void setActivityDriving(int _activity)
		{
			activiyDriving = _activity;
		}

		public int getActivityWaiting()
		{
			return activiyWaiting;
		}
		public void setActivityWaiting(int _activity)
		{
			activiyWaiting = _activity;
		}

		public int getInboxUpdateInterval()
		{ return inboxUpdateInterval; }
		
		public void setInboxUpdateInterval(int _inboxUpdateInterval)
		{ inboxUpdateInterval = _inboxUpdateInterval; }
		
		public String getLanguage()
		{ return strLang; }
		
		public void setLanguage(String _strLang)
		{ strLang = _strLang; }


		
		public String getCompanyName()
		{ return strCompanyName; }
		
		public void setCompanyName(String _strCompanyName)
		{ strCompanyName = _strCompanyName; }
		
		public int getTimout()
		{ return nTimout; }
		

		
		public void setTimout(int _nTimout)
		{ nTimout = _nTimout; }

		public String getPhoneNumber()
		{
			if (phoneNumber.Length > 0)
			{
				if (phoneNumber.Substring(0, 1) == "+")
					return "00" + phoneNumber.Substring(1);
			}
			
			return phoneNumber;
			
		}
		public void setPhoneNumber(String _phoneNumber) { phoneNumber = _phoneNumber; }

		public int getConnectionType() { return connectionType; }
		public void setConnectionType(int connectionType) { this.connectionType = connectionType; }
		

		public String getUserName()
		{ return strUserName; }
		
		public void setUsarName(String _strName)
		{ strUserName = _strName;
			
		}
		
				
		public String getUserBarcode()
		{ return strUserBarcode; }
		
		public void setUserBarcode(String _strUserBarcode)
		{ strUserBarcode = _strUserBarcode; }
		


		
		public int getOutboxUpdateInterval()
		{ return outboxUpdateInterval; }
		
		public void setOutboxUpdateInterval(int _outboxUpdateInterval)
		{
			outboxUpdateInterval = _outboxUpdateInterval;
		}
		

		
		public int getGspPositionSending()
		{ return gspPositionSending; }
		
		public void setGspPositionSending(int _gspPositionSending)
		{
			gspPositionSending = _gspPositionSending;
		}

		public String txUserName="";
		
		public String getTxUserName()
		{ return txUserName; }
		
		public void setTxUserName(String _txUserName)
		{
			txUserName = _txUserName;

		}
		
		private String txUserPassword="";
		
		public String getTxUserPassword()
		{ return txUserPassword; }
		
		public void setTxUserPassword(String _txUserPassword)
		{
			txUserPassword = _txUserPassword;
		}
		
		private String txSystemNr="0";
		
		public String getTxSystemNr()
		{ return txSystemNr; }
		
		public void setTxSystemNr(String _txSystemNr)
		{
			txSystemNr = _txSystemNr;
		}

		public int getAutoTrip()
		{
			return autoTrip;
		}
		public void setAutoTrip(int _trip){
			autoTrip = _trip;
		}

		public ConfigurationModel clone()
		{
			ConfigurationModel _model = new ConfigurationModel();

			_model.inboxUpdateInterval=inboxUpdateInterval;
			_model.strLang = strLang;
			_model.strCompanyName = strCompanyName;
			_model.nTimout = nTimout;     
			_model.phoneNumber = phoneNumber;
			_model.connectionType = connectionType;
			_model.strUserName = strUserName;
			_model.strUserBarcode = strUserBarcode;
			_model.outboxUpdateInterval=outboxUpdateInterval;
			_model.gspPositionSending=gspPositionSending;

			_model.txSystemNr=txSystemNr;
			_model.txUserPassword=txUserPassword;
			_model.txUserName=txUserName;
			_model.activiyDriving = activiyDriving;
			_model.activiyWaiting = activiyWaiting;
			_model.autoTrip = autoTrip;

			return _model;
		}

		public bool isConfigurationDane()
		{
			if (inboxUpdateInterval <= 0)
				return false;

			if (nTimout <= 0)
				return false;


			if (strUserName == "")
				return false;

			if (strUserBarcode == "")
				return false;

			if (outboxUpdateInterval <= 0)
				return false;

			if (gspPositionSending <= 0)
				return false;

			if (txSystemNr == "")
				return false;

			if (txUserPassword == "")
				return false;

			if (txUserName == "")
				return false;

			if (activiyDriving <= 0)
				return false;

			if (activiyWaiting <= 0)
				return false;

			return true;

		}

		public void saveConfiguration()
		{
			XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);// Create the root element
			XmlElement root = doc.CreateElement("CONFIG");
			doc.AppendChild(root);
			

			XmlElement param4 = doc.CreateElement("COMPANY_NAME");
			param4.InnerText = getCompanyName();

			
		
			XmlElement param6 = doc.CreateElement("LANG");
			param6.InnerText = getLanguage();
		
			
			XmlElement param20 = doc.CreateElement("TIMEOUT");
			param20.InnerText = getTimout().ToString();
			

			root.AppendChild(param4);
		
			root.AppendChild(param6);
			root.AppendChild(param20);
			

			
			
			XmlElement param32 = doc.CreateElement("PHONE_NUMBER");
			param32.InnerText = getPhoneNumber();
			root.AppendChild(param32);
			

			
			XmlElement param17 = doc.CreateElement("CONNECTION_TYPE");
			param17.InnerText = getConnectionType().ToString();
			root.AppendChild(param17);
			
			XmlElement param41 = doc.CreateElement("INBOX_UPDATE");
			param41.InnerText = getInboxUpdateInterval().ToString();
			root.AppendChild(param41);

			XmlElement param42 = doc.CreateElement("OUTBOX_UPDATE");
			param42.InnerText = getOutboxUpdateInterval().ToString();
			root.AppendChild(param42);

			XmlElement param422 = doc.CreateElement("AUTO_TRIP");
			param422.InnerText = getAutoTrip().ToString();
			root.AppendChild(param422);
			
			XmlElement param43 = doc.CreateElement("USER_NAME");
			param43.InnerText = getUserName();
			root.AppendChild(param43);

			
			XmlElement param44 = doc.CreateElement("USER_BARCODE");
			param44.InnerText = getUserBarcode();
			root.AppendChild(param44);

			
			XmlElement param45 = doc.CreateElement("TX_USER");
			param45.InnerText = getTxUserName();
			root.AppendChild(param45);

			
			XmlElement param46 = doc.CreateElement("TX_PWD");
			param46.InnerText = getTxUserPassword();
			root.AppendChild(param46);

			
			XmlElement param47 = doc.CreateElement("TX_SYSTEM");
			param47.InnerText = getTxSystemNr();
			root.AppendChild(param47);

			
			XmlElement param48 = doc.CreateElement("GPS_SENDING");
			param48.InnerText = getGspPositionSending().ToString();
			root.AppendChild(param48);



			XmlElement param55 = doc.CreateElement("ACIVITY_DRIVING");
			param55.InnerText = activiyDriving.ToString();
			root.AppendChild(param55);

			XmlElement param56 = doc.CreateElement("ACIVITY_WAITING");
			param56.InnerText = activiyWaiting.ToString();
			root.AppendChild(param56);

			Stream f = MainActivity.getContext ().OpenFileOutput("Config_DTMD.XML", Android.Content.FileCreationMode.Private);
								
			XmlTextWriter writer = new XmlTextWriter (f, null);//ApplicationData.Instance.getApplicationPath() + ApplicationData.Instance.getApplicationName() + ".xml", null);
			writer.Formatting = Formatting.Indented;
			doc.Save(writer);
			writer.Close();

		}

		public bool loadConfiguration()
		{
			String strCurrentNode = "";


			try
			{
				Stream f = MainActivity.getContext ().OpenFileInput("Config_DTMD.XML");

				using (XmlReader reader = XmlReader.Create(f))//ApplicationData.Instance.getApplicationPath() + ApplicationData.Instance.getApplicationName() + ".xml"))
				{
					while (reader.Read())
					{
						switch (reader.NodeType)
						{
						case XmlNodeType.Element:
							strCurrentNode = reader.Name;
							break;
						case XmlNodeType.Text:

							if (strCurrentNode == "COMPANY_NAME")
							{
								setCompanyName(reader.Value.Trim());
							}
							else if (strCurrentNode == "LANG")
							{
								setLanguage(reader.Value.Trim());
							}
							else if (strCurrentNode == "TIMEOUT")
							{
								setTimout(Convert.ToInt32(reader.Value.Trim()));
							}
							else if (strCurrentNode == "CONNECTION_TYPE")
							{
								setConnectionType(Convert.ToInt32(reader.Value.Trim()));
							}
							else if (strCurrentNode == "PHONE_NUMBER")
							{
								setPhoneNumber(reader.Value.Trim());
							}
							else if (strCurrentNode == "INBOX_UPDATE")
							{
								setInboxUpdateInterval(Convert.ToInt32(reader.Value.Trim()));
							}
							else if (strCurrentNode == "OUTBOX_UPDATE")
							{
								setOutboxUpdateInterval(Convert.ToInt32(reader.Value.Trim()));
							}
							else if (strCurrentNode == "AUTO_TRIP")
							{
								setAutoTrip(Convert.ToInt32(reader.Value.Trim()));
							}
							else if (strCurrentNode == "USER_NAME")
							{
								setUsarName(reader.Value.Trim());

							}
							else if (strCurrentNode == "USER_BARCODE")
							{
								setUserBarcode(reader.Value.Trim());
							}
							else if (strCurrentNode == "TX_USER")
							{
								setTxUserName(reader.Value.Trim());
							}
							else if (strCurrentNode == "TX_PWD")
							{
								setTxUserPassword(reader.Value.Trim());
							}
							else if (strCurrentNode == "TX_SYSTEM")
							{
								setTxSystemNr(reader.Value.Trim());
							}
							else if (strCurrentNode == "GPS_SENDING")
							{
								setGspPositionSending(Convert.ToInt32(reader.Value.Trim()));
							}
							else if (strCurrentNode == "ACIVITY_DRIVING")
							{
								setActivityDriving(Convert.ToInt32(reader.Value.Trim()));
							}
							else if (strCurrentNode == "ACIVITY_WAITING")
							{
								setActivityWaiting(Convert.ToInt32(reader.Value.Trim()));
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
			catch (Exception e)
			{ return false; }
			
			
			
			return true;
		}

		public void setDefaults()
		{
			inboxUpdateInterval=1;
			strLang = "FR";
			strCompanyName = "Damaris";
			nTimout = 300;     
			phoneNumber = "";
			connectionType = 0;
			strUserName = "ROBERT";
			strUserBarcode = "USER";
			outboxUpdateInterval=2;
			gspPositionSending=1;
			txSystemNr="3820";
			txUserPassword="dealtis1049";
			txUserName="DEAL_MOB";
			activiyDriving = 128;
			activiyWaiting = 8;
			autoTrip = 0;
		}
	}
}

