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
{   [Table("TableUser")]
	public class TableUser
	{
		[PrimaryKey, AutoIncrement, Column("_Id")]
		public int Id { get; set; }

		[MaxLength(50)]
		public String userandsoft { get; set; }

		[MaxLength(50)]
		public String usertransics { get; set; }

		[MaxLength(50)]
		public DateTime datelog { get; set; }

		[MaxLength(50)]
		public bool login { get; set; }


		[MaxLength(50)]
		public string usermdp { get; set; }


	}

}