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
	public class CampgroundDAL
	{
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        public List<Campground> GetAllCampgrounds(int parkId)
        {
            List<Campground> campgrounds = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE park_id = @parkId ORDER BY campground_id;", conn);
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        campground.ClosingMonth = Convert.ToInt32(reader["open_to_mm"]);
                        campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.OpeningMonth = Convert.ToInt32(reader["open_from_mm"]);
                        campground.ParkId = Convert.ToInt32(reader["park_id"]);
                        campgrounds.Add(campground);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error getting parks: " + ex.Message);
            }
            return campgrounds;
        }
    }
}
