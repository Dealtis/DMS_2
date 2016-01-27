
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

namespace DMSvStandard
{
	[Activity (Label = "MyListViewAdapter")]			
	public class MyListViewAdapter : BaseAdapter<Livraison> {
		private List<Livraison> mItems;
		private Context mContext;
		public MyListViewAdapter(Context context,List<Livraison> items){
			mItems = items;
			mContext = context;
		}
		public override long GetItemId(int position)
		{
			return position;
		}

		public override Livraison this[int position] {  
			get { return mItems[position]; }
		}
		public override int Count {
			get { return mItems.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = convertView;

			if (row == null) {
				switch (mItems [position].StatutLivraison) {
				default:
					break;
				case "0":
					if (mItems [position].typeMission == "L") {
						row = LayoutInflater.From (mContext).Inflate (Resource.Layout.ListeViewRow, null, false);
					} else {
						row = LayoutInflater.From (mContext).Inflate (Resource.Layout.ListeViewRowRamasse, null, false);
					}
					break;
				case "1":
					row = LayoutInflater.From (mContext).Inflate (Resource.Layout.ListeViewRowValide, null, false);
					break;
				case "2":
					if (mItems [position].imgpath == null) {
						row = LayoutInflater.From (mContext).Inflate (Resource.Layout.ListeViewRowAnomalie, null, false);
					} else {
						row = LayoutInflater.From (mContext).Inflate (Resource.Layout.ListeViewRowAnomaliePJ,null,false);
					}
					break;
				}
				if(mItems[position].imgpath == "SUPPLIV"){
					row = LayoutInflater.From (mContext).Inflate (Resource.Layout.ListeViewRowStroke,null,false);
				}
			}

			TextView txtName = row.FindViewById<TextView> (Resource.Id.txtName);
			//FONTSNEXALIGHT
			Typeface tf = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaLight.ttf");
			txtName.SetTypeface(tf, TypefaceStyle.Normal);
			txtName.Text = "OT: "+mItems[position].numCommande+" "+mItems[position].planDeTransport+"\n"+mItems[position].ADRGrp+mItems[position].nomPayeur+"\n"+mItems[position].CpLivraison+"."+mItems[position].villeLivraison+"\tCol: "+mItems[position].nbrColis+" Pal:"+mItems[position].nbrPallette+"\n"+mItems [position].instrucLivraison;

			//ApplicationData.CR = mItems [position].CR;

			return row;
	}
}
}

