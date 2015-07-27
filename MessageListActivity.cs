
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
	[Activity (Label = "MessageListActivity",Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]			
	public class MessageListActivity : Activity
	{
		ListView listView;
		int m_ListType;
		ImageButton btnPrev=null;
		List<TextMessage> msgList;
		int msgActivityId = 0;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.MessageList);

			m_ListType = Intent.GetIntExtra("ListType", 1);
			ApplicationData.Instance.setMessageListOrdering (1);


			//InitView();

		}

		private void InitView()
		{
			if (msgActivityId != 0) {
				FinishActivity (msgActivityId);
			
				msgActivityId = 0;
			}

			TextView lblTitle = FindViewById<TextView>(Resource.Id.lblTitle);


			if (m_ListType == TextMessage.MSG_INBOX)
				lblTitle.Text =  ApplicationData.Instance.getTranslator().translateMessage("formmessages.menuinbox");
			else if (m_ListType == TextMessage.MSG_OUTBOX)
				lblTitle.Text =  ApplicationData.Instance.getTranslator().translateMessage("formmessages.menusentbox");


			listView = FindViewById<ListView>(Resource.Id.listView1);
			listView.ItemClick += OnListItemClick;  // to be defined

			msgList = ApplicationActions.Instance.loadMessages(m_ListType);
			if (ApplicationData.Instance.getMessageListOrdering() == 0)
				msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p1.ArrivalDate.CompareTo(p2.ArrivalDate);});
			else if (ApplicationData.Instance.getMessageListOrdering() == 1)
				msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p2.ArrivalDate.CompareTo(p1.ArrivalDate);});
			else if (ApplicationData.Instance.getMessageListOrdering() == 2)
				msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p1.Status.CompareTo(p2.Status);});
			else if (ApplicationData.Instance.getMessageListOrdering() == 3)
				msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p2.Status.CompareTo(p1.Status);});
			else msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p2.ArrivalDate.CompareTo(p1.ArrivalDate);});

			

			
			listView.Adapter = new MessageListAdapter(this, msgList);

			btnPrev = FindViewById<ImageButton> (Resource.Id.imageButton1);
			btnPrev.Click += delegate { goBack();	};

			ImageButton btnSend = FindViewById<ImageButton> (Resource.Id.imageButton2);
			btnSend.Visibility = ViewStates.Invisible;


			this.Title = "DMS";

		}

		protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			listView = sender as ListView;

			TextMessage msg = msgList[e.Position];

			if (m_ListType == TextMessage.MSG_INBOX) 
			{
				ApplicationActions.Instance.setTextMessageRead (msg.Id);
			}

			ApplicationData.Instance.CurrentTextMessage = msg;
			Intent i = new Intent(this, typeof(NewMessageActivity));
			i.PutExtra("MessageType", m_ListType);
			//StartActivity(i);
			if (msgActivityId != 0) {
				FinishActivity (msgActivityId);
				msgActivityId = 0;
			}
			msgActivityId = 1;
			StartActivityForResult(i, 1);
		}

		protected void goBack()
		{
			Finish();
			
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			menu.Add (0, 0, 0, ApplicationData.Instance.getTranslator ().translateMessage ("formmessagelist.delete"));
			menu.Add (0, 1, 1, ApplicationData.Instance.getTranslator ().translateMessage ("menusort"));



			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case 0:
				Intent i = new Intent (this, typeof(MessageListDeleteActivity));
				i.PutExtra ("ListType", m_ListType);
				StartActivity (i);
				return true;
			case 1:
				Intent i2 = new Intent (this, typeof(MessageListSortingActivity));
				i2.PutExtra ("order", ApplicationData.Instance.getMessageListOrdering());
				StartActivity (i2);
				return true;
			default:
				return base.OnOptionsItemSelected (item);


			}
		}

		protected override void OnResume()
		{
			base.OnResume ();

			InitView ();
		}


	}
}

