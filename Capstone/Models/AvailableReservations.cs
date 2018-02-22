using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
	public class AvailableReservations
	{
		public int SiteNumber { get; set; }
		public int MaxOccupancy { get; set; }
		public bool Accessible { get; set; }
		public int MaxRvLenth { get; set; }
		public bool Utilities { get; set; }
		public decimal Cost { get; set; }


	}
}
