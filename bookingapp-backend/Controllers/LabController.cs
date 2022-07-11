using bookingapp_backend.Models;
using bookingapp_backend.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bookingapp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : ControllerBase
    {

        private readonly ILogger<LabController> _logger;
        private readonly ILabRepository _labRepository;

        public LabController(ILogger<LabController> logger, ILabRepository labRepository)
        {
            _logger = logger;
            _labRepository = labRepository;
        }

        // GET: api/<LabController>
        [HttpGet]
        public async Task<IEnumerable<Lab>> GetLabs()
        {
            return await _labRepository.Get();
        }

        // GET api/<LabController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lab>> GetSingleLab(int id)
        {
            return await _labRepository.Get(id);
        }

        // POST api/<LabController>
        [HttpPost]
        public async Task<ActionResult<Lab>> Post([FromBody] Lab lab)
        {
            var newLab = await _labRepository.Create(lab);
            return CreatedAtAction(nameof(GetSingleLab), new { id = newLab.Id }, newLab);
        }

        // PUT api/<LabController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Lab lab)
        {
            if (id != lab.Id)
            {
                return BadRequest();
            }

            await _labRepository.Update(lab);

            return NoContent();
        }

        // DELETE api/<LabController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var labToDelete = await _labRepository.Get(id);

            if (labToDelete == null)
                return NotFound();

            await _labRepository.Delete(id);

            return NoContent();
        }
    }
}
