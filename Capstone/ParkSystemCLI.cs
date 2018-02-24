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
            int userInput;
            int subMenuInput;

			Console.WriteLine();
            List<Park> parks = PrintAllParks();
			Console.WriteLine();
            Console.WriteLine("Please enter the number of the park you would like to visit.");
            userInput = cliHelper.GetInteger();
            int campgroundInput;
            DateTime arrivalDate;
            DateTime departureDate;

            while (userInput < 0 || userInput > parks.Count)
            {
                Console.WriteLine("Invalid input. Please try again.");
                userInput = cliHelper.GetInteger();
            }

            if (userInput == 0)
            {
				Environment.Exit(0);
            }

            PrintParkDetails(parks[userInput - 1]);

            subMenuInput = PrintCampgroundMenu();
            bool goBack = false;
            while (goBack == false)
            {
                switch (subMenuInput)
                {
                    case 0:
                        subMenuInput = PrintCampgroundMenu();
                        break;
                    case 1:
                        List<Campground> campgrounds = PrintAllCampgrounds(parks[userInput - 1].ParkId);
                        Console.WriteLine("Select a command");
                        Console.WriteLine("1) Search for Available Reservation");
                        Console.WriteLine("2) Return to Previous Screen");
                        int input = cliHelper.GetInteger();
                        if(input == 1)
                        {
                            subMenuInput = 2;
                        }
                        else if(input == 2)
                        {
                            subMenuInput = 0;
                        }
                        break;
                    case 2:
                        bool reservationsAvailable = false;
                        while (reservationsAvailable == false)
                        {
                            campgrounds = PrintAllCampgrounds(parks[userInput - 1].ParkId);
                            Console.WriteLine("Which campground (enter 0 to cancel)? ");

                            campgroundInput = cliHelper.GetInteger();
                            while(campgroundInput < 0 || !campgrounds.Exists(c => c.CampgroundId == campgroundInput))
                            {
                                Console.WriteLine("Invalid option. Please try again.");
                                campgroundInput = cliHelper.GetInteger();
                            }
                            if(campgroundInput == 0)
                            {
                                reservationsAvailable = true;
                                subMenuInput = 0;
                                break;
                            }
							Console.WriteLine();
                            Console.WriteLine("What is the arrival date? ");
                            arrivalDate = cliHelper.GetDateTime();
							Console.WriteLine();
                            Console.WriteLine("What is the departure date? ");
                            departureDate = cliHelper.GetDateTime();
                            List<AvailableReservations> reservations = PrintReservations(parks[userInput - 1].ParkId, campgroundInput, arrivalDate, departureDate);
                            reservationsAvailable = reservations.Count > 0;
                            if (reservationsAvailable)
                            {
                                MakeReservation(arrivalDate, departureDate, reservations);
                                RunCLI();
                            }
                        }
                        break;
                    case 3:
                        Console.WriteLine();
                        RunCLI();
                        break;
                }
            }

        }

        public List<AvailableReservations> PrintReservations(int parkId, int campgroundId, DateTime startDate, DateTime endDate)
        {
            ReservationDAL dal = new ReservationDAL();
            List<AvailableReservations> reservations = dal.GetAllReservations(parkId, campgroundId, startDate, endDate);
            if (reservations.Count == 0)
            {
				Console.WriteLine();
                Console.WriteLine("No reservations are available. Please make a selection:");
				Console.WriteLine("1) Enter new dates");
				Console.WriteLine("2) Return to Main Menu");
				int submenuInput = cliHelper.GetInteger();
                while(submenuInput != 1 && submenuInput != 2)
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    submenuInput = cliHelper.GetInteger();
                }
				if (submenuInput == 1)
				{
					return reservations;
				}
				else if(submenuInput == 2)
				{
					RunCLI();
				}
            }
			Console.Clear();
            Console.WriteLine("Results Matching Your Search Criteria");
			Console.WriteLine();
            Console.WriteLine("Site No.".PadRight(10) + "Max Occup.".PadRight(12) + "Accessible?".PadRight(15) + "Max RV Length".PadRight(15) + "Utilities?".PadRight(15) + "Total Cost");
            for (int i = 0; i < 5 && i < reservations.Count; i++)
            {
                Console.WriteLine($"{reservations[i].SiteNumber}".PadRight(10) + $"{reservations[i].MaxOccupancy}".PadRight(12) + $"{reservations[i].Accessible}".PadRight(15)
                    + $"{reservations[i].MaxRvLenth}".PadRight(15) + $"{reservations[i].Utilities}".PadRight(15) + $"{(reservations[i].Cost * (decimal)((endDate - startDate).TotalDays)).ToString("C")}");
            }
            return reservations;
        }

        public void MakeReservation(DateTime startDate, DateTime endDate, List<AvailableReservations> reservations)
        {
            int siteToReserve;
            string reservationName = "";
			Console.WriteLine();
            Console.WriteLine("Which site would you like to reserve (enter 0 to cancel)?");
            siteToReserve = cliHelper.GetInteger();
            while(!reservations.Exists(s => s.SiteNumber == siteToReserve))
            {
                Console.WriteLine("Invalid option. Please try again.");
                siteToReserve = cliHelper.GetInteger();
            }
			Console.WriteLine();
            Console.WriteLine("What name should the reservation be made under?");
            reservationName = cliHelper.GetString();
            ReservationDAL dal = new ReservationDAL();
            int confirmationId = dal.Reserve(siteToReserve, reservationName, startDate, endDate);
			Console.WriteLine();
            Console.WriteLine("The reservation has been made and the confirmation id is " + confirmationId);
            Console.WriteLine();
        }

        public List<Campground> PrintAllCampgrounds(int parkId)
        {
			Console.Clear();
            CampgroundDAL dal = new CampgroundDAL();
            List<Campground> campgrounds = dal.GetAllCampgrounds(parkId);
            int counter = 1;
            Console.WriteLine("ID".PadRight(6) + "Name".PadRight(35) + "Open".PadRight(15) + "Close".PadRight(15) + "Daily Fee".PadRight(15));
            foreach (Campground campground in campgrounds)
            {
				Console.WriteLine("#" + campground.CampgroundId.ToString().PadRight(5) + campground.Name.PadRight(35) + campground.OpeningMonth.ToString().PadRight(15) + campground.ClosingMonth.ToString().PadRight(15) + campground.DailyFee.ToString("C").PadRight(15));
                counter++;
            }
			Console.WriteLine();
            return campgrounds;
        }

        public int PrintCampgroundMenu()
        {
			
            Console.WriteLine();
            Console.WriteLine("Select a Command");
            Console.WriteLine("1) View Campgrounds");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("3) Return to Previous Screen");
            int userInput = cliHelper.GetInteger();
            while(userInput <= 0 && userInput > 3) {
                Console.WriteLine("Invalid option. Please try again.");
                userInput = cliHelper.GetInteger();
            }
            return userInput;
        }

        public void PrintParkDetails(Park park)
        {
			Console.Clear();
            Console.WriteLine("Name:".PadRight(25) + park.Name);
            Console.WriteLine("Location:".PadRight(25) +  park.Location);
            Console.WriteLine("Established:".PadRight(25) + park.EstablishedDate);
            Console.WriteLine("Area:".PadRight(25) + park.Area);
            Console.WriteLine("Annual Visitors:".PadRight(25) + park.AnnualVisitorCount);
            Console.WriteLine();
            Console.WriteLine(park.Description);
        }

        public List<Park> PrintAllParks()
        {
            ParkDAL dal = new ParkDAL();
            List<Park> parks = dal.GetAllParks();
            int counter = 1;
            foreach (Park park in parks)
            {
                Console.WriteLine(counter + ")" + " " + park.Name);
                counter++;
            }
            Console.WriteLine("0) Quit Program");
            return parks;

        }

    }
}
