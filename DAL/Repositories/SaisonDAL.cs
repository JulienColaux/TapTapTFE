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
                                ID_Trophee = id
                            };
                        }
                    }
                }
            }
            return saison;
        }

    }
}
