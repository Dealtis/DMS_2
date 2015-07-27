
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
	[Activity (Label = "ActivityLoginchoose")]			
	public class ActivityLoginchoose : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView(Resource.Layout.LoginChoose);
			// Create your application here





			Button btncodebarre = FindViewById<Button> (Resource.Id.codebarre);
			btncodebarre.Click += Btncodebarre_Click;


		}

		void Btncodebarre_Click (object sender, EventArgs e)
		{
			StartActivity(typeof(LoginActivity));
		}
	}
}

