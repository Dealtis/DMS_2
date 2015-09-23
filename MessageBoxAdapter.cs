
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
	[Activity (Label = "MessageBoxAdapter")]			
	public class MessageBoxAdapter : BaseAdapter<Message> {
		private List<Message> mItems;
		private Context mContext;
		public MessageBoxAdapter(Context context,List<Message> items){
			mItems = items;
			mContext = context;
		}
		public override long GetItemId(int position)
		{
			return position;
		}

		public override Message this[int position] {  
			get { return mItems[position]; }
		}
		public override int Count {
			get { return mItems.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = convertView;

			if(mItems [position].typemsg == 1){
				
				row = LayoutInflater.From (mContext).Inflate (Resource.Layout.RowRight, null, false);
				

				TextView txtName = row.FindViewById<TextView> (Resource.Id.txtName);
				TextView txtdatestatut = row.FindViewById<TextView> (Resource.Id.textds);
				//HorizontalScrollView rmq = row.FindViewById<HorizontalScrollView> (Resource.Id.rmq);
				//FONTSNEXALIGHT
				Typeface tf = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaLight.ttf");
				txtName.SetTypeface(tf, TypefaceStyle.Normal);
				txtName.Text = ""+mItems[position].texte+"";

				txtdatestatut.Text=""+mItems[position].datemessage.Hour+":"+mItems[position].datemessage.Minute+" "+mItems[position].statut+"";
			}else{



				row = LayoutInflater.From (mContext).Inflate (Resource.Layout.RowLeft, null, false);


				TextView txtName = row.FindViewById<TextView> (Resource.Id.txtName);
				TextView txtdatestatut = row.FindViewById<TextView> (Resource.Id.textds);
				//HorizontalScrollView rmq = row.FindViewById<HorizontalScrollView> (Resource.Id.rmq);
				//FONTSNEXALIGHT
				Typeface tf = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaLight.ttf");
				txtName.SetTypeface(tf, TypefaceStyle.Normal);
				txtName.Text = ""+mItems[position].texte+"";

				txtdatestatut.Text=""+mItems[position].datemessage.Hour+":"+mItems[position].datemessage.Minute+" "+mItems[position].statut+"";
			}

			return row;
		}
	}
}

