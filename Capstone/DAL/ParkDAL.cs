using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ParkDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        public List<Park> GetAllParks()
        {
            List<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM park ORDER BY name;", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.AnnualVisitorCount = Convert.ToInt32(reader["visitors"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Description = Convert.ToString(reader["description"]);
                        park.EstablishedDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.ParkId = Convert.ToInt32(reader["park_id"]);
                        parks.Add(park);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error getting parks: " + ex.Message);
            }
            return parks;
        }
    }
}
