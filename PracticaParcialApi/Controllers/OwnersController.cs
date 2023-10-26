using Microsoft.AspNetCore.Mvc;
using PracticaParcialApi.DAL.Entities;
using PracticaParcialApi.Domain.Interfaces;
using PracticaParcialApi.Domain.Services;
using System.Diagnostics.Metrics;

namespace PracticaParcialApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")] //Esta es la primera parte de la URL de esta API: URL = api/countries
    public class OwnersController : Controller
    {
        private readonly IOwnerService _ownerService;

        public OwnersController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        [HttpGet, ActionName("Get")]
        [Route("Get")] //Aquí concateno la URL inicial: URL = api/countries/get
        public async Task<ActionResult<IEnumerable<Owner>>> GetOwnersAsync()
        {
            var owners = await _ownerService.GetOwnersAsync(); //Aquí estoy yendo a mi capa de Domain para traerme la lista de países

            //El método Any() significa si hay al menos un elemento.
            //El Método !Any() significa si no hay absoluta/ nada.
            if (owners == null || !owners.Any())
            {
                return NotFound(); //NotFound = 404 Http Status Code
            }
            return Ok(owners); //Ok = 200 Http Status Code
        }


        [HttpPost, ActionName("Create")]
        [Route("Create")]
        public async Task<ActionResult> CreateOwnerAsync(Owner owner)
        {
            try
            {
                var createdOwner = await _ownerService.CreateOwnerAsync(owner);

                if (createdOwner == null)
                {
                    return NotFound(); //NotFound = 404 Http Status Code
                }

                return Ok(createdOwner); //Retorne un 200 y el objeto Country
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    return Conflict(String.Format("El propietario {0} ya existe.", owner.Name)); //Confilct = 409 Http Status Code Error
                }
                return Conflict(ex.Message);
            }
        }


        [HttpGet, ActionName("Get")]
        [Route("GetById/{id}")] //URL: api/countries/get
        public async Task<ActionResult<Owner>> GetOwnerByIdAsync(Guid id)
        {
            if (id == null) return BadRequest("Id es requerido!");

            var owner = await _ownerService.GetOwnerByIdAsync(id);

            if (owner == null) return NotFound(); // 404

            return Ok(owner); // 200
        }

        [HttpGet, ActionName("Get")]
        [Route("GetByName/{name}/{lastName}")] //URL: api/countries/get
        public async Task<ActionResult<Owner>> GetOwnerByNameAsync(string name, string lastName)
        {
            if (name == null) return BadRequest("Nombre del propietario requerido!");

            if (lastName == null) return BadRequest("Apellido del propietario requerido!");

            var owner = await _ownerService.GetOwnerByNameAsync(name, lastName);

            if (owner == null) return NotFound(); // 404

            return Ok(owner); // 200
        }


        [HttpPut, ActionName("Edit")]
        [Route("Edit")]
        public async Task<ActionResult<Owner>> EditOwnerAsync(Owner owner)
        {
            try
            {
                var editedOwner = await _ownerService.EditOwnerAsync(owner);
                return Ok(editedOwner);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", owner.Name));

                return Conflict(ex.Message);
            }
        }

        [HttpDelete, ActionName("Delete")]
        [Route("Delete")]
        public async Task<ActionResult<Owner>> DeleteOwnerAsync(Guid id)
        {
            if (id == null) return BadRequest("Id es requerido!");

            var deletedOwner = await _ownerService.DeleteOwnerAsync(id);

            if (deletedOwner == null) return NotFound("País no encontrado!");

            return Ok(deletedOwner);
        }


    }
}
