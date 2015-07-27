using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;


namespace DMSvStandard
{
	/// <summary>
	/// This class is reponsible for multi-lingual support of application.
	/// It works with bundle files, and provides translation of UI messages depending on user's language.
	/// </summary>
	public class Translator
	{
		private Dictionary<string, string> bundleMap;
		
		public Translator(String _strLang, String appName)
		{
			bundleMap = new Dictionary<string, string>();


			String strFileName = "lang/" + appName + "_" + _strLang + ".len";
			
			
			try
			{
				using (Stream s = (Stream)MainActivity.getContext().Assets.Open(strFileName)/*new StreamReader(strFileName, System.Text.Encoding.UTF8)*/)
				{

					StreamReader sr = new StreamReader(s);
					String line;
					
					while ((line = sr.ReadLine()) != null)
					{
						int pos = line.IndexOf("=");
						if (pos >= 0) 
						{
							bundleMap.Add(line.Substring(0,pos).Trim(),line.Substring(pos+1,line.Length-pos-1).Trim());
						}                            
					}
				}
			}
			catch (Exception e)
			{
				
			}
			
		}
	
		public String translateMessage(String _msgKey)
		{
			if (bundleMap.ContainsKey(_msgKey))
			{
				return bundleMap[_msgKey];
			}
			return _msgKey;
			
			
		}

		
	}
}


