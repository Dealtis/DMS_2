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
    public class StatutLivraison
    {
       	
		[PrimaryKey, AutoIncrement]
		public int IdS { get; set; }


        [MaxLength(50)]
        public int id { get; set; }

		[MaxLength(50)]
		public string codesuiviliv { get; set; }

        [MaxLength(50)]
        public String statut { get; set; }

        [MaxLength(50)]
		public String commandesuiviliv { get; set; }

		[MaxLength(50)]
		public String datesuiviliv { get; set; }

		[MaxLength(50)]
		public String libellesuiviliv { get; set; }

        [MaxLength(50)]
		public String memosuiviliv { get; set; }

		public String datajson { get; set; }


    }
}