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


			var table = db.Query<Message> ("SELECT * FROM Message");


			foreach (var item in table) {

				mItems.Add (new Message () {
					texte = item.texte,
					utilisateurAndsoft = item.utilisateurAndsoft,
					statut = item.statut,
					datemessage = item.datemessage,
					typemsg = item.typemsg,
					Id = item.Id
				});
			}

			View view = LayoutInflater.From (this).Inflate (Resource.Layout.ListeViewDelete, null, false);
			mListView.AddHeaderView (view);
			view.Click += Btndeletemsg_Click;
			MessageBoxAdapter adapter = new MessageBoxAdapter (this, mItems);
			mListView.Adapter = adapter;
			//mListView.ItemClick += MListView_ItemClick;
			//mListView.ItemLongClick += MListView_ItemLongClick;

			//EDITTEXT
			var btnsend = FindViewById<Button>(Resource.Id.btnsend);
			btnsend.Click += Btnsend_Click;




		}

		void  Btnsend_Click (object sender, EventArgs e){

			DBRepository dbr = new DBRepository ();
			var newmessage = FindViewById<TextView>(Resource.Id.editnewmsg);
			var resinteg = dbr.InsertDataMessage ("",newmessage.Text,3,DateTime.Now,2);

			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);


			var table = db.Query<Message> ("SELECT * FROM Message");


			foreach (var item in table) {

				mItems.Add (new Message () {
					texte = item.texte,
					utilisateurAndsoft = item.utilisateurAndsoft,
					statut = item.statut,
					datemessage = item.datemessage,
					typemsg = item.typemsg,
					Id = item.Id
				});
			}


			MessageBoxAdapter adapter = new MessageBoxAdapter (this, mItems);
			mListView.Adapter = adapter;


			
		}


		void  Btndeletemsg_Click (object sender, EventArgs e){

			DBRepository dbr = new DBRepository ();
			var newmessage = FindViewById<TextView>(Resource.Id.editnewmsg);
			var resinteg = dbr.InsertDataMessage ("",newmessage.Text,3,DateTime.Now,2);

			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			var del = db.Query<Message> ("DELETE FROM Message");

			var table = db.Query<Message> ("SELECT * FROM Message");


			foreach (var item in table) {

				mItems.Add (new Message () {
					texte = item.texte,
					utilisateurAndsoft = item.utilisateurAndsoft,
					statut = item.statut,
					datemessage = item.datemessage,
					typemsg = item.typemsg,
					Id = item.Id
				});
			}


			MessageBoxAdapter adapter = new MessageBoxAdapter (this, mItems);
			mListView.Adapter = adapter;



		}
	}
}

