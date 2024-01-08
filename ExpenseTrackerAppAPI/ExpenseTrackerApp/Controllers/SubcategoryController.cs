using AutoMapper;
using ExpenseTrackerApp.DTO;
using ExpenseTrackerApp.Interfaces;
using ExpenseTrackerApp.Models;
using ExpenseTrackerApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : Controller
    {
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public SubcategoryController(ISubcategoryRepository subcategoryRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Subcategory>))]
        public IActionResult GetSubcategories()
        {
            var subcategories = _mapper.Map<List<SubcategoryDTO>>(_subcategoryRepository.GetSubcategories());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(subcategories);
        }

        [HttpGet("{subcategoryId}")]
        [ProducesResponseType(200, Type = typeof(Subcategory))]
        [ProducesResponseType(400)]
        public IActionResult GetSubcategory(int subcategoryId)
        {
            if(_subcategoryRepository.SubcategoryExists(subcategoryId))
            {
                return NotFound();
            }

            var subcategory = _mapper.Map<SubcategoryDTO>(_subcategoryRepository.GetSubcategory(subcategoryId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(subcategory);
        }

        [HttpGet("subcategoriesByCategory/{categoryId}")]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Subcategory>))]
        [ProducesResponseType(400)]
        public IActionResult GetSubcategoriesByCategory(int categoryId)
        {
            var subcategories = _mapper.Map<List<Subcategory>>(_subcategoryRepository.GetSubcategoriesByCategory(categoryId));
            

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(subcategories);
        }

        [HttpPost("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSubcategory(int categoryId, [FromBody] SubcategoryDTO subcategoryCreate)
        {
            if(subcategoryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var subcategory = _subcategoryRepository.GetSubcategories()
                .Where(s => s.Name.Trim().ToUpper() == subcategoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(subcategory != null)
            {
                ModelState.AddModelError("", "Subcategory already exists!");
                return StatusCode(500, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subcategoryMap = _mapper.Map<Subcategory>(subcategoryCreate);

            subcategoryMap.Category = _categoryRepository.GetCategory(categoryId);

            if(!_subcategoryRepository.CreateSubcategory(subcategoryMap))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPut("{subcategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSubcategory(int subcategoryId, [FromBody] SubcategoryDTO updatedSubcategory)
        {
            if(updatedSubcategory == null)
            {
                BadRequest(ModelState);
            }

            if(subcategoryId != updatedSubcategory.Id)
            {
                BadRequest(ModelState);
            }

            if(!_subcategoryRepository.SubcategoryExists(subcategoryId))
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subcategoryMap = _mapper.Map<Subcategory>(updatedSubcategory);

            if(!_subcategoryRepository.UpdateSubcategory(subcategoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{subcategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSubcategory(int subcategoryId)
        {
            if (!_subcategoryRepository.SubcategoryExists(subcategoryId))
            {
                return NotFound();
            }

            var subcategoryToDelete = _subcategoryRepository.GetSubcategory(subcategoryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_subcategoryRepository.DeleteSubcategory(subcategoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting!");
            }

            return NoContent();
        }
    }
}
