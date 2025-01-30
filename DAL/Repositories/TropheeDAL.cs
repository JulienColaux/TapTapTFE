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
                            trophee.Url_image = await GetUrlImageTropheeById(trophee.ID_Trophée);
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
                string sql = "SELECT Image_URL FROM imagesStock WHERE ID_Trophée = @tropheeId";

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


    }
}
