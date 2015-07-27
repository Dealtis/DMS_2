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
{   [Table("ToDoTask")]
    public class ToDoTask
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }

		[MaxLength(50)]
		public String codeLivraison { get; set; }

		[MaxLength(50)]
		public String numCommande { get; set; }

		[MaxLength(20)]
		public String nomClient { get; set; }

		[MaxLength(20)]
		public String refClient { get; set; }

		[MaxLength(50)]
		public String nomPayeur { get; set; }

		[MaxLength(50)]
		public String adresseLivraison { get; set; }

		[MaxLength(50)]
		public String CpLivraison { get; set; }

		[MaxLength(50)]
		public String villeLivraison { get; set; }

		[MaxLength(50)]
		public String dateHeure { get; set; }

		[MaxLength(50)]
		public String dateExpe { get; set; }

		[MaxLength(50)]
		public String nbrColis { get; set; }

		[MaxLength(50)]
		public String nbrPallette { get; set; }

		[MaxLength(50)]
		public String poids { get; set; }

		[MaxLength(50)]
		public String adresseExpediteur { get; set; }

		[MaxLength(50)]
		public String CpExpediteur { get; set; }

		[MaxLength(50)]
		public String villeExpediteur { get; set; }

		[MaxLength(50)]
		public String nomExpediteur { get; set; }

		[MaxLength(50)]
		public String StatutLivraison { get; set; }

		[MaxLength(50)]
		public String instrucLivraison { get; set; }

		[MaxLength(50)]
		public String groupage { get; set; }

		[MaxLength(50)]
		public String ADRLiv { get; set; }

		[MaxLength(50)]
		public String ADRGrp { get; set; }


		[MaxLength(50)]
		public String planDeTransport { get; set; }

		[MaxLength(50)]
		public String typeMission { get; set; }

		[MaxLength(50)]
		public String typeSegment { get; set; }
		  
		[MaxLength(50)]
		public String CR { get; set; }

		[MaxLength(50)]
		public String remarque { get; set; }

		[MaxLength(50)]
		public String codeAnomalie { get; set; }

		[MaxLength(50)]
		public String libeAnomalie { get; set; }

		[MaxLength(50)]
		public int dateBDD { get; set; }

		public string imgpath{ get; set; }


		[MaxLength(50)]
		public string Datemission{ get; set; }


		[MaxLength(50)]
		public string Ordremission{ get; set; }


           
	       

        

    }

}