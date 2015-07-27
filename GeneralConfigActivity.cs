
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
	[Activity (Label = "GeneralConfigActivity")]			
	public class GeneralConfigActivity : Activity
	{
		Spinner spinnerLanguage = null;
		ArrayAdapter<String> langList =null;
		Spinner spinnerConnection = null;
		ArrayAdapter<String> connectionList =null;

		TextView lblTitle=null;
		TextView lblCompany=null;
		TextView lblPhone=null;
		TextView lblTimeout=null;
		TextView lblInbox=null;
		TextView lblOutbox=null;
		TextView lblGPS=null;
		TextView lblConnection=null;
		TextView lblLanguage=null;


		EditText txCompany=null;
		EditText txPhone=null;
		EditText txTimeout=null;
		EditText txInbox=null;
		EditText txOutbox=null;
		EditText txGPS=null;

		ImageButton btnPrev=null;
		ImageButton btnNext=null;
		CheckBox autoTrip=null;



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here

			SetContentView (Resource.Layout.GeneralConfig);

			initView();
		}

		private void initView()
		{
			this.Title = "DMS";

			spinnerLanguage = FindViewById<Spinner> (Resource.Id.spinnerLanguage);
			
			langList = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem);
			
			langList.Add("Français");
			langList.Add("English");
			langList.Add("Deutsh");
			langList.Add("Italiano");
			langList.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			
			spinnerLanguage.Adapter = langList;


			
			spinnerConnection = FindViewById<Spinner> (Resource.Id.spinnerConnection);
			
			connectionList = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem);
			
			connectionList.Add(ApplicationData.Instance.getTranslator().translateMessage("connection.type.any"));
			connectionList.Add(ApplicationData.Instance.getTranslator().translateMessage("connection.type.usb"));
			connectionList.Add(ApplicationData.Instance.getTranslator().translateMessage("connection.type.wifi"));
			connectionList.Add(ApplicationData.Instance.getTranslator().translateMessage("connection.type.gprs"));
			connectionList.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			
			spinnerConnection.Adapter = connectionList;

			//spinnerConnection.

			lblTitle=FindViewById<TextView> (Resource.Id.lblTitle);
			lblCompany=FindViewById<TextView> (Resource.Id.textViewCompany);
			lblPhone=FindViewById<TextView> (Resource.Id.textViewPhone);
			lblTimeout=FindViewById<TextView> (Resource.Id.textViewTimeout);
			lblInbox=FindViewById<TextView> (Resource.Id.textViewInbox);
			lblOutbox=FindViewById<TextView> (Resource.Id.textViewOutbox);
			lblGPS=FindViewById<TextView> (Resource.Id.textViewGPS);
			lblConnection=FindViewById<TextView> (Resource.Id.textViewConnection);
			lblLanguage=FindViewById<TextView> (Resource.Id.textViewLanguage);

			txCompany=FindViewById<EditText> (Resource.Id.editTextCompany);
			txPhone=FindViewById<EditText> (Resource.Id.editTextPhone);
			txTimeout=FindViewById<EditText> (Resource.Id.editTextTimeout);
			txInbox=FindViewById<EditText> (Resource.Id.editTextInbox);
			txOutbox=FindViewById<EditText> (Resource.Id.editTextOutbox);
			txGPS=FindViewById<EditText> (Resource.Id.editTextGPS);
			autoTrip=FindViewById<CheckBox> (Resource.Id.checkBox1);


			lblTitle.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.setp1");
			lblCompany.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.companyname");
			lblPhone.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.phonenumber");
			lblTimeout.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.timeout");
			lblInbox.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.inboxinterval");
			lblOutbox.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.outboxinterval");
			lblGPS.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.gps");
			lblConnection.Text=ApplicationData.Instance.getTranslator().translateMessage("configform.connection");
			lblLanguage.Text= ApplicationData.Instance.getTranslator().translateMessage("configform.lang");
			autoTrip.Text = ApplicationData.Instance.getTranslator().translateMessage("msg.autotrimp");

			btnPrev = FindViewById<ImageButton> (Resource.Id.imageButton1);
			btnPrev.Click += delegate { Button_Prev();	};
			
			btnNext = FindViewById<ImageButton> (Resource.Id.imageButton2);
			btnNext.Click += delegate { Button_Next();	};


			if (ApplicationData.Instance.getTempConfigModel() != null)
			{
				txCompany.Text=ApplicationData.Instance.getTempConfigModel().getCompanyName();
				txPhone.Text=ApplicationData.Instance.getTempConfigModel().getPhoneNumber();
				txTimeout.Text=ApplicationData.Instance.getTempConfigModel().getTimout().ToString();
				txInbox.Text=ApplicationData.Instance.getTempConfigModel().getInboxUpdateInterval().ToString();
				txOutbox.Text=ApplicationData.Instance.getTempConfigModel().getOutboxUpdateInterval().ToString();
				txGPS.Text=ApplicationData.Instance.getTempConfigModel().getGspPositionSending().ToString();
			}


			if (ApplicationData.Instance.getTempConfigModel().getLanguage() == "FR")
				spinnerLanguage.SetSelection(0);
			else if (ApplicationData.Instance.getTempConfigModel().getLanguage() == "EN")
				spinnerLanguage.SetSelection(1);
			else if (ApplicationData.Instance.getTempConfigModel().getLanguage() == "DE")
				spinnerLanguage.SetSelection(2);
			else if (ApplicationData.Instance.getTempConfigModel().getLanguage() == "IT")
				spinnerLanguage.SetSelection(3);

			spinnerConnection.SetSelection(ApplicationData.Instance.getTempConfigModel().getConnectionType());

			if (ApplicationData.Instance.getTempConfigModel ().getAutoTrip () == 1)
				autoTrip.Checked = true;
			else autoTrip.Checked = false;
		}



		private void Button_Prev()
		{

			Finish();
		}

		private void Button_Next()
		{

			/*_model.setConnectionType(this.connectionType.SelectedSegment);
			if (this.language.SelectedSegment==0)
				_model.setLanguage("FR");
			else if (this.language.SelectedSegment==1)
				_model.setLanguage("EN");
			else if (this.language.SelectedSegment==2)
				_model.setLanguage("DE");
			else if (this.language.SelectedSegment==3)
				_model.setLanguage("IT");
			else _model.setLanguage("FR");*/


			int langPos = spinnerLanguage.SelectedItemPosition;

			if (langPos==0)
				ApplicationData.Instance.getTempConfigModel().setLanguage("FR");
			else if (langPos==1)
				ApplicationData.Instance.getTempConfigModel().setLanguage("EN");
			else if (langPos==2)
				ApplicationData.Instance.getTempConfigModel().setLanguage("DE");
			else if (langPos==3)
				ApplicationData.Instance.getTempConfigModel().setLanguage("IT");
			else ApplicationData.Instance.getTempConfigModel().setLanguage("FR");

			ApplicationData.Instance.getTempConfigModel().setConnectionType(spinnerConnection.SelectedItemPosition);
			
			
			ApplicationData.Instance.getTempConfigModel().setCompanyName(txCompany.Text);
			ApplicationData.Instance.getTempConfigModel().setPhoneNumber(txPhone.Text);
			try
			{
				ApplicationData.Instance.getTempConfigModel().setTimout(Convert.ToInt32(txTimeout.Text));
			}
			catch (Exception e) {
				ApplicationData.Instance.getTempConfigModel().setTimout(0);
			}
			try{
			ApplicationData.Instance.getTempConfigModel().setInboxUpdateInterval(Convert.ToInt32(txInbox.Text));
			}
			catch (Exception e) {
				ApplicationData.Instance.getTempConfigModel().setInboxUpdateInterval(0);
			}
			try{
			ApplicationData.Instance.getTempConfigModel().setOutboxUpdateInterval(Convert.ToInt32(txOutbox.Text));
			}
			catch (Exception e) {
				ApplicationData.Instance.getTempConfigModel().setOutboxUpdateInterval(0);
			}
			try{
			ApplicationData.Instance.getTempConfigModel().setGspPositionSending(Convert.ToInt32(txGPS.Text));
			}
			catch (Exception e) {
				ApplicationData.Instance.getTempConfigModel().setGspPositionSending(0);
			}

			if (autoTrip.Checked)
				ApplicationData.Instance.getTempConfigModel ().setAutoTrip (1);
			else ApplicationData.Instance.getTempConfigModel ().setAutoTrip (0);

			StartActivity(typeof(AuthConfigActivity));
			Finish();
		}


	}
}

