using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Controllers
{
    [ApiController]
    [Route("/tour")]
    public class TourController : ControllerBase
    {
        private readonly ToursDbContext _context;

        public TourController(ToursDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(string? country, string? type)
        {
            IQueryable<Tour> result = _context.Tours;
            if (country != null)
            {
                result = result.Where(x => x.Country.Contains(country));
            }
            if (type != null)
            {
                result = result.Where(x => x.Type.Contains(type));
            }
            var list = result.OrderByDescending(x => x.CreatedAt).ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetTour(string id)
        {
            IQueryable<Tour> result = _context.Tours;
            if (id != null)
            {
                result = result.Where(x => x.Name.Contains(id));
            }
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        public IActionResult AddTour(Tour tour)
        {
            var now = DateTime.Now;
            var myTour = new Tour()
            {
                Name = tour.Name.Trim(),
                Description = tour.Description.Trim(),
                Country = tour.Country.Trim(),
                Type = tour.Type.Trim(),
                Price = tour.Price,
                CreatedAt = now,
                UpdatedAt = now
            };
            _context.Tours.Add(myTour);
            _context.SaveChanges();
            return Ok(myTour);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateTour(int id, Tour tour)
        {
            var myTour = _context.Tours.Find(id);
            if (myTour == null)
            {
                return NotFound();
            }
            myTour.Name = tour.Name.Trim();
            myTour.Description = tour.Description.Trim();
            myTour.Country = tour.Country.Trim();
            myTour.Type = tour.Type.Trim();
            myTour.Price = tour.Price;
            myTour.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteTour(int id)
        {
            var myTour = _context.Tours.Find(id);
            if (myTour == null)
            {
                return NotFound();
            }
            _context.Tours.Remove(myTour);
            _context.SaveChanges();
            return Ok();
        }
    }
}
