using StuffNThings.Repository.Models;
using System.Collections.Generic;

namespace StuffNThings.Repository.Interfaces
{
	interface IPostRepository
	{
		IEnumerable<Post> GetAllPosts();
		Post GetPostById(int id);
		int AddPost(Post post);
		//void UpdatePost(Post post);
		//void RemovePost(Post post);
	}
}
