
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DMSvStandard
{
	[Activity (Label = "AuthConfigActivity")]			
	public class AuthConfigActivity : Activity
	{
		TextView lblTitle=null;
		TextView lblHeader1=null;
		TextView lblUserName=null;
		TextView lblUserBarcode=null;
		TextView lblHeader2=null;
		TextView lblTruckID=null;
		TextView lblPass=null;
		TextView lblSystemNr=null;
		TextView lblActivityDriving=null;
		TextView lblActivityWaiting=null;
		TextView lblActivities=null;
		
		
		EditText txUserName=null;
		EditText txUserBarcode=null;
		EditText txTruckID=null;
		EditText txPass=null;
		EditText txSystemNr=null;
		EditText txActivityDriving=null;
		EditText txActivityWaiting=null;

		ImageButton btnPrev=null;
		ImageButton btnSave=null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.AuthConfig);
			InitView();
		}

		private void InitView()
		{
			this.Title = "DMS";

			lblTitle=FindViewById<TextView> (Resource.Id.lblTitle);
			lblHeader1=FindViewById<TextView> (Resource.Id.textViewHeader1);
			lblUserName=FindViewById<TextView> (Resource.Id.textViewUserName);
			lblUserBarcode=FindViewById<TextView> (Resource.Id.textViewBarcode);
			lblHeader2=FindViewById<TextView> (Resource.Id.textViewHeader2);
			lblTruckID=FindViewById<TextView> (Resource.Id.textViewTruckID);
			lblPass=FindViewById<TextView> (Resource.Id.textViewTXPassword);
			lblSystemNr=FindViewById<TextView> (Resource.Id.textViewTXSystemNr);
			
			
			txUserName=FindViewById<EditText> (Resource.Id.editTextUserName);
			txUserBarcode=FindViewById<EditText> (Resource.Id.editTextBarcode);
			txTruckID=FindViewById<EditText> (Resource.Id.editTextTruckID);
			txPass=FindViewById<EditText> (Resource.Id.editTextTXPassword);
			txSystemNr=FindViewById<EditText> (Resource.Id.editTextTXSystemNr);

			txActivityDriving=FindViewById<EditText> (Resource.Id.editTextDriving);
			txActivityWaiting=FindViewById<EditText> (Resource.Id.editTextWaiting);
			lblActivityDriving=FindViewById<TextView> (Resource.Id.textViewDriving);
			lblActivityWaiting=FindViewById<TextView> (Resource.Id.textViewWaiting);
			lblActivities=FindViewById<TextView> (Resource.Id.textViewHeader10);

			lblTitle.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.setp2");
			lblHeader1.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.auth.dtdmheader");
			lblUserName.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.username");
			lblUserBarcode.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.userbarcode");
			lblHeader2.Text= ApplicationData.Instance.getTranslator().translateMessage("configform.auth.txheader");
			lblTruckID.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.txusername");
			lblPass.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.txuserpassword");
			lblSystemNr.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.txsystemnr");

			lblActivityDriving.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.activitydriving") + ":";
			lblActivityWaiting.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.activitywaiting") + ":";
			lblActivities.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.activitytitle") + ":";

			btnPrev = FindViewById<ImageButton> (Resource.Id.imageButton1);
			btnPrev.Click += delegate { Button_Prev();	};
			
			btnSave = FindViewById<ImageButton> (Resource.Id.imageButton2);
			btnSave.Click += delegate { Button_Save();	};

			if (ApplicationData.Instance.getTempConfigModel()!=null)
			{
				txUserName.Text=ApplicationData.Instance.getTempConfigModel().getUserName();
				txUserBarcode.Text=ApplicationData.Instance.getTempConfigModel().getUserBarcode();
				txTruckID.Text=ApplicationData.Instance.getTempConfigModel().getTxUserName();
				txPass.Text=ApplicationData.Instance.getTempConfigModel().getTxUserPassword();
				txSystemNr.Text=ApplicationData.Instance.getTempConfigModel().getTxSystemNr();
				txActivityDriving.Text = ApplicationData.Instance.getTempConfigModel ().getActivityDriving ().ToString();
				txActivityWaiting.Text = ApplicationData.Instance.getTempConfigModel ().getActivityWaiting ().ToString();
			}
		}

		private void Button_Prev()
		{
			ApplicationData.Instance.getTempConfigModel().setUsarName(txUserName.Text);
			ApplicationData.Instance.getTempConfigModel().setUserBarcode(txUserBarcode.Text);
			ApplicationData.Instance.getTempConfigModel().setTxUserName(txTruckID.Text);

			ApplicationData.Instance.getTempConfigModel().setTxUserPassword(txPass.Text);
			ApplicationData.Instance.getTempConfigModel().setTxSystemNr(txSystemNr.Text);
			ApplicationData.Instance.getTempConfigModel().setActivityDriving(Convert.ToInt32(txActivityDriving.Text));
			ApplicationData.Instance.getTempConfigModel().setActivityWaiting(Convert.ToInt32(txActivityWaiting.Text));

			StartActivity(typeof(GeneralConfigActivity));
			Finish();
		}

		private void Button_Save()
		{

			ApplicationData.Instance.getTempConfigModel().setUsarName(txUserName.Text);
			ApplicationData.Instance.getTempConfigModel().setUserBarcode(txUserBarcode.Text);
			ApplicationData.Instance.getTempConfigModel().setTxUserName(txTruckID.Text);
			ApplicationData.Instance.getTempConfigModel().setTxUserPassword(txPass.Text);
			ApplicationData.Instance.getTempConfigModel().setTxSystemNr(txSystemNr.Text);
			//ApplicationData.User = Convert.ToString(txUserName);
			try{
			ApplicationData.Instance.getTempConfigModel().setActivityDriving(Convert.ToInt32(txActivityDriving.Text));
			}
			catch(Exception e){
				ApplicationData.Instance.getTempConfigModel().setActivityDriving(0);
			}

			try{
			ApplicationData.Instance.getTempConfigModel().setActivityWaiting(Convert.ToInt32(txActivityWaiting.Text));
			}
			catch(Exception e){
				ApplicationData.Instance.getTempConfigModel().setActivityWaiting(0);
			}

			if (!ApplicationData.Instance.getTempConfigModel ().isConfigurationDane ()) {
				Toast.MakeText (this, ApplicationData.Instance.getTranslator ().translateMessage ("confignotdone2"), ToastLength.Short).Show ();
				return;
			}


			ApplicationData.Instance.getTempConfigModel().saveConfiguration();
			ApplicationData.Instance.setConfigurationModel (ApplicationData.Instance.getTempConfigModel());
			ApplicationData.Instance.setTempConfigModel(null);


			ConfigurationModel _model = new ConfigurationModel ();
			_model.loadConfiguration ();

			////////////////////////
			ApplicationData.Instance.setConfigurationModel (_model);
			ApplicationData.Instance.setTranslator (ApplicationData.Instance.getConfigurationModel ().getLanguage (), "DTMD");

//			if (ApplicationData.Instance.getConfigurationModel ().isConfigurationDane ()) {
//				if (!ServerActions.Instance.isServerStarted ()) {
//					ServerActions.Instance.StartServer ();				
//					ApplicationActions.Instance.initTimers ();
//
//					List<TextMessage> existingMessages = ApplicationActions.Instance.loadMessages (TextMessage.MSG_OUTBOX);
//					ApplicationActions.Instance.updateOutboxMessageList (existingMessages, new List<TextMessage> ());
//
//					existingMessages = ApplicationActions.Instance.loadMessages (TextMessage.MSG_INBOX);
//					ApplicationActions.Instance.updateInboxMessageList (existingMessages, new List<TextMessage> ());
//				}
//				else ApplicationActions.Instance.restartTimers ();
//
//				ApplicationData.Instance.setUserLogin (false);
//				MainActivity.getContext ().loginCanceled = false;
//
//
//				if (ApplicationData.Instance.getConfigurationModel().getAutoTrip() == 1)
//					ApplicationActions.Instance.setTripStarted (false);
//				else ApplicationActions.Instance.setTripStarted (true);
//				ApplicationActions.Instance.ChangeTripState ();
//			}


			Finish ();
		}
	}
}

