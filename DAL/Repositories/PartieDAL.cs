using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class PartieDAL
    {
        //-------------------------------CONFIG  CONNECTION  STRING-------------------------------------------------------------------------------------



        private readonly string _connectionString;

        public PartieDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //---------------------------------GET ALL INFO OF A PARTIE----------------------------------------------------------------------------------


        public async Task<Partie> GetPartieByID(int id)
        {
            Partie partie = null;
            List<Joueur> joueurs = new List<Joueur>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string sql = "SELECT P.ID_Partie, P.Date_Partie, P.Amical, " +
                             "J.ID_Joueur, J.Nom, J.Avatar_URL, Jo.Points " +
                             "FROM Joue Jo " +
                             "JOIN Joueur J ON Jo.ID_Joueur = J.ID_Joueur " +
                             "JOIN Partie P ON Jo.ID_Partie = P.ID_Partie " +
                             "WHERE P.ID_Partie = @id";  // Utilisation correcte du paramètre

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (partie == null)
                            {
                                // Initialisation de la partie (on le fait une seule fois grace au if
                                partie = new Partie
                                {
                                    ID_Partie = reader.GetInt32(reader.GetOrdinal("ID_Partie")),
                                    Date_Partie = reader.GetDateTime(reader.GetOrdinal("Date_Partie")),
                                    Amical = reader.GetBoolean(reader.GetOrdinal("Amical")),
                                    Joueurs = new List<JoueurPartie>() // Initialisation de la liste des joueurs
                                };
                            }

                            // Création d'un joueur pour chaque ligne trouvée
                            JoueurPartie joueur = new JoueurPartie
                            {
                                ID_Joueur = reader.GetInt32(reader.GetOrdinal("ID_Joueur")),
                                Nom = reader.GetString(reader.GetOrdinal("Nom")),
                                Avatar_URL = reader.IsDBNull(reader.GetOrdinal("Avatar_URL")) ? null : reader.GetString(reader.GetOrdinal("Avatar_URL")),
                                Points = reader.GetInt32(reader.GetOrdinal("Points"))
                            };

                            // Ajout du joueur à la liste
                            partie.Joueurs.Add(joueur);
                        }
                    }
                }
            }
            return partie;
        }

        //---------------------------------GET ALL INFO OF A PARTIE----------------------------------------------------------------------------------

        public async Task<List<Partie>> GetAllParties()
        {
            List<Partie> parties = new List<Partie>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string sql = "SELECT P.ID_Partie, P.Date_Partie, P.Amical, " +
                             "J.ID_Joueur, J.Nom, J.Avatar_URL, Jo.Points " +
                             "FROM Joue Jo " +
                             "JOIN Joueur J ON Jo.ID_Joueur = J.ID_Joueur " +
                             "JOIN Partie P ON Jo.ID_Partie = P.ID_Partie " +
                             "ORDER BY P.ID_Partie"; // On trie par ID pour faciliter le regroupement

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        Partie currentPartie = null;

                        while (await reader.ReadAsync())
                        {
                            int partieId = reader.GetInt32(reader.GetOrdinal("ID_Partie"));

                            // Si on change de partie, on en crée une nouvelle
                            if (currentPartie == null || currentPartie.ID_Partie != partieId)
                            {
                                currentPartie = new Partie
                                {
                                    ID_Partie = partieId,
                                    Date_Partie = reader.GetDateTime(reader.GetOrdinal("Date_Partie")),
                                    Amical = reader.GetBoolean(reader.GetOrdinal("Amical")),
                                    Joueurs = new List<JoueurPartie>()
                                };

                                parties.Add(currentPartie);
                            }

                            // Création du joueur et ajout à la partie actuelle
                            JoueurPartie joueur = new JoueurPartie
                            {
                                ID_Joueur = reader.GetInt32(reader.GetOrdinal("ID_Joueur")),
                                Nom = reader.GetString(reader.GetOrdinal("Nom")),
                                Avatar_URL = reader.IsDBNull(reader.GetOrdinal("Avatar_URL")) ? null : reader.GetString(reader.GetOrdinal("Avatar_URL")),
                                Points = reader.GetInt32(reader.GetOrdinal("Points"))
                            };

                            currentPartie.Joueurs.Add(joueur);
                        }
                    }
                }
            }

            return parties;
        }



        //-----------------------------------------ADD PARTIE-------------------------------------------------------------------------------------


        public async Task<int> AddPartie(bool amical)
        {
            int partieId = 0; // Pour stocker l'ID de la partie insérée

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string sql = @"INSERT INTO Partie (Date_Partie, Amical) 
                       VALUES (GETDATE(), @Amical);
                       SELECT SCOPE_IDENTITY();"; // Récupérer l'ID généré pour ajouter joueur

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Amical", amical);

                    // Exécuter la requête et récupérer l'ID
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null)
                    {
                        partieId = Convert.ToInt32(result);
                    }
                }
            }

            return partieId; // Retourne l'ID de la partie ajoutée
        }

        //-----------------------------------------ADD JOUE-------------------------------------------------------------------------------------

        public  async Task AddJoue(int joueurId, int partieId, int points)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "INSERT INTO Joue (ID_Joueur, ID_Partie,Points) VALUES (@joueurId, @partieId, @points);";

                using(SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@joueurId", joueurId);
                    cmd.Parameters.AddWithValue("@partieId", partieId);
                    cmd.Parameters.AddWithValue("@points", points);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
