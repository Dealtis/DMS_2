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

	
		public String codeChauffeur{ get; set; }


		public String texteMessage { get; set; }


		public String utilisateurEmetteur { get; set; }


		public int statutMessage { get; set; }


		public DateTime dateImportMessage { get; set; }


		public int typeMessage { get; set; }


		public int numMessage { get; set; }
		
	}
}

