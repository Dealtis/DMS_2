using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;

using Android.Text;
using Android.Views;
using Android.Widget;
using DMSvStandard.ORM;
using SQLite;
using AndroidHUD;
using ZXing;

namespace DMSvStandard
{
	[Activity (Label = "ActivityChat",Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]			
	public class ActivityChat : Activity
	{	

		private List<Message> mItems;
		private ListView mListView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.ChatLayout);

			//LISTVIEW
			mListView = FindViewById<ListView> (Resource.Id.listViewBox);

			mItems = new List<Message> ();

			DBRepository dbr = new DBRepository ();
		
			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);


			var table = db.Query<Message> ("SELECT * FROM Message where codeChauffeur=?",ApplicationData.UserAndsoft);
			var i = 0;

			foreach (var item in table) {
				mItems.Add (new Message () {
					texteMessage = item.texteMessage,
					utilisateurEmetteur = item.utilisateurEmetteur,
					statutMessage = item.statutMessage,
					dateImportMessage = item.dateImportMessage,
					typeMessage = item.typeMessage,
					Id = item.Id
				});
				i++;
			}

			if(i > 3){
				View view = LayoutInflater.From (this).Inflate (Resource.Layout.ListeViewDelete, null, false);
				mListView.AddHeaderView (view);
				view.Click += Btndeletemsg_Click;
			}

			MessageBoxAdapter adapter = new MessageBoxAdapter (this, mItems);
			mListView.Adapter = adapter;
			//mListView.ItemClick += MListView_ItemClick;
			//mListView.ItemLongClick += MListView_ItemLongClick;

			//EDITTEXT
			var btnsend = FindViewById<Button>(Resource.Id.btnsend);
			btnsend.Click += Btnsend_Click;

			//STATUT DES MESSAGES RECU TO 1

			var tablemsgrecu = db.Query<Message> ("SELECT * FROM Message where statutMessage = 0");
			foreach (var item in tablemsgrecu) {
				var updatestatutmessage = db.Query<Message> ("UPDATE Message SET statutMessage = 1 WHERE statutMessage = 0");
			}

		}

		void  Btnsend_Click (object sender, EventArgs e){

			DBRepository dbr = new DBRepository ();
			var newmessage = FindViewById<TextView>(Resource.Id.editnewmsg);
			if (newmessage.Text == "") {
				
			} else {
				var resinteg = dbr.InsertDataMessage (ApplicationData.UserAndsoft,"", newmessage.Text,2, DateTime.Now, 2,0);

			}
		


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);


//			var table = db.Query<Message> ("SELECT * FROM Message");
//
//
//			foreach (var item in table) {
//
//				mItems.Add (new Message () {
//					texte = item.texte,
//					utilisateurAndsoft = item.utilisateurAndsoft,
//					statut = item.statut,
//					datemessage = item.datemessage,
//					typemsg = item.typemsg,
//					Id = item.Id
//				});
//			}


//			MessageBoxAdapter adapter = new MessageBoxAdapter (this, mItems);
//			mListView.Adapter = adapter;

			StartActivity(typeof(ActivityChat));
			
		}


		void  Btndeletemsg_Click (object sender, EventArgs e){

			DBRepository dbr = new DBRepository ();
			var newmessage = FindViewById<TextView>(Resource.Id.editnewmsg);


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);


//
//			var table = db.Query<Message> ("SELECT * FROM Message");
//
//
//			foreach (var item in table) {
//
//				mItems.Add (new Message () {
//					texte = item.texte,
//					utilisateurAndsoft = item.utilisateurAndsoft,
//					statut = item.statut,
//					datemessage = item.datemessage,
//					typemsg = item.typemsg,
//					Id = item.Id
//				});
//			}

			var del = dbr.DropTableMessage();

			//MessageBoxAdapter adapter = new MessageBoxAdapter (this, mItems);
			//mListView.Adapter = adapter;

			StartActivity(typeof(ActivityChat));

		}

		public override void OnBackPressed ()
		{
			StartActivity(typeof(MainActivity));
		}   
	}
}

