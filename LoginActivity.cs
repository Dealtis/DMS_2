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
	[Activity (Label = "LoginActivity", ConfigurationChanges=Android.Content.PM.ConfigChanges.Orientation)]			
	public class LoginActivity : Activity
	{  
		TextView lblTimeout;
		bool firstTime=true;
		bool isScanning=false;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.Auth);
			var imgcon = FindViewById<ImageView> (Resource.Id.imgcon);
			var txttable = FindViewById<TextView> (Resource.Id.txttable);


			// Test de connexion
			var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);


			var activeConnection = connectivityManager.ActiveNetworkInfo;
			if ((activeConnection != null) && activeConnection.IsConnected) {
				imgcon.SetBackgroundResource (Resource.Drawable.SBBadgeBGGREEN);

			} else {
				imgcon.SetBackgroundResource (Resource.Drawable.SBBadgeBG);
			}

			txttable.Text = "Table bien chargée";
		
			//initView ();



		}

		private void initView()
		{
			this.Title = "DMS";

		
			Button btnpass = FindViewById<Button> (Resource.Id.buttonpass);
			//btnLogin.Click += delegate { onLogin();	};
			btnpass.LongClick += delegate	{ user_LongClick();};
			btnpass.Click += (sender, e) => { 
				//ApplicationData.Instance.setTempConfigModel(ApplicationData.Instance.getConfigurationModel().clone());
				//StartActivity (typeof(GeneralConfigActivity));

				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				//BNT PART et CR
				var viewAD = this.LayoutInflater.Inflate (Resource.Layout.login, null);

			
			
				builder.SetTitle("Login USER");
				builder.SetView (viewAD);

				builder.SetCancelable(false);
				builder.SetPositiveButton("Valider", delegate {
					//comparer le input à la base
					string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
						(Environment.SpecialFolder.Personal), "ormDMS.db3");

					var db = new SQLiteConnection(dbPath);

					DBRepository dbr = new DBRepository ();
					var resultA = dbr.SelectDataUserAndsoft((viewAD.FindViewById<EditText> (Resource.Id.edittextlogin).Text).ToUpper());
					var resultT = dbr.SelectDataUserTransics((viewAD.FindViewById<EditText> (Resource.Id.edittextlogin).Text).ToUpper());
					var resultP = dbr.SelectDataUserMdp((viewAD.FindViewById<EditText> (Resource.Id.edittextlogin).Text).ToUpper(),(viewAD.FindViewById<EditText> (Resource.Id.edittextpsw)).Text);
					if(resultP == ""){
						Toast.MakeText (this, "Wrg USER OR PSW", ToastLength.Short).Show ();
					}else{
						//si il existe connexion
						//changement du fichier de conf
						ApplicationData.UserAndsoft = resultA;
						ApplicationData.UserTransic = resultT;
						DateTime datelog = DateTime.Now;
						var setlog = dbr.UpdateLogin(true,datelog,ApplicationData.UserAndsoft);
						var setdate = dbr.UpdateDate(true,datelog,ApplicationData.UserAndsoft);
						StartActivity (typeof(MainActivity));
					}
				});
				builder.SetNegativeButton("Annuler", delegate {StartActivity (typeof(LoginActivity));});
				builder.Show();
			};


			if (firstTime) {

				firstTime = false;
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			initView();

			if (!Insights.IsInitialized) {
				
				StartActivity (typeof(MainActivity));

			}
		}

		protected override void OnStop()
		{	
			DBRepository dbr = new DBRepository ();
			bool selectlogin = dbr.Selectlogin (ApplicationData.UserAndsoft);
			if (selectlogin == true) {
				StartActivity (typeof(MainActivity));
			}

			base.OnStop();
		}

		protected void onLogin()
		{

		}

		protected void Login(String _szBarcode)
		{
			if (_szBarcode.Equals (ApplicationData.Instance.getConfigurationModel ().getUserBarcode ())) {

				ApplicationData.Instance.setUserLogin (true);




			} else {

				ApplicationData.Instance.setUserLogin (false);
				lblTimeout.Text = ApplicationData.Instance.getTranslator().translateMessage("userauthentication.incorrectuser");
				Finish ();




			}

		}
		public void user_LongClick(){
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");

			var db = new SQLiteConnection(dbPath);

			DBRepository dbr = new DBRepository ();
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

					Toast.MakeText (this, "Table mise à jour", ToastLength.Short).Show ();

				}
				catch (Exception e)
				{
					Insights.Report (e,Xamarin.Insights.Severity.Error);
					Toast.MakeText (this, "Erreur Webclient", ToastLength.Short).Show ();
				}


			}else{
				var imgcon = FindViewById<ImageView> (Resource.Id.imgcon);
				//else prendre ancienne config et revenir cherche quaund connexion
				Console.Out.WriteLine ("Pas de connexion");
				Toast.MakeText (this, "Pas de connexion", ToastLength.Short).Show ();
				imgcon.SetBackgroundResource (Resource.Drawable.SBBadgeBG);
			}
		}
		public override void OnBackPressed ()
		{
			StartActivity(typeof(LoginActivity));
		}

		public void starttrip(){

			ApplicationData.Instance.setTempConfigModel(ApplicationData.Instance.getConfigurationModel().clone());
			//START TRIP
//			if (!ServerActions.Instance.isServerStarted ()) {
//				ServerActions.Instance.StartServer ();				
//				ApplicationActions.Instance.initTimers ();
//
//				List<TextMessage> existingMessages = ApplicationActions.Instance.loadMessages (TextMessage.MSG_OUTBOX);
//				ApplicationActions.Instance.updateOutboxMessageList (existingMessages, new List<TextMessage> ());
//
//				existingMessages = ApplicationActions.Instance.loadMessages (TextMessage.MSG_INBOX);
//				ApplicationActions.Instance.updateInboxMessageList (existingMessages, new List<TextMessage> ());
//			}
//			else ApplicationActions.Instance.restartTimers ();
//
//			if (ApplicationData.Instance.getConfigurationModel().getAutoTrip() == 1)
//				ApplicationActions.Instance.setTripStarted (false);
//			else ApplicationActions.Instance.setTripStarted (true);
//			ApplicationActions.Instance.ChangeTripState ();
//			ApplicationData.Instance.setUserLogin (true);


		}



	}
}