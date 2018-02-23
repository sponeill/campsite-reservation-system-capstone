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
	public class ParkTests
	{
		string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

		[TestMethod]
		public void GetAllParksTest()
		{
			using (TransactionScope transaction = new TransactionScope())
			{
				//Arrange
				int parkId = InsertFakeParK("Blue Stone", "Canada", DateTime.UtcNow, 587, 4, "This is a park.");
				ParkDAL testClass = new ParkDAL();

				//Act
				List<Park> parks = testClass.GetAllParks();
				bool containsAPark = parks.Exists(p => p.Name == "Blue Stone");

				//Assert
				Assert.IsTrue(containsAPark);

			}


		}

		public static int InsertFakeParK(string name, string location, DateTime establishDate, int area, int visitors, string description)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
			int parkId = 0;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES (@name, @location, " +
						"@establishDate, @area, @visitors, @description);", conn);
					cmd.Parameters.AddWithValue("@name", name);
					cmd.Parameters.AddWithValue("@location", location);
					cmd.Parameters.AddWithValue("@establishDate", establishDate);
					cmd.Parameters.AddWithValue("@area", area);
					cmd.Parameters.AddWithValue("@visitors", visitors);
					cmd.Parameters.AddWithValue("@description", description);
					cmd.ExecuteNonQuery();

					cmd = new SqlCommand("SELECT MAX(park_id) FROM park;", conn);
					parkId = Convert.ToInt32(cmd.ExecuteScalar());

				}

			}
			catch(SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}

			return parkId;

		}
	}
}
