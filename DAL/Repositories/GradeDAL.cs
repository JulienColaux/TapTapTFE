using System;
using System.Collections.Generic;
using System.Data;  
using Microsoft.Data.SqlClient;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models;


namespace DAL.Repositories
{
    public class GradeDAL
    {
        //-------------------------------CONFIG  CONNECTION  STRING-------------------------------------------------------------------------------------


        private readonly string _connectionString;

        public GradeDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //-------------------------------GET GRADE BY ID--------------------------------------------------------------------------------------------------------




        public async Task<Grade> GetGradeById(int id)
        {
            Grade grade = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "SELECT * FROM EchelleGrade WHERE ID_EchelleGrade = @id ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            grade = new Grade
                            {
                                ID_EchelleGrade = reader.GetInt32(reader.GetOrdinal("ID_EchelleGrade")),
                                Nom = reader["Nom"] != DBNull.Value ? reader["Nom"].ToString() : "Inconnu",
                                ValeurMinimal = reader["ValeurMinimal"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("ValeurMinimal")) : 0,// Gestion du NULL
                            };
                        }
                    }
                }
            }
            return grade;
        }


        //-------------------------------GET ALL GRADE--------------------------------------------------------------------------------------------------------


        public async Task<List<Grade>> GetAllGrades()
        {
            List<Grade> grades = new List<Grade>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "SELECT * FROM EchelleGrade"; // On récupère toutes les lignes

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) // Lire chaque ligne de la table
                        {
                            Grade grade = new Grade
                            {
                                ID_EchelleGrade = reader.GetInt32(reader.GetOrdinal("ID_EchelleGrade")),
                                Nom = reader["Nom"] != DBNull.Value ? reader["Nom"].ToString() : "Inconnu",
                                ValeurMinimal = reader["ValeurMinimal"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("ValeurMinimal")) : 0
                            };

                            grades.Add(grade); // On ajoute le grade à la liste
                        }
                    }
                }
            }
            return grades;
        }

    }
}

