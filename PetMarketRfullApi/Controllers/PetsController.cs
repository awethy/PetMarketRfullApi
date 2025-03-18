using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources.PetsResources;
using PetMarketRfullApi.Sevices;

namespace PetMarketRfullApi.Controllers
{
    [Route("api/pets")]
    [ApiController]
    public class PetsController : Controller
    {
        private readonly IPetService _petService;

        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        // GET: api/pets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetResource>>> GetPets()
        {
            var pets = await _petService.GetAllPetsAsync();
            return Ok(pets);
        }

        //GET: api/pets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PetResource>> GetPet(int id)
        {
            var petResource = await _petService.GetPetByIdAsync(id);
            if (petResource == null)
            {
                return NotFound();
            }
            return Ok(petResource);
        }

        //POST: api/pets/post
        [HttpPost]
        public async Task<ActionResult<PetResource>> CreatePet(CreatePetResource createPetResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var petResource = await _petService.CreatePetAsync(createPetResource);
            return CreatedAtAction(nameof(GetPet), new { id = petResource.Id }, petResource);
        }

        //DELETE: api/pets/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<PetResource>> DeletePet(int id)
        {
            try
            {
                await _petService.DeletePetAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // 404 not found
            }
        }

        //PUT: api/pets/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PetResource>> UpdatePet(int id, /*[FromBody]*/ UpdatePetResource updatePetResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _petService.UpdatePetAsync(id, updatePetResource);
                return Ok(updatePetResource);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
