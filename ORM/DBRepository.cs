using System;
using System.Data;
using System.IO;
using SQLite;
using Android.Graphics;

namespace DMSvStandard.ORM
{
    public class DBRepository
    {
        //création de la database
        public string CreateDB()
        {
            var output = "";
            output += "Création de la DATABASE :";
            string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.Personal),"ormDMS.db3");
            var db = new SQLiteConnection(dbPath);
            output += "\nDatabase crée...";
            return output;            
        }

        //création des tables
        public string CreateTable()
        {
            try
            {
                string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.Personal),"ormDMS.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<ToDoTask>();
			Console.Out.WriteLine("!!!!!!!!!!!!CREATE T1!!!!!!!!!!!!!!!!!!!!!!!!");
			db.CreateTable<StatutLivraison>();
			Console.Out.WriteLine("!!!!!!!!!!!!CREATE T2!!!!!!!!!!!!!!!!!!!!!!!!");
            
            string result = "Table crée avec succès";
            return result;
            }
            catch (Exception ex)
            {	
				
                return "Erreur : " + ex.Message;

            }
        }

		public string CreateTableStatut()
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				db.CreateTable<StatutLivraison>();
				Console.Out.WriteLine("!!!!!!!!!!!!CREATE STATUT!!!!!!!!!!!!!!!!!!!!!!!!");


				string result = "Table crée avec succès";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		public string CreateTableUser()
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				db.CreateTable<TableUser>();
				Console.Out.WriteLine("!!!!!!!!!!!!CREATE USER!!!!!!!!!!!!!!!!!!!!!!!!");


				string result = "Table crée avec succès";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		public string CreateTableMessage()
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				db.CreateTable<Message>();
				Console.Out.WriteLine("!!!!!!!!!!!!CREATE MESSAGE!!!!!!!!!!!!!!!!!!!!!!!!");


				string result = "Table crée avec succès";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		public string CreateTableStatutMessage()
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				db.CreateTable<StatutMessage>();
				Console.Out.WriteLine("!!!!!!!!!!!!CREATE STATUT MESSAGE!!!!!!!!!!!!!!!!!!!!!!!!");


				string result = "Table crée avec succès";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		//DROP TABLE
		public string DropTable()
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				db.DeleteAll<ToDoTask>();
				db.DeleteAll<StatutLivraison>();


				string result = "Table crée avec succès";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		public string DropTableUser()
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				db.DeleteAll<TableUser>();



				string result = "delete";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		public string DropTableMessage()
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				db.DeleteAll<Message>();



				string result = "delete";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		public string DropTableStatutMessage()
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				db.DeleteAll<StatutMessage>();



				string result = "delete";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		public string deletenotif(int id)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				db.Delete<StatutMessage>(id);

				string result = "delete";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}


		//DROP TABLE
//		public string DropTableDay()
//		{
//			try
//			{
//				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
//					(Environment.SpecialFolder.Personal),"ormDMS.db3");
//				var db = new SQLiteConnection(dbPath);
//
//				db.Delete(ToDoTask,+"!="+DateTime.Now.Day,null);
////				var query = db.Table<ToDoTask>().Where(rt=> rt.dateBDD != ApplicationData.datedj);
////
////				if (query != null) {
////					foreach (var object in query.Table<ToDoTask>()) {
////						db.Delete<ToDoTask>(object.PrimaryKeyId);
////					}
////				}
////				db.Commit ();
//
//
//				string result = "DropTableDay";
//				return result;
//			}
//			catch (Exception ex)
//			{
//				return "Erreur : " + ex.Message;
//
//			}
//		}

		public string DropTableStatut()
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal),"ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				//db.DeleteAll<ToDoTask>();
				db.DeleteAll<StatutLivraison>();


				string result = "Table vid¨¦e";
				return result;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}


        //Insertion des données
		public string InsertData(string codeLivraison,string numCommande, string refClient, string nomPayeur, string nomExpediteur,string adresseExpediteur, string villeExpediteur, string CpExpediteur, string dateExpe, string nomClient, string adresseLivraison, string villeLivraison, string CpLivraison, string dateHeure, string poids, string nbrPallette, string nbrColis, string instrucLivraison, string typeMission, string typeSegment, string GROUPAGE,string AdrLiv, string AdrGrp, string statutLivraison, string CR,int dateBDD, string Datemission, string Ordremission, string planDeTransport, string Userandsoft, string nomClientLivraison, string villeClientLivraison, string imgpath)
        {
            try
            {
                string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.Personal), "ormDMS.db3");
                var db = new SQLiteConnection(dbPath);
                ToDoTask item = new ToDoTask();

				item.codeLivraison =  codeLivraison;
				item.numCommande = numCommande;
				item.nomClient =  nomClient ;
				item.refClient = refClient ;
				item.nomPayeur = nomPayeur;
				item.adresseLivraison = adresseLivraison;
				item.CpLivraison = CpLivraison;
				item.villeLivraison = villeLivraison;
				item.dateHeure = dateHeure;
				item.nbrColis = nbrColis;
				item.nbrPallette = nbrPallette;
				item.poids = poids;
				item.adresseExpediteur = adresseExpediteur;
				item.CpExpediteur = CpExpediteur;
				item.dateExpe = dateExpe;
				item.villeExpediteur = villeExpediteur;
				item.nomExpediteur = nomExpediteur;
				item.instrucLivraison = instrucLivraison;
				item.groupage = GROUPAGE;
				item.ADRLiv = AdrLiv;
				item.ADRGrp = AdrGrp;
				item.typeMission = typeMission;
				item.typeSegment = typeSegment;
				item.StatutLivraison = statutLivraison;
				item.CR = CR;
				item.dateBDD = dateBDD;
				item.Datemission = Datemission;
				item.Ordremission = Ordremission;
				item.planDeTransport = planDeTransport;
				item.Userandsoft = Userandsoft;
				item.nomClientLivraison = nomClientLivraison;
				item.villeClientLivraison = villeClientLivraison;
				item.imgpath = imgpath;


                db.Insert(item);
                return "Insertion good";
            }
            catch (Exception ex)
            {
                return "Erreur : " + ex.Message;

            }
        }
		//Insertion des don¨¦es USER

		public string InsertDataUser(string userandsoft, string usertransics, string mdp)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				TableUser item = new TableUser();

				item.userandsoft =  userandsoft;
				item.usertransics = usertransics;
				item.usermdp = mdp;



				db.Insert(item);
				return "Insertion good";
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		//Insertion des don¨¦es USER

		public string InsertDataMessage(string codeChauffeur,string utilisateurEmetteur, string texteMessage, int statutMessage, DateTime dateImportMessage, int typeMessage, int numMessage)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				Message item = new Message();
				item.codeChauffeur = codeChauffeur;
				item.utilisateurEmetteur =  utilisateurEmetteur;				
				item.texteMessage = texteMessage;
				item.statutMessage = statutMessage;
				item.dateImportMessage = dateImportMessage;
				item.typeMessage = typeMessage;
				item.numMessage = numMessage;


				db.Insert(item);
				return "Insertion good";
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}
		//Insertion des donn¨¦es STATUT MESSAGE

		public string InsertDataStatutMessage(int statutNotificationMessage, DateTime dateNotificationMessage, int numMessage, string numCommande, string groupage)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				StatutMessage item = new StatutMessage();
				item.statutNotificationMessage = statutNotificationMessage;
				item.dateNotificationMessage =  dateNotificationMessage;				
				item.numMessage = numMessage;
				item.numCommande = numCommande;
				item.groupage = groupage;

				db.Insert(item);
				return "WWWWWWWInsertion"+statutNotificationMessage+" "+numCommande;
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}


        //SELECT DES DATA'
        public string SelectData()
        {
            try
            {
                string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.Personal), "ormDMS.db3");
                var db = new SQLiteConnection(dbPath);
                string output = "";
                //output += "Données ORM";
				var table = db.Query<ToDoTask>("SELECT * FROM ToDoTask WHERE StatutLivraison = '0'");
                foreach (var item in table)
                {
					output += "\n" + item.codeLivraison + "\n" + item.nomClient + "\n" + item.refClient + "\n" + item.adresseLivraison + "\n" + item.CpLivraison + "\n" + item.villeLivraison + "\n" + item.dateHeure + "\n" + item.nbrColis + "\n" + item.nbrPallette + "\n" + item.poids + "\n" + item.adresseExpediteur + "\n" + item.CpExpediteur + "\n" + item.villeExpediteur + "\n" + item.nomExpediteur;



                }
                return output;

                
            }
            catch (Exception ex)
            {
                return "Erreur : " + ex.Message;

            }
        }

		//SELECT DES DATA USERANDSOFT'
		public string SelectDataUserAndsoft(string user)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				string output = "";
				string usertxt = user;
				var table = db.Query<TableUser>("SELECT * FROM TableUser WHERE userandsoft= ?",usertxt);
				foreach (var item in table)
				{
					output += item.userandsoft;



				}
				return output;


			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		//SELECT DES DATA USERTRANSICS'
		public string SelectDataUserTransics(string user)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				string output = "";
				string usertxt = user;
				var table = db.Query<TableUser>("SELECT * FROM TableUser WHERE userandsoft= ?",usertxt);
				foreach (var item in table)
				{
					output += item.usertransics;



				}
				return output;


			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}


		//SELECT DES DATA USERTRANSICS'
		public string SelectDataUserMdp(string user, string mdp)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				string output = "";

				var table = db.Query<TableUser>("SELECT * FROM TableUser WHERE userandsoft = ?and usermdp= ?",user,mdp);
				foreach (var item in table)
				{
					output += "GOOD";



				}
				return output;


			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		//SELECT DU STATUT DE LOG
		public bool Selectlogin(string userandsoft)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				bool output = false;
				var table = db.Query<TableUser>("SELECT * FROM TableUser WHERE userandsoft = ?",userandsoft);
				foreach (var item in table)
				{
					output = item.login;

				}
				return output;


			}
			catch (Exception ex)
			{
				return false;

			}
		}

		//SELECT DE LA DATE DE LOG
		public DateTime Selectdatelog(string userandsoft)
		{
			
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				DateTime output = DateTime.Now;
				var table = db.Query<TableUser>("SELECT * FROM TableUser WHERE userandsoft= ?",userandsoft);
				foreach (var item in table)
				{
					output = item.datelog;
				}
				return output;
		}
		//VERIF SI USERANDSOFT DEJA DANS BASE


		public string verifusertable(string userandsoft, string usertransics, string mdp)
		{

			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			string output = "0";
			var table = db.Query<TableUser>("SELECT * FROM TableUser WHERE userandsoft = ? and usertransics = ? and usermdp = ?",userandsoft,usertransics,mdp);
			foreach (var item in table)
			{	
				
					output = "1";

			}
			return output;
		}
		//SELECT TABLE SI LOG 1
		public bool Selectlogone()
		{

			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			bool output = false;
			var table = db.Query<TableUser>("SELECT * FROM TableUser WHERE login = 1");
			foreach (var item in table)
			{	
				if (((item.datelog.Hour).CompareTo (DateTime.Now.Hour)) < 12) {
					output = true;
					ApplicationData.UserAndsoft = item.userandsoft;
					ApplicationData.UserTransic = item.usertransics;
				} else {
					var updatelog = db.Query<TableUser>("UPDATE TableUser SET login = 0 WHERE userandsoft = ?",item.userandsoft);
					output = false;
				}
			}
			return output;
		}



        //SELECT PAR ID
        public string GetLivraisonbyID(int id)
        {
            string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.Personal), "ormDMS.db3");
            var db = new SQLiteConnection(dbPath);
            string output = "";
            var item = db.Get<ToDoTask>(id);
			ApplicationData.codemissionactive = item.numCommande;
			ApplicationData.groupagemissionactive = item.groupage;
			string unite;
			string cr;


			if (Convert.ToDouble((item.poids).Replace ('.', ',')) < 1) {
				unite = ((Convert.ToDouble((item.poids).Replace ('.', ','))) * 1000)+" kilos";
			} else {
				unite = item.poids+" tonnes";
			}

			if (item.CR == "") {
				cr = "";
			} else {
				cr = item.CR +" euros";
			}



			output += item.nomPayeur+"\n"+item.adresseLivraison+"\n"+item.CpLivraison+" "+item.villeLivraison+"\n"+item.nbrColis+" COLIS   "+item.nbrPallette+" PALETTE\n"+unite+"\n"+item.dateHeure+"\n"+cr;
            return output;

        }

		public string GetCodeLivraison(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			string output = "";
			var item = db.Get<ToDoTask>(id);
			output += "\n"+item.numCommande+"\n";
			return output;

		}
		public string GetnumCommande(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			string output = "";
			var item = db.Get<ToDoTask>(id);
			output += item.numCommande;
			return output;

		}
		public string GetFinalDest(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			string output = "";
			var item = db.Get<ToDoTask>(id);
			output += ""+item.nomClientLivraison+"\n"+item.villeClientLivraison+"";
			return output;

		}

		public string GetAnomalie(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			string output = "";
			var item = db.Get<ToDoTask>(id);
			output += "\n"+item.libeAnomalie+"\n"+item.remarque;
			return output;

		}

		public string GetTitle(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			var item = db.Get<ToDoTask>(id);
			string title = "";
			if (item.typeMission == "L") {
				title = "Livraison";
			} else {
				title = "Ramasse";
			}
			return title;

		} 	

		public string GetInfoClient(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			string output = "";
			var item = db.Get<ToDoTask>(id);
			output += "\n"+item.nomClient + "\nRef: "+item.refClient+"\nTournee : "+item.planDeTransport;
			return output;

		}

		public string GetInfoSupp(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			string output = "";
			var item = db.Get<ToDoTask>(id);
			output += "\n" + item.instrucLivraison+"\n";
			return output;

		}



		//GET STATUT LIVRAISON


		public string GetStatutLivraison(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			string output = "";
			var item = db.Get<ToDoTask>(id);
			output += item.StatutLivraison;
			return output;

		}

		//GET TYPE POSITION
		public string GetTypePosition(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);
			string output = "";
			var item = db.Get<ToDoTask>(id);
			output += item.typeMission;
			return output;

		}



		//GET IMAGE ANOMALIE
		public string GetImageAnomalie(int id)
		{
			string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
				(Environment.SpecialFolder.Personal), "ormDMS.db3");
			var db = new SQLiteConnection(dbPath);

			var item = db.Get<ToDoTask>(id);
			string imgpath = item.imgpath;

			return imgpath;

		}
		//INSERT DATA STATUT ID ET STATUT
		public string InsertDataStatut(int id,string codesuiviliv, string statut, string libellesuiviliv,string commandesuiviliv, string memosuiviliv, string datesuiviliv, string datajson)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);
				StatutLivraison item = new StatutLivraison();
				item.id = id;
				item.commandesuiviliv = commandesuiviliv;
				item.codesuiviliv = codesuiviliv;
				item.statut = statut;
				item.libellesuiviliv = libellesuiviliv;
				item.memosuiviliv = memosuiviliv;
				item.datesuiviliv = datesuiviliv;
				item.datajson = datajson;
				db.Insert(item);

				return "Insertion good";
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}
		//UPDATE WHEN LOGIN
		public string UpdateLogin(bool login, DateTime datelogin, string userandsoft)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);

				var updatelog = db.Query<TableUser>("UPDATE TableUser SET login = ? WHERE userandsoft = ?",login,userandsoft);



				return "Upadate good";
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}

		//UPDATE WHEN LOGIN
		public string UpdateDate(bool login, DateTime datelogin, string userandsoft)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);

			
				var updatedate = db.Query<TableUser>("UPDATE TableUser SET datelog = ? WHERE userandsoft = ?",datelogin,userandsoft);


				return "Upadate good";
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}




		public string UpdateStatutValideLivraison(int id, string statut, string libeAnomalie,string remarque,string codeAnomalie,string imgpath)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				

				var dbbis = new SQLiteConnection(dbPath);

				var itembis = dbbis.Get<ToDoTask>(id);

				itembis.StatutLivraison = statut;
				itembis.codeAnomalie = codeAnomalie;
				itembis.libeAnomalie = libeAnomalie;
				itembis.remarque = remarque;
				itembis.imgpath = imgpath;
				itembis.codeAnomalie = codeAnomalie;

				dbbis.Update(itembis);

				return "UpdateStatutLivraisonGood";

			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}
		//UPDATE STATUT MESSAGE 
		public string UpdateStatutMessage(int statut, int numMessage)
		{
			try
			{
				string dbPath = System.IO.Path.Combine(Environment.GetFolderPath
					(Environment.SpecialFolder.Personal), "ormDMS.db3");
				var db = new SQLiteConnection(dbPath);

				var updatelog = db.Query<StatutMessage>("UPDATE StatutMessage SET statutMessage = ? WHERE Id = ?",statut,numMessage);



				return "Upadate good";
			}
			catch (Exception ex)
			{
				return "Erreur : " + ex.Message;

			}
		}
    }

}