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
    public class SaisonDAL
    {
        //-------------------------------CONFIG  CONNECTION  STRING-------------------------------------------------------------------------------------


        private readonly string _connectionString;

        public SaisonDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //-------------------------------GET SAISON BY ID--------------------------------------------------------------------------------------------------------    !!!!!!

        //Attention ici check si ca marche parcque j ai mis que l id trophee = id saison donc meme si a priori cest le cas a voir


        public async Task<Saison>GetSaisonById(int id)
        {
            Saison saison = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "SELECT * FROM Saison WHERE ID_Saison = @id ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using(SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            saison = new Saison
                            {
                                ID_Saison = reader.GetInt32(reader.GetOrdinal("ID_Saison")),
                                Decompte = reader.GetDateTime(reader.GetOrdinal("Decompte")),
                                ID_Trophée = id
                            };
                        }
                    }
                }
            }
            return saison;
        }


        //-----------------------------------------ADD SAISON-------------------------------------------------------------------------------------


        public async Task<int> AddSaison()
        {
            int saisonId = 0; // Pour stocker l'ID de la saison insérée

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string sql = @"
            INSERT INTO Saison (Decompte, ID_Trophée) 
            VALUES (DATEADD(DAY, 30, GETDATE()), NULL); -- ID_Trophee sera NULL
            SELECT SCOPE_IDENTITY();"; // Récupérer l'ID de la dernière insertion

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Exécuter la requête et récupérer l'ID de la nouvelle saison
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        saisonId = id;
                    }
                }
            }

            return saisonId; // Retourne l'ID de la saison ajoutée
        }


        //-----------------------------------------ADD JOUE-------------------------------------------------------------------------------------

        public async Task AddParticipe(int joueurId, int saisonId, int points)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "INSERT INTO Participe (ID_Joueur, ID_Saison,Points) VALUES (@joueurId, @saisonId, @points);";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@joueurId", joueurId);
                    cmd.Parameters.AddWithValue("@saisonId", saisonId);
                    cmd.Parameters.AddWithValue("@points", points);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        //--------------------------------------GET CLASSEMENT--------------------------------------------------------------------------

        public async Task<List<JoueurPartie>> GetClassement(int saisonId)
        {
            List<JoueurPartie> classement = new List<JoueurPartie>();  // Liste des joueurs classés

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string sql = "SELECT j.ID_Joueur, j.Nom, j.Avatar_URL, SUM(p.points) AS Total_Points " +
                             "FROM Participe p " +
                             "JOIN Joueur j ON p.ID_Joueur = j.ID_Joueur " +
                             "WHERE p.ID_Saison = @saisonId " +
                             "GROUP BY j.ID_Joueur, j.Nom, j.Avatar_URL " +
                             "ORDER BY Total_Points DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@saisonId", saisonId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())  // Boucle sur tous les joueurs trouvés
                        {
                            JoueurPartie joueur = new JoueurPartie
                            {
                                ID_Joueur = reader.GetInt32(0),  // Récupère ID_Joueur
                                Nom = reader.GetString(1),  // Récupère Nom
                                Avatar_URL = reader.IsDBNull(2) ? null : reader.GetString(2),  // Gère Avatar_URL NULL
                                Points = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)  // Gère Total_Points NULL
                            };

                            classement.Add(joueur);
                        }
                    }
                }
            }

            return classement;  // Retourne la liste des joueurs classés
        }


    }
}
