using System;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;

using Xamarin;

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

			//initView ();



		}

		private void initView()
		{
			this.Title = "DMS";

			TextView lblMessage=FindViewById<TextView> (Resource.Id.lblMsg);
			lblTimeout=FindViewById<TextView> (Resource.Id.lblTimeout);

			Button btnLogin = FindViewById<Button> (Resource.Id.buttonLogin);
			Button btnpass = FindViewById<Button> (Resource.Id.buttonpass);
			//btnLogin.Click += delegate { onLogin();	};

			btnLogin.Click += (sender, e) => {

				ApplicationData.Instance.setTempConfigModel(ApplicationData.Instance.getConfigurationModel().clone());
				isScanning = true;
				var scanner = new ZXing.Mobile.MobileBarcodeScanner(this);

				var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
				options.PossibleFormats = new List<ZXing.BarcodeFormat>() { 
					ZXing.BarcodeFormat.CODE_39  //, ZXing.BarcodeFormat.Ean13 
				};

				scanner.Scan(options).ContinueWith((t) =>
					{
						isScanning = false;
						if (t.IsFaulted)
						{
							Login("ERROR!");
						}
						else if (t.Result != null)
						{

							Login(t.Result.Text);

							StartActivity (typeof(MainActivity));
							ApplicationData.Instance.setUserLogin (true);
							//starttrip();

						}
					});
			};


			btnpass.Click += (sender, e) => { 
				//ApplicationData.Instance.setTempConfigModel(ApplicationData.Instance.getConfigurationModel().clone());
				//StartActivity (typeof(GeneralConfigActivity));

				AlertDialog.Builder builder = new AlertDialog.Builder(this);


				EditText input = new EditText(this);
				builder.SetTitle("Login");
				builder.SetView (input);
				//builder.SetMessage("Voulez-vous valider cette livraison ?");
				builder.SetCancelable(false);
				builder.SetPositiveButton("Valider", delegate {


					if(input.Text =="phiphi"){

						//starttrip();
						ApplicationData.Instance.setUserLogin (true);
						StartActivity (typeof(MainActivity));

					}else{Toast.MakeText (this, "Wrg PASS", ToastLength.Short).Show ();}
				});
				builder.SetNegativeButton("Annuler", delegate {  });
				builder.Show();
			};

			lblMessage.Text=ApplicationData.Instance.getTranslator().translateMessage("userauthentication.usermessage");

			if (firstTime) {
				lblTimeout.Text = "";
				firstTime = false;
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			initView();

			if (!Insights.IsInitialized) {
				starttrip ();
				StartActivity (typeof(MainActivity));

			}
		}

		protected override void OnStop()
		{
			if ((!isScanning)&&(!ApplicationData.Instance.isUserLogin ())) {
				MainActivity.getContext ().loginCanceled = true;
				MainActivity.getContext ().Finish ();
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

		public override void OnBackPressed ()
		{
			StartActivity(typeof(LoginActivity));
		}

		public void starttrip(){

			ApplicationData.Instance.setTempConfigModel(ApplicationData.Instance.getConfigurationModel().clone());
			//START TRIP
			if (!ServerActions.Instance.isServerStarted ()) {
				ServerActions.Instance.StartServer ();				
				ApplicationActions.Instance.initTimers ();

				List<TextMessage> existingMessages = ApplicationActions.Instance.loadMessages (TextMessage.MSG_OUTBOX);
				ApplicationActions.Instance.updateOutboxMessageList (existingMessages, new List<TextMessage> ());

				existingMessages = ApplicationActions.Instance.loadMessages (TextMessage.MSG_INBOX);
				ApplicationActions.Instance.updateInboxMessageList (existingMessages, new List<TextMessage> ());
			}
			else ApplicationActions.Instance.restartTimers ();

			if (ApplicationData.Instance.getConfigurationModel().getAutoTrip() == 1)
				ApplicationActions.Instance.setTripStarted (false);
			else ApplicationActions.Instance.setTripStarted (true);
			ApplicationActions.Instance.ChangeTripState ();
			ApplicationData.Instance.setUserLogin (true);


		}



	}
}