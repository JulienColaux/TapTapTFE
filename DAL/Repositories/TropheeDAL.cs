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
    public class TropheeDAL
    {

        //-------------------------------CONFIG  CONNECTION  STRING-------------------------------------------------------------------------------------



        private readonly string _connectionString;

        public TropheeDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //------------------------------GET TROPHEE BY ID------------------------------------------------------------------------------------------------------


        public async Task<Trophee> GetTropheeById(int id)
        {
            Trophee trophee = null;

            using(SqlConnection conn = new SqlConnection(_connectionString))
            {

                await conn .OpenAsync();

                string sql = "SELECT * FROM Trophée WHERE ID_Trophée = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using(SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            trophee = new Trophee
                            {
                                ID_Trophée = reader.GetInt32(reader.GetOrdinal("ID_Trophée")),
                                Nom = reader["Nom"] != DBNull.Value ? reader["Nom"].ToString() : "Inconnu",
                                Date_Acquisition = reader.GetDateTime(reader.GetOrdinal("Date_Acquisition")),
                                ID_Joueur = reader["ID_Joueur"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("ID_Joueur")) : (int?)null
                            };
                            trophee.Url_image = await GetUrlImageTropheeByTropheeId(trophee.ID_Trophée);
                        }
                    }
                }
            }
            return trophee;
        }

        //--------------------------GET URL IMAGE TROPHEE BY ID---------------------------------------------------------------------------------------


        public async Task<string> GetUrlImageTropheeById(int id)
        {

            string url = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                string sql = "SELECT Image_URL FROM imagesStock WHERE ID_imagesStock = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            url = reader.GetString(reader.GetOrdinal("Image_URL"));
                        }
                        else
                        {
                            Console.WriteLine($" Aucune URL trouvée pour ID {id}");
                        }
                    }
                }
            }
            return url;
        }
  


        //--------------------------GET URL IMAGE TROPHEE BY TROPHEE ID-------------------------------------------------------------------------------

        //je sais pas si je dois chopper l url avec l id du trophee ou avec lid du image stock pour moi cest pareil vu quelle sont en identity et creer en meme temps


        public async Task<string> GetUrlImageTropheeByTropheeId(int tropheeId)
        {

            string url = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                string sql = "SELECT i.Image_URL FROM Trophée t JOIN imagesStock i ON t.Id_imagesStock = i.Id_imagesStock WHERE t.ID_Trophée = @tropheeId;";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@tropheeId", tropheeId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            url = reader.GetString(reader.GetOrdinal("Image_URL"));
                        }
                        else
                        {
                            Console.WriteLine($" Aucune URL trouvée pour ID_Trophée = {tropheeId}");
                        }
                    }
                }
            }
            return url;
        }


        //--------------------------ADD TROPHEE---------------------------------------------------------------------------------------------------

        //la méthode retourne l id du trophee creer au cas ou faut check 

        public async Task<int> CreateTrophee(CreateTropheeDto trophee)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();


                Random random = new Random();
                int randomImageStockId = random.Next(1, 4); // génère un nombre entre 1 et 30


                string sql = " INSERT INTO Trophée (Nom, ID_imagesStock, ID_Joueur, ID_Saison) VALUES (@Nom, @ID_imagesStock, @ID_Joueur, @ID_Saison) SELECT CAST (SCOPE_IDENTITY() AS int);";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Nom", SqlDbType.VarChar, 50).Value = trophee.Nom;
                    cmd.Parameters.Add("@ID_imagesStock", SqlDbType.Int).Value = randomImageStockId;
                    cmd.Parameters.Add("@ID_Joueur", SqlDbType.Int).Value = trophee.ID_Joueur;
                    cmd.Parameters.Add("@ID_Saison", SqlDbType.Int).Value = trophee.ID_Saison;

                    var result = await  cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        //--------------------------GET ALL TROPHEES--------------------------------------------------------------------------------------------------


        public async Task<List<TropheeForGetAll>> GetAllTrophee()
        {
            List<TropheeForGetAll> trophees = new List<TropheeForGetAll>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "SELECT ID_Trophée, ID_Joueur, ID_Saison, Nom, Date_Acquisition, ID_ImagesStock  FROM Trophée;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) // Utilisation de ReadAsync() pour meilleure performance
                        {
                            int idTrophee = reader.GetInt32(reader.GetOrdinal("ID_Trophée"));

                            trophees.Add(new TropheeForGetAll
                            {
                                ID_Trophée = reader.GetInt32(reader.GetOrdinal("ID_Trophée")),
                                ID_Joueur = reader.GetInt32(reader.GetOrdinal("ID_Joueur")),
                                ID_Saison = reader.GetInt32(reader.GetOrdinal("ID_Saison")),
                                Nom = reader["Nom"] != DBNull.Value ? reader["Nom"].ToString() : "Inconnu",
                                Date_Acquisition = reader.GetDateTime(reader.GetOrdinal("Date_Acquisition")),
                                Url_image = await GetUrlImageTropheeByTropheeId(idTrophee)
                            });
                        }
                    }
                }
            }
            return trophees; // Retourne la liste des joueurs
        }
    }
}
