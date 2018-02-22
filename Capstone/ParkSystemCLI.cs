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
		CLIHelper cliHelper = new CLIHelper();

        public void RunCLI()
		{
			string userInput = "";
			Console.WriteLine("Welcome to the National Park System Reservation System!");
			List<Park> parks = PrintAllParks();
			Console.WriteLine("Please enter the number of the park you would like to visit.");
			userInput = Console.ReadLine().ToString();

			while ((int.Parse(userInput) < 1 || int.Parse(userInput) > parks.Count))
			{
				Console.WriteLine("Invalid input. Please try again.");
				userInput = Console.ReadLine();
			}

			//while(userInput.ToLower() != "q")
			//{
			//	Console.WriteLine("Invalid input. Please try again.");
			//	userInput = Console.ReadLine();
			//}

			if (userInput == "q")
			{
				return;
			}

			PrintParkDetails(parks[int.Parse(userInput) - 1]);

			userInput = PrintCampgroundMenu();
			



		}

		public List<Campground> ViewCampgrounds()
		{


		}

		public string PrintCampgroundMenu()
		{
			Console.WriteLine();
			Console.WriteLine("Select a Command");
			Console.WriteLine("1) View Campground");
			Console.WriteLine("2) Search for Reservation");
			Console.WriteLine("3) Return to Previous Screen");
			string userInput = Console.ReadLine();
			return userInput;

		}

		public void PrintParkDetails(Park park)
		{
			Console.WriteLine(park.Name);
			Console.WriteLine($"Location:     {park.Location}");
			Console.WriteLine($"Established:     {park.EstablishedDate}");
			Console.WriteLine($"Area:     {park.Area}");
			Console.WriteLine($"Annual Visitors:     {park.AnnualVisitorCount}");
			Console.WriteLine();
			Console.WriteLine(park.Description);
		}

		public List<Park>  PrintAllParks()
        {
            ParkDAL dal = new ParkDAL();
            List<Park> parks = dal.GetAllParks();
            int counter = 1;
            foreach(Park park in parks)
            {
                Console.WriteLine(counter + ")" + " " + park.Name);
                counter++;
            }
			Console.WriteLine("Q) Quit Program");
			return parks;
		
        }

    }
}
