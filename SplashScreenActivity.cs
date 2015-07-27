using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Media;
using Android.Widget;
using Android.OS;
using AndroidHUD;
using System.Collections.Generic;
using System.Linq;
using DMSvStandard.ORM;
//using System.Net;
//using System.Security.Cryptography;
//using ZXing;
using System.IO;
using Android.Graphics;
using System.Data;
using SQLite;
using System.Json;
using System.Xml;
using Newtonsoft;
using System.Net;
using System.Xml.Linq;
//using Android.Support.V4.App;
using DMSvStandard;
using Xamarin;



using Environment = System.Environment;
using String = System.String;
using Newtonsoft.Json.Linq;
using Android.Net;

namespace DMSvStandard
{	
	

    using System.Threading;

    using Android.App;
   	
	public static class Data{

		public static string content;
		public static int countliv;
		public static int countram;
	}

	[Activity(Theme = "@style/Theme.Splash",MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);


							




			//Cr閍tion de la Database
			DBRepository dbr = new DBRepository ();
			var result = dbr.CreateDB ();



			//Cr閍tion des tables
			DBRepository dbrbis = new DBRepository ();
			var resultbis = dbrbis.CreateTable ();
			DBRepository dbrbiss = new DBRepository ();
			var resultbiss = dbrbiss.CreateTableStatut ();

			//Insights.Identify(Insights.Traits.GuestIdentifier, null);

			//DROP TABLE
		    //var resultte = dbrbis.DropTable ();

	

			//SUPP BDD PAS DU JOUR
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "orm.db3");
			var db = new SQLiteConnection(dbPath);
			//var resdel = db.ExecuteScalar<int>("DELETE FROM ToDoTask where dateBDD != '"+DateTime.Now.Day+"'");
			//"+DateTime.Now.Day+"



			Thread.Sleep (50); // Simulate a long loading process on app startup.
			//  AndHUD.Shared.Show(this, "Loading… 60%", 60);


			StartActivity (typeof(MainActivity));



		}

//		public void Threadapp()
//		{
//
//			//THREAD SEND DATA API
//			var timer = new System.Threading.Timer((o) =>
//				{ 	
//					string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
//						(Environment.SpecialFolder.Personal), "orm.db3");
//
//					var db = new SQLiteConnection(dbPath);
//					var table = db.Query<StatutLivraison> ("SELECT * FROM StatutLivraison");
//					
//					var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
//
//					var activeConnection = connectivityManager.ActiveNetworkInfo;
//					if ((activeConnection != null) && activeConnection.IsConnected) {
//						
//					foreach (var item in table) {
//						//API LIVRER OK
//						string _url = "http://.jeantettransport.com/api/livraison";
//						var webClient = new WebClient();
//						webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
//						//webClient.Encoding = Encoding.UTF8;
//
//						webClient.UploadString(_url,item.datajson);
//						Console.Out.WriteLine(">>>>>THREAD DATA SEND ....<<<<<");
//					}
//					//THREAD IMPORT DATA
//					Integdata();
//					DBRepository dbr = new DBRepository();
//					var resultdrop = dbr.DropTableStatut();
//					Console.Out.WriteLine(">>>>>THREAD NO DATA ....<<<<<"+DateTime.Now.Minute);
//					}else{
//						Console.Out.WriteLine(">>>>>NO CONNECTION WAIT ....<<<<<");
//					}
//
//				}, null, 0,120000);
//
//
//
//
//		}
//
//		public void Integdata(){
//			var rxcui = "198440";
//			var request = HttpWebRequest.Create(string.Format(@"http://.jeantettransport.com/api/commande?codechauffeur=SMART_ALEX&datecommande=20150603", rxcui));
//			request.ContentType = "application/json";
//			request.Method = "GET";
//
//			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
//			{
//				if (response.StatusCode != HttpStatusCode.OK)
//					Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
//				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
//				{
//					var content = reader.ReadToEnd();
//					if(string.IsNullOrWhiteSpace(content)) {
//						Console.Out.WriteLine("Response contained empty body...");
//					}
//					else {
//						Console.Out.WriteLine("Response Body: \r\n {0}", content);
//						Data.content = content;
//
//					
//					}
//
//
//				}
//			}
//			Data.countliv = 0;
//			Data.countram = 0;
//			//if(Data.content == "[]"){}else{alert ();}
//
//			JArray jsonVal = JArray.Parse(Data.content) as JArray;
//			var jsonarr = jsonVal;
//
//			foreach (var item in jsonarr)
//			{
//				DBRepository dbr = new DBRepository();
//				var resinteg = dbr.InsertData(Convert.ToString(item["codeLivraison"]),Convert.ToString(item["numCommande"]),Convert.ToString(item["refClient"]),Convert.ToString(item["nomPayeur"]),Convert.ToString(item["nomExpediteur"]),Convert.ToString(item["adresseExpediteur"]),Convert.ToString(item["villeExpediteur"]),Convert.ToString(item["CpExpediteur"]),Convert.ToString(item["dateExpe"]),Convert.ToString(item["nomClient"]),Convert.ToString(item["adresseLivraison"]),Convert.ToString(item["villeLivraison"]),Convert.ToString(item["CpLivraison"]),Convert.ToString(item["dateHeure"]),Convert.ToString(item["poids"]),Convert.ToString(item["nbrPallette"]),Convert.ToString(item["nbrColis"]),Convert.ToString(item["instrucLivraison"]),Convert.ToString(item["typeMission"]),Convert.ToString(item["typeSegment"]),Convert.ToString(item["groupage"]),Convert.ToString(item["ADRCom"]),Convert.ToString(item["ADRGrp"]),"0",Convert.ToString(item["CR"]),DateTime.Now.Day,Convert.ToString(item["Datemission"]),Convert.ToString(item["Ordremission"]),Convert.ToString(item["planDeTransport"]));
//
//				Console.WriteLine (item ["numCommande"]);
//				Console.WriteLine (resinteg);
//	
//			}
//			//SET BADGE
//			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
//				(Environment.SpecialFolder.Personal), "orm.db3");
//
//			var db = new SQLiteConnection(dbPath);
//			var tableliv = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV'");
//
//
//
//			foreach( var row in tableliv){
//				
//				Data.countliv++;
//			}
//
//
//			var tableram = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='C' AND typeSegment='RAM'");
//			foreach( var rows in tableram){
//				
//				Data.countram++;
//			}
//			ApplicationData.Instance.setLivraisonIndicator (Data.countliv);
//			ApplicationData.Instance.setEnlevementIndicator(Data.countram);
//
//
//		
//
//
//
//
//}
//
//		public void alert()
//		{
//
//			MediaPlayer _player;
//			_player = MediaPlayer.Create(this,Resource.Raw.beep2);
//			_player.Start();
//		}

    }
}