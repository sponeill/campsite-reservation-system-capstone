using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class CLIHelper
    {
        public DateTime GetDateTime()
        {
            string userInput = "";
            DateTime dateValue = DateTime.MinValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid date format. Please try again");
                }

                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!DateTime.TryParse(userInput, out dateValue));

            return dateValue;
        }

        public int GetInteger()
        {
            string userInput = "";
            int intValue = 0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!int.TryParse(userInput, out intValue));

            return intValue;

        }


        public static double GetDouble(string message)
        {
            string userInput = String.Empty;
            double doubleValue = 0.0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!double.TryParse(userInput, out doubleValue));

            return doubleValue;

        }

        public bool GetBool()
        {
            string userInput = String.Empty;
            bool boolValue = false;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!bool.TryParse(userInput, out boolValue));

            return boolValue;
        }

        public string GetString()
        {
            string userInput = String.Empty;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }
                
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (String.IsNullOrEmpty(userInput));

            return userInput;
        }
    }
}
