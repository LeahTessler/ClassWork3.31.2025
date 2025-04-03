using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3._31._2025.Data
{
    public class Ad
    { 
      public int Id { get; set; }
      public string Name { get; set; }
      public DateTime DatePosted { get; set; }
      public int PhoneNum { get; set; }
      public string AdContent { get; set; }
    }


    public class AdManager
    {
        private readonly string _connectionString;

        public AdManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Ad> GetAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Listings ORDER BY DATE DESC";

            connection.Open();
            var reader = cmd.ExecuteReader();
            List<Ad> ads = new();
            while (reader.Read())
            {
                ads.Add(new Ad()
                {

                    Id = (int)reader["Id"],
                    Name = (string)reader["PersonsName"],
                    DatePosted = (DateTime)reader["Date"],
                    PhoneNum = (int)reader["PhoneNum"],
                    AdContent = (string)reader["AdContent"]



                });


            };

            return ads;

        }

        public int AddAd(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Listings (PersonsName, Date, PhoneNum, AdContent) VALUES (@personsName, @date, @phoneNum, @adContent) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@personsName", ad.Name);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@phoneNum", ad.PhoneNum);
            cmd.Parameters.AddWithValue("@adContent", ad.AdContent);

            connection.Open();

            int id = (int)(decimal)cmd.ExecuteScalar();
            return id;

        }

        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Listings WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();



        }
    }
}
