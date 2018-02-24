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
	public class CampgroundTests
	{
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        [TestMethod]
		public void ReturnsCampgroundsCorrectly()
		{
            // Arrange
            CampgroundDAL testClass = new CampgroundDAL();

            // Act
            int expectedParkCount1 = GetCampgroundCount(1);
            List<Campground> campgrounds1 = testClass.GetAllCampgrounds(1);
            int expectedParkCount2 = GetCampgroundCount(2);
            List<Campground> campgrounds2 = testClass.GetAllCampgrounds(2);
            int expectedParkCount3 = GetCampgroundCount(3);
            List<Campground> campgrounds3 = testClass.GetAllCampgrounds(3);

            // Assert
            Assert.AreEqual(expectedParkCount1, campgrounds1.Count);
            Assert.AreEqual(expectedParkCount2, campgrounds2.Count);
            Assert.AreEqual(expectedParkCount3, expectedParkCount3);
		}


        public static int GetCampgroundCount(int parkId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

            using (SqlConnection conn= new SqlConnection(connectionString))
            {
                int count = 0;
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS 'campgroundCount' FROM campground WHERE park_id=@parkId;", conn);
                cmd.Parameters.AddWithValue("@parkId", parkId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    count = Convert.ToInt32(reader["campgroundCount"]);
                }

                return count;
            }
        }
	}
}
