using StuffNThings.Repository.Models;
using System.Collections.Generic;

namespace StuffNThings.Repository.Interfaces
{
	interface ILocationRepository
	{
		IEnumerable<Region> GetAllRegionsByState(int stateId);
		Region GetRegionById(int id);
		IEnumerable<Region> GetRegionsByPostId(int postId);
		void PersistPostRegions(int postId, IEnumerable<Region> regions);
		void PersistUserRegions(int userId, IEnumerable<Region> regions);
		IEnumerable<State> GetStatesByCountryId(int countryId);
		IEnumerable<Country> GetAllCountries();
		//int AddRegion(Region region);
		//void ArchiveRegion(int regionId);

	}
}
