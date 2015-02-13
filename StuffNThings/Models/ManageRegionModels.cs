using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using StuffNThings.Models;

namespace StuffNThings.Models
{
	public class ManageRegionModels
	{
		public int UserId { get; set; }
		public StateViewModel StateViewModel { get; set; }
		public RegionViewModel RegionViewModel { get; set; }
	}
}