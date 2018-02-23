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

		//[TestMethod]
		//public void ReturnAvailableSitesTest()
		//{
		//	try
		//	{
		//		using (TransactionScope transaction = new TransactionScope())
		//		{
		//			//Arrange
					
					
		//			//Act
					

		//			//Assert
					
		//		}
		//	}
		//	catch (SqlException ex)
		//	{
		//		Console.WriteLine(ex.Message);
		//		throw;
		//	}
		//}


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
