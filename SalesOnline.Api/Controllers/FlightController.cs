using Microsoft.AspNetCore.Mvc;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;

namespace SalesOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IUnitOfWork<Flight> _unitOfWork;

        public FlightController(IUnitOfWork<Flight> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> Get()
        {
            var flights = await _unitOfWork.Repository.GetAllAsync();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> Get(int id)
        {
            var flight = await _unitOfWork.Repository.GetByIdAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> Post([FromBody] Flight flight)
        {
            await _unitOfWork.Repository.AddAsync(flight);
            return CreatedAtAction(nameof(Get), new { id = flight.Id }, flight);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Flight flight)
        {
            if (id != flight.Id)
            {
                return BadRequest();
            }

            var success = await _unitOfWork.Repository.UpdateAsync(flight);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _unitOfWork.Repository.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableFlights([FromQuery] string origin, [FromQuery] string destination, [FromQuery] DateTime? date)
        {
            // Note: This method is not updated to use UnitOfWork as per the provided instructions
            // You may need to update this method according to your requirements
            return Ok();
        }
    }
}
