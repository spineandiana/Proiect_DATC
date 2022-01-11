using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebAPILocatii.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private ILocationRepository _slocationsRepository;

        public LocationsController(ILocationRepository locationsRepository)
        {
            _slocationsRepository = locationsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Location>> Get()
        {
            return await _slocationsRepository.GetAllLocations();
        }

        [HttpGet("{id}")]
        public async Task<Location> GetLocation([FromRoute] string id)
        {
            return await _slocationsRepository.GetLocation(id);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] Location location)
        {
            try
            {
                await _slocationsRepository.InsertNewLocation(location);

                return "S-a adaugat cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }
        }

        [HttpDelete("{id}")]
        public async Task<string> Delete([FromRoute] string id)
        {
            try
            {
                await _slocationsRepository.DeleteLocation(id);

                return "S-a sters cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }

        }

        [HttpPut]
        public async Task<string> Edit([FromBody] Location location) {
            try
            {
                await _slocationsRepository.EditLocation(location);

                return "S-a modificat cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }
        }
    }
}
