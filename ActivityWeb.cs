
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft;
using System.Net;
using System.Json;
using System.Xml;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;


namespace DMSvStandard
{	
	

	[Activity (Label = "ActivityWeb")]			
	public class ActivityWeb : Activity
	{
		protected override void OnCreate (Bundle bundle)


		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Webservice);






			var rxcui = "198440";
			var request = HttpWebRequest.Create(string.Format(@"http://10.1.2.70/MVCDMS/api/commande?codechauffeur=MHOUOT&datecommande=20140716", rxcui));
			request.ContentType = "application/json";
			request.Method = "GET";

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				if (response.StatusCode != HttpStatusCode.OK)
					Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					Data.content = reader.ReadToEnd();
					if(string.IsNullOrWhiteSpace(Data.content)) {
						Console.Out.WriteLine("Response contained empty body...");
					}
					else {
						Console.Out.WriteLine("Response Body: \r\n {0}", Data.content);
					}

//					TextView testweb = FindViewById<TextView> (Resource.Id.textView1);
//					testweb.Text= Data.content;






				}
			}



			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(Data.content);
			foreach (XmlElement x in xmlDoc.SelectNodes("ArrayOfCommande/"))
			{
				Console.Write(x.InnerXml);
				TextView testweb = FindViewById<TextView> (Resource.Id.textView1);
				testweb.Text = x.InnerXml;
			}



		}


	}
}

