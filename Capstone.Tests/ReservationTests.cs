using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using System.Configuration;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;
using Capstone;

namespace Capstone.Tests
{
	[TestClass]
	public class ReservationTests
	{
		string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

		[TestMethod]
		public void MakeReservationTest()
		{
			using (TransactionScope transaction = new TransactionScope())
			{
				//Arrange
				InsertFakeReservation(1, "Test Family", DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
				ReservationDAL testClass = new ReservationDAL();

				//Act
				List<Reservation> reservations = testClass.SearchReservations();
				bool containsNewReservation = reservations.Exists(r => r.Name == "Test Family");

				//Assert
				Assert.IsTrue(containsNewReservation);

			}
		}

		[TestMethod]
		public void ReserveMethodTest()
		{
			try
			{
				using (TransactionScope transaction = new TransactionScope())
				{
					//Arrange
					ReservationDAL testClass = new ReservationDAL();
					int reservationId = testClass.Reserve(20, "Test Family", DateTime.UtcNow, DateTime.UtcNow);

					//Act
					List<Reservation> reservations = testClass.SearchReservations();
					bool containsReservationId = reservations.Exists(r => r.ReservationId == reservationId);

					//Assert
					Assert.IsTrue(containsReservationId);

				}

			}
			catch(SqlException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}

		}

		[TestMethod]
		public void ReturnAvailableSitesTest()
		{
			try
			{
				using (TransactionScope transaction = new TransactionScope())
				{
					//Arrange
					ReservationDAL testClass = new ReservationDAL();


					//Act
					List<AvailableReservations> availableSites = testClass.GetAllReservationsUnlimted(1, 1, DateTime.Today, DateTime.Today);
					int expectedSiteCount = GetCorrectSiteCount(1, 1, DateTime.Today, DateTime.Today);


					//Assert
					Assert.AreEqual(expectedSiteCount, availableSites.Count);

				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public static int GetCorrectSiteCount(int parkId, int campgroundId, DateTime startDate, DateTime endDate)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
			int count = 0;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand("SELECT COUNT(*) as 'siteCount' FROM campground "
					+ "LEFT JOIN site ON campground.campground_id = site.campground_id "
					+ "WHERE site.site_id NOT IN(SELECT reservation.site_id FROM reservation "
					+ "WHERE(@startDate >= reservation.from_date OR @startDate <= reservation.to_date) "
					+ "AND(@endDate >= reservation.from_date OR @endDate <= reservation.to_date) "
					+ "AND(reservation.from_date >= @startDate) "
					+ "AND(reservation.to_date <= @endDate)) "
					+ "AND(MONTH(@startDate) >= campground.open_from_mm AND(MONTH(@startDate)) <= campground.open_to_mm) "
					+ "AND((MONTH(@endDate)) >= campground.open_from_mm AND(MONTH(@endDate)) <= campground.open_to_mm) "
					+ "AND campground.campground_id = @campgroundId", conn);
					cmd.Parameters.AddWithValue("@parkId", parkId);
					cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
					cmd.Parameters.AddWithValue("@startDate", startDate);
					cmd.Parameters.AddWithValue("@endDate", endDate);

					SqlDataReader reader = cmd.ExecuteReader();
					while(reader.Read())
					{
						count = Convert.ToInt32(reader["siteCount"]);
					}
				}


			}
			catch(SqlException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}

			return count;

		}

		public static bool InsertFakeReservation(int siteId, string name, DateTime fromDate, DateTime endDate, DateTime createDate)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date, create_date) VALUES (@siteId, @name, @fromDate, @endDate, @createDate);", conn);
					cmd.Parameters.AddWithValue("@siteId", siteId);
					cmd.Parameters.AddWithValue("@name", name);
					cmd.Parameters.AddWithValue("@fromDate", fromDate);
					cmd.Parameters.AddWithValue("@endDate", endDate);
					cmd.Parameters.AddWithValue("@createDate", createDate);
					cmd.ExecuteNonQuery();

					return true;
				}

			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
	}
}
