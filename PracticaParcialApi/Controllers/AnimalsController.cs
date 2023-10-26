using Microsoft.AspNetCore.Mvc;
using PracticaParcialApi.DAL.Entities;
using PracticaParcialApi.Domain.Interfaces;
using PracticaParcialApi.Domain.Services;
using System.Diagnostics.Metrics;

namespace PracticaParcialApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")] //Esta es la primera parte de la URL de esta API: URL = api/countries
    public class AnimalsController : Controller
    {
        private readonly IAnimalService _animalService;

        public AnimalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpGet, ActionName("Get")]
        [Route("Get")] //Aquí concateno la URL inicial: URL = api/countries/get
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimalsByOwnerIdAsync(Guid ownerId)
        {
            var animal = await _animalService.GetAnimalsByOwnerIdAsync(ownerId); //Aquí estoy yendo a mi capa de Domain para traerme la lista de países

            //El método Any() significa si hay al menos un elemento.
            //El Método !Any() significa si no hay absoluta/ nada.
            if (animal == null || !animal.Any())
            {
                return NotFound(); //NotFound = 404 Http Status Code
            }
            return Ok(animal); //Ok = 200 Http Status Code
        }


        [HttpPost, ActionName("Create")]
        [Route("Create")]
        public async Task<ActionResult> CreateAnimalAsync(Animal animal, Guid ownerId)
        {
            try
            {
                var createdAnimal = await _animalService.CreateAnimalAsync(animal, ownerId);

                if (createdAnimal == null)
                {
                    return NotFound(); //NotFound = 404 Http Status Code
                }

                return Ok(createdAnimal); //Retorne un 200 y el objeto Country
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    return Conflict(String.Format("El animal {0} ya existe.", animal.Name)); //Confilct = 409 Http Status Code Error
                }
                return Conflict(ex.Message);
            }
        }

        [HttpGet, ActionName("Get")]
        [Route("GetById/{id}")] //URL: api/countries/get
        public async Task<ActionResult<Animal>> GetAnimalByIdAsync(Guid id)
        {
            if (id == null) return BadRequest("Id es requerido!");

            var animal = await _animalService.GetAnimalByIdAsync(id);

            if (animal == null) return NotFound(); // 404

            return Ok(animal); // 200
        }


        [HttpPut, ActionName("Edit")]
        [Route("Edit")]
        public async Task<ActionResult<Animal>> EditAnimalAsync(Animal animal, Guid id)
        {
            try
            {
                var editedAnimal = await _animalService.EditAnimalAsync(animal, id);
                return Ok(editedAnimal);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", animal.Name));

                return Conflict(ex.Message);
            }
        }

        [HttpDelete, ActionName("Delete")]
        [Route("Delete")]
        public async Task<ActionResult<Animal>> DeleteAnimalAsync(Guid id)
        {
            if (id == null) return BadRequest("Id es requerido!");

            var deletedAnimal = await _animalService.DeleteAnimalAsync(id);

            if (deletedAnimal == null) return NotFound("País no encontrado!");

            return Ok(deletedAnimal);
        }

    }
}
