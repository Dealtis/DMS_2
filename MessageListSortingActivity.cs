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
	[Activity (Label = "MessageListSortingActivity")]			
	public class MessageListSortingActivity : Activity
	{
		private int order = 0;

		public int getOrder()
		{
			return order;
		}
		public void setOrder(int _order)
		{
			order = _order;
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.MessageListSorting);
			order = Intent.GetIntExtra("order", 1);
		}

		private void InitView()
		{

			TextView lblTitle = FindViewById<TextView>(Resource.Id.lblTitle);
			lblTitle.Text =  ApplicationData.Instance.getTranslator().translateMessage("formsort.title");

			TextView lblDateAsc = FindViewById<TextView>(Resource.Id.lblDateAsc);
			lblDateAsc.Text =  ApplicationData.Instance.getTranslator().translateMessage("formsort.dateasc");

			TextView lblDateDesc = FindViewById<TextView>(Resource.Id.lblDateDesc);
			lblDateDesc.Text =  ApplicationData.Instance.getTranslator().translateMessage("formsort.datedesc");

			TextView lblStateAsc = FindViewById<TextView>(Resource.Id.lblStateAsc);
			lblStateAsc.Text =  ApplicationData.Instance.getTranslator().translateMessage("formsort.stateasc");

			TextView lblStateDesc = FindViewById<TextView>(Resource.Id.lblStateDesc);
			lblStateDesc.Text =  ApplicationData.Instance.getTranslator().translateMessage("formsort.statedesc");

			Button btnSort = FindViewById<Button>(Resource.Id.button1);
			btnSort.Text =  ApplicationData.Instance.getTranslator().translateMessage("formsort.sort");
			btnSort.Click += SaveSorting;



			ImageButton btnPrev = FindViewById<ImageButton> (Resource.Id.imageButton1);
			btnPrev.Click += delegate { goBack();	};


			RadioButton radioDateAsc = FindViewById<RadioButton> (Resource.Id.chDateAsc);
			radioDateAsc.Click += RadioButtonClick;

			RadioButton radioDateDesc = FindViewById<RadioButton> (Resource.Id.chDateDesc);
			radioDateDesc.Click += RadioButtonClick;

			RadioButton radioStateAsc = FindViewById<RadioButton> (Resource.Id.chStateAsc);
			radioStateAsc.Click += RadioButtonClick;

			RadioButton radioStateDesc = FindViewById<RadioButton> (Resource.Id.chStateDesc);
			radioStateDesc.Click += RadioButtonClick;

			this.Title = "DMS";

			if (order == 0) {
				radioDateAsc.Checked = true;
			} else if (order == 1) {			
				radioDateDesc.Checked = true;
			} else if (order == 2) {			
				radioStateAsc.Checked = true;
			} else if (order == 3) {			
				radioStateDesc.Checked = true;
			}


										
		}

		protected void goBack()
		{
			Finish();

		}

		protected override void OnResume()
		{
			base.OnResume ();

			InitView ();
		}

		protected void SaveSorting(object sender, EventArgs e)
		{
			ApplicationData.Instance.setMessageListOrdering (order);
			Finish();
		}

		protected void RadioButtonClick(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb. Id == Resource.Id.chDateAsc) {
				RadioButton radioDateDesc = FindViewById<RadioButton> (Resource.Id.chDateDesc);
				radioDateDesc.Checked = false;

				RadioButton radioStateAsc = FindViewById<RadioButton> (Resource.Id.chStateAsc);
				radioStateAsc.Checked = false;

				RadioButton radioStateDesc = FindViewById<RadioButton> (Resource.Id.chStateDesc);
				radioStateDesc.Checked = false;
				order = 0;
			}
			else if (rb.Id == Resource.Id.chDateDesc) {
				RadioButton radioDateAsc = FindViewById<RadioButton> (Resource.Id.chDateAsc);
				radioDateAsc.Checked = false;

				RadioButton radioStateAsc = FindViewById<RadioButton> (Resource.Id.chStateAsc);
				radioStateAsc.Checked = false;

				RadioButton radioStateDesc = FindViewById<RadioButton> (Resource.Id.chStateDesc);
				radioStateDesc.Checked = false;
				order = 1;
			}
			else if (rb.Id == Resource.Id.chStateAsc) {
				RadioButton radioDateAsc = FindViewById<RadioButton> (Resource.Id.chDateAsc);
				radioDateAsc.Checked = false;

				RadioButton radioDateDesc = FindViewById<RadioButton> (Resource.Id.chDateDesc);
				radioDateDesc.Checked = false;

				RadioButton radioStateDesc = FindViewById<RadioButton> (Resource.Id.chStateDesc);
				radioStateDesc.Checked = false;
				order = 2;
			}
			else if (rb.Id == Resource.Id.chStateDesc) {
				RadioButton radioDateAsc = FindViewById<RadioButton> (Resource.Id.chDateAsc);
				radioDateAsc.Checked = false;

				RadioButton radioDateDesc = FindViewById<RadioButton> (Resource.Id.chDateDesc);
				radioDateDesc.Checked = false;

				RadioButton radioStateAsc = FindViewById<RadioButton> (Resource.Id.chStateAsc);
				radioStateAsc.Checked = false;
				order = 3;
			}
		}

	}
}

