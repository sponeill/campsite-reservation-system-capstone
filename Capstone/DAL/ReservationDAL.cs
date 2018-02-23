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
    public class ReservationDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        public List<AvailableReservations> GetAllReservations(int parkId, int campgroundId, DateTime startDate, DateTime endDate)
        {
            List<AvailableReservations> reservations = new List<AvailableReservations>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.daily_fee" +
                        " FROM park JOIN campground ON park.park_id = campground.park_id JOIN site ON campground.campground_id = site.campground_id JOIN reservation ON " +
                        " site.site_id = reservation.site_id WHERE campground.campground_id = @campgroundId AND (@startDate <= reservation.from_date OR " +
                        " @startDate >= reservation.to_date) AND (@endDate <= reservation.from_date OR @endDate >= reservation.to_date) AND (MONTH(@startDate)) >= campground.open_from_mm AND" +
                        " (MONTH(@endDate)) <= campground.open_to_mm;", conn);

                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@startDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@endDate", endDate.Date);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        AvailableReservations availableReservations = new AvailableReservations();
                        availableReservations.Accessible = Convert.ToBoolean(reader["accessible"]);
                        availableReservations.Cost = Convert.ToDecimal(reader["daily_fee"]);
                        availableReservations.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        availableReservations.MaxRvLenth = Convert.ToInt32(reader["max_rv_length"]);
                        availableReservations.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        availableReservations.Utilities = Convert.ToBoolean(reader["utilities"]);

                        reservations.Add(availableReservations);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error getting parks: " + ex.Message);
            }
            catch (NullReferenceException nullEx)
            {
                Console.WriteLine(nullEx.Message);
            }
            return reservations;
        }
        public int Reserve(int siteId, string name, DateTime startDate, DateTime endDate)
        {
            int confirmationId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation(site_id, name, from_date, to_date, create_date) VALUES (@siteId, @name, @fromDate, @toDate, @createDate);", conn);
                    cmd.Parameters.AddWithValue("@siteId", siteId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@fromDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", endDate.Date);
                    cmd.Parameters.AddWithValue("@createDate", DateTime.UtcNow);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT reservation.reservation_id FROM reservation WHERE site_id = @siteId AND name = @name AND from_date = @fromDate AND to_date = @toDate;", conn);
                    cmd.Parameters.AddWithValue("@siteId", siteId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@fromDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", endDate.Date);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        confirmationId = Convert.ToInt32(reader["reservation_id"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error happened making reservation.");
            }

            return confirmationId;
        }
    }
}
