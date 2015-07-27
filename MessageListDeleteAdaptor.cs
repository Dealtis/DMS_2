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
	class MessageListDeleteAdaptor: BaseAdapter<TextMessage>{
		List<TextMessage> items;
		Activity context;


		public MessageListDeleteAdaptor(Activity context, List<TextMessage> items)
			: base()
		{
			this.context = context;
			this.items = items;		
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override TextMessage this[int position]
		{
			get { return items[position]; }
		}
		public override int Count
		{
			get { return items.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
			{
				view = context.LayoutInflater.Inflate(Resource.Layout.MessageListDeleteItem, null);
			}

			//context.LayoutInflater.Inflate(Resource.Layout.MessageListItem, null);

			TextView txtMsg = view.FindViewById<TextView>(Resource.Id.Text1);
			TextView txtDate = view.FindViewById<TextView>(Resource.Id.Text2);
			TextView txtStatus = view.FindViewById<TextView>(Resource.Id.Text3);
			ImageView img = view.FindViewById<ImageView>(Resource.Id.Image);

			int pos = item.Message.IndexOf (System.Environment.NewLine);
			if ((pos <= 25) && (pos >= 0))
				txtMsg.Text = item.Message.Substring (0, pos - 1) + "...";
			else if (pos > 25)
				txtMsg.Text = item.Message.Substring (0, 25) + "...";
			else if (pos < 0) {
				if (item.Message.Length>25)
					txtMsg.Text = item.Message.Substring (0, 25) + "...";
				else txtMsg.Text = item.Message;					 
			}

			txtDate.Text = item.ArrivalDate.ToShortDateString() + " " + item.ArrivalDate.ToShortTimeString();
			txtStatus.Text = item.getStatusValue();

			if ((item.Status == TextMessage.STATUS_TOBESENT)||
				(item.Status == TextMessage.STATUS_UNREAD))
			{
				img.SetImageResource(Resource.Drawable.newmessageicon);
				txtMsg.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
				txtDate.SetTypeface(null, Android.Graphics.TypefaceStyle.BoldItalic);
				txtStatus.SetTypeface(null, Android.Graphics.TypefaceStyle.BoldItalic);

			}
			else
			{
				img.SetImageResource(Resource.Drawable.nonewmessagesicon);
				txtMsg.SetTypeface(null, Android.Graphics.TypefaceStyle.Normal);
				txtDate.SetTypeface(null, Android.Graphics.TypefaceStyle.Italic);
				txtStatus.SetTypeface(null, Android.Graphics.TypefaceStyle.Italic);

			}
			CheckBox check = view.FindViewById<CheckBox>(Resource.Id.checkbox);
			if (item.isCheckedItem())
				check.Checked = true;
			else check.Checked = false;

			return view;
		}
	}
}

