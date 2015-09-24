using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using DMSvStandard.ORM;
//using System.Net;
//using System.Security.Cryptography;
//using ZXing;
using System.Data;
using System.IO;
using System.Json;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Android.Graphics;
using Newtonsoft;
using SQLite;
//using Android.Support.V4.App;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using Android.App;
using Android.Locations;

using Android.Net;
using Android.OS;
using Android.Util;
using Android.Widget;
using DMSvStandard;





using Newtonsoft.Json.Linq;
using Xamarin;
using Environment = System.Environment;
using String = System.String;

namespace DMSvStandard
{	
	

    using System.Threading;

    using Android.App;
   	
	public static class Data{

		public static string content;
		public static string contentmsg;
		public static int countliv;
		public static int countram;
	}

	[Activity(Theme = "@style/Theme.Splash",MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity
    {	


	


        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");

			var db = new SQLiteConnection(dbPath);

							




			//Cr閍tion de la Database
			DBRepository dbr = new DBRepository ();
			var result = dbr.CreateDB ();



			//Cr閍tion des tables
			DBRepository dbrbis = new DBRepository ();
			var resultbis = dbrbis.CreateTable ();
			DBRepository dbrbiss = new DBRepository ();
			var resultbiss = dbrbiss.CreateTableStatut ();

			var rgtvtyh = dbrbiss.CreateTableUser();

			var resmessage = dbrbiss.CreateTableMessage();

			var resstatutmessage = dbrbiss.CreateTableStatutMessage ();

			//TEST MESSAGE
//			var msg1 = dbrbiss.InsertDataMessage("Exploitant","Ceci est un test de message exploitant #HYpe",0,DateTime.Now,1);
//			var msg2 =dbrbiss.InsertDataMessage("","Ceci est un test de message chauffeur #hoppla",2,DateTime.Now,2);

			// Test de connexion
			var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

			var activeConnection = connectivityManager.ActiveNetworkInfo;
			if ((activeConnection != null) && activeConnection.IsConnected) {
				//DELETE DE LA BASE
				//var resdrop = dbr.DropTableUser();
				//Si connexion download du xml


				try
				{
				string _url = "http://dms.jeantettransport.com/api/authen?chaufmdp=";
					var webClient = new WebClient ();
					webClient.Headers [HttpRequestHeader.ContentType] = "application/json";
					//webClient.Encoding = Encoding.UTF8;
					string user= "";
					user = webClient.DownloadString (_url);
					Console.Out.WriteLine ("");


					//Parse du xml et intégration dans base
					JArray jsonVal = JArray.Parse (user) as JArray;
					var jsonarr = jsonVal;

					foreach (var item in jsonarr) {

						var veriftable = dbr.verifusertable (Convert.ToString(item ["userandsoft"]),Convert.ToString(item ["usertransics"]),Convert.ToString(item ["mdpandsoft"]));
						if (veriftable != "0") {

						} else {
							var deletetable = db.Query<TableUser> ("DELETE FROM TableUser WHERE userandsoft = ? ",Convert.ToString(item ["userandsoft"]));
							var resinteg = dbr.InsertDataUser (Convert.ToString(item ["userandsoft"]),Convert.ToString(item ["usertransics"]),Convert.ToString(item ["mdpandsoft"]));
						}


						//Console.WriteLine (Convert.ToString(item ["userandsoft"])+Convert.ToString(item ["usertransics"]));
				}
				

				}
				catch (Exception e)
				{
					Insights.Report (e);
				}


			}else{
				//else prendre ancienne config et revenir cherche quaund connexion
				Console.Out.WriteLine ("Pas de connexion");
			}


					










			StartActivity (typeof(MainActivity));



		}



    }
}