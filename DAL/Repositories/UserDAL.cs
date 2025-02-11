using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;


namespace DAL.Repositories
{
    public class UserDAL
    {

        //-------------------------------CONFIG  CONNECTION  STRING-------------------------------------------------------------------------------------



        private readonly string _connectionString;


        public UserDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }


        //-------------------------------ADD USER-------------------------------------------------------------------------------------

        public bool AddUser(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                string query = "INSERT INTO Userr (Email, MotDePasse, EstActive) VALUES (@Email, @Password, 1)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }


        //-------------------------------CHECK USER INFO-------------------------------------------------------------------------------------




        public bool ValidateUser(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT MotDePasse FROM Userr WHERE Email = @Email AND EstActive = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string storedPassword = result.ToString();
                    return BCrypt.Net.BCrypt.Verify(password, storedPassword);
                }
                return false;
            }
        }

        //------------------------------GET USER  ID BY MAIL----------------------------------------------------------------------------
        public int? GetUserIdWithMail(string mail)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT ID_Connexion FROM Userr WHERE Email = @mail;";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@mail", mail);

                conn.Open();
                object result = cmd.ExecuteScalar();

                // Vérifier si le résultat est NULL
                return result != null ? (int?)result : null;
            }
        }

        //------------------------------GET USER JOUEUR ID----------------------------------------------------------------------------

        public int? GetJoueurIdWithUserId(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT ID_Joueur FROM Joueur WHERE ID_Connexion = @userId;";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@userId", userId);

                conn.Open();
                object result = cmd.ExecuteScalar();

                // Vérifier si le résultat est NULL
                return result != null ? (int?)result : null;
            }
        }

    }
}
