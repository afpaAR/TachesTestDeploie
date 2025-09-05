using System.Data;
using Microsoft.Data.SqlClient;

namespace TachesTestDeploie
{
    internal class TacheRepository
    {

        private SqlConnection activeConnexion;
        public TacheRepository()
        {
            this.dbConnecter();
        }

        private void dbConnecter()
        {
            Connexion con = new Connexion();
            this.activeConnexion = con.GetConnection();
            this.activeConnexion.Open();
        }

        private void chkConnexion()
        {
            if (this.activeConnexion == null || this.activeConnexion.State == ConnectionState.Closed)
            {
                this.dbConnecter();
            }
        }

        public void AddTache(Taches Tache)
        {
            this.chkConnexion();

            SqlCommand RequestUpdatetache = activeConnexion.CreateCommand();
            RequestUpdatetache.CommandText = "INSERT INTO TACHES (Id, Nom, Description, DateCreation) VALUES (@Id, @Nom, @Desc, @DateCrea)";

            RequestUpdatetache.Parameters.Add("@Id", SqlDbType.Int).Value = getLastId();
            RequestUpdatetache.Parameters.Add("@Nom", SqlDbType.NChar).Value = Tache.Nom;
            RequestUpdatetache.Parameters.Add("@Desc", SqlDbType.VarChar).Value = Tache.Description;
            RequestUpdatetache.Parameters.Add("@DateCrea", SqlDbType.Date).Value = Tache.DateCreation;

            RequestUpdatetache.ExecuteNonQuery();

            // Fermeture de la connexion
            this.activeConnexion.Close();
        }

        public int getLastId()
        {

            this.chkConnexion();

            int lastId = 0;
            SqlCommand RequestGetLastId = activeConnexion.CreateCommand();
            RequestGetLastId.CommandText = "Select max(Id) from TACHES";

            var id = RequestGetLastId.ExecuteScalar().ToString();

            // Fermeture de la connexion
            this.activeConnexion.Close();

            if (id is null || id.Length == 0)
            {
                lastId = 1;
            }
            else
            {
                lastId = Convert.ToInt32(id);
                lastId = lastId + 1;
            }
            return lastId;
        }

        public List<Taches> GetTaches()
        {
            this.chkConnexion();

            List<Taches> Taches = new List<Taches>();

            SqlCommand RequestGetTaches = activeConnexion.CreateCommand();
            RequestGetTaches.CommandText = "Select * from taches";

            SqlDataReader taches = RequestGetTaches.ExecuteReader();

            while (taches.Read())
            {
                Taches oneTache = new Taches();
                oneTache.Id = Int32.Parse(taches[0].ToString());
                oneTache.Nom = $"{taches[1]}";
                oneTache.Description = $"{taches[2]}";
                oneTache.DateCreation = DateTime.Parse(taches[3].ToString());
                oneTache.DateFermeture = taches[4].ToString() == "" ? DateTime.MinValue : DateTime.Parse(taches[4].ToString());
                Taches.Add(oneTache);
            }

            // Fermeture de la connexion
            this.activeConnexion.Close();


            return Taches;
        }
        public void MarkCompleted(int Id)
        {
            this.chkConnexion();

            SqlCommand RequestUpdatetache = activeConnexion.CreateCommand();
            RequestUpdatetache.CommandText = "UPDATE TACHES SET dateFermeture = @dateFermeture WHERE Id = @id";

            RequestUpdatetache.Parameters.Add("@dateFermeture", SqlDbType.Date).Value = DateTime.Now.Date;
            RequestUpdatetache.Parameters.Add("@id", SqlDbType.Int).Value = Id;

            RequestUpdatetache.ExecuteNonQuery();

            // Fermeture de la connexion
            this.activeConnexion.Close();
        }

        public void DeleteTache(int Id)
        {

            this.chkConnexion();

            //Meilleure pratique d'uliser le using, il n'y a plus à se soucier des open/close de la connection
            using SqlCommand RequestUpdatetache = activeConnexion.CreateCommand();
            RequestUpdatetache.CommandText = "DELETE TACHES WHERE Id = @id";

            RequestUpdatetache.Parameters.Add("@id", SqlDbType.Int).Value = Id;

            RequestUpdatetache.ExecuteNonQuery();
        }

        //Ici la meilleure façon d'écrire GetTaches() avec les ouvertures/fermetures de connection et stream avec using - Plus difficile à lire mais qui ressemble à ce que vous devez arriver in fine
        //public List<Taches> GetTaches()
        //{
        //    this.chkConnexion();

        //    List<Taches> Taches = new List<Taches>();

        //    using (SqlCommand RequestGetTaches = activeConnexion.CreateCommand())
        //    {
        //        RequestGetTaches.CommandText = "SELECT * FROM TACHES";

        //        using (SqlDataReader taches = RequestGetTaches.ExecuteReader())
        //        {
        //            while (taches.Read())
        //            {
        //                Taches oneTache = new Taches
        //                {
        //                    Id = Int32.Parse(taches[0].ToString()),
        //                    Nom = $"{taches[1]}",
        //                    Description = $"{taches[2]}",
        //                    DateCreation = DateTime.Parse(taches[3].ToString()),
        //                    DateFermeture = taches[4].ToString() == "" ? DateTime.MinValue : DateTime.Parse(taches[4].ToString())
        //                };
        //                Taches.Add(oneTache);
        //            }
        //        }
        //    }
        //    return Taches;
        //}
    }
}
