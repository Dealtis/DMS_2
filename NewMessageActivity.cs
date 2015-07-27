
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
	[Activity (Label = "NewMessageActivity")]			
	public class NewMessageActivity : Activity
	{
		ImageButton btnPrev=null;
		ImageButton btnSend=null;
		//Button btnSuj=null;
		TextView lblTitle=null;
		EditText txMsg=null;
		int m_MessageType;
		Button btnSend2 =null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here

			SetContentView (Resource.Layout.NewMessage);



			lblTitle = FindViewById<TextView> (Resource.Id.lblTitle);
			
			
			btnPrev = FindViewById<ImageButton> (Resource.Id.imageButton1);
			btnPrev.Click += delegate { goBack();	};

			btnSend = FindViewById<ImageButton> (Resource.Id.imageButton2);
			//btnSend.Click += delegate { SendMessage();	};

			txMsg = FindViewById<EditText> (Resource.Id.editText1);

			m_MessageType = Intent.GetIntExtra("MessageType", TextMessage.MSG_NEW);
			btnSend2 = FindViewById<Button>(Resource.Id.button1);
			/*btnSuj = FindViewById<Button>(Resource.Id.btnSuj);
			btnSuj.Click += delegate {
				OpenSuj();
			};*/

			// btnSuj.Visibility = ViewStates.Visible;

			initView();
		}

		void OpenSuj()
		{
			Intent i = new Intent(this, typeof(SujListActivity));
			i.PutExtra("MSG", txMsg.Text);
			StartActivityForResult(i, 1);
			Finish();
		}

		protected void initView()
		{
			this.Title = "DMS";




			if (m_MessageType == TextMessage.MSG_NEW) {
				txMsg.Text = "";

				//btnSend.Click += SendMessage;

				lblTitle.Text = ApplicationData.Instance.getTranslator ().translateMessage ("formnewmessage.title");
				btnSend2.Text = ApplicationData.Instance.getTranslator ().translateMessage ("formnewmessage.send");
				btnSend2.Click += SendMessage;

				/*btnSuj.Visibility = ViewStates.Visible;*/
			} 
			else if (m_MessageType == TextMessage.MSG_INBOX) {

				if (ApplicationData.Instance.CurrentTextMessage != null)
				{
					txMsg.Text = ApplicationData.Instance.CurrentTextMessage.Message;
					txMsg.Text += System.Environment.NewLine;
					txMsg.Text += System.Environment.NewLine;
					txMsg.Text += ApplicationData.Instance.CurrentTextMessage.ArrivalDate.ToShortDateString();
					txMsg.Text += System.Environment.NewLine;
					txMsg.Text += ApplicationData.Instance.CurrentTextMessage.ArrivalDate.ToShortTimeString();									
				}

				
				lblTitle.Text = ApplicationData.Instance.getTranslator ().translateMessage ("formmessages.title");
				btnSend2.Text = ApplicationData.Instance.getTranslator ().translateMessage ("formmessages.reply");
				btnSend2.Click += ReplyMessage;
				//btnSuj.Visibility = ViewStates.Invisible;
			}
			else if (m_MessageType == TextMessage.MSG_OUTBOX) {

				if (ApplicationData.Instance.CurrentTextMessage != null)
				{
					txMsg.Text = ApplicationData.Instance.CurrentTextMessage.Message;
					txMsg.Text += System.Environment.NewLine;
					txMsg.Text += System.Environment.NewLine;
					txMsg.Text += ApplicationData.Instance.CurrentTextMessage.ActionDate.ToShortDateString();
					txMsg.Text += System.Environment.NewLine;
					txMsg.Text += ApplicationData.Instance.CurrentTextMessage.ActionDate.ToShortTimeString();									
					
				}

				lblTitle.Text = ApplicationData.Instance.getTranslator ().translateMessage ("formmessages.title");
				//btnSend2.Text = ApplicationData.Instance.getTranslator ().translateMessage ("formmessages.reply");
				//btnSend2.Click += ReplyMessage;

				//btnSuj.Visibility = ViewStates.Invisible;
			}

			btnSend.Visibility = ViewStates.Invisible;


			if (m_MessageType == TextMessage.MSG_OUTBOX) {
				btnSend2.Visibility = ViewStates.Invisible;

			} else {
				//btnSend.Visibility = ViewStates.Visible;
				btnSend2.Visibility = ViewStates.Visible;
			}

			string x = "";
			if(!(Intent.GetStringExtra("V1") == null))
			x = Intent.GetStringExtra("V1");
			if(x.Equals("2"))
				txMsg.Text = txMsg.Text + Intent.GetStringExtra("V2") + " ";

		}
		
		protected void goBack()
		{
			Finish();
			
		}

		protected void ReplyMessage(object sender, EventArgs e)
		{
			m_MessageType = TextMessage.MSG_NEW;
			btnSend2.Click -= ReplyMessage;
			initView ();
		}

		protected void SendMessage(object sender, EventArgs e)
		{
			if (txMsg.Text.Length > 0) {
				TextMessage _msg = new TextMessage ();
				_msg.Status = TextMessage.STATUS_TOBESENT;
				_msg.ActionDate = DateTime.Now;
				_msg.ArrivalDate = DateTime.Now;
				_msg.Sender = ApplicationData.Instance.getConfigurationModel ().getUserName ();
				_msg.Message = txMsg.Text;
			
				ApplicationData.Instance.setOutboxIndicator (ApplicationData.Instance.getOutboxIndicator () + 1);
			
				ApplicationActions.Instance.sendTextMessage (_msg);
			}

			Finish();
			
		}

		protected override void OnStop()
		{
			base.OnStop();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy ();

		}

		protected override void OnPause()
		{
			base.OnPause ();

		}


	}
}

