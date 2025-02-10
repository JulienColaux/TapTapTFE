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
            Random random = new Random();
            int randomImageStockId = random.Next(1, 4); // génère un nombre entre 1 et 30

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
                               trophee.Url_image  =  await _tropheeDAL.GetUrlImageTropheeByTropheeId(trophee.ID_Trophée);
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
                                Trophees = await GetAllTropheesByJoueurId(id) 
                            };
                        }
                    }
                }
            }
            return joueur;
        }



        //-----------------------------------ADD SEASON POINT TO JOUEUR------------------------------------------------------------------------------------

        public async Task AddPoints(int joueurId, int seasonId, int pointsToAdd)
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
                        string sqlParticipe = "UPDATE Participe SET Points = Points + @pointsToAdd WHERE ID_Joueur = @joueurId  AND ID_Saison = @seasonId;";
                        using (SqlCommand cmdParticipe = new SqlCommand(sqlParticipe, conn, transaction))
                        {
                            cmdParticipe.Parameters.AddWithValue("@joueurId", joueurId);
                            cmdParticipe.Parameters.AddWithValue("@seasonId", seasonId);
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
        //-----------------------------------RESET SEASON POINT OF JOUEUR---------------------------------------------------------------

        public async Task ResetPointsById(int joueurId)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "UPDATE Joueur SET Elo = 0 WHERE ID_Joueur = @joueurId;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@joueurId", SqlDbType.Int).Value = joueurId;

                    await cmd.ExecuteNonQueryAsync();

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

                // Vérifier si le joueur doit monter de grade
                await UpgradeJoueurIfNeeded(joueurId, conn);
            }
        }


        //-----------------------------------CHECK SI MONTE DE NIVEAU----------------------------------------------------------------------


        private async Task UpgradeJoueurIfNeeded(int joueurId, SqlConnection conn)
        {
            bool hasUpgraded = false;

            do
            {
                hasUpgraded = false;

                // 1️⃣ Récupérer l'XP et le grade actuel du joueur
                string sqlJoueur = "SELECT XP, Id_EchelleGrade FROM Joueur WHERE ID_Joueur = @joueurId";
                int xpJoueur = 0;
                int gradeActuel = 0;

                using (SqlCommand cmd = new SqlCommand(sqlJoueur, conn))
                {
                    cmd.Parameters.AddWithValue("@joueurId", joueurId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            xpJoueur = Convert.ToInt32(reader["XP"]);
                            gradeActuel = Convert.ToInt32(reader["Id_EchelleGrade"]);
                        }
                    }
                }

                // 2️⃣ Récupérer la valeur minimale du grade actuel
                string sqlGrade = "SELECT ValeurMinimal FROM EchelleGrade WHERE Id_EchelleGrade = @gradeId";
                int valeurMinimal = 0;

                using (SqlCommand cmd = new SqlCommand(sqlGrade, conn))
                {
                    cmd.Parameters.AddWithValue("@gradeId", gradeActuel);
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null)
                        valeurMinimal = Convert.ToInt32(result);
                }

                // 3️⃣ Vérifier si le joueur doit monter en grade
                if (xpJoueur >= valeurMinimal)
                {
                    // Récupérer le prochain grade
                    string sqlNextGrade = @"
                SELECT TOP 1 Id_EchelleGrade 
                FROM EchelleGrade 
                WHERE ValeurMinimal > @valeurMinimal 
                ORDER BY ValeurMinimal ASC";

                    int? nextGrade = null;

                    using (SqlCommand cmd = new SqlCommand(sqlNextGrade, conn))
                    {
                        cmd.Parameters.AddWithValue("@valeurMinimal", valeurMinimal);
                        object result = await cmd.ExecuteScalarAsync();
                        if (result != null)
                            nextGrade = Convert.ToInt32(result);
                    }

                    if (nextGrade != null)
                    {
                        // Déduire l'XP du grade actuel et passer au suivant
                        xpJoueur -= valeurMinimal;

                        // Mettre à jour le joueur avec le nouveau grade et l'XP restant
                        string sqlUpdateJoueur = "UPDATE Joueur SET Id_EchelleGrade = @nextGrade, XP = @xpJoueur WHERE ID_Joueur = @joueurId";

                        using (SqlCommand cmd = new SqlCommand(sqlUpdateJoueur, conn))
                        {
                            cmd.Parameters.AddWithValue("@nextGrade", nextGrade);
                            cmd.Parameters.AddWithValue("@xpJoueur", xpJoueur);
                            cmd.Parameters.AddWithValue("@joueurId", joueurId);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        hasUpgraded = true; // On vérifie à nouveau s'il peut encore monter
                    }
                }

            } while (hasUpgraded); // Tant qu'il peut monter, on continue
        }


    }
}
