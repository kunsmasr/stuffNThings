using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StuffNThings.Repository.Models;

namespace StuffNThings.Repository.Interfaces
{
	interface ITypeOfPostRepository
	{
		IEnumerable<TypeOfPost> GetAllTypeOfPosts();
		//TypeOfPost GetTypeOfPostById(int id);
	}
}
