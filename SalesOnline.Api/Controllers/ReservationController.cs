using Microsoft.AspNetCore.Mvc;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using System.Threading.Tasks;

namespace SalesOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ITourRepository _tourRepository;
        private readonly ICustomerRepository _customerRepository;

        public ReservationController(
            IReservationRepository reservationRepository,
            ITourRepository tourRepository,
            ICustomerRepository customerRepository)
        {
            _reservationRepository = reservationRepository;
            _tourRepository = tourRepository;
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationRequest request)
        {
            // Validar que el tour existe
            var tour = await _tourRepository.GetByIdAsync(request.TourId);
            if (tour == null)
                return BadRequest("Tour no encontrado");

            // Validar que el cliente existe
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                return BadRequest("Cliente no encontrado");

            // Validar disponibilidad
            var existingReservations = await _reservationRepository.GetByTourIdAsync(request.TourId);
            int totalReserved = existingReservations.Sum(r => r.NumberOfPersons);
            if (totalReserved + request.NumberOfPersons > tour.MaxGroupSize)
                return BadRequest("No hay suficiente capacidad disponible en el tour");

            var reservation = new Reservation
            {
                CustomerId = request.CustomerId,
                TourId = request.TourId,
                ReservationDate = request.ReservationDate,
                NumberOfPersons = request.NumberOfPersons,
                SpecialRequirements = request.SpecialRequirements,
                Status = ReservationStatus.Pending,
                TotalAmount = tour.Price * request.NumberOfPersons,
                CreatedDate = DateTime.UtcNow
            };

            await _reservationRepository.AddAsync(reservation);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReservationRequest request)
        {
            var existingReservation = await _reservationRepository.GetByIdAsync(id);
            if (existingReservation == null)
                return NotFound();

            var tour = await _tourRepository.GetByIdAsync(request.TourId);
            if (tour == null)
                return BadRequest("Tour no encontrado");

            // Recalcular disponibilidad excluyendo la reserva actual
            var existingReservations = await _reservationRepository.GetByTourIdAsync(request.TourId);
            int totalReserved = existingReservations
                .Where(r => r.Id != id)
                .Sum(r => r.NumberOfPersons);

            if (totalReserved + request.NumberOfPersons > tour.MaxGroupSize)
                return BadRequest("No hay suficiente capacidad disponible en el tour");

            existingReservation.NumberOfPersons = request.NumberOfPersons;
            existingReservation.ReservationDate = request.ReservationDate;
            existingReservation.SpecialRequirements = request.SpecialRequirements;
            existingReservation.TotalAmount = tour.Price * request.NumberOfPersons;
            existingReservation.ModifiedDate = DateTime.UtcNow;

            await _reservationRepository.UpdateAsync(existingReservation);
            return Ok(existingReservation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
                return NotFound();

            await _reservationRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ReservationStatus newStatus)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
                return NotFound();

            reservation.Status = newStatus;
            reservation.ModifiedDate = DateTime.UtcNow;

            await _reservationRepository.UpdateAsync(reservation);
            return Ok(reservation);
        }
    }

    public class ReservationRequest
    {
        public int CustomerId { get; set; }
        public int TourId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfPersons { get; set; }
        public string SpecialRequirements { get; set; }
    }
}
