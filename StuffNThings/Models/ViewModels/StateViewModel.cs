using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StuffNThings.Models
{
	public class StateViewModel
	{
		// Display Attribute will appear in the Html.LabelFor
		[Display(Name = "State")]
		public int SelectedStateId { get; set; }
		public IEnumerable<SelectListItem> States { get; set; }
	}
}