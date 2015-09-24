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

using SQLite;
using System.Data;
using System.IO;

namespace DMSvStandard
{   [Table("StatutMessage")]
	public class StatutMessage
	{
		[PrimaryKey, AutoIncrement, Column("_Id")]
		public int Id { get; set; }


		public int statutNotificationMessage { get; set; }


		public DateTime dateNotificationMessage { get; set; }


		public int numMessage { get; set; }

	}
}

