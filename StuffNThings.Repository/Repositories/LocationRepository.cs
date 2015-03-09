using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StuffNThings.Repository.Models;
using StuffNThings.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace StuffNThings.Repository.Repositories
{
	public class LocationRepository : ILocationRepository
	{
		private string _connectionString = string.Empty;

		#region Constructors

		public LocationRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		#endregion

		public IEnumerable<Region> GetAllRegionsByState(int stateId)
		{
			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT * FROM Region WHERE StateId = @stateId";
				var command = new SqlCommand(commandText, conn);

				var parameters = new[] {new SqlParameter {ParameterName = "stateId", DbType = DbType.Int32, Value = stateId}};
				command.Parameters.AddRange(parameters);

				conn.Open();

				return GetRegionsFromCommand(command);
			}
		}

		public Region GetRegionById(int id)
		{
			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT * FROM Region WHERE Id = @id";
				var command = new SqlCommand(commandText, conn);

				var parameters = new[] { new SqlParameter { ParameterName = "id", DbType = DbType.Int32, Value = id } };
				command.Parameters.AddRange(parameters);

				conn.Open();

				return GetRegionsFromCommand(command).Select(p => p).First();
			}
		}

		public IEnumerable<Region> GetRegionsByPostId(int postId)
		{
			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT r.* FROM PostRegion pr JOIN Region r ON pr.RegionId = r.Id WHERE PostId = @postId";
				var command = new SqlCommand(commandText, conn);

				var parameters = new[] {new SqlParameter {ParameterName = "postId", DbType = DbType.Int32, Value = postId}};
				command.Parameters.AddRange(parameters);

				conn.Open();

				return GetRegionsFromCommand(command);
			}
		}

		public IEnumerable<Region> GetRegionsByUserId(int userId)
		{
			using (var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT r.* FROM UserRegion ur JOIN Region r ON ur.RegionId = r.Id WHERE UserId = @userId";
				var command = new SqlCommand(commandText, conn);

				var parameters = new[] {new SqlParameter {ParameterName = "userId", DbType = DbType.Int32, Value = userId}};
				command.Parameters.AddRange(parameters);

				conn.Open();

				return GetRegionsFromCommand(command);
			}
		}

		public void PersistPostRegions(int postId, IEnumerable<Region> regions)
		{
			var regionIdsString = string.Join("|", regions.Select(r=> r.Id));

			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = "procPersistPostRegionsByPostId";
				var command = new SqlCommand(commandText, conn);
				command.CommandType = CommandType.StoredProcedure;

				var parameters = new[] {new SqlParameter{ParameterName = "postId", DbType = DbType.Int32, Value = postId}, 
										new SqlParameter{ParameterName = "regionIdsString", DbType = DbType.String, Value = regionIdsString}};
				command.Parameters.AddRange(parameters);

				conn.Open();
				command.ExecuteNonQuery();
			}
		}

		public void PersistUserRegions(int userId, int singleRegionId, int stateId)
		{
			var regionIds = new List<int>();
			regionIds.Add(singleRegionId);

			PersistUserRegions(userId, regionIds, stateId);
		}

		public void PersistUserRegions(int userId, IEnumerable<int> regionIds, int stateId)
		{
			var regionIdsString = regionIds == null ? string.Empty : string.Join("|", regionIds.Select(r => r.ToString()));

			using (var conn = new SqlConnection(_connectionString))
			{
				var commandText = "procPersistUserRegionsByUserId";
				var command = new SqlCommand(commandText, conn);
				command.CommandType = CommandType.StoredProcedure;

				var parameters = new [] {new SqlParameter{ParameterName = "userId", DbType = DbType.Int32, Value = userId},
										 new SqlParameter{ParameterName = "stateId", DbType = DbType.Int32, Value = stateId},
										 new SqlParameter{ParameterName = "regionIdsString", DbType = DbType.String, Value = regionIdsString}};
				command.Parameters.AddRange(parameters);

				conn.Open();
				command.ExecuteNonQuery();
			}
		}

		public IEnumerable<State> GetStatesByCountryId(int countryId)
		{
			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT * FROM [State] WHERE CountryId = @countryId ORDER BY [Name]";
				var command = new SqlCommand(commandText, conn);

				var parameters = new[] { new SqlParameter { ParameterName = "countryId", DbType = DbType.Int32, Value = countryId } };
				command.Parameters.AddRange(parameters);

				conn.Open();
				
				return GetStatesFromCommand(command);
			}
		}

		public IEnumerable<Country> GetAllCountries()
		{
			using(var conn = new SqlConnection(_connectionString))
			{
				var commandText = @"SELECT * FROM [Country]";
				var command = new SqlCommand(commandText, conn);

				conn.Open();

				return GetCountriesFromCommand(command);
			}
		}

		#region Internal Methods

		private List<Region> GetRegionsFromCommand(SqlCommand command)
		{
			var regions = new List<Region>();
			using(var dr = command.ExecuteReader())
			{
				while(dr.Read())
					regions.Add(AssembleRegion(dr));
			}

			return regions;
		}

		private List<State> GetStatesFromCommand(SqlCommand command)
		{
			var states = new List<State>();
			using(var dr = command.ExecuteReader())
			{
				while(dr.Read())
					states.Add(AssembleState(dr));
			}

			return states;
		}

		private List<Country> GetCountriesFromCommand(SqlCommand command)
		{
			var countries = new List<Country>();
			using (var dr = command.ExecuteReader())
			{
				while(dr.Read())
					countries.Add(AssembleCountry(dr));
			}

			return countries;
		}

		#region Assemblers

		private Region AssembleRegion(SqlDataReader dr)
		{
			var region = new Region(){
				Id = (int)dr["Id"],
				Name = (string)dr["Name"],
				StateId = (int)dr["StateId"],
				Description = dr["Description"] == DBNull.Value ? string.Empty : (string)dr["Description"],
				BannerImageUrl = dr["BannerImageUrl"] == DBNull.Value ? string.Empty : (string)dr["BannerImageUrl"],
				IsArchived = (bool)dr["IsArchived"],
				CreatedDate = dr["CreatedDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["CreatedDate"],
				ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["ModifiedDate"]
			};
			
			return region;
		}

		private State AssembleState(SqlDataReader dr)
		{
			var state = new State(){
				Id = (int)dr["Id"],
				Name = (string)dr["Name"],
				ISOAbbreviation = (string)dr["ISOAbbreviation"],
				CountryId = (int)dr["CountryId"]
			};
			
			return state;
		}

		private Country AssembleCountry(SqlDataReader dr)
		{
			var country = new Country(){
				Id = (int)dr["Id"],
				Name = (string)dr["Name"],
				ISOAbbreviation = (string)dr["ISOAbbreviation"]
			};

			return country;
		}
		#endregion
		#endregion
	}
}
