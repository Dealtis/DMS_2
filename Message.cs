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
{   [Table("Message")]
	public class Message
	{
		[PrimaryKey, AutoIncrement, Column("_Id")]
		public int Id { get; set; }

		[MaxLength(50)]
		public String texte { get; set; }

		[MaxLength(50)]
		public String utilisateurAndsoft { get; set; }

		[MaxLength(20)]
		public String statut { get; set; }

		[MaxLength(20)]
		public DateTime datemessage { get; set; }
		
	}
}

