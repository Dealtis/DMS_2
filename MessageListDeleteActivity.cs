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
	[Activity (Label = "MessageListDeleteActivity",Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]			
	public class MessageListDeleteActivity : Activity
	{
		ListView listView;
		int m_ListType;
		ImageButton btnPrev=null;
		List<TextMessage> msgList;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.MessageListDelete);

			m_ListType = Intent.GetIntExtra("ListType", 1);



			InitView();

		}

		private void InitView()
		{

			TextView lblTitle = FindViewById<TextView>(Resource.Id.lblTitle);


			/*if (m_ListType == TextMessage.MSG_INBOX)
				lblTitle.Text =  ApplicationData.Instance.getTranslator().translateMessage("formmessages.menuinbox");
			else if (m_ListType == TextMessage.MSG_OUTBOX)
				lblTitle.Text =  ApplicationData.Instance.getTranslator().translateMessage("formmessages.menusentbox");
*/
			lblTitle.Text =  ApplicationData.Instance.getTranslator().translateMessage("formmessages.delete") + " " + 
			                ApplicationData.Instance.getTranslator().translateMessage("formmessages.title");



			listView = FindViewById<ListView>(Resource.Id.listView1);
			listView.ItemClick += OnListItemClick;  // to be defined

			Button deleteSel = FindViewById<Button>(Resource.Id.button1);
			deleteSel.Text = ApplicationData.Instance.getTranslator().translateMessage("btnDelSel");
			deleteSel.Click += OnDeleteSelected;

			Button deleteAll = FindViewById<Button>(Resource.Id.button2);
			deleteAll.Text = ApplicationData.Instance.getTranslator().translateMessage("btnDelAll");
			deleteAll.Click += OnDeleteAll;


			msgList = ApplicationActions.Instance.loadMessages(m_ListType);
			msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p2.ArrivalDate.CompareTo(p1.ArrivalDate);});



			listView.Adapter = new MessageListDeleteAdaptor(this, msgList);
			listView.ChoiceMode = Android.Widget.ChoiceMode.Multiple;

			btnPrev = FindViewById<ImageButton> (Resource.Id.imageButton1);
			btnPrev.Click += delegate { goBack();	};

		}

		protected void OnDeleteSelected(object sender, EventArgs e)
		{
			foreach( TextMessage msg in msgList)
			{
				if (msg.isCheckedItem ())
					ApplicationActions.Instance.deleteMessage (msg, m_ListType);
			}
			InitView ();
		}

		protected void OnDeleteAll(object sender, EventArgs e)
		{
			ApplicationActions.Instance.deleteAllMessage (m_ListType);
			InitView ();
		} 

		protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			listView = sender as ListView;

			TextMessage msg = msgList[e.Position];

			if (msg.isCheckedItem ())
				msg.setCheckedItem (false);
			else msg.setCheckedItem (true);

		}

		protected void goBack()
		{
			Finish();

		}



	}
}

