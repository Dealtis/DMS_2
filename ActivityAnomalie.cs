using Android.Net;
using DMSvStandard;
using System.IO;
using System.Net;

namespace DMSvStandard
{
	using System;
	using System.Collections.Generic;

	using Android.App;
	using Android.Content;
	using Android.Content.PM;
	using Android.Graphics;
	using Android.OS;
	using Android.Provider;
	using Android.Widget;
	using DMSvStandard.ORM;
	using SQLite;
	using Environment = Android.OS.Environment;



	using Uri = Android.Net.Uri;


	public static class App{
		public static Java.IO.File _file;
		public static Java.IO.File _dir;     
		public static Bitmap bitmap;
		public static string txtSpin;
		public static string codeanomalie;
		public static string codebarre;
	}


	[Activity(Label = "ActivityAnomalie",Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
	public class ActivityAnomalie : Activity
	{

		private ImageView _imageView;

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			// make it available in the gallery
			Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
 			Uri contentUri = Uri.FromFile(App._file);
			mediaScanIntent.SetData(contentUri);
			SendBroadcast(mediaScanIntent);

			// display in ImageView. We will resize the bitmap to fit the display
			// Loading the full sized image will consume to much memory 
			// and cause the application to crash.


			//System.Console.Out.WriteLine("!!!!!!!!!!!!BITMAP SUPP!!!!!!!!!!!!!!!!!!!!!!!!");
			initProcess ();
			setBitmap ();
			System.Console.Out.WriteLine("!!!!!!!!!!!!BITMAP UP!!!!!!!!!!!!!!!!!!!!!!!!");


		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Anomalie);
		


			//Conn DATABASE
			string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);

			if (IsThereAnAppToTakePictures())
			{
				CreateDirectoryForPictures();
				//App.bitmap = null;
				Button button = FindViewById<Button>(Resource.Id.openCamera); 
				_imageView = FindViewById<ImageView>(Resource.Id.imageView1);
//				if (App.bitmap != null) {
//					_imageView.SetImageBitmap (App.bitmap);
//					App.bitmap = null;
//				}

				_imageView.SetImageBitmap (App.bitmap);
				button.Click +=  TakeAPicture;
			}
			Spinner spinner = FindViewById<Spinner> (Resource.Id.spinnerAnomalie);


			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);
			var adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.anomalielivraisonlist, Android.Resource.Layout.SimpleSpinnerItem);

			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;


			//BUTTON VALIDE ANOMALIE
			Button btnAnomalieValide = FindViewById<Button>(Resource.Id.valideAnomalie);
			btnAnomalieValide.Click += BtnAnomalieValide_Click;

			//App._file = null;

		}
		public void initProcess()
		{	
			int height = Resources.DisplayMetrics.HeightPixels;
			int width = _imageView.Height ;
			App.bitmap = App._file.Path.LoadAndResizeBitmap (width, height);
		}

		void BtnAnomalieValide_Click (object sender, EventArgs e)
		{
			//RECUP ID 
			string idDATA = Intent.GetStringExtra ("ID");
			int i = int.Parse(idDATA);

			App.bitmap = null;

			if (App.txtSpin == "Choisir une anomalie" ) {
				Toast.MakeText (this,"Merci de choisir une anomalie", ToastLength.Long).Show ();
			} else {
				updateAnomalieStatut();
				StartActivity (typeof(MainActivity));
			}




//			var activity2 = new Intent(this, typeof(ActivityAnomalie));
//			activity2.PutExtra("ID", idDATA);
//
//
//
//			string id = Intent.GetStringExtra("ID");
//			StartActivity(activity2);


		}


		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			EditText txtRem = FindViewById<EditText>(Resource.Id.edittext);
			string txtSpin = string.Format ("{0}", spinner.GetItemAtPosition (e.Position));

			App.txtSpin = txtSpin;

			if (App.txtSpin == "Restaure en non traite" || App.txtSpin == "Choisir une anomalie" ) {
				txtRem.Visibility = Android.Views.ViewStates.Gone;
			} else {
				txtRem.Visibility = Android.Views.ViewStates.Visible;
			}
			//Toast.MakeText (this, App.txtSpin, ToastLength.Long).Show ();
		}



		private void CreateDirectoryForPictures()
		{
			App._dir = new Java.IO.File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "DMS");
			if (!App._dir.Exists())
			{
				App._dir.Mkdirs();
			}
		}

		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}

		private void setBitmap(){
			
				CreateDirectoryForPictures ();
				//App.bitmap = null;
				_imageView = FindViewById<ImageView> (Resource.Id.imageView1);
				_imageView.SetImageBitmap (App.bitmap);
				
				

		}

		private void TakeAPicture(object sender, EventArgs eventArgs)
		{	
			//RECUP ID 
			string id = Intent.GetStringExtra ("ID");
			int i = int.Parse(id);

			DBRepository dbr = new DBRepository();

			var numCom = dbr.GetnumCommande(i);
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			//var activity2 = new Intent(this, typeof(ActivityAnomalie));
			string dateoj=  Convert.ToString (DateTime.Now.Day)+ Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Minute);


			App._file = new Java.IO.File(App._dir, String.Format("photo_"+dateoj+"_"+numCom+".jpg", Guid.NewGuid()));

			intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
			_imageView.SetImageBitmap (App.bitmap);
			setBitmap();
			StartActivityForResult(intent, 0);
			System.Console.Out.WriteLine("!!!!!!!!!!!!START ACTIVITY!!!!!!!!!!!!!!!!!!!!!!!!");

			System.Console.Out.WriteLine("!!!!!!!!!!!!BITMAP UP!!!!!!!!!!!!!!!!!!!!!!!!");
		}


		public void updateAnomalieStatut(){

			//RECUP ID 
			string id = Intent.GetStringExtra ("ID");
			int i = int.Parse(id);


			EditText txtRem = FindViewById<EditText>(Resource.Id.edittext);

			DBRepository dbrbis = new DBRepository();

			if(App.txtSpin == "Livre avec manquant"){
				App.codeanomalie = "LIVRMQ";

			}
			if(App.txtSpin == "Livre avec reserves pour avaries"){
				App.codeanomalie = "LIVRCA";

			}
			if(App.txtSpin == "Livre mais recepisse non rendu"){
				App.codeanomalie = "LIVDOC";

			}
			if(App.txtSpin == "Livre avec manquants + avaries"){
				App.codeanomalie = "LIVRMA";

			}
			if(App.txtSpin == "Refuse pour avaries"){
				App.codeanomalie = "RENAVA";

			}
			if(App.txtSpin == "Avise (avis de passage)"){
				App.codeanomalie = "RENAVI";

			}
			if(App.txtSpin == "Rendu non livre : complement adresse"){
				App.codeanomalie = "RENCAD";

			}
			if(App.txtSpin == "Refus divers ou sans motifs"){
				App.codeanomalie = "RENDIV";

			}
			if(App.txtSpin == "Refuse manque BL"){
				App.codeanomalie = "RENDOC";

			}
			if(App.txtSpin == "Refuse manquant partiel"){
				App.codeanomalie = "RENMQP";

			}
			if(App.txtSpin == "Refuse non commande"){
				App.codeanomalie = "RENDIV";

			}
			if(App.txtSpin == "Refuse cause port du"){
				App.codeanomalie = "RENSPD";

			}
			if(App.txtSpin == "Refuse cause contre remboursement"){
				App.codeanomalie = "RENDRB";

			}
			if(App.txtSpin == "Refuse livraison trop tardive"){
				App.codeanomalie = "RENTAR";

			}
			if(App.txtSpin == "Rendu non justifie"){
				App.codeanomalie = "RENNJU";
			}
			//MODIFICATION 20/10/2015
			if(App.txtSpin == "Fermeture hebdomadaire"){
				App.codeanomalie = "RENFHB";
			}
			if(App.txtSpin == "Non charge"){
				App.codeanomalie = "RENNCG";
			}
			if(App.txtSpin == "Inventaire"){
				App.codeanomalie = "RENINV";
			}
			if(App.txtSpin == "Refuse manquant partiel"){
				App.codeanomalie = "RENMQP";
			}
			if(App.txtSpin == "Restaure en non traite"){
				App.codeanomalie = "RESTNT";
			}

			if (App._file == null) {
				var resultfor = dbrbis.UpdateStatutValideLivraison(i,"2",App.txtSpin,Convert.ToString(txtRem.Text),App.codeanomalie,null);
				System.Console.Out.WriteLine (resultfor);
			
			} else {
				var resultfor = dbrbis.UpdateStatutValideLivraison(i,"2",App.txtSpin,Convert.ToString(txtRem.Text),App.codeanomalie,App._file.Path);
				System.Console.Out.WriteLine (resultfor);

				UploadFile ("ftp://10.1.2.75",App._file.Path,"DMS","Linuxr00tn","");
			}
			
			if(App.txtSpin == "Restaure en non traite"){
				var resultyyy = dbrbis.UpdateStatutValideLivraison (i,"0",null,null,null,null);

				StartActivity(typeof(MainActivity));
				Finish ();
			}

	




			  

				string datapost ="{\"codesuiviliv\":\""+App.codeanomalie+"\",\"memosuiviliv\":\""+Convert.ToString(txtRem.Text)+"\",\"libellesuiviliv\":\""+Convert.ToString(App.txtSpin)+"\",\"commandesuiviliv\":\""+ApplicationData.codemissionactive+"\",\"groupagesuiviliv\":\""+ApplicationData.groupagemissionactive+"\",\"datesuiviliv\":\""+ApplicationData.datedj+"\",\"posgps\":\""+ApplicationData.GPS+"\"}";


				System.Console.Out.WriteLine("!!!!!!!!!!!!DATA CREE!!!!!!!!!!!!!!!!!!!!!!!!");





				//AJOUT DANS LA BASE POUR ENVOIE AVEC THREAD
				DBRepository dbr = new DBRepository();
				//INSERT DATA STATUT
				var resultbis = dbr.InsertDataStatut(i,""+App.codeanomalie+"","2",""+App.txtSpin+"",""+ApplicationData.codemissionactive+"",""+Convert.ToString(txtRem.Text)+"",""+ApplicationData.datedj+"",""+datapost+"");


			


		}
		public static string UploadFile(string FtpUrl, string fileName, string userName, string password,string
			UploadDirectory="")
		{
		try{
			string PureFileName = new FileInfo(fileName).Name;
			String uploadUrl = String.Format("{0}{1}/{2}", FtpUrl,UploadDirectory,PureFileName);
			FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(uploadUrl);
			req.Proxy = null;
			req.Method = WebRequestMethods.Ftp.UploadFile;
			req.Credentials = new NetworkCredential(userName,password);
			req.UseBinary = true;
			req.UsePassive = true;
			byte[] data = File.ReadAllBytes(fileName);
			req.ContentLength = data.Length;
			System.IO.Stream stream = req.GetRequestStream();
			stream.Write(data, 0, data.Length);
			stream.Close();
			FtpWebResponse res = (FtpWebResponse)req.GetResponse();
			return res.StatusDescription;
			
			} catch (Exception ex) {
				UploadFile ("ftp://10.1.2.75",App._file.Path,"DMS","Linuxr00tn","");
				return "ERREUR";
			}
		}
	public override void OnBackPressed ()
		{	
			
			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetTitle("Validation");
			builder.SetMessage("Voulez-vous annulée l'anomalie ?");
			builder.SetCancelable(false);
			builder.SetPositiveButton("Oui", delegate {



				StartActivity(typeof(ActivityListLivraison));
			});
			builder.SetNegativeButton("Non", delegate {  });

			builder.Show();

				}
	}
}
