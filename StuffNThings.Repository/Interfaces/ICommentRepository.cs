using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StuffNThings.Repository.Models;

namespace StuffNThings.Repository.Interfaces
{
	interface ICommentRepository
	{
		int AddComment(Comment comment);
		//void DeleteComment(Comment comment);
		IEnumerable<Comment> GetPostsComments(int postId);
	}
}
