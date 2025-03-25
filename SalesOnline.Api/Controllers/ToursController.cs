using Microsoft.AspNetCore.Mvc;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;

namespace SalesOnline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToursController : ControllerBase
    {
        private readonly ITourRepository _tourRepository;

        public ToursController(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tour>>> GetTours()
        {
            var tours = await _tourRepository.GetAllAsync();
            return Ok(tours);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tour>> GetTour(int id)
        {
            var tour = await _tourRepository.GetByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            return Ok(tour);
        }

        [HttpPost]
        public async Task<ActionResult<Tour>> CreateTour(Tour tour)
        {
            await _tourRepository.AddAsync(tour);
            return CreatedAtAction(nameof(GetTour), new { id = tour.Id }, tour);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTour(int id, Tour tour)
        {
            if (id != tour.Id)
            {
                return BadRequest();
            }

            var result = await _tourRepository.UpdateAsync(tour);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var result = await _tourRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Tour>>> GetAvailableTours()
        {
            var tours = await _tourRepository.GetAvailableToursAsync();
            return Ok(tours);
        }

        [HttpGet("by-destination/{destination}")]
        public async Task<ActionResult<IEnumerable<Tour>>> GetToursByDestination(string destination)
        {
            var tours = await _tourRepository.GetToursByDestinationAsync(destination);
            return Ok(tours);
        }

        [HttpGet("by-price-range")]
        public async Task<ActionResult<IEnumerable<Tour>>> GetToursByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var tours = await _tourRepository.GetToursByPriceRangeAsync(minPrice, maxPrice);
            return Ok(tours);
        }
    }
}
