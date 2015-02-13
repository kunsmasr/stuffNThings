using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffNThings.Repository.Models
{
	public class Region
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int StateId { get; set; }
		public string Description { get; set; }
		public string BannerImageUrl { get; set; }
		public bool IsArchived { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
	}
}
