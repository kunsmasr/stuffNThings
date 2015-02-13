using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StuffNThings.Repository.Interfaces;
using StuffNThings.Repository.Models;
using System.Data.SqlClient;
using System.Data;

namespace StuffNThings.Repository.Repositories
{
	public class CommentRepository : ICommentRepository
	{
		private string _connectionString = string.Empty;

		public CommentRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public int AddComment(Comment comment)
		{
			var commentId = 0;
			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"INSERT INTO [Comment] ([Body], [PostId])
									VALUES (@Body, @PostId)
									SELECT @CommentId = SCOPE_IDENTITY()";

				using (var command = new SqlCommand(commandText, conn))
				{
					var parameters = new [] {
									   new SqlParameter {ParameterName = "Body", SqlDbType = SqlDbType.NVarChar, Value = comment.Body},
									   new SqlParameter {ParameterName = "PostId", SqlDbType = SqlDbType.Int, Value = comment.PostId},
									   new SqlParameter {ParameterName = "CommentId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output}
					};

					command.Parameters.AddRange(parameters);
					conn.Open();
					command.ExecuteNonQuery();
					commentId = Convert.ToInt32(command.Parameters["CommentId"].Value);
				}
			}

			return commentId;
		}

		public IEnumerable<Comment> GetPostsComments(int postId)
		{
			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT * FROM Comment WHERE PostId = @PostId";

				using (var command = new SqlCommand(commandText, conn))
				{
					var parameters = new [] {
									   new SqlParameter {ParameterName = "PostId", SqlDbType = SqlDbType.Int, Value = postId}
					};

					command.Parameters.AddRange(parameters);
					conn.Open();
					return GetCommentsFromCommand(command);
				}
			}
		}

		#region Internal Methods

		private List<Comment> GetCommentsFromCommand(SqlCommand command)
		{
			var comments = new List<Comment>();
			using(var dr = command.ExecuteReader())
			{
				while(dr.Read())
				{
					comments.Add(AssembleComment(dr));
				}
			}

			return comments;
		}

		private Comment AssembleComment(SqlDataReader dr)
		{
			var comment = new Comment()
			{
				Id = (int)dr["Id"],
				Body = (string)dr["Body"],
				CreatedDate = (DateTime)dr["CreatedDate"],
				PostId = (int)dr["PostId"]
			};

			return comment;
		}

		#endregion
	}
}
