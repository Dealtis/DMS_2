
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
	[Activity (Label = "ImageDetailView",Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]			
	public class ImageDetailView : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.ImageView);

			ImageView imgd = FindViewById<ImageView> (Resource.Id.imageView1);
			imgd.SetImageBitmap (App.bitmap);

			Button btnback = FindViewById<Button> (Resource.Id.retour);

			btnback.Click += Btnback_Click;

		}

		void Btnback_Click (object sender, EventArgs e)
		{
			OnBackPressed ();
		}
	}
}

