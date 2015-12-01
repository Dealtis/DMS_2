using Android.Net;
using DMSvStandard;
using System.IO;
using System.Net;
using System.Threading;
using Java.IO;
using System.Threading.Tasks;

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


	using System.Data;
	using System.IO;
	using SQLite;
	using System.Net;





	using AndroidHUD;


	//using System.Net;
	//using System.Security.Cryptography;
	//using ZXing;






	[Activity(Label = "ActivityAnomalie",Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
	public class ActivityAnomalieEnlevement : Activity
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


			int height = Resources.DisplayMetrics.HeightPixels;
			int width = _imageView.Height ;
			App.bitmap = App._file.Path.LoadAndResizeBitmap (width, height);

			//Thread threadInit = new Thread(() => initProcess());
			//threadInit.Start ();
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
				this, Resource.Array.anomalieramasselist, Android.Resource.Layout.SimpleSpinnerItem);

			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;


			//BUTTON VALIDE ANOMALIE
			Button btnAnomalieValide = FindViewById<Button>(Resource.Id.valideAnomalie);
			btnAnomalieValide.Click += BtnAnomalieValide_Click;


		}
		public void initProcess()
		{	

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
			//				if (App.bitmap != null) {
			//					_imageView.SetImageBitmap (App.bitmap);
			//					App.bitmap = null;
			//				}

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
			string dateoj=  Convert.ToString (DateTime.Now.Day)+ Convert.ToString(DateTime.Now.Month);


			App._file = new Java.IO.File(App._dir, String.Format(""+dateoj+"_"+numCom+".jpg", Guid.NewGuid()));

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

//			if (App.txtSpin == "Ramasse pas faite") {
//				App.codeanomalie = "RAMPFT";
//			}
//			if (App.txtSpin == "Positions non chargees") {
//				App.codeanomalie = "RENNCG";
//			}
//			if (App.txtSpin == "Avis de passage") {
//				App.codeanomalie = "RENAVI";
//			}
//			if (App.txtSpin == "Fermeture hebdomadaire") {
//				App.codeanomalie = "RENFHB";
//			}
//			if (App.txtSpin == "Ramasse diverse") {
//				App.codeanomalie = "RAMDIV";
//			}
//			if(App.txtSpin == "Restaure en non traite"){
//				App.codeanomalie = "RESTNT";
//			}

			switch(App.txtSpin)
			{
			case "Ramasse pas faite":
				App.codeanomalie = "RAMPFT";
				break;
			case "Positions non chargees":
				App.codeanomalie = "RENNCG";
				break;
			case "Avis de passage":
				App.codeanomalie = "RENAVI";
				break;
			case "Fermeture hebdomadaire":
				App.codeanomalie = "RENFHB";
				break;
			case "Ramasse diverse":
				App.codeanomalie = "RAMDIV";
				break;
			case "Restaure en non traite":
				App.codeanomalie = "RESTNT";
				break;
			default:
				break;
			}


			if (App._file == null) {
				var resultfor = dbrbis.UpdateStatutValideLivraison(i,"2",App.txtSpin,txtRem.Text,App.codeanomalie,null);
				System.Console.Out.WriteLine (resultfor);

			} else {
				var resultfor = dbrbis.UpdateStatutValideLivraison(i,"2",App.txtSpin,txtRem.Text,App.codeanomalie,App._file.Path);
				System.Console.Out.WriteLine (resultfor);


				Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeFile (App._file.Path);
				Bitmap rbmp = Bitmap.CreateScaledBitmap(bmp, bmp.Width/5,bmp.Height/5, true);
				string newPath = App._file.Path.Replace(".jpg", "-1_1.jpg");
				using (var fs = new FileStream (newPath, FileMode.OpenOrCreate)) {
					rbmp.Compress (Android.Graphics.Bitmap.CompressFormat.Jpeg,100, fs);
				}
				App._rfile = newPath;
				App.rbitmap = rbmp;


				Thread thread = new Thread(() => UploadFile("ftp://77.158.93.75",App._rfile,"DMS","Linuxr00tn",""));
				thread.Start ();
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
				var resultbis = dbr.InsertDataStatut(i,""+App.codeanomalie+"","2",""+App.txtSpin+"",""+ApplicationData.codemissionactive+"",""+Convert.ToString(txtRem)+"",""+ApplicationData.datedj+"",""+datapost+"");





		}
		public string UploadFile(string FtpUrl, string fileName, string userName, string password,string
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
				byte[] data = System.IO.File.ReadAllBytes(fileName);
				req.ContentLength = data.Length;
				System.IO.Stream stream = req.GetRequestStream();
				stream.Write(data, 0, data.Length);
				stream.Close();
				FtpWebResponse res = (FtpWebResponse)req.GetResponse();
				Console.Out.Write("FTP//"+res.StatusDescription+"\n");
				return res.StatusDescription;

			} catch (Exception ex) {
				Thread.Sleep (12000);
				UploadFile (FtpUrl,fileName,userName,password,"");
				Console.Out.Write("ERREUR");
				return "erreur";
			}
		}
		public override void OnBackPressed ()
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetTitle("Validation");
			builder.SetMessage("Voulez-vous annulée l'anomalie ?");
			builder.SetCancelable(false);
			builder.SetPositiveButton("Oui", delegate {



				StartActivity(typeof(ActivityListEnlevement));
			});
			builder.SetNegativeButton("Non", delegate {  });

			builder.Show();

		}
	}
}
