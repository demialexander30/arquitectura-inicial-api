using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SalesOnline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IReviewRepository reviewRepository, ILogger<ReviewsController> logger)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las reseñas");
                var reviews = await _reviewRepository.GetAllAsync();
                _logger.LogInformation($"Se encontraron {reviews.Count()} reseñas");
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las reseñas");
                return StatusCode(500, "Error interno del servidor al obtener las reseñas");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            try
            {
                _logger.LogInformation($"Obteniendo reseña con ID: {id}");
                var review = await _reviewRepository.GetByIdAsync(id);
                if (review == null)
                {
                    _logger.LogWarning($"Reseña con ID {id} no encontrada");
                    return NotFound();
                }
                return Ok(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la reseña con ID {id}");
                return StatusCode(500, "Error interno del servidor al obtener la reseña");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            try
            {
                _logger.LogInformation("Creando nueva reseña");
                await _reviewRepository.AddAsync(review);
                _logger.LogInformation($"Reseña creada exitosamente con ID: {review.Id}");
                return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la reseña");
                return StatusCode(500, "Error interno del servidor al crear la reseña");
            }
        }

        [HttpGet("by-tour/{tourId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByTour(int tourId)
        {
            try
            {
                _logger.LogInformation($"Obteniendo reseñas para el tour con ID: {tourId}");
                var reviews = await _reviewRepository.GetByTourIdAsync(tourId);
                _logger.LogInformation($"Se encontraron {reviews.Count()} reseñas para el tour {tourId}");
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener las reseñas para el tour {tourId}");
                return StatusCode(500, "Error interno del servidor al obtener las reseñas del tour");
            }
        }

        [HttpGet("by-customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByCustomer(int customerId)
        {
            try
            {
                _logger.LogInformation($"Obteniendo reseñas del cliente con ID: {customerId}");
                var reviews = await _reviewRepository.GetByCustomerIdAsync(customerId);
                _logger.LogInformation($"Se encontraron {reviews.Count()} reseñas del cliente {customerId}");
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener las reseñas del cliente {customerId}");
                return StatusCode(500, "Error interno del servidor al obtener las reseñas del cliente");
            }
        }
    }
}
