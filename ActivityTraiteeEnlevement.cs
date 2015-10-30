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
using DMSvStandard.ORM;

using System.Data;
using System.IO;
using SQLite;
using AndroidHUD;
using Android.Text;
using Android.Graphics;
namespace DMSvStandard
{
	[Activity (Label = "ActivityTraitee",Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]			
	public class ActivityTraiteeEnlevement : Activity
	{	
		private List<Livraison> mItems;
		private ListView mListView;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView(Resource.Layout.ListeTraiteeEnlevement);

			DBRepository dbr = new DBRepository();
			var result = dbr.SelectData();


			string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);


			var table = db.Query<ToDoTask>("SELECT * FROM ToDoTask WHERE StatutLivraison = '1' AND typeMission='C' AND typeSegment='RAM' OR StatutLivraison = '2' AND typeMission='C'AND typeSegment='RAM' ORDER BY _Id DESC");
			var layout = new LinearLayout(this);


			//GRP
			Appli.nbgroupage = 0;
			var grp = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '1' AND typeMission='C' AND typeSegment='RAM' OR StatutLivraison = '2' AND typeMission='C' AND typeSegment='RAM' GROUP BY groupage");
			var i = 0;
			foreach (var item in grp){
				Appli.groupagestring = Convert.ToString(item.groupage);
				Appli.grp[i] = item.groupage;
				Appli.nbgroupage++;
				i++;
			}


			//LISTVIEW
			mListView = FindViewById<ListView>(Resource.Id.listView1);

			mItems = new List<Livraison> ();

			foreach(var item in table){

				mItems.Add(new Livraison() {
					numCommande = item.numCommande,
					nomClient = item.nomClient.Truncate(25),
					refClient = item.refClient,
					nomPayeur = item.nomPayeur,
					adresseLivraison = item.adresseLivraison,
					CpLivraison = item.CpLivraison,
					villeLivraison = item.villeLivraison.Truncate(10),
					dateHeure = item.dateHeure,
					nbrColis = item.nbrColis,
					nbrPallette = item.nbrPallette,
					poids = item.poids,
					adresseExpediteur = item.adresseExpediteur,
					CpExpediteur = item.CpExpediteur,
					dateExpe = item.dateExpe,
					villeExpediteur = item.villeExpediteur,
					nomExpediteur = item.nomExpediteur,
					StatutLivraison = item.StatutLivraison,
					instrucLivraison = (item.instrucLivraison).Truncate(25),
					groupage = item.groupage,
					planDeTransport = item.planDeTransport,
					typeMission = item.typeMission,
					typeSegment = item.typeSegment,
					CR = item.CR,
					imgpath = item.imgpath,
					Id = Convert.ToString(item.Id)});
			};

			MyListViewAdapter adapter = new MyListViewAdapter (this, mItems);
			mListView.Adapter = adapter;
			mListView.ItemClick += MListView_ItemClick;

			Button btnLivraison = FindViewById<Button> (Resource.Id.btnLivraison);
			btnLivraison.Click += BtnLivraison_Click;
			Typeface tf = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaBold.ttf");
			btnLivraison.SetTypeface(tf, TypefaceStyle.Normal);


			//BTN SEARCH
			Button btnsearchb = FindViewById<Button> (Resource.Id.btnsearch);
			btnsearchb.Click += Btnsearch_Click;


			Button btngrpAll = FindViewById<Button> (Resource.Id.toutgrp);
			btngrpAll.Click += BtngrpAll_Click;

			Button btngrpUn = FindViewById<Button> (Resource.Id.grp1);
			btngrpUn.Click += BtngrpUn_Click;

			Button btngrpDeux = FindViewById<Button> (Resource.Id.grp2);
			btngrpDeux.Click += BtngrpDeux_Click;

			Button btngrpTrois = FindViewById<Button> (Resource.Id.grp3);
			btngrpTrois.Click += BtngrpTroisClick;

			Button btngrpQuatre = FindViewById<Button> (Resource.Id.grp4);
			btngrpQuatre.Click += BtngrpQuatre_Click;

			Button btngrpCinq = FindViewById<Button> (Resource.Id.grp5);
			btngrpCinq.Click += BtngrpCinq_Click;


			TextView textViewGrp = FindViewById<TextView> (Resource.Id.textViewGrp);

			textViewGrp.Visibility=ViewStates.Gone;
			btngrpAll.Visibility=ViewStates.Gone;
			btngrpUn.Visibility=ViewStates.Gone;
			btngrpDeux.Visibility=ViewStates.Gone;
			btngrpTrois.Visibility=ViewStates.Gone;
			btngrpQuatre.Visibility=ViewStates.Gone;
			btngrpCinq.Visibility=ViewStates.Gone;

			if (Appli.nbgroupage == 0) {
				btngrpUn.Visibility=ViewStates.Visible;
				btngrpUn.Text = "Pas d'enlevement traitée";
				btngrpUn.SetWidth (5000);
			} else {}
			if (Appli.nbgroupage == 1) {
				btngrpUn.Visibility=ViewStates.Visible;
				btngrpUn.Text = Appli.grp [0];
				btngrpUn.SetWidth (5000);
			} else {}
			if (Appli.nbgroupage == 2) {
				btngrpAll.Visibility=ViewStates.Visible;
				btngrpUn.Visibility=ViewStates.Visible;
				btngrpDeux.Visibility=ViewStates.Visible;

				btngrpAll.SetBackgroundColor(Color.DarkBlue);

				btngrpUn.Text = Appli.grp [0];
				btngrpDeux.Text = Appli.grp [1];
			} else {}
			if (Appli.nbgroupage == 3) {
				btngrpAll.Visibility=ViewStates.Visible;
				btngrpUn.Visibility=ViewStates.Visible;
				btngrpDeux.Visibility=ViewStates.Visible;
				btngrpTrois.Visibility=ViewStates.Visible;

				btngrpAll.SetBackgroundColor(Color.DarkBlue);

			} else {}
			if (Appli.nbgroupage == 4) {
				btngrpAll.Visibility=ViewStates.Visible;
				btngrpUn.Visibility=ViewStates.Visible;
				btngrpDeux.Visibility=ViewStates.Visible;
				btngrpTrois.Visibility=ViewStates.Visible;
				btngrpQuatre.Visibility=ViewStates.Visible;

				btngrpAll.SetBackgroundColor(Color.DarkBlue);

			} else {}
			if (Appli.nbgroupage == 5) {
				btngrpAll.Visibility=ViewStates.Visible;
				btngrpUn.Visibility=ViewStates.Visible;
				btngrpDeux.Visibility=ViewStates.Visible;
				btngrpTrois.Visibility=ViewStates.Visible;
				btngrpQuatre.Visibility=ViewStates.Visible;
				btngrpCinq.Visibility=ViewStates.Visible;

				btngrpAll.SetBackgroundColor(Color.DarkBlue);

			} else {}

		}

		void BtngrpAll_Click (object sender, EventArgs e)
		{



			Button btngrpAll = FindViewById<Button> (Resource.Id.toutgrp);
			Button btngrpUn = FindViewById<Button> (Resource.Id.grp1);
			Button btngrpDeux = FindViewById<Button> (Resource.Id.grp2);	
			Button btngrpTrois = FindViewById<Button> (Resource.Id.grp3);
			Button btngrpQuatre = FindViewById<Button> (Resource.Id.grp4);
			Button btngrpCinq = FindViewById<Button> (Resource.Id.grp5);
			TextView textViewGrp = FindViewById<TextView> (Resource.Id.textViewGrp);

			textViewGrp.Visibility = ViewStates.Gone;

			btngrpAll.SetBackgroundColor(Color.DarkBlue);
			btngrpUn.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.jaune_button);

			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '1' AND typeMission='C' OR StatutLivraison = '2' AND typeMission='C' ORDER BY _Id DESC");
			//LISTVIEW
			mListView = FindViewById<ListView> (Resource.Id.listView1);

			mItems = new List<Livraison> ();


			foreach (var item in table) {

				mItems.Add (new Livraison () {
					numCommande = item.numCommande,
					nomClient = item.nomClient.Truncate (25),
					refClient = item.refClient,
					nomPayeur = item.nomPayeur,
					adresseLivraison = item.adresseLivraison,
					CpLivraison = item.CpLivraison,
					villeLivraison = item.villeLivraison.Truncate (10),
					dateHeure = item.dateHeure,
					nbrColis = item.nbrColis,
					nbrPallette = item.nbrPallette,
					poids = item.poids,
					adresseExpediteur = item.adresseExpediteur,
					CpExpediteur = item.CpExpediteur,
					dateExpe = item.dateExpe,
					villeExpediteur = item.villeExpediteur,
					nomExpediteur = item.nomExpediteur,
					StatutLivraison = item.StatutLivraison,
					instrucLivraison = (item.instrucLivraison).Truncate (25),
					groupage = item.groupage,
					planDeTransport = item.planDeTransport,
					typeMission = item.typeMission,
					typeSegment = item.typeSegment,
					CR = item.CR,
					imgpath = item.imgpath,
					Id = Convert.ToString (item.Id)
				});
			}
			;
			MyListViewAdapter adapter = new MyListViewAdapter (this, mItems);
			mListView.Adapter = adapter;
		}
		void BtngrpUn_Click (object sender, EventArgs e)
		{	

			Button btngrpAll = FindViewById<Button> (Resource.Id.toutgrp);
			Button btngrpUn = FindViewById<Button> (Resource.Id.grp1);
			Button btngrpDeux = FindViewById<Button> (Resource.Id.grp2);	
			Button btngrpTrois = FindViewById<Button> (Resource.Id.grp3);
			Button btngrpQuatre = FindViewById<Button> (Resource.Id.grp4);
			Button btngrpCinq = FindViewById<Button> (Resource.Id.grp5);
			TextView textViewGrp = FindViewById<TextView> (Resource.Id.textViewGrp);

			textViewGrp.Visibility = ViewStates.Visible;
			textViewGrp.Text = Appli.grp[0];

			btngrpAll.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpUn.SetBackgroundColor(Color.DarkBlue);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.jaune_button);


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '1' AND typeMission='C' AND groupage='"+Appli.grp[0]+ "' OR StatutLivraison = '2' AND typeMission='C' AND groupage='"+Appli.grp[0]+"' ORDER BY _Id DESC");
			//LISTVIEW
			mListView = FindViewById<ListView> (Resource.Id.listView1);


			mItems = new List<Livraison> ();


			foreach (var item in table) {

				mItems.Add (new Livraison () {
					numCommande = item.numCommande,
					nomClient = item.nomClient.Truncate (25),
					refClient = item.refClient,
					nomPayeur = item.nomPayeur,
					adresseLivraison = item.adresseLivraison,
					CpLivraison = item.CpLivraison,
					villeLivraison = item.villeLivraison.Truncate (10),
					dateHeure = item.dateHeure,
					nbrColis = item.nbrColis,
					nbrPallette = item.nbrPallette,
					poids = item.poids,
					adresseExpediteur = item.adresseExpediteur,
					CpExpediteur = item.CpExpediteur,
					dateExpe = item.dateExpe,
					villeExpediteur = item.villeExpediteur,
					nomExpediteur = item.nomExpediteur,
					StatutLivraison = item.StatutLivraison,
					instrucLivraison = (item.instrucLivraison).Truncate (25),
					groupage = item.groupage,
					planDeTransport = item.planDeTransport,
					typeMission = item.typeMission,
					typeSegment = item.typeSegment,
					CR = item.CR,
					imgpath = item.imgpath,
					Id = Convert.ToString (item.Id)
				});
			}
			;

			MyListViewAdapter adapter = new MyListViewAdapter (this, mItems);
			mListView.Adapter = adapter;

		}
		void BtngrpDeux_Click (object sender, EventArgs e)
		{



			Button btngrpAll = FindViewById<Button> (Resource.Id.toutgrp);
			Button btngrpUn = FindViewById<Button> (Resource.Id.grp1);
			Button btngrpDeux = FindViewById<Button> (Resource.Id.grp2);	
			Button btngrpTrois = FindViewById<Button> (Resource.Id.grp3);
			Button btngrpQuatre = FindViewById<Button> (Resource.Id.grp4);
			Button btngrpCinq = FindViewById<Button> (Resource.Id.grp5);
			TextView textViewGrp = FindViewById<TextView> (Resource.Id.textViewGrp);

			textViewGrp.Visibility = ViewStates.Visible;
			textViewGrp.Text = Appli.grp[1];

			btngrpAll.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpDeux.SetBackgroundColor(Color.DarkBlue);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.jaune_button);

			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '1' AND typeMission='C' AND groupage='"+Appli.grp[1]+"' OR StatutLivraison = '2' AND typeMission='C' AND groupage='"+Appli.grp[1]+"' ORDER BY _Id DESC");
			//LISTVIEW
			mListView = FindViewById<ListView> (Resource.Id.listView1);

			mItems = new List<Livraison> ();


			foreach (var item in table) {

				mItems.Add (new Livraison () {
					numCommande = item.numCommande,
					nomClient = item.nomClient.Truncate (25),
					refClient = item.refClient,
					nomPayeur = item.nomPayeur,
					adresseLivraison = item.adresseLivraison,
					CpLivraison = item.CpLivraison,
					villeLivraison = item.villeLivraison.Truncate (10),
					dateHeure = item.dateHeure,
					nbrColis = item.nbrColis,
					nbrPallette = item.nbrPallette,
					poids = item.poids,
					adresseExpediteur = item.adresseExpediteur,
					CpExpediteur = item.CpExpediteur,
					dateExpe = item.dateExpe,
					villeExpediteur = item.villeExpediteur,
					nomExpediteur = item.nomExpediteur,
					StatutLivraison = item.StatutLivraison,
					instrucLivraison = (item.instrucLivraison).Truncate (25),
					groupage = item.groupage,
					planDeTransport = item.planDeTransport,
					typeMission = item.typeMission,
					typeSegment = item.typeSegment,
					CR = item.CR,
					imgpath = item.imgpath,
					Id = Convert.ToString (item.Id)
				});
			}


			MyListViewAdapter adapter = new MyListViewAdapter (this, mItems);
			mListView.Adapter = adapter;

		}

		void BtngrpTroisClick (object sender, EventArgs e)
		{
			Button btngrpAll = FindViewById<Button> (Resource.Id.toutgrp);
			Button btngrpUn = FindViewById<Button> (Resource.Id.grp1);
			Button btngrpDeux = FindViewById<Button> (Resource.Id.grp2);	
			Button btngrpTrois = FindViewById<Button> (Resource.Id.grp3);
			Button btngrpQuatre = FindViewById<Button> (Resource.Id.grp4);
			Button btngrpCinq = FindViewById<Button> (Resource.Id.grp5);
			TextView textViewGrp = FindViewById<TextView> (Resource.Id.textViewGrp);

			textViewGrp.Visibility = ViewStates.Visible;
			textViewGrp.Text = Appli.grp[2];

			btngrpAll.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpTrois.SetBackgroundColor(Color.DarkBlue);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.jaune_button);


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '1' AND typeMission='C' AND groupage='"+Appli.grp[2]+"' OR StatutLivraison = '2' AND typeMission='C' AND groupage='"+Appli.grp[2]+"' ORDER BY _Id DESC");

			//LISTVIEW
			mListView = FindViewById<ListView> (Resource.Id.listView1);

			mItems = new List<Livraison> ();


			foreach (var item in table) {

				mItems.Add (new Livraison () {
					numCommande = item.numCommande,
					nomClient = item.nomClient.Truncate (25),
					refClient = item.refClient,
					nomPayeur = item.nomPayeur,
					adresseLivraison = item.adresseLivraison,
					CpLivraison = item.CpLivraison,
					villeLivraison = item.villeLivraison.Truncate (10),
					dateHeure = item.dateHeure,
					nbrColis = item.nbrColis,
					nbrPallette = item.nbrPallette,
					poids = item.poids,
					adresseExpediteur = item.adresseExpediteur,
					CpExpediteur = item.CpExpediteur,
					dateExpe = item.dateExpe,
					villeExpediteur = item.villeExpediteur,
					nomExpediteur = item.nomExpediteur,
					StatutLivraison = item.StatutLivraison,
					instrucLivraison = (item.instrucLivraison).Truncate (25),
					groupage = item.groupage,
					planDeTransport = item.planDeTransport,
					typeMission = item.typeMission,
					typeSegment = item.typeSegment,
					CR = item.CR,
					imgpath = item.imgpath,
					Id = Convert.ToString (item.Id)
				});
			}


			MyListViewAdapter adapter = new MyListViewAdapter (this, mItems);
			mListView.Adapter = adapter;
		}

		void BtngrpQuatre_Click (object sender, EventArgs e)
		{
			Button btngrpAll = FindViewById<Button> (Resource.Id.toutgrp);
			Button btngrpUn = FindViewById<Button> (Resource.Id.grp1);
			Button btngrpDeux = FindViewById<Button> (Resource.Id.grp2);	
			Button btngrpTrois = FindViewById<Button> (Resource.Id.grp3);
			Button btngrpQuatre = FindViewById<Button> (Resource.Id.grp4);
			Button btngrpCinq = FindViewById<Button> (Resource.Id.grp5);
			TextView textViewGrp = FindViewById<TextView> (Resource.Id.textViewGrp);

			textViewGrp.Visibility = ViewStates.Visible;
			textViewGrp.Text = Appli.grp[3];

			btngrpAll.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpQuatre.SetBackgroundColor(Color.DarkBlue);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.jaune_button);

			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);



			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '1' AND typeMission='C' AND groupage='"+Appli.grp[3]+"' OR StatutLivraison = '2' AND typeMission='C' AND groupage='"+Appli.grp[3]+"' ORDER BY _Id DESC");

			//LISTVIEW
			mListView = FindViewById<ListView> (Resource.Id.listView1);

			mItems = new List<Livraison> ();


			foreach (var item in table) {

				mItems.Add (new Livraison () {
					numCommande = item.numCommande,
					nomClient = item.nomClient.Truncate (25),
					refClient = item.refClient,
					nomPayeur = item.nomPayeur,
					adresseLivraison = item.adresseLivraison,
					CpLivraison = item.CpLivraison,
					villeLivraison = item.villeLivraison.Truncate (10),
					dateHeure = item.dateHeure,
					nbrColis = item.nbrColis,
					nbrPallette = item.nbrPallette,
					poids = item.poids,
					adresseExpediteur = item.adresseExpediteur,
					CpExpediteur = item.CpExpediteur,
					dateExpe = item.dateExpe,
					villeExpediteur = item.villeExpediteur,
					nomExpediteur = item.nomExpediteur,
					StatutLivraison = item.StatutLivraison,
					instrucLivraison = (item.instrucLivraison).Truncate (25),
					groupage = item.groupage,
					planDeTransport = item.planDeTransport,
					typeMission = item.typeMission,
					typeSegment = item.typeSegment,
					CR = item.CR,
					imgpath = item.imgpath,
					Id = Convert.ToString (item.Id)
				});
			}


			MyListViewAdapter adapter = new MyListViewAdapter (this, mItems);
			mListView.Adapter = adapter;


		}

		void BtngrpCinq_Click (object sender, EventArgs e)
		{
			Button btngrpAll = FindViewById<Button> (Resource.Id.toutgrp);
			Button btngrpUn = FindViewById<Button> (Resource.Id.grp1);
			Button btngrpDeux = FindViewById<Button> (Resource.Id.grp2);	
			Button btngrpTrois = FindViewById<Button> (Resource.Id.grp3);
			Button btngrpQuatre = FindViewById<Button> (Resource.Id.grp4);
			Button btngrpCinq = FindViewById<Button> (Resource.Id.grp5);
			TextView textViewGrp = FindViewById<TextView> (Resource.Id.textViewGrp);

			textViewGrp.Visibility = ViewStates.Visible;
			textViewGrp.Text = Appli.grp[4];

			btngrpAll.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpCinq.SetBackgroundColor(Color.DarkBlue);


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);



			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '1' AND typeMission='C' AND groupage='"+Appli.grp[4]+"' OR StatutLivraison = '2' AND typeMission='C' AND groupage='"+Appli.grp[4]+"' ORDER BY _Id DESC");

			//LISTVIEW
			mListView = FindViewById<ListView> (Resource.Id.listView1);

			mItems = new List<Livraison> ();


			foreach (var item in table) {

				mItems.Add (new Livraison () {
					numCommande = item.numCommande,
					nomClient = item.nomClient.Truncate (25),
					refClient = item.refClient,
					nomPayeur = item.nomPayeur,
					adresseLivraison = item.adresseLivraison,
					CpLivraison = item.CpLivraison,
					villeLivraison = item.villeLivraison.Truncate (10),
					dateHeure = item.dateHeure,
					nbrColis = item.nbrColis,
					nbrPallette = item.nbrPallette,
					poids = item.poids,
					adresseExpediteur = item.adresseExpediteur,
					CpExpediteur = item.CpExpediteur,
					dateExpe = item.dateExpe,
					villeExpediteur = item.villeExpediteur,
					nomExpediteur = item.nomExpediteur,
					StatutLivraison = item.StatutLivraison,
					instrucLivraison = (item.instrucLivraison).Truncate (25),
					groupage = item.groupage,
					planDeTransport = item.planDeTransport,
					typeMission = item.typeMission,
					typeSegment = item.typeSegment,
					CR = item.CR,
					imgpath = item.imgpath,
					Id = Convert.ToString (item.Id)
				});
			}


			MyListViewAdapter adapter = new MyListViewAdapter (this, mItems);
			mListView.Adapter = adapter;



		}

		void Btnsearch_Click (object sender, EventArgs e)
		{	


			Button btngrpAll = FindViewById<Button> (Resource.Id.toutgrp);
			Button btngrpUn = FindViewById<Button> (Resource.Id.grp1);
			Button btngrpDeux = FindViewById<Button> (Resource.Id.grp2);	
			Button btngrpTrois = FindViewById<Button> (Resource.Id.grp3);
			Button btngrpQuatre = FindViewById<Button> (Resource.Id.grp4);
			Button btngrpCinq = FindViewById<Button> (Resource.Id.grp5);
			TextView textViewGrp = FindViewById<TextView> (Resource.Id.textViewGrp);

			textViewGrp.Visibility = ViewStates.Visible;


			btngrpAll.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.jaune_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.jaune_button);


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			AlertDialog.Builder builder = new AlertDialog.Builder(this);


			EditText input = new EditText(this);
			builder.SetTitle("Rechercher");

			var viewAD = this.LayoutInflater.Inflate (Resource.Layout.btncodebarre, null);
			EditText editrecherche =  viewAD.FindViewById<EditText> (Resource.Id.editrecherche);
			builder.SetNeutralButton("Recherche par code barre", delegate {
				scan();


			});
			builder.SetView (viewAD);
			builder.SetCancelable(false);
			builder.SetPositiveButton("Annuler", delegate {  });
			builder.SetNegativeButton("Chercher", delegate {

				var table = db.Query<ToDoTask>("SELECT * FROM ToDoTask WHERE typeMission='C' AND typeSegment='RAM' AND (numCommande LIKE '%"+editrecherche.Text+"%' OR  villeLivraison LIKE '%"+editrecherche.Text+"%' OR nomPayeur LIKE '%"+editrecherche.Text+"%'OR CpLivraison LIKE '%"+editrecherche.Text+"%' OR refClient LIKE '%"+editrecherche.Text+"%') OR typeMission='C' AND typeSegment='RAM' AND (numCommande LIKE '%"+editrecherche.Text+"%' OR  villeLivraison LIKE '%"+editrecherche.Text+"%' OR nomPayeur LIKE '%"+editrecherche.Text+"%'OR CpLivraison LIKE '%"+editrecherche.Text+"%' OR refClient LIKE '%"+editrecherche.Text+"%'OR nomClient LIKE'%"+editrecherche.Text+"%')");
				textViewGrp.Text = "Recherche \""+editrecherche.Text+"\"";

				//LISTVIEW
				mListView = FindViewById<ListView> (Resource.Id.listView1);

				mItems = new List<Livraison> ();


				foreach (var item in table) {

					mItems.Add (new Livraison () {
						numCommande = item.numCommande,
						nomClient = item.nomClient.Truncate (25),
						refClient = item.refClient,
						nomPayeur = item.nomPayeur,
						adresseLivraison = item.adresseLivraison,
						CpLivraison = item.CpLivraison,
						villeLivraison = item.villeLivraison.Truncate (10),
						dateHeure = item.dateHeure,
						nbrColis = item.nbrColis,
						nbrPallette = item.nbrPallette,
						poids = item.poids,
						adresseExpediteur = item.adresseExpediteur,
						CpExpediteur = item.CpExpediteur,
						dateExpe = item.dateExpe,
						villeExpediteur = item.villeExpediteur,
						nomExpediteur = item.nomExpediteur,
						StatutLivraison = item.StatutLivraison,
						instrucLivraison = (item.instrucLivraison).Truncate (25),
						groupage = item.groupage,
						planDeTransport = item.planDeTransport,
						typeMission = item.typeMission,
						typeSegment = item.typeSegment,
						CR = item.CR,
						imgpath = item.imgpath,
						Id = Convert.ToString (item.Id)
					});
				}


				MyListViewAdapter adapter = new MyListViewAdapter (this, mItems);
				mListView.Adapter = adapter;

			});
			builder.Show();


		}

		public async void scan() {

			var scanner = new ZXing.Mobile.MobileBarcodeScanner(this);

			var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
			options.PossibleFormats = new List<ZXing.BarcodeFormat>() { 
				ZXing.BarcodeFormat.CODE_128  //, ZXing.BarcodeFormat.Ean13 
			};
			var result =   await scanner.Scan(options);
			if (result != null) {
				Console.WriteLine ("Scanned Barcode: " + result.Text);
				App.codebarre = result.Text;
				listview ();
			}

		}
		public void listview(){

			Button btngrpAll = FindViewById<Button> (Resource.Id.toutgrp);
			Button btngrpUn = FindViewById<Button> (Resource.Id.grp1);
			Button btngrpDeux = FindViewById<Button> (Resource.Id.grp2);	
			Button btngrpTrois = FindViewById<Button> (Resource.Id.grp3);
			Button btngrpQuatre = FindViewById<Button> (Resource.Id.grp4);
			Button btngrpCinq = FindViewById<Button> (Resource.Id.grp5);
			TextView textViewGrp = FindViewById<TextView> (Resource.Id.textViewGrp);

			textViewGrp.Visibility = ViewStates.Visible;
			textViewGrp.Text = Appli.grp[3];

			btngrpAll.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpQuatre.SetBackgroundColor(Color.DarkBlue);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.bleu_button);

			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);
			var table = db.Query<ToDoTask>("SELECT * FROM ToDoTask WHERE typeMission='C' AND typeSegment='RAM' AND (numCommande LIKE '%"+App.codebarre+"%' OR  villeLivraison LIKE '%"+App.codebarre+"%' OR nomPayeur LIKE '%"+App.codebarre+"%'OR CpLivraison LIKE '%"+App.codebarre+"%' OR refClient LIKE '%"+App.codebarre+"%') OR typeMission='C' AND typeSegment='RAM' AND (numCommande LIKE '%"+App.codebarre+"%' OR  villeLivraison LIKE '%"+App.codebarre+"%' OR nomPayeur LIKE '%"+App.codebarre+"%'OR CpLivraison LIKE '%"+App.codebarre+"%' OR refClient LIKE '%"+App.codebarre+"%')");
			textViewGrp.Text = "Recherche \""+App.codebarre+"\"";

			//LISTVIEW
			mListView = FindViewById<ListView> (Resource.Id.listView1);

			mItems = new List<Livraison> ();


			foreach (var item in table) {

				mItems.Add (new Livraison () {
					numCommande = item.numCommande,
					nomClient = item.nomClient.Truncate (25),
					refClient = item.refClient,
					nomPayeur = item.nomPayeur,
					adresseLivraison = item.adresseLivraison,
					CpLivraison = item.CpLivraison,
					villeLivraison = item.villeLivraison.Truncate (10),
					dateHeure = item.dateHeure,
					nbrColis = item.nbrColis,
					nbrPallette = item.nbrPallette,
					poids = item.poids,
					adresseExpediteur = item.adresseExpediteur,
					CpExpediteur = item.CpExpediteur,
					dateExpe = item.dateExpe,
					villeExpediteur = item.villeExpediteur,
					nomExpediteur = item.nomExpediteur,
					StatutLivraison = item.StatutLivraison,
					instrucLivraison = (item.instrucLivraison).Truncate (25),
					groupage = item.groupage,
					planDeTransport = item.planDeTransport,
					typeMission = item.typeMission,
					typeSegment = item.typeSegment,
					CR = item.CR,
					imgpath = item.imgpath,
					Id = Convert.ToString (item.Id)
				});
			}
		}


		void MListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			var activity2 = new Intent(this, typeof(ActivityDetailEnlevement));
			activity2.PutExtra("ID", Convert.ToString(mItems[e.Position].Id));



			string id = Intent.GetStringExtra("ID");
			StartActivity(activity2);
		}

		void MListView_ItemLongClick (object sender, AdapterView.ItemLongClickEventArgs e)
		{	
			EditText input = new EditText(this);
			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetTitle("Suppression");
			builder.SetView (input);
			builder.SetMessage ("Voulez-vous supprimer cette enlevement ?");
			builder.SetCancelable(false);
			builder.SetPositiveButton("Oui", delegate {
				if(input.Text =="phiphi"){
					string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
						(System.Environment.SpecialFolder.Personal), "ormDMS.db3");

					var db = new SQLiteConnection (dbPath);

					var supprimeliv = db.Query<ToDoTask> ("delete from ToDoTask where numCommande ='" + mItems[e.Position].numCommande + "'");
					Console.Out.Write("Enlevement suppimer "+mItems[e.Position].numCommande);

					StartActivity (typeof(ActivityListEnlevement));

				}else{Toast.MakeText (this, "Mauvais MDP", ToastLength.Short).Show ();}

			});
			builder.SetNegativeButton("Non", delegate {

				AndHUD.Shared.ShowError(this, "Annulée!", AndroidHUD.MaskType.Clear, TimeSpan.FromSeconds(1.5));
			});


			builder.Show();
		}




		void BtnLivraison_Click (object sender, EventArgs e)
		{
			StartActivity(typeof(ActivityListEnlevement));
		}

		public override void OnBackPressed ()
		{
			StartActivity(typeof(MainActivity));
		}
	}
}


