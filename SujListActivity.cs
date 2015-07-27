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
using System.IO;
using System.Xml;

namespace DMSvStandard
{
	[Activity (Label = "SujListActivity")]			
	public class SujListActivity : Activity
	{
		ListView listView;
		//	string[] items;
		List<string> items;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.SujList);

			InitView ();
		}


		public bool isExternalStorageAvailable()
		{
			String state = Android.OS.Environment.ExternalStorageState;

			if (Android.OS.Environment.MediaMounted.Equals(state)) {
				return true;
			}
			return false;
		}


		private void InitView()
		{
			listView = FindViewById<ListView>(Resource.Id.SujListView);
			//listView.ItemClick += OnListItemClick;  // to be defined

			//	items = new string[] { "milk", "butter", "yogurt", "ice cream" };
			items = new List<string>{ };
			///////////////////////
			/// 
			System.IO.Stream fs = null;


			String fileName = "suj_messages.xml";

			if (isExternalStorageAvailable ()) {
				String storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

				if (!Directory.Exists (storagePath + "/DTMD"))
					Directory.CreateDirectory (storagePath + "/DTMD");					 

				fileName = storagePath + "/DTMD/" + fileName;

				if (!File.Exists (fileName)) {                
					TextWriter tw = new StreamWriter (fileName);                
					tw.WriteLine ("<?xml version=\"1.0\"?>");
					tw.WriteLine ("<Messages>");
					tw.WriteLine ("<Message>milk</Message>");
					tw.WriteLine ("<Message>butter</Message>");
					tw.WriteLine ("<Message>yogurt</Message>");
					tw.WriteLine ("<Message>ice cream</Message>");
					tw.WriteLine ("</Messages>");                
					tw.Close ();
				}

				fs = new FileStream (fileName, FileMode.Open);
			} else {
				try
				{
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
				catch (Exception e)
				{ 
					fs = MainActivity.getContext ().OpenFileOutput (fileName, Android.Content.FileCreationMode.Private);
					TextWriter tw = new StreamWriter(fs);                
					tw.WriteLine("<?xml version=\"1.0\"?>");
					tw.WriteLine("<Messages>");
					tw.WriteLine ("<Message>milk</Message>");
					tw.WriteLine ("<Message>butter</Message>");
					tw.WriteLine ("<Message>yogurt</Message>");
					tw.WriteLine ("<Message>ice cream</Message>");
					tw.WriteLine("</Messages>");                
					tw.Close();
					fs.Close();
					fs = MainActivity.getContext ().OpenFileInput(fileName);
				}
			}

			try
			{
				using (XmlReader reader = XmlReader.Create(fs))
				{
					while (reader.Read())
					{
						switch (reader.NodeType)
						{
						case XmlNodeType.Element:
							if (reader.Name == "Message")
							{
								items.Add(reader.Value);
							
							}
							break;

						case XmlNodeType.Text:
						case XmlNodeType.XmlDeclaration:
						case XmlNodeType.ProcessingInstruction:
						case XmlNodeType.Comment:                                
						case XmlNodeType.EndElement:
							break;
						}
					}
				}
			}
			catch (FileNotFoundException e)
			{ 
				fs.Close ();
			}
			fs.Close ();


			/// 
			/////////////////////////////////////////////////////////////////////////////////////////

			//	items = new string[] { "milk", "butter", "yogurt", "ice cream" };

			ArrayAdapter<string> adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, items);

			listView.Adapter = adapter; 

		
			/*
			if (ApplicationData.Instance.getMessageListOrdering() == 0)
				msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p1.ArrivalDate.CompareTo(p2.ArrivalDate);});
			else if (ApplicationData.Instance.getMessageListOrdering() == 1)
				msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p2.ArrivalDate.CompareTo(p1.ArrivalDate);});
			else if (ApplicationData.Instance.getMessageListOrdering() == 2)
				msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p1.Status.CompareTo(p2.Status);});
			else if (ApplicationData.Instance.getMessageListOrdering() == 3)
				msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p2.Status.CompareTo(p1.Status);});
			else msgList.Sort(delegate(TextMessage p1, TextMessage p2) {return p2.ArrivalDate.CompareTo(p1.ArrivalDate);});

*/
			listView.ItemClick += OnListItemClick;


		}

		protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			 listView = sender as ListView;

			//	string msg = listView.GetItemAtPosition(e.Position).ToString;
			string msg = items[e.Position];

			string x = Intent.GetStringExtra("MSG");

			Intent i = new Intent(this, typeof(NewMessageActivity));
			i.PutExtra("V1", "2");
			i.PutExtra("V2", x+msg);
			//StartActivity(i);
			StartActivityForResult(i, 1);
			Finish();
		}

	}
}

