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
	public static class Appli{
		
		public static string groupagestring;
		public static int nbgroupage;
		public static int verifdtb;
		public static string[] grp = new string[10];
	}


	[Activity(Label = "Livraison",Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class ActivityListLivraison : Activity
    {	


		private List<Livraison> mItems;
		private ListView mListView;


        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.ListeLivraisonMultiGrp);

			DBRepository dbr = new DBRepository ();
			var result = dbr.SelectData ();

			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);


			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV' AND Userandsoft = ? ORDER by groupage, Datemission",ApplicationData.UserAndsoft);
			var layout = new LinearLayout (this);
			layout.Orientation = Orientation.Vertical;

			//GRP
			Appli.nbgroupage = 0;
			var grp = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV'  AND Userandsoft = ?  GROUP BY groupage",ApplicationData.UserAndsoft);
			var i = 0;
			foreach (var item in grp){
				Appli.groupagestring = Convert.ToString(item.groupage);
				Appli.grp[i] = item.groupage;
				Appli.nbgroupage++;
				i++;
			}

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
					ADRLiv = item.ADRLiv,
					ADRGrp = item.ADRGrp,
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
			mListView.ItemClick += MListView_ItemClick;
			mListView.ItemLongClick += MListView_ItemLongClick;


			Button btnTraitees = FindViewById<Button> (Resource.Id.traitees);
			btnTraitees.Click += BtnTraitees_Click;
			Typeface tf = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaBold.ttf");
			btnTraitees.SetTypeface (tf, TypefaceStyle.Normal);

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
				btngrpUn.Text = "Pas de livraison";
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
			btngrpUn.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.bleu_button);

			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV'  AND Userandsoft = ?  ORDER by groupage, Datemission",ApplicationData.UserAndsoft);

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

			btngrpAll.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpUn.SetBackgroundColor(Color.DarkBlue);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.bleu_button);


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV' AND Userandsoft = ? AND groupage='"+Appli.grp[0]+"' ORDER BY Datemission",ApplicationData.UserAndsoft);
				
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

			btngrpAll.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpDeux.SetBackgroundColor(Color.DarkBlue);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.bleu_button);




			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV'  AND Userandsoft = ? AND groupage='"+Appli.grp[1]+"' ORDER BY Datemission",ApplicationData.UserAndsoft);
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

			btngrpAll.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpTrois.SetBackgroundColor(Color.DarkBlue);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.bleu_button);


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV'  AND Userandsoft = ? AND groupage='"+Appli.grp[2]+"' ORDER BY Datemission",ApplicationData.UserAndsoft);

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

			btngrpAll.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpQuatre.SetBackgroundColor(Color.DarkBlue);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.bleu_button);

			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);



			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV'  AND Userandsoft = ? AND groupage='"+Appli.grp[3]+"' ORDER BY Datemission",ApplicationData.UserAndsoft);

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

			btngrpAll.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpCinq.SetBackgroundColor(Color.DarkBlue);


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);



			var table = db.Query<ToDoTask> ("SELECT * FROM ToDoTask WHERE StatutLivraison = '0' AND typeMission='L' AND typeSegment='LIV' AND Userandsoft = ? AND groupage='"+Appli.grp[4]+"' ORDER BY Datemission",ApplicationData.UserAndsoft);

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
			textViewGrp.Text = "Recherche";

			btngrpAll.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpUn.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpDeux.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpTrois.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpQuatre.SetBackgroundResource (Resource.Drawable.bleu_button);
			btngrpCinq.SetBackgroundResource (Resource.Drawable.bleu_button);


			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection (dbPath);

			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			App.codebarre = "";


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
				var table = db.Query<ToDoTask>("SELECT * FROM ToDoTask WHERE  typeMission='L' AND typeSegment='LIV' AND Userandsoft = ? AND (numCommande LIKE '%"+editrecherche.Text+"%' OR  villeLivraison LIKE '%"+editrecherche.Text+"%' OR nomPayeur LIKE '%\"+input.Text+\"%'OR CpLivraison LIKE '%"+editrecherche.Text+"%' OR refClient LIKE '%"+editrecherche.Text+"%')",ApplicationData.UserAndsoft);
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

//		void Btncodebarre_Click (object sender, EventArgs e)
//		{
//			
//			isScanning = true;
//			var scanner = new ZXing.Mobile.MobileBarcodeScanner(this);
//
//			var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
//			options.PossibleFormats = new List<ZXing.BarcodeFormat>() { 
//				ZXing.BarcodeFormat.CODE_128  //, ZXing.BarcodeFormat.Ean13 
//			};
//
//			scanner.Scan(options).ContinueWith((t) =>
//				{
//					isScanning = false;
//					if (t.IsFaulted)
//					{
//						Login("ERROR!");
//					}
//					else if (t.Result != null)
//					{
//
//						Login(t.Result.Text);
//						App.codebarre = t.Result.Text;
//
//
//
//					}
//				});
//
//
//		}


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
			var table = db.Query<ToDoTask>("SELECT * FROM ToDoTask WHERE  typeMission='L' AND typeSegment='LIV' AND Userandsoft = ? AND (numCommande LIKE '%"+App.codebarre+"%' OR  villeLivraison LIKE '%"+App.codebarre+"%' OR nomPayeur LIKE '%\"+input.Text+\"%'OR CpLivraison LIKE '%"+App.codebarre+"%' OR refClient LIKE '%"+App.codebarre+"%')",ApplicationData.UserAndsoft);
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


			MyListViewAdapter adapter = new MyListViewAdapter (this, mItems);
			mListView.Adapter = adapter;
		}


        void MListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {	
			
			var activity2 = new Intent(this, typeof(ActivityDetailLivraison));
			activity2.PutExtra("ID", Convert.ToString(mItems[e.Position].Id));
			ApplicationData.CR = mItems[e.Position].CR;

			string id = Intent.GetStringExtra("ID");
			StartActivity(activity2);
        }

		void MListView_ItemLongClick (object sender, AdapterView.ItemLongClickEventArgs e)
		{	
			
						
		}



        void BtnTraitees_Click (object sender, EventArgs e)
        {
			StartActivity(typeof(ActivityTraitee));
        }
//		protected override void OnResume()
//		{
//			base.OnResume();
//			StartActivity(typeof(MainActivity));
//		}

		protected void opendetaillivraison()
        {
            StartActivity(typeof(ActivityDetailLivraison));
        }

		public override void OnBackPressed ()
		{
			StartActivity(typeof(MainActivity));
		}      
    }
}



