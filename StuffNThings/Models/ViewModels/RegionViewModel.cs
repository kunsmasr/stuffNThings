using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StuffNThings.Models
{
	public class RegionViewModel
	{
		// Display Attribute will appear in the Html.LabelFor
		[Display(Name = "Region")]
		public IEnumerable<int> SelectedRegionIds { get; set; }
		public IEnumerable<SelectListItem> Regions { get; set; }

		public RegionViewModel()
		{ }

		public RegionViewModel(List<SelectListItem> _regions)
		{
			Regions = _regions;
			SelectedRegionIds = new List<int>();
		}
	}
}
