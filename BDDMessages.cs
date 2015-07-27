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

{
	[Table("StatutLivraison")]
	public class BDDMessages
	{

		[MaxLength(50)]
		public int id { get; set; }

		[MaxLength(50)]
		public string codesuiviliv { get; set; }

	}
}
