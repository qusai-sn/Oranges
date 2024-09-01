using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oranges.Models;

namespace Oranges.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public MealsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("Resturants")]
        public IActionResult GetAllResturants()
        {
            var allRestaurants = _db.Restaurants.ToList();
            var baseUrl = $"{Request.Scheme}://{Request.Host}/Images/";

            var restaurantsWithImageUrls = allRestaurants.Select(r => new
            {
                r.Id,
                r.Name,
                r.Description,
                ImageUrl = string.IsNullOrEmpty(r.Image) ? null : baseUrl + r.Image
            }).ToList();

            return Ok(restaurantsWithImageUrls);
        }

        [HttpGet]
        public IActionResult GetAllMeals()
        {
            var allMeals = _db.Meals.ToList();
            if (allMeals == null) { return NotFound(); }
            return Ok(allMeals);
        }

        [HttpGet("{id}")]
        public IActionResult GetMealsByResturantID(int id)
        {
            var resturantMeals = _db.Meals.Where(x => x.RestaurantId == id).ToList();
            var baseUrl = $"{Request.Scheme}://{Request.Host}/Images/";
            var restaurantsWithImageUrls = resturantMeals.Select(r => new
            {
                r.Id,
                r.Name,
                r.Price,
                r.RestaurantId,
                r.Description,
                ImageUrl = string.IsNullOrEmpty(r.Image) ? null : baseUrl + r.Image
            }).ToList();

            return Ok(restaurantsWithImageUrls);
        }


        [HttpPost]
        public async Task<IActionResult> AddMeal([FromForm] mealRequestDTO mealDTO)
        {
            if (mealDTO.Image == null || mealDTO.Image.Length == 0)
            {
                return BadRequest("Image file is missing.");
            }

            var imageUrl = await UploadImageAsync(mealDTO.Image);

            var newMeal = new Meals
            {
                RestaurantId = mealDTO.RestaurantId,
                Name = mealDTO.Name,
                Price = Convert.ToDecimal(mealDTO.Price),
                Description = mealDTO.Description,
                Image = imageUrl // Store the image URL in the database
            };

            _db.Meals.Add(newMeal);
            await _db.SaveChangesAsync();
            return Ok(newMeal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMeal(int id, [FromForm] mealRequestDTO mealDTO)
        {
            var meal = _db.Meals.Find(id);
            if (meal == null) return NotFound();

            if (mealDTO.Image != null && mealDTO.Image.Length > 0)
            {
                var imageUrl = await UploadImageAsync(mealDTO.Image);
                meal.Image = imageUrl;
            }

            meal.RestaurantId = mealDTO.RestaurantId ;
            meal.Name = mealDTO.Name ?? meal.Name;
            meal.Description = mealDTO.Description ?? meal.Description;
            meal.Price = mealDTO.Price ?? meal.Price;

            _db.Meals.Update(meal);
            await _db.SaveChangesAsync();
            return Ok(meal);
        }

        [Route("Meals/{id}")]
        [HttpDelete]
        public IActionResult DeleteMeal(int id)
        {
            if (id <= 0) { return BadRequest(); }
            var meal = _db.Meals.FirstOrDefault(x => x.Id == id);
            if (meal == null) { return NotFound(); }

            _db.Meals.Remove(meal);
            _db.SaveChanges();
            return NoContent();
        }

        // Helper method to upload images
        private async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var imageFilePath = Path.Combine(uploadFolder, imageFile.FileName);

            using (var stream = new FileStream(imageFilePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Return the full URL to the image
            var imageUrl = $"{Request.Scheme}://{Request.Host}/Images/{imageFile.FileName}";
            return imageUrl;
        }
    }
    }
