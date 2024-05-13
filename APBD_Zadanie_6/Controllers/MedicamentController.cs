using APBD_Zadanie_6.Models;
using Microsoft.AspNetCore.Mvc;
using Zadanie5.Services;

namespace Zadanie5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentController : ControllerBase
    {
        private readonly IMedicamentService _medicamentService;

        public MedicamentController(IMedicamentService medicamentService)
        {
            _medicamentService = medicamentService;
        }

        [HttpGet]
        public ActionResult GetMedicament(int id)
        {
            try
            {
                Task<MedicamentDTO> toReturn = _medicamentService.GetMedicament(id);
                MedicamentDTO result = toReturn.Result;
                return Ok(result);
            }catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpDelete]
        public ActionResult DeletePatient(int id)
        {
            try
            {
                _medicamentService.DeletePatient(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
