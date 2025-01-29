using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
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

        public JoueurDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //---------------------------------GET ALL TROPHY D UN JOUEUR----------------------------------------------------------------------------------

        //J ai mis la méthode en version synchrone par simpliciter a voir si je change apres


        public List<Trophee> GetAllTropheesByJoueurId(int id)
        {
            List<Trophee> trophees = new List<Trophee>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();  // Suppression du "await"
                string sql = "SELECT * FROM Trophée WHERE ID_Joueur = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())  // Suppression du "await"
                    {
                        while (reader.Read())
                        {
                            Trophee trophee = new Trophee
                            {
                                ID_Trophée = reader.GetInt32(reader.GetOrdinal("ID_Trophée")),
                                Nom = reader.GetString(reader.GetOrdinal("Nom")),
                                Date_Acquisition = reader.GetDateTime(reader.GetOrdinal("Date_Acquisition")),
                                ID_Joueur = reader.GetInt32(reader.GetOrdinal("ID_Joueur"))
                            };
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
                                Trophees = GetAllTropheesByJoueurId(id)
                            };
                        }
                    }
                }
            }
            return joueur;
        }



        //-----------------------------------GET CLASSEMENT JOUEUR------------------------------------------------------------------------------------


        public async Task<List<Joueur>> GetClassement()
        {
            List<Joueur> joueurs = new List<Joueur>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = @"
                    SELECT Joueur.ID_Joueur, Joueur.Nom, COUNT(Trophée.ID_Trophée) AS NombreDeTrophées
                    FROM Joueur
                    LEFT JOIN Trophée ON Joueur.ID_Joueur = Trophée.ID_Joueur
                    GROUP BY Joueur.ID_Joueur, Joueur.Nom
                    ORDER BY NombreDeTrophées DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            joueurs.Add(new Joueur
                            {
                                ID_Joueur = (int)reader["ID_Joueur"],
                                Nom = reader["Nom"].ToString(),
                            });
                        }
                    }
                }
            }

            return joueurs;
        }
    }
}
