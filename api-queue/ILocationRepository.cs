using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

public interface ILocationRepository
{
    Task<List<Location>> GetAllLocations();

    Task<Location> GetLocation(string id);

    Task InsertNewLocation(Location location);

    Task EditLocation(Location location);

    Task DeleteLocation(string id);
}