using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using StuffNThings.Repository.Interfaces;
using StuffNThings.Repository.Models;
using System.Data.SqlClient;
using System.Data;

namespace StuffNThings.Repository.Repositories
{
	public class PostRepository : IPostRepository
	{
		private string _connectionString = string.Empty;

		#region Constructors

		public PostRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		#endregion

		public IEnumerable<Post> GetAllPosts()
		{
			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT * FROM Post";

				var command = new SqlCommand(commandText, conn);

				conn.Open();

				return GetPostsFromCommand(command);
			}
		}

		public Post GetPostById(int id)
		{
			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT * FROM Post WHERE Id = @id";
				var command = new SqlCommand(commandText, conn);

				var parameters = new[] { new SqlParameter { ParameterName = "id", DbType = DbType.Int32, Value = id } };
				command.Parameters.AddRange(parameters);

				conn.Open();

				Post post = GetPostsFromCommand(command).Select(p => p).First();

				var locationRepository = new LocationRepository(_connectionString);
				post.Regions = locationRepository.GetRegionsByPostId(post.Id).ToList();

				return post;
			}
		}

		public int AddPost(Post post)
		{ //TODO: Change this to a persist to insert/update a post
			using (var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"INSERT INTO [Post] ([Title], [Description], [Price], [Location], [TypeOfPostId])
									 VALUES (@Title, @Description, @Price, @Location, @TypeOfPostId)
									 SELECT @InsertedPostId = SCOPE_IDENTITY()";

				var command = new SqlCommand(commandText, conn);

				var parameters = new [] {
								new SqlParameter {ParameterName = "Title", SqlDbType = SqlDbType.NVarChar, Value = post.Title},
								new SqlParameter {ParameterName = "Description", SqlDbType = SqlDbType.NVarChar, Value = post.Description},
								new SqlParameter {ParameterName = "Price", SqlDbType = SqlDbType.Decimal, Value = post.Price},
								new SqlParameter {ParameterName = "Location", SqlDbType = SqlDbType.NVarChar, Value = post.Location},
								new SqlParameter {ParameterName = "TypeOfPostId", SqlDbType = SqlDbType.Int, Value = post.TypeOfPostId},
								new SqlParameter {ParameterName = "InsertedPostId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output}
				};
				command.Parameters.AddRange(parameters);

				conn.Open();
				command.ExecuteNonQuery();
				var postId = Convert.ToInt32(command.Parameters["InsertedPostId"].Value);

				var locationRepository = new LocationRepository(_connectionString);
				locationRepository.PersistPostRegions(postId, post.Regions);

				return postId;
			}
		}

		#region Internal Methods

		private List<Post> GetPostsFromCommand(SqlCommand command)
		{
			var posts = new List<Post>();
			using(var dr = command.ExecuteReader())
			{
				while(dr.Read())
				{
					posts.Add(AssemblePost(dr));
				}
			}

			return posts;
		}

		private Post AssemblePost(SqlDataReader dr)
		{
			var post = new Post(){
				Id = (int)dr["Id"],
				Title = (string)dr["Title"],
				Description = (string)dr["Description"],
				Price = (decimal)dr["Price"],
				Location = (string)dr["Location"],
				NumberOfBumps = (int)dr["NumberOfBumps"],
				LastBumpDate = dr["LastBumpDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["LastBumpDate"],
				CreatedDate = (DateTime)dr["CreatedDate"],
				ModifiedDate = (DateTime)dr["ModifiedDate"]
				//TypeOfPostId = (int)dr [ "TypeOfPostId" ]
			};

			return post;
		}

		#endregion
	}
}