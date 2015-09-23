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


			MessageBoxAdapter adapter = new MessageBoxAdapter (this, mItems);
			mListView.Adapter = adapter;
			//mListView.ItemClick += MListView_ItemClick;
			//mListView.ItemLongClick += MListView_ItemLongClick;
		}
	}
}

