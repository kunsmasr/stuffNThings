﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffNThings.Repository.Models
{
	public class State
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ISOAbbreviation { get; set; }
		public int CountryId { get; set; }
	}
}