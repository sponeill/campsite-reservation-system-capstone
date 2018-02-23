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
            string subMenuInput = "";

            List<Park> parks = PrintAllParks();
            Console.WriteLine("Please enter the number of the park you would like to visit.");
            userInput = Console.ReadLine().ToString();
            int campgroundInput;
            DateTime arrivalDate;
            DateTime departureDate;

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

            subMenuInput = PrintCampgroundMenu();
            bool goBack = false;
            while (goBack == false)
            {
                switch (subMenuInput)
                {
                    case "0":
                        subMenuInput = PrintCampgroundMenu();
                        break;
                    case "1":
                        List<Campground> campgrounds = PrintAllCampgrounds(parks[int.Parse(userInput) - 1].ParkId);
                        Console.WriteLine("Select a command");
                        Console.WriteLine("1) Search for Available Reservation");
                        Console.WriteLine("2) Return to Previous Screen");
                        string input = Console.ReadLine();
                        if(input == "1")
                        {
                            subMenuInput = "2";
                        }
                        else if(input == "2")
                        {
                            subMenuInput = "0";
                        }
                        break;
                    case "2":
                        bool reservationsAvailable = false;
                        while (reservationsAvailable == false)
                        {
                            campgrounds = PrintAllCampgrounds(parks[int.Parse(userInput) - 1].ParkId);
                            Console.WriteLine("Which campground (enter 0 to cancel)? ");
                            campgroundInput = int.Parse(Console.ReadLine());
                            if(campgroundInput == 0)
                            {
                                reservationsAvailable = true;
                                subMenuInput = "0";
                                break;
                            }
                            Console.WriteLine("What is the arrival date? ");
                            arrivalDate = Convert.ToDateTime(Console.ReadLine());
                            Console.WriteLine("What is the departure date? ");
                            departureDate = Convert.ToDateTime(Console.ReadLine());
                            reservationsAvailable = PrintReservations(parks[int.Parse(userInput) - 1].ParkId, campgroundInput, arrivalDate, departureDate);
                            if (reservationsAvailable)
                            {
                                MakeReservation(arrivalDate, departureDate);
                                RunCLI();
                            }
                        }
                        break;
                    case "3":
                        Console.WriteLine();
                        RunCLI();
                        break;
                }
            }

        }

        public bool PrintReservations(int parkId, int campgroundId, DateTime startDate, DateTime endDate)
        {
            ReservationDAL dal = new ReservationDAL();
            List<AvailableReservations> reservations = dal.GetAllReservations(parkId, campgroundId, startDate, endDate);
            if (reservations.Count == 0)
            {
                Console.WriteLine("No reservations are available. Please make another selection.\n");
                return false;
            }
            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine("Site No. \t Max Occup. \t Accessible? \t Max RV Length \t Utility \t Cost");
            foreach (AvailableReservations reservation in reservations)
            {
                Console.WriteLine($"{reservation.SiteNumber}".PadRight(10) + $"{reservation.MaxOccupancy}".PadRight(10) + $"{reservation.Accessible}".PadRight(5)
                    + $"{reservation.MaxRvLenth}".PadRight(5) + $"{reservation.Utilities}".PadRight(5) + $"{(reservation.Cost * (decimal)((endDate - startDate).TotalDays)).ToString("C")}");
            }
            return true;
        }

        public void MakeReservation(DateTime startDate, DateTime endDate)
        {
            int siteToReserve;
            string reservationName = "";
            Console.WriteLine("Which site should be reserved (enter 0 to cancel)?");
            siteToReserve = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("What name should the reservation be made under?");
            reservationName = Console.ReadLine();
            ReservationDAL dal = new ReservationDAL();
            int confirmationId = dal.Reserve(siteToReserve, reservationName, startDate, endDate);

            Console.WriteLine("The reservation has been made and the confirmation id is " + confirmationId);
            Console.WriteLine();
        }

        public List<Campground> PrintAllCampgrounds(int parkId)
        {
            CampgroundDAL dal = new CampgroundDAL();
            List<Campground> campgrounds = dal.GetAllCampgrounds(parkId);
            int counter = 1;
            Console.WriteLine("\t Name \t Open \t Close \t Daily Fee");
            foreach (Campground campground in campgrounds)
            {
                Console.WriteLine($"#{campground.CampgroundId}\t {campground.Name} \t {campground.OpeningMonth} \t {campground.ClosingMonth} \t {campground.DailyFee.ToString("C")}");
                counter++;
            }
            return campgrounds;
        }

        public string PrintCampgroundMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Select a Command");
            Console.WriteLine("1) View Campgrounds");
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
            Console.WriteLine("Q) Quit Program");
            return parks;

        }

    }
}
