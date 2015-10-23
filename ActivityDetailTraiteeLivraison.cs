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
using Android.Graphics;

using DMSvStandard.ORM;

using System.Data;
using System.IO;
using SQLite;

namespace DMSvStandard
{
    [Activity(Label = "ActivityDetailTraiteeLivraison")]
    public class ActivityDetailTraiteeLivraison : Activity
    {
		

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.DetailLivraison);
            

            //Conn DATABASE
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal), "ormDMS.db3");
            var db = new SQLiteConnection(dbPath);



            //RECUP ID 
			string id = Intent.GetStringExtra ("ID");


			//Toast.MakeText(this, id, ToastLength.Short).Show();
			int i = int.Parse(id);
            

            DBRepository dbr = new DBRepository();
			var res = dbr.GetLivraisonbyID(i);
			var resbis = dbr.GetCodeLivraison(i);
			var restri = dbr.GetTitle(i);
			var resfor = dbr.GetInfoClient(i);
			var ressix = dbr.GetInfoSupp(i);
			var resanomalie = dbr.GetAnomalie (i);

			//INSERT DATA STATUT
			//var resultbis = dbr.InsertDataStatut(i,"0","0","Pas de remarque");
            
          
			//AFFICHE DATA
			TextView codelivraison = FindViewById<TextView>(Resource.Id.codelivraison);
			codelivraison.Gravity = GravityFlags.Center;
			codelivraison.Text = resbis;

			TextView infolivraison = FindViewById<TextView>(Resource.Id.infolivraison);
			infolivraison.Gravity = GravityFlags.Center;
			infolivraison.Text = res;

			TextView title = FindViewById<TextView>(Resource.Id.title);
			infolivraison.Gravity = GravityFlags.Center;
			infolivraison.Text = restri;

			TextView infosupp = FindViewById<TextView>(Resource.Id.infosupp);
			infolivraison.Gravity = GravityFlags.Center;
			infolivraison.Text = ressix;


			TextView infoclient = FindViewById<TextView>(Resource.Id.infoclient);
			infolivraison.Gravity = GravityFlags.Center;
			infolivraison.Text = resfor;

			TextView client = FindViewById<TextView>(Resource.Id.client);
			client.Text = "Client";

			TextView anomaliet = FindViewById<TextView> (Resource.Id.anomaliet);
			TextView anomalie = FindViewById<TextView> (Resource.Id.infoanomalie);
			anomalie.Text = resanomalie;

			//Hide box anomalie if no anomalie
			anomalie.Visibility = ViewStates.Gone;
			anomaliet.Visibility = ViewStates.Gone;

			//FONTSNEXALIGHT
			Typeface nexalight = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaLight.ttf");
			Typeface nexabold = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaBold.ttf");

			codelivraison.SetTypeface(nexabold, TypefaceStyle.Normal);
			title.SetTypeface(nexabold, TypefaceStyle.Normal);
			infosupp.SetTypeface(nexabold, TypefaceStyle.Normal);
			client.SetTypeface(nexabold, TypefaceStyle.Normal);

			infolivraison.SetTypeface(nexalight, TypefaceStyle.Normal);
			infoclient.SetTypeface(nexalight, TypefaceStyle.Normal);



			//SCROLL
//			countDownTextView.setMovementMethod(new ScrollingMovementMethod());
            //Button
            Button btnValide = FindViewById<Button>(Resource.Id.valide);
            btnValide.Click += btnValide_Click;
        }

        public void btnValide_Click(object sender, EventArgs e)
        {	
			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetTitle("Dévalidation");
			builder.SetMessage("Voulez-vous dévalider cette livraison ?");
			builder.SetCancelable(false);
			builder.SetPositiveButton("Oui", delegate {



					updateValideStatut();
					StartActivity(typeof(ActivityListLivraison));
					
			
			});
			builder.SetNegativeButton("Non", delegate {  });
			builder.Show();
        }

		public void updateValideStatut(){

			//RECUP ID 
			string id = Intent.GetStringExtra ("ID");
			int i = int.Parse(id);

			DBRepository dbrbis = new DBRepository();

			//var resulttri = dbrbis.UpdateStatutValide(i,"0","","",null);
			var resultfor = dbrbis.UpdateStatutValideLivraison(i,"1","","","",null);
			Toast.MakeText(this, "UPDATE VALIDE", ToastLength.Short).Show();
		}

        



    }
}