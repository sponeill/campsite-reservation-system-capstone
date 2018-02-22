using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;

namespace Capstone
{
    public class ParkSystemCLI
    {
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        public void RunCLI()
        {
            Console.WriteLine("Welcome to the National Park System Reservation System!");
            PrintAllParks();
        }

        public void PrintAllParks()
        {
            ParkDAL dal = new ParkDAL();
            List<Park> parks = dal.GetAllParks();
            int counter = 1;
            foreach(Park park in parks)
            {
                Console.WriteLine(counter + ")" + " " + park.Name);
                counter++;
            }
        }

    }
}
