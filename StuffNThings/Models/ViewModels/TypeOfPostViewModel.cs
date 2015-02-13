using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StuffNThings.Models
{
	public class TypeOfPostViewModel
	{
		// Display Attribute will appear in the Html.LabelFor
		[Display(Name = "Type of Posts")]
		public int SelectedTypeOfPostId { get; set; }
		public IEnumerable<SelectListItem> TypeOfPosts { get; set; }
	}
}