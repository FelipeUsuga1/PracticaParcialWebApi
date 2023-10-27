using Microsoft.AspNetCore.Mvc;
using PracticaParcialApi.DAL.Entities;
using PracticaParcialApi.Domain.Interfaces;
using PracticaParcialApi.Domain.Services;
using System.Diagnostics.Metrics;

namespace PracticaParcialApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")] //Esta es la primera parte de la URL de esta API: URL = api/countries
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet, ActionName("Get")]
        [Route("Get")] //Aquí concateno la URL inicial: URL = api/appointments/get
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByAnimalIdAsync(Guid animalId)
        {
            var appointment = await _appointmentService.GetAppointmentsByAnimalIdAsync(animalId); //Aquí estoy yendo a mi capa de Domain para traerme la lista de países

            //El método Any() significa si hay al menos un elemento.
            //El Método !Any() significa si no hay absoluta/ nada.
            if (appointment == null || !appointment.Any())
            {
                return NotFound(); //NotFound = 404 Http Status Code
            }
            return Ok(appointment); //Ok = 200 Http Status Code
        }


        [HttpPost, ActionName("Create")]
        [Route("Create")]
        public async Task<ActionResult> CreateAppointmentAsync(Appointment appointment, Guid animalId)
        {
            try
            {
                var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointment, animalId);

                if (createdAppointment == null)
                {
                    return NotFound(); //NotFound = 404 Http Status Code
                }

                return Ok(createdAppointment); //Retorne un 200 y el objeto Country
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    return Conflict(String.Format("La consulta {0} ya existe.", appointment.Fecha)); //Confilct = 409 Http Status Code Error
                }
                return Conflict(ex.Message);
            }
        }

        [HttpGet, ActionName("Get")]
        [Route("GetById/{id}")] //URL: api/countries/get
        public async Task<ActionResult<Appointment>> GetAppointmentByIdAsync(Guid id)
        {
            if (id == null) return BadRequest("Id es requerido!");

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            if (appointment == null) return NotFound(); // 404

            return Ok(appointment); // 200
        }


        [HttpPut, ActionName("Edit")]
        [Route("Edit")]
        public async Task<ActionResult<Appointment>> EditAppointmentAsync(Appointment appointment, Guid id)
        {
            try
            {
                var editedAppointment = await _appointmentService.EditAppointmentAsync(appointment, id);
                return Ok(editedAppointment);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", appointment.Fecha));

                return Conflict(ex.Message);
            }
        }

        [HttpDelete, ActionName("Delete")]
        [Route("Delete")]
        public async Task<ActionResult<Animal>> DeleteAppointmentAsync(Guid id)
        {
            if (id == null) return BadRequest("Id es requerido!");

            var deletedAppointment = await _appointmentService.DeleteAppointmentAsync(id);

            if (deletedAppointment == null) return NotFound("País no encontrado!");

            return Ok(deletedAppointment);
        }

    }
}
