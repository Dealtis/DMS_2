using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Android.App;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.Locations;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.OS;
using Android.Runtime;

using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Widget;
using AndroidHUD;

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;

using DMSvStandard;

using DMSvStandard.ORM;


using Newtonsoft;
using Newtonsoft.Json.Linq;
using SQLite;
using Xamarin;
using Environment = System.Environment;
using String = System.String;

namespace DMSvStandard
{
	/*	public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
	{
		public TrustAllCertificatePolicy() 
		{}

		public bool CheckValidationResult(ServicePoint sp, 
			X509Certificate cert,WebRequest req, int problem)
		{
			return true;
		}
	}*/

	[Activity (Label = "Menu",Theme = "@android:style/Theme.Black.NoTitleBar",ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : Activity, ILocationListener
	{
		


		TextView m_lblDelivery=null;
		TextView m_lblPeekup=null;
		TextView m_lblNewMsg=null;
		TextView m_lblInbox=null;
		TextView m_lblOutbox=null;
		TextView m_lblActivity=null;
		//TextView m_lblTrip=null;
		TextView m_lblConfig=null;
		TextView m_lblTitle=null;

		RelativeLayout m_deliveryBadge=null;
		RelativeLayout m_peekupBadge=null;
		RelativeLayout m_newMsgBadge=null;
		RelativeLayout m_inboxBadge=null;
		RelativeLayout m_outboxBadge=null;
		RelativeLayout m_activityBadge=null;
		RelativeLayout m_tripBadge=null;
		RelativeLayout m_configBadge=null;

		TextView m_deliveryBadgeText=null;
		TextView m_peekupBadgeText=null;
		TextView m_newMsgBadgeText=null;
		TextView m_inboxBadgeText=null;
		TextView m_outboxBadgeText=null;
		TextView m_activityBadgeText=null;
		TextView m_tripBadgeText=null;
		TextView m_configBadgeText=null;


		TextView _addressText;
		Location _currentLocation;
		LocationManager _locationManager;

		string _locationProvider;
		TextView _locationText;

		System.Timers.Timer indicatorTimer;

		private static MainActivity appContext;
		public bool loginCanceled = false;
		private static readonly int ButtonClickNotificationId = 1000;



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, c, ch, ssl) => true;



			// Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
			//ApplicationData.Instance.setUserLogin (false);

			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");

			var db = new SQLiteConnection(dbPath);

			DBRepository dbr = new DBRepository ();

			bool resdate = dbr.Selectlogone ();

			if(resdate == false){
				StartActivity(typeof(LoginActivity));
				Console.Out.Write (">>>>>>>>>>>>>>>>>>LOG FALSE<<<<<<<<<<<<<<<<<<<<<<");

			}
			Xamarin.Insights.Initialize("4845750bb6fdffe0e3bfaebe810ca335f0f87030", this);

			Insights.Identify(ApplicationData.UserAndsoft,"Name",ApplicationData.UserAndsoft);

			InitializeLocationManager ();
			bool selectlogin = dbr.Selectlogin (ApplicationData.UserAndsoft);
			DateTime Selectdatelog = dbr.Selectdatelog (ApplicationData.UserAndsoft);

			if ((selectlogin) && (ApplicationData.ithread == 0 )) {

				//ShowProgressDemo(progress => AndHUD.Shared.Show(this, null, progress, MaskType.Clear));
				//LANCEMENT THREAD
				Threadapp ();
				Console.Out.Write (">>>>>>>>>>>>>>>>>>THREAD START<<<<<<<<<<<<<<<<<<<<<<");
				ApplicationData.ithread++;
			}





			//SET BADGE

			Data.countliv = 0;
			Data.countram = 0;
			Data.countmess = 0;



			var tableliv = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV' AND Userandsoft = ?",ApplicationData.UserAndsoft);





			foreach( var row in tableliv){ 

				Data.countliv++;
			}


			var tableram = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='C' AND typeSegment='RAM' AND Userandsoft = ?",ApplicationData.UserAndsoft);
			foreach( var rows in tableram){

				Data.countram++;
			}

			var tablemess = db.Query<Message> ("SELECT * FROM Message WHERE statutMessage = 0 AND codeChauffeur=?",ApplicationData.UserAndsoft);



			foreach( var row in tablemess){

				Data.countmess++;
			}


			if (ApplicationData.Instance.getLivraisonIndicator () == Data.countliv)  {
				ImageView bggLiv = FindViewById<ImageView>(Resource.Id.bdgLiv);
				bggLiv.SetImageResource(Resource.Drawable.SBBadgeBGUP);
			}
			if (ApplicationData.Instance.getEnlevementIndicator ()  == Data.countram) {
				ImageView bggLiv = FindViewById<ImageView>(Resource.Id.bdgRam);
				bggLiv.SetImageResource(Resource.Drawable.SBBadgeBGUP);
				
			}


			var datedujour = DateTime.Today.DayOfWeek;

			Context context = this.ApplicationContext;
			var version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;



			appContext = this;

			// Get our button from the layout resource,
			// and attach an event to it

			m_lblNewMsg = FindViewById<TextView> (Resource.Id.lblButton3);
			//m_lblInbox = FindViewById<TextView> (Resource.Id.lblButton4);

//			m_lblActivity = FindViewById<TextView> (Resource.Id.lblButton6);
//			m_lblTrip = FindViewById<TextView> (Resource.Id.lblButton7);
			m_lblConfig = FindViewById<TextView> (Resource.Id.lblButton8);
			m_lblTitle = FindViewById<TextView> (Resource.Id.lblTitle);
			m_lblTitle.Text = ApplicationData.UserTransic+" "+version;
			//m_lblTitle.Text = String.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);
			Typeface tf = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaBold.ttf");
			m_lblTitle.SetTypeface(tf, TypefaceStyle.Normal);


			LinearLayout btn1 = FindViewById<LinearLayout> (Resource.Id.columnlayout1_1);
			btn1.Click += delegate { delivery_Click();	};

			btn1.LongClick += delegate {
				delivery_LongClick();
			};
			LinearLayout btn2 = FindViewById<LinearLayout> (Resource.Id.columnlayout1_2);
			btn2.Click += delegate { peekup_Click();	};

			LinearLayout btn3 = FindViewById<LinearLayout> (Resource.Id.columnlayout2_1);
			btn3.Click += delegate { chat_Click();	};

			//LinearLayout btn4 = FindViewById<LinearLayout> (Resource.Id.columnlayout2_2);
			//btn4.Click += delegate { inbox_Click();	};

			//LinearLayout btn5 = FindViewById<LinearLayout> (Resource.Id.columnlayout3_1);
			//btn5.Click += delegate { outbox_Click();	};

			LinearLayout btn8 = FindViewById<LinearLayout> (Resource.Id.columnlayout4_2);
			btn8.Click += delegate { config_Click();	};
			btn8.LongClick += delegate	{ config_LongClick();};

			m_deliveryBadge = FindViewById<RelativeLayout> (Resource.Id.deliveryBadge); 
			m_deliveryBadge.Visibility = ViewStates.Invisible;
			m_deliveryBadgeText = FindViewById<TextView> (Resource.Id.deliveryBadgeText);

			m_peekupBadge = FindViewById<RelativeLayout> (Resource.Id.peekupBadge);
			m_peekupBadge.Visibility = ViewStates.Invisible;
			m_peekupBadgeText = FindViewById<TextView> (Resource.Id.peekupBadgeText);

			m_newMsgBadge = FindViewById<RelativeLayout> (Resource.Id.newMsgBadge);
			m_newMsgBadge.Visibility = ViewStates.Invisible;
			m_newMsgBadgeText = FindViewById<TextView> (Resource.Id.newMsgBadgeText);



			ApplicationData.Instance.setLivraisonIndicator (Data.countliv);
			ApplicationData.Instance.setEnlevementIndicator(Data.countram);
			ApplicationData.Instance.setmessageIndicator(Data.countmess);
//
//			m_newMsgBadgeText.Text = Convert.ToString (Data.countmess);
//			m_deliveryBadgeText.Text= Convert.ToString (Data.countliv);
//			m_peekupBadgeText.Text= Convert.ToString (Data.countram);
			loginCanceled = false;





}




		protected override void OnResume()
		{
			base.OnResume();

			if (_locationProvider == "") {
				Toast.MakeText (this, "No location provider find", ToastLength.Long).Show ();
			} else {
				_locationManager.RequestLocationUpdates (_locationProvider, 0, 0, this);
				Console.Out.Write ("Listening for location updates using " + _locationProvider + ".");
			}
			//si valeur date de la base + de 12 h login

//			if ((!loginCanceled)&&(ApplicationData.Instance.getConfigurationModel ().isConfigurationDane ())&&(!ApplicationData.Instance.isUserLogin ())) {
//				loginCanceled = false;
//				StartActivity(typeof(LoginActivity));
//			}

			initView();
		}

		protected override void OnStop()
		{
			base.OnStop();


			if (indicatorTimer != null)
				indicatorTimer.Stop ();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy ();

		}



		public static MainActivity getContext()
		{
			return appContext;
		}

		protected void initView()
		{	

			Context context = this.ApplicationContext;
			var version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;

			m_lblTitle.Text = ApplicationData.UserAndsoft+" "+version;
			//m_lblTitle.Text = String.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);

			this.Title = "DMS";
//			m_lblDelivery.Text = ApplicationData.Instance.getTranslator ().translateMessage ("mainfrom.menuLivraison");
//			m_lblPeekup.Text = ApplicationData.Instance.getTranslator ().translateMessage ("mainfrom.menuEnlevement");
//			m_lblNewMsg.Text = ApplicationData.Instance.getTranslator ().translateMessage ("formmessages.menunew");
//			m_lblInbox.Text = ApplicationData.Instance.getTranslator ().translateMessage ("formmessages.menuinbox");
//			m_lblOutbox.Text = ApplicationData.Instance.getTranslator ().translateMessage ("formmessages.menusentbox");
//			m_lblConfig.Text = ApplicationData.Instance.getTranslator ().translateMessage ("mainfrom.menuconfig");

//			if (indicatorTimer != null)
//				indicatorTimer.Stop ();
//
//
//
//
//
			indicatorTimer = new System.Timers.Timer();
			indicatorTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnIndicatorTimerHandler);
			indicatorTimer.Interval = 1000;//ApplicationData.Instance.getConfigurationModel().getInboxUpdateInterval();
			indicatorTimer.Enabled = true;
			indicatorTimer.Start();

		}



		private void OnIndicatorTimerHandler(object source, System.Timers.ElapsedEventArgs args)
		{
//			if (ApplicationData.Instance.getOutboxIndicator () > 0) {
//				RunOnUiThread (() => m_outboxBadgeText.Text = ApplicationData.Instance.getOutboxIndicator().ToString());
//				RunOnUiThread (() => m_outboxBadge.Visibility = ViewStates.Visible);
//			} else {
//				RunOnUiThread (() => m_outboxBadge.Visibility = ViewStates.Invisible);
//			}
//
//			if (ApplicationData.Instance.getInboxIndicator () > 0) {
//				RunOnUiThread (() => m_inboxBadgeText.Text= ApplicationData.Instance.getInboxIndicator ().ToString());
//				RunOnUiThread (() => m_inboxBadge.Visibility = ViewStates.Visible);
//			} else {
//				RunOnUiThread (() => m_inboxBadge.Visibility = ViewStates.Invisible);
//			}

			if (ApplicationData.Instance.getLivraisonIndicator () > 0) {
				RunOnUiThread (() => m_deliveryBadgeText.Text= ApplicationData.Instance.getLivraisonIndicator ().ToString());
				RunOnUiThread (() => m_deliveryBadge.Visibility = ViewStates.Visible);
			} else {
				RunOnUiThread (() => m_deliveryBadge.Visibility = ViewStates.Invisible);
			}

			if (ApplicationData.Instance.getEnlevementIndicator () > 0) {
				RunOnUiThread (() => m_peekupBadgeText.Text= ApplicationData.Instance.getEnlevementIndicator ().ToString());
				RunOnUiThread (() => m_peekupBadge.Visibility = ViewStates.Visible);
			} else {
				RunOnUiThread (() => m_peekupBadge.Visibility = ViewStates.Invisible);
			}

			if (ApplicationData.Instance.getmessageIndicator () > 0) {
				RunOnUiThread (() => m_newMsgBadgeText.Text= ApplicationData.Instance.getmessageIndicator ().ToString());
				RunOnUiThread (() => m_newMsgBadge.Visibility = ViewStates.Visible);
			} else {
				RunOnUiThread (() => m_newMsgBadge.Visibility = ViewStates.Invisible);
			}
		}

		protected void delivery_Click()
        {
            
//			if (!ApplicationData.Instance.getConfigurationModel ().isConfigurationDane ()) {
//				Toast.MakeText (this, ApplicationData.Instance.getTranslator ().translateMessage ("confignotdane"), ToastLength.Short).Show ();
//				return;
//			} else if (ApplicationData.Instance.isAdminLogin ()) {
//

//				StartActivity(typeof(ActivityListLivraison));
//			}

			StartActivity(typeof(ActivityListLivraison));
		}



		protected void peekup_Click()
		{
			
//			if (!ApplicationData.Instance.getConfigurationModel ().isConfigurationDane ()) {
//				Toast.MakeText (this, ApplicationData.Instance.getTranslator ().translateMessage ("confignotdane"), ToastLength.Short).Show ();
//				return;
//			} else if (ApplicationData.Instance.isAdminLogin ()) {
//				StartActivity(typeof(ActivityListEnlevement));
//			}

			StartActivity(typeof(ActivityListEnlevement));
		}

		protected void chat_Click()
		{
			StartActivity(typeof(ActivityChat));
		}


		protected void inbox_Click()
		{
			//Show an error image with a message with a Dimmed background, and auto-dismiss after 2 seconds
			AndHUD.Shared.ShowError(this, "Non Disponible", MaskType.Black, TimeSpan.FromSeconds(2));


		}

		protected void outbox_Click()
		{
			//Show an error image with a message with a Dimmed background, and auto-dismiss after 2 seconds
			AndHUD.Shared.ShowError(this, "Non Disponible", MaskType.Black, TimeSpan.FromSeconds(2));

		}

		protected void activity_Click()
		{


			//Show an error image with a message with a Dimmed background, and auto-dismiss after 2 seconds
			AndHUD.Shared.ShowError(this, "Non Disponible", MaskType.Black, TimeSpan.FromSeconds(2));

		}

	
		protected void config_Click()
		{
			

		}

		protected void config_LongClick(){
			AlertDialog.Builder builder = new AlertDialog.Builder(this);
					
			builder.SetTitle("Deconnexion");

			builder.SetMessage("Voulez-vous vous déconnecter ?");
			builder.SetCancelable(false);
			builder.SetPositiveButton("Annuler", delegate {  });
			builder.SetNegativeButton("Déconnexion", delegate {
				DBRepository dbr = new DBRepository ();
				var resetuser = dbr.UpdateLogin(false,DateTime.Now,ApplicationData.UserAndsoft);

				StartActivity (typeof(LoginActivity));
				

			});
			builder.Show();
		}

		protected void delivery_LongClick(){

			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");

			var db = new SQLiteConnection(dbPath);
			// MODIFICATION ALEX Deplacement de la recuperation de l'heure pour la recuperation des data

			if (DateTime.Now.Day < 10) {
				ApplicationData.day = "0" + DateTime.Now.Day;
			} else {
				ApplicationData.day = Convert.ToString (DateTime.Now.Day);
			}

			if (DateTime.Now.Month < 10) {
				ApplicationData.mouth = "0" + DateTime.Now.Month;
			} else {
				ApplicationData.mouth = Convert.ToString (DateTime.Now.Month);
			}

			if (DateTime.Now.Hour < 10) {
				ApplicationData.hour = "0" + DateTime.Now.Hour;
			} else {
				ApplicationData.hour = Convert.ToString (DateTime.Now.Hour);
			}

			if (DateTime.Now.Minute < 10) {
				ApplicationData.minute = "0" + DateTime.Now.Minute;
			} else {
				ApplicationData.minute = Convert.ToString (DateTime.Now.Minute);
			}


			ApplicationData.dateimport = DateTime.Now.Year + ApplicationData.mouth + ApplicationData.day;
			ApplicationData.datedj = ApplicationData.day + "/" + ApplicationData.mouth + "/" + DateTime.Now.Year + " " + ApplicationData.hour + ":" + ApplicationData.minute;

			//MODIF ROMAIN 1307 HTTPWEBREQUEST TO WEBCLIENT

			try {
				//API DATA XML
				string _url = "http://dms.jeantettransport.com/api/commande?codechauffeur=" + ApplicationData.UserTransic + "&datecommande=" + ApplicationData.dateimport+"";
				var webClient = new WebClient ();
				webClient.Headers [HttpRequestHeader.ContentType] = "application/json";
				//webClient.Encoding = Encoding.UTF8;

				Data.content = webClient.DownloadString (_url);
				Console.Out.WriteLine (">>>>>THREAD INTEG DONE....<<<<<");
			} catch (Exception ex) {
				Data.content = "[]";
				Insights.Report (ex,Xamarin.Insights.Severity.Error);
			}


			Data.countliv = 0;
			Data.countram = 0;
			Data.countmess = 0;

			//SON
			if (Data.content == "[]") {
			} else {
				alert ();
			}

			JArray jsonVal = JArray.Parse (Data.content) as JArray;
			var jsonarr = jsonVal;

			foreach (var item in jsonarr) {
				int verifpos = 0;
				var verifbasecode = db.Query<ToDoTask>("SELECT * FROM ToDoTask WHERE numCommande = ?",Convert.ToString (item ["numCommande"]));



				foreach (var pos in verifbasecode) {
					verifpos++;
				}

				DBRepository dbr = new DBRepository ();

				if (verifpos > 1) {
				} else {
					var resinteg = dbr.InsertData (Convert.ToString (item ["codeLivraison"]), Convert.ToString (item ["numCommande"]), Convert.ToString (item ["refClient"]), Convert.ToString (item ["nomPayeur"]), Convert.ToString (item ["nomExpediteur"]), Convert.ToString (item ["adresseExpediteur"]), Convert.ToString (item ["villeExpediteur"]), Convert.ToString (item ["CpExpediteur"]), Convert.ToString (item ["dateExpe"]), Convert.ToString (item ["nomClient"]), Convert.ToString (item ["adresseLivraison"]), Convert.ToString (item ["villeLivraison"]), Convert.ToString (item ["CpLivraison"]), Convert.ToString (item ["dateHeure"]), Convert.ToString (item ["poids"]), Convert.ToString (item ["nbrPallette"]), Convert.ToString (item ["nbrColis"]), Convert.ToString (item ["instrucLivraison"]), Convert.ToString (item ["typeMission"]), Convert.ToString (item ["typeSegment"]), Convert.ToString (item ["groupage"]), Convert.ToString (item ["ADRCom"]), Convert.ToString (item ["ADRGrp"]), "0", Convert.ToString (item ["CR"]), DateTime.Now.Day, Convert.ToString (item ["Datemission"]), Convert.ToString (item ["Ordremission"]), Convert.ToString (item ["planDeTransport"]), ApplicationData.UserAndsoft, Convert.ToString (item ["nomClientLivraison"]), Convert.ToString (item ["villeClientLivraison"]));
					var resintegnotif = dbr.InsertDataStatutMessage (10, DateTime.Now, 1, Convert.ToString (item ["numCommande"]), Convert.ToString (item ["groupage"]));
					Console.WriteLine (resintegnotif);
					Console.WriteLine (resinteg);

				}


				Console.WriteLine (item ["numCommande"]);


			}

			//SET BADGE

			var tableliv = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV' AND Userandsoft = ?",ApplicationData.UserAndsoft);



			foreach (var row in tableliv) {

				Data.countliv++;
			}


			var tableram = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='C' AND typeSegment='RAM' AND Userandsoft = ?",ApplicationData.UserAndsoft);
			foreach (var rows in tableram) {

				Data.countram++;
			}
		var tablemess = db.Query<Message> ("SELECT * FROM Message WHERE statutMessage = 0 AND codeChauffeur=?",ApplicationData.UserAndsoft);
			foreach( var row in tablemess){

				Data.countmess++;
			}

			ApplicationData.Instance.setLivraisonIndicator (Data.countliv);
			ApplicationData.Instance.setEnlevementIndicator (Data.countram);
			ApplicationData.Instance.setmessageIndicator(Data.countmess);







			// SUPPRESSION DES BORDEREAUX CLOTURER

			string retour = "";

			var tablegroupage = db.Query<ToDoTask> ("SELECT groupage FROM ToDoTask group by groupage");
			foreach (var items2 in tablegroupage) {

				retour = Convert.ToString (items2.groupage);

				try {
					string _urlb = "http://dms.jeantettransport.com/api/groupage?voybdx="+ retour+"";
					var webClient = new WebClient ();
					webClient.Headers [HttpRequestHeader.ContentType] = "application/json";
					//webClient.Encoding = Encoding.UTF8;
					Data.content = webClient.DownloadString (_urlb);

					JObject jsonarr2 = JObject.Parse (Data.content);
					Console.WriteLine ((string)jsonarr2 ["etat"]);
					if ((string)jsonarr2 ["etat"] == "CLO") {
						var supprimegroupage = db.Query<ToDoTask> ("delete from ToDoTask where groupage ='" + retour + "'");
						Console.Out.WriteLine (">>>>>DELETE DU GROUPAGE ....<<<<<");
					}

				} catch (Exception ex) {
					Data.content = "[]";
					Insights.Report (ex,Xamarin.Insights.Severity.Error);
				}


			}

			Toast.MakeText (this,"Mise à jour", ToastLength.Short).Show ();
		}
		public void Threadapp()
		{
			//NEW THREAD 1307 ROMAIN

			Thread ThreadAppInteg = new Thread(new ThreadStart(this.Integdata));
			Thread ThreadAppCom = new Thread(new ThreadStart(this.ComWebservice));
			Thread ThreadAppGPS = new Thread(new ThreadStart(this.ComPosGPS));
			ThreadAppCom.Start();
			Console.Out.Write ("///////////////ThreadAppCom START///////////////");
			Thread.Sleep (10);
			ThreadAppInteg.Start();
			Console.Out.Write ("///////////////ThreadAppInteg START///////////////");
			Thread.Sleep (10);
			ThreadAppGPS.Start ();
			Console.Out.Write ("///////////////ThreadAppGPS START///////////////");

		}
		public void ComPosGPS(){
			int idgps = 0;
			while (idgps == 0) {

				//CONN
				var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

				var activeConnection = connectivityManager.ActiveNetworkInfo;
				if ((activeConnection != null) && activeConnection.IsConnected) {




						//API GPS OK
						string _url = "http://dms.jeantettransport.com/api/leslie";
						string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
							(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
						var db = new SQLiteConnection (dbPath);

						DBRepository dbr = new DBRepository ();
						var webClient = new WebClient ();

						try {


						//ROUTINE INTEG MESSAGE
						try {
							//API LIVRER OK
							string _urlb = "http://dms.jeantettransport.com/api/leslie?codechauffeur=" + ApplicationData.UserAndsoft +"";
							var webClientb = new WebClient ();
							webClientb.Headers [HttpRequestHeader.ContentType] = "application/json";
							//webClient.Encoding = Encoding.UTF8;

							Data.contentmsg = webClientb.DownloadString (_urlb);
							Console.Out.WriteLine (">>>>>THREAD INTEG DONE....<<<<<");
						} catch (Exception ex) {
							Data.contentmsg = "[]";
							Insights.Report (ex,Xamarin.Insights.Severity.Error);

						}

						//SON MSG
						if (Data.contentmsg == "[]") {
						} else {
							alertsms ();
						}

						JArray jsonVal = JArray.Parse (Data.contentmsg) as JArray;
						var jsonarr = jsonVal;

						foreach (var item in jsonarr) {
							var resinteg = dbr.InsertDataMessage (Convert.ToString (item ["codeChauffeur"]), Convert.ToString (item ["utilisateurEmetteur"]), Convert.ToString (item ["texteMessage"]),0,DateTime.Now,1, Convert.ToInt32 (item ["numMessage"]));
							var resintegstatut = dbr.InsertDataStatutMessage(0,DateTime.Now, Convert.ToInt32 (item ["numMessage"]),"","");

							Console.WriteLine (item ["numMessage"]);
							Console.WriteLine (resinteg);

						}
						Data.countmess=0;

						var tablemess = db.Query<Message> ("SELECT * FROM Message WHERE statutMessage = 0 AND codeChauffeur=?",ApplicationData.UserAndsoft);
						foreach( var row in tablemess){

							Data.countmess++;
						}



						ApplicationData.Instance.setmessageIndicator(Data.countmess);

					
						Data.datajson ="";
						Data.datagps="";
						Data.datamsg="";
						Data.datanotif="";

						webClient.Headers [HttpRequestHeader.ContentType] = "application/json";
						//webClient.Encoding = Encoding.UTF8;

						Data.datagps = "{\"posgps\":\"" + ApplicationData.GPS + "\",\"userandsoft\":\"" + ApplicationData.UserAndsoft + "\"}";


						//webClient.UploadString (_url, datagps);


					




						var tablestatutmessage = db.Query<StatutMessage> ("SELECT * FROM StatutMessage");

						//SIMULATION
						//var resintegnotif = dbr.InsertDataStatutMessage (10,DateTime.Now,0,"1510057005","GR150108");
						//SEND NOTIF
						foreach (var item in tablestatutmessage) {
							Data.datanotif += "{\"statutNotificationMessage\":\"" + item.statutNotificationMessage + "\",\"dateNotificationMessage\":\"" + item.dateNotificationMessage + "\",\"numMessage\":\""+item.numMessage+"\",\"numCommande\":\""+item.numCommande+"\",\"groupage\":\""+item.groupage+"\"},";

						//	webClient.UploadString (_url,datamessage);
						//	var resultdelete = db.Query<StatutMessage> (" DELETE FROM StatutLivraison WHERE Id='"+item.Id+"'");
						}

						



						//SEND MESSAGE
						var tablemessage = db.Query<Message> ("SELECT * FROM Message WHERE statutMessage = 2");
						foreach (var item in tablemessage) {
							Data.datamsg += "{\"codeChauffeur\":\"" + item.codeChauffeur + "\",\"texteMessage\":\"" + item.texteMessage + "\",\"utilisateurEmetteur\":\""+item.utilisateurEmetteur+"\",\"dateImportMessage\":\""+item.dateImportMessage+"\",\"typeMessage\":\""+item.typeMessage+"\"},";

						//	webClient.UploadString (_url,datamessage);
						// var updatestatutmessage = db.Query<Message> ("UPDATE Message SET statutMessage = 3 WHERE _Id = ?",item.Id);
						}
						if(Data.datanotif == ""){
							Data.datanotif ="{}";
						}else{
							Data.datanotif = Data.datanotif.Remove(Data.datanotif.Length - 1);
						}
						if(Data.datamsg == ""){
							Data.datamsg ="{}";
						}else{
							Data.datamsg = Data.datamsg.Remove(Data.datamsg.Length - 1);
						}

						Data.datajson = "{\"suivgps\":"+Data.datagps+",\"statutmessage\":["+Data.datanotif+"],\"Message\":["+Data.datamsg+"]}";


						//API MSG/NOTIF/GPS

						try{
							webClient.UploadString (_url,Data.datajson);
							foreach (var item in tablestatutmessage) {
								var resultdelete = dbr.deletenotif(item.Id);
							}
							foreach (var item in tablemessage) {							
								var updatestatutmessage = db.Query<Message> ("UPDATE Message SET statutMessage = 3 WHERE _Id = ?",item.Id);
							}
						}
						catch (Exception e)
						{
							Insights.Report (e,Xamarin.Insights.Severity.Error);
						}






						} catch (Exception ex) {
							Data.datajson = "Crash " + ex;
							Insights.Report (ex,Xamarin.Insights.Severity.Error);
							Console.Out.Write(ex);

						}

					

					


					
					Console.Out.WriteLine (">>>>>THREAD Leslie SEND " +DateTime.Now.Minute+ Data.datajson);


					



				}
				Thread.Sleep (120000);
			}
		}




		public void ComWebservice(){
			int idcom = 0;
			while(idcom == 0){
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");

			var db = new SQLiteConnection(dbPath);
			var table = db.Query<StatutLivraison> ("SELECT * FROM StatutLivraison");
			//ApplicationData.datedj =DateTime.Now.Day+"/"+DateTime.Now.Month+"/"+DateTime.Now.Year+""+DateTime.Now.Hour+":"+DateTime.Now.Minute;
			var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

			var activeConnection = connectivityManager.ActiveNetworkInfo;
			if ((activeConnection != null) && activeConnection.IsConnected) {

				foreach (var item in table) {
						Console.Out.WriteLine(">>>>> "+item.commandesuiviliv+" / "+item.IdS+"<br>");

						try
						{
							//API LIVRER OK
							string _url = "http://dms.jeantettransport.com/api/livraisongroupage";
							var webClient = new WebClient();
							webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
							//webClient.Encoding = Encoding.UTF8;

							webClient.UploadString(_url,item.datajson);
							Console.Out.WriteLine(">>>>>THREAD DATA SEND "+item.datajson);
							var resultdelete = db.Query<StatutLivraison> (" DELETE FROM StatutLivraison WHERE datajson='"+item.datajson+"'");

						}
						catch (Exception e)
						{
							Insights.Report (e,Xamarin.Insights.Severity.Error);
						}

				}
				

				DBRepository dbr = new DBRepository();
				//var resultdrop = dbr.DropTableStatut();
					foreach (var item in table) {
						Console.Out.WriteLine(">>>>>Après "+item.codesuiviliv+""+item.id+"<br>");
					}

				
				//Console.Out.WriteLine (resultdrop);
				Console.Out.WriteLine(">>>>>THREAD NO DATA ....<<<<<"+DateTime.Now.Minute);
				Console.WriteLine("///////Thread Com RUNNING////");
					Thread.Sleep (300000);
					//Thread.Sleep (3000);
			}else{

				
				Console.Out.WriteLine(">>>>>NO CONNECTION WAIT ....<<<<<");
			}

			
			}
		}


		public void Integdata(){
			int idcom = 0;
			while (idcom == 0) {

				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");

				var db = new SQLiteConnection(dbPath);
				// MODIFICATION ALEX Deplacement de la recuperation de l'heure pour la recuperation des data

				if (DateTime.Now.Day < 10) {
					ApplicationData.day = "0" + DateTime.Now.Day;
				} else {
					ApplicationData.day = Convert.ToString (DateTime.Now.Day);
				}

				if (DateTime.Now.Month < 10) {
					ApplicationData.mouth = "0" + DateTime.Now.Month;
				} else {
					ApplicationData.mouth = Convert.ToString (DateTime.Now.Month);
				}

				if (DateTime.Now.Hour < 10) {
					ApplicationData.hour = "0" + DateTime.Now.Hour;
				} else {
					ApplicationData.hour = Convert.ToString (DateTime.Now.Hour);
				}

				if (DateTime.Now.Minute < 10) {
					ApplicationData.minute = "0" + DateTime.Now.Minute;
				} else {
					ApplicationData.minute = Convert.ToString (DateTime.Now.Minute);
				}


				ApplicationData.dateimport = DateTime.Now.Year + ApplicationData.mouth + ApplicationData.day;
				ApplicationData.datedj = ApplicationData.day + "/" + ApplicationData.mouth + "/" + DateTime.Now.Year + " " + ApplicationData.hour + ":" + ApplicationData.minute;

				//MODIF ROMAIN 1307 HTTPWEBREQUEST TO WEBCLIENT
				var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

				var activeConnection = connectivityManager.ActiveNetworkInfo;
				if ((activeConnection != null) && activeConnection.IsConnected) {
					try {
						//API DATA XML
						string _url = "http://dms.jeantettransport.com/api/commande?codechauffeur=" + ApplicationData.UserTransic + "&datecommande=" + ApplicationData.dateimport + "";
						var webClient = new WebClient ();
						webClient.Headers [HttpRequestHeader.ContentType] = "application/json";
						//webClient.Encoding = Encoding.UTF8;

						Data.content = webClient.DownloadString (_url);
						Console.Out.WriteLine (">>>>>THREAD INTEG DONE....<<<<<");
					} catch (Exception ex) {
						Data.content = "[]";
						Insights.Report (ex,Xamarin.Insights.Severity.Error);

					}
				

				Data.countliv = 0;
				Data.countram = 0;

				//SON
				if (Data.content == "[]") {
				} else {
					alert ();
				}

				JArray jsonVal = JArray.Parse (Data.content) as JArray;
				var jsonarr = jsonVal;

					foreach (var item in jsonarr) {
						int verifpos = 0;
						var verifbasecode = db.Query<ToDoTask>("SELECT * FROM ToDoTask WHERE numCommande = ?",Convert.ToString (item ["numCommande"]));



						foreach (var pos in verifbasecode) {
							verifpos++;
						}

						DBRepository dbr = new DBRepository ();

						if (verifpos > 1) {
						} else {
							var resinteg = dbr.InsertData (Convert.ToString (item ["codeLivraison"]), Convert.ToString (item ["numCommande"]), Convert.ToString (item ["refClient"]), Convert.ToString (item ["nomPayeur"]), Convert.ToString (item ["nomExpediteur"]), Convert.ToString (item ["adresseExpediteur"]), Convert.ToString (item ["villeExpediteur"]), Convert.ToString (item ["CpExpediteur"]), Convert.ToString (item ["dateExpe"]), Convert.ToString (item ["nomClient"]), Convert.ToString (item ["adresseLivraison"]), Convert.ToString (item ["villeLivraison"]), Convert.ToString (item ["CpLivraison"]), Convert.ToString (item ["dateHeure"]), Convert.ToString (item ["poids"]), Convert.ToString (item ["nbrPallette"]), Convert.ToString (item ["nbrColis"]), Convert.ToString (item ["instrucLivraison"]), Convert.ToString (item ["typeMission"]), Convert.ToString (item ["typeSegment"]), Convert.ToString (item ["groupage"]), Convert.ToString (item ["ADRCom"]), Convert.ToString (item ["ADRGrp"]), "0", Convert.ToString (item ["CR"]), DateTime.Now.Day, Convert.ToString (item ["Datemission"]), Convert.ToString (item ["Ordremission"]), Convert.ToString (item ["planDeTransport"]), ApplicationData.UserAndsoft, Convert.ToString (item ["nomClientLivraison"]), Convert.ToString (item ["villeClientLivraison"]));
							var resintegnotif = dbr.InsertDataStatutMessage (10, DateTime.Now, 1, Convert.ToString (item ["numCommande"]), Convert.ToString (item ["groupage"]));
							Console.WriteLine (resintegnotif);
							Console.WriteLine (resinteg);

						}


						Console.WriteLine (item ["numCommande"]);


					}
						

				//SET BADGE
				
				var tableliv = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV' AND Userandsoft = ?",ApplicationData.UserAndsoft);



				foreach (var row in tableliv) {

					Data.countliv++;
				}


				var tableram = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='C' AND typeSegment='RAM' AND Userandsoft = ?",ApplicationData.UserAndsoft);
				foreach (var rows in tableram) {

					Data.countram++;
				}
				ApplicationData.Instance.setLivraisonIndicator (Data.countliv);
				ApplicationData.Instance.setEnlevementIndicator (Data.countram);
				ApplicationData.Instance.setmessageIndicator(Data.countmess);


			




				// SUPPRESSION DES BORDEREAUX CLOTURER

				string retour = "";

				var tablegroupage = db.Query<ToDoTask> ("SELECT groupage FROM ToDoTask group by groupage");
				foreach (var items2 in tablegroupage) {

					retour = Convert.ToString (items2.groupage);

					try {
						string _urlb = "http://dms.jeantettransport.com/api/groupage?voybdx="+ retour+"";
							var webClient = new WebClient ();
						webClient.Headers [HttpRequestHeader.ContentType] = "application/json";
						//webClient.Encoding = Encoding.UTF8;
						Data.content = webClient.DownloadString (_urlb);

						JObject jsonarr2 = JObject.Parse (Data.content);
						Console.WriteLine ((string)jsonarr2 ["etat"]);
						if ((string)jsonarr2 ["etat"] == "CLO") {
						var supprimegroupage = db.Query<ToDoTask> ("delete from ToDoTask where groupage ='" + retour + "'");
						Console.Out.WriteLine (">>>>>DELETE DU GROUPAGE ....<<<<<");
						}

					} catch (Exception ex) {
						Data.content = "[]";
							Insights.Report (ex,Xamarin.Insights.Severity.Error);
						
					}


			}
		}
				Console.WriteLine ("///////Thread Integ RUNNING////"+DateTime.Now.Minute);
				Thread.Sleep (300000);

		}
		}


		public void alert()
		{

			MediaPlayer _player;
			_player = MediaPlayer.Create(this,Resource.Raw.beep4);
			_player.Start();
		}

		public void alertsms()
		{

			MediaPlayer _player;
			_player = MediaPlayer.Create(this,Resource.Raw.msg3);
			_player.Start();
		}

		public override void OnBackPressed ()
		{
			StartActivity(typeof(MainActivity));
		}

		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
			if (_currentLocation == null)
			{
				
				ApplicationData.GPS = "Unable to determine your location.";
			}
			else
			{

				ApplicationData.GPS = ""+_currentLocation.Latitude+";"+ _currentLocation.Longitude+"";
					//Toast.MakeText (this,ApplicationData.GPS, ToastLength.Short).Show ();

			}
		}

		public void OnProviderDisabled(string provider)
		{
		}

		public void OnProviderEnabled(string provider)
		{
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			Console.Out.Write("{0}, {1}");
		}



		void InitializeLocationManager()
		{
			_locationManager = (LocationManager)GetSystemService(LocationService);
		
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				_locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				_locationProvider = String.Empty;
			}
			Console.Out.Write("Using " + _locationProvider + ".");

		}

		//		protected override void OnResume()
		//		{
		//			base.OnResume();
		//			_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
		//			Console.Out.Write("Listening for location updates using " + _locationProvider + ".");
		//		}
		//
		//		protected override void OnPause()
		//		{
		//			base.OnPause();
		//			_locationManager.RemoveUpdates(this);
		//			Log.Debug(LogTag, "No longer listening for location updates.");
		//		}
	}
}


