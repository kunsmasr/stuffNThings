using System;
using System.Collections.Generic;

namespace StuffNThings.Repository.Models
{
	public class Post
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string Location { get; set; } // TODO: Potentially make this a separate class.
		public int NumberOfBumps { get; set; }
		public DateTime? LastBumpDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public int TypeOfPostId { get; set; } //TODO: Change this to TypeOfPost class
		public List<Region> Regions { get; set; }
		public int UserId { get; set; }
		//public IEnumerable<Tag> Tags { get; set; } //values such as new in box (id, value, abbreviation, description)
		//public IEnumerable<Comment> Comments { get; set; }
		//public IEnumerable<Picture> Pictures { get; set; }
		//public IEnumerable<Category> Categories { get; set; }
	}
}