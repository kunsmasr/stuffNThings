using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffNThings.Repository.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public string Body { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int PostId { get; set; }
		//public int UserId { get; set; } //CommenterId
	}
}
