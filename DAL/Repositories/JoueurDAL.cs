using System;
using System.Collections.Generic;
using System.Data;  // ✅ Obligatoire pour CommandType et DataTable
using Microsoft.Data.SqlClient;  // ✅ Correct pour SQL Server
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models;

namespace DAL.Repositories
{
    public class JoueurDAL
    {

        //-------------------------------CONFIG  CONNECTION  STRING-------------------------------------------------------------------------------------


        private readonly string _connectionString;
        private readonly TropheeDAL _tropheeDAL;


        public JoueurDAL(IConfiguration configuration, TropheeDAL tropheeDAL)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tropheeDAL = tropheeDAL;

        }

        //---------------------------------GET ALL TROPHY D UN JOUEUR----------------------------------------------------------------------------------

        //J ai mis la méthode en version synchrone par simpliciter a voir si je change apres


        public async Task<List<Trophee>> GetAllTropheesByJoueurId(int id)
        {
            List<Trophee> trophees = new List<Trophee>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync(); // Utilisation de await pour éviter les blocages
                string sql = "SELECT * FROM Trophée WHERE ID_Joueur = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) // Ajout de await ici
                    {
                        while (await reader.ReadAsync()) // Ajout de await ici
                        {
                            Trophee trophee = new Trophee
                            {
                                ID_Trophée = reader.GetInt32(reader.GetOrdinal("ID_Trophée")),
                                Nom = reader.GetString(reader.GetOrdinal("Nom")),
                                Date_Acquisition = reader.GetDateTime(reader.GetOrdinal("Date_Acquisition")),
                                ID_Joueur = reader.GetInt32(reader.GetOrdinal("ID_Joueur")) 
                            };
                               trophee.Url_image  =  await _tropheeDAL.GetUrlImageTropheeById(id);
                            trophees.Add(trophee);
                        }
                    }
                }
            }
            return trophees;
        }




        //----------------------------------GET JOUEUR BY ID----------------------------------------------------------------------------------------------------



        public async Task<Joueur> GetJoueurById(int id)
        {
            Joueur joueur = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "SELECT * FROM Joueur WHERE ID_Joueur = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            joueur = new Joueur
                            {
                                ID_Joueur = reader.GetInt32(reader.GetOrdinal("ID_Joueur")),
                                Nom = reader["Nom"] != DBNull.Value ? reader["Nom"].ToString() : "Inconnu",
                                Avatar_URL = reader["Avatar_URL"] != DBNull.Value ? reader["Avatar_URL"].ToString() : null,
                                XP = reader["XP"] != DBNull.Value ? reader.GetDecimal(reader.GetOrdinal("XP")) : 0m,
                                ID_EchelleGrade = reader["ID_EchelleGrade"] != DBNull.Value ? (int?)reader["ID_EchelleGrade"] : null,
                                Elo = reader["Elo"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("Elo")) : 0 ,// Gestion du NULL
                                Trophees = await GetAllTropheesByJoueurId(id) //rajout de await car la methode est synchrone
                            };
                        }
                    }
                }
            }
            return joueur;
        }



        //-----------------------------------ADD SEASON POINT TO JOUEUR------------------------------------------------------------------------------------

        public async Task AddPoints(int joueurId, int pointsToAdd)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Démarrer une transaction pour regrouper les deux mises à jour
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Mise à jour de la table Joueur : ajouter des points à la colonne Elo
                        string sqlJoueur = "UPDATE Joueur SET Elo = Elo + @pointsToAdd WHERE ID_Joueur = @joueurId";
                        using (SqlCommand cmdJoueur = new SqlCommand(sqlJoueur, conn, transaction))
                        {
                            cmdJoueur.Parameters.AddWithValue("@joueurId", joueurId);
                            cmdJoueur.Parameters.AddWithValue("@pointsToAdd", pointsToAdd);
                            await cmdJoueur.ExecuteNonQueryAsync();
                        }

                        // Mise à jour de la table Participe : ajouter des points à la colonne Points
                        // (Supposant que la table Participe possède une colonne ID_Joueur pour identifier le joueur)
                        string sqlParticipe = "UPDATE Participe SET Points = Points + @pointsToAdd WHERE ID_Joueur = @joueurId";
                        using (SqlCommand cmdParticipe = new SqlCommand(sqlParticipe, conn, transaction))
                        {
                            cmdParticipe.Parameters.AddWithValue("@joueurId", joueurId);
                            cmdParticipe.Parameters.AddWithValue("@pointsToAdd", pointsToAdd);
                            await cmdParticipe.ExecuteNonQueryAsync();
                        }

                        // Valider la transaction si les deux mises à jour ont réussi
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Annuler la transaction en cas d'erreur
                        transaction.Rollback();
                        throw;  // Vous pouvez aussi logger l'erreur ou la gérer selon vos besoins
                    }
                }
            }
        }



        //-----------------------------------ADD  XP TO JOUEUR------------------------------------------------------------------------------------

        public async Task AddXP(int joueurId, int xpToAdd)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string sql = "UPDATE Joueur SET XP = XP + @xpToAdd WHERE ID_Joueur = @joueurId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@joueurId", joueurId);
                    cmd.Parameters.AddWithValue("@xpToAdd", xpToAdd);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


    }
}
