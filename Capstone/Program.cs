using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;


namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {

            // Use this so that you don't need to copy your connection string all over your code!            
            // ConfigurationManager opens up the App.config file and looks for an entry called "CapstoneDatabase".
            //     <add name="CapstoneDatabase" connectionString=""/>
            // The actual connection string for the database is found connectionString attribute.            
            string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

            ParkSystemCLI cli = new ParkSystemCLI();
            Console.WriteLine("Welcome to the National Park System Reservation System!");


			Console.WriteLine(@" _   _       _   _                   _   ____            _        ");
			Console.WriteLine(@"| \ | | __ _| |_(_) ___  _ __   __ _| | |  _ \ __ _ _ __| | _____ ");
			Console.WriteLine(@"|  \| |/ _` | __| |/ _ \| '_ \ / _` | | | |_) / _` | '__| |/ / __|");
			Console.WriteLine(@"| |\  | (_| | |_| | (_) | | | | (_| | | |  __/ (_| | |  |   <\__ \");
			Console.WriteLine(@"|_| \_|\__,_|\__|_|\___/|_| |_|\__,_|_| |_|   \__,_|_|  |_|\_\___/");

			var mediaPlayer = new System.Media.SoundPlayer();
			mediaPlayer.SoundLocation = @"C:\Users\Shane O'Neill\c-module-2-capstone-team-0\Capstone\SmokeyTheBear.wav";
			mediaPlayer.Play();

			cli.RunCLI();
        }
    }
}
