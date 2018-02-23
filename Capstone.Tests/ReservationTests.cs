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


				//Act


				//Assert

			}
		}

		public static int InsertFakeReservation(int siteId, string name, DateTime fromDate, DateTime endDate, DateTime createDate)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

			int reservationId = 0;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date, create_date) VALUES (@siteId, @name, @fromDate, @endDate, @createDate;", conn);
					cmd.Parameters.AddWithValue("@siteId", siteId);
					cmd.Parameters.AddWithValue("@name", name);
					cmd.Parameters.AddWithValue("@fromDate", fromDate);
					cmd.Parameters.AddWithValue("@endDate", endDate);
					cmd.Parameters.AddWithValue("@createDate", createDate);
					cmd.ExecuteNonQuery();

					cmd = new SqlCommand("SELECT")

				}

			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}

			return reservationId;
		}
	}
}
