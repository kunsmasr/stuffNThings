using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StuffNThings.Repository.Interfaces;
using StuffNThings.Repository.Models;
using System.Data.SqlClient;

namespace StuffNThings.Repository.Repositories
{
	public class TypeOfPostRepository : ITypeOfPostRepository
	{
		private string _connectionString = string.Empty;

		public TypeOfPostRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IEnumerable<TypeOfPost> GetAllTypeOfPosts()
		{
			using (var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT * FROM TypeOfPost";
				var command = new SqlCommand(commandText, conn);

				conn.Open();

				return GetTypeOfPostsFromCommand(command);
			}
		}

		private List<TypeOfPost> GetTypeOfPostsFromCommand(SqlCommand command)
		{
			var typeOfPosts = new List<TypeOfPost>();
			using (var dr = command.ExecuteReader())
			{
				while (dr.Read())
				{
					typeOfPosts.Add(AssembleTypeOfPost(dr));
				}
			}

			return typeOfPosts;
		}

		private TypeOfPost AssembleTypeOfPost(SqlDataReader dr)
		{
			var typeOfPost = new TypeOfPost()
			{
				Id = (int)dr [ "Id" ],
				Name = (string)dr [ "Name" ],
				Description = (string)dr [ "Description" ]
			};

			return typeOfPost;
		}
	}
}
