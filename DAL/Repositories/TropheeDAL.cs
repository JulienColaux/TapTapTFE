using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                                ID_Joueur = reader["ID_Joueur"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("ID_Joueur")) : (int?)null //!= DBNULL check si c est pas nul et :(int?) null renvoie null si c est null
                        };
                        }
                    }
                }
            }
            return trophee;
        }
    }
}
