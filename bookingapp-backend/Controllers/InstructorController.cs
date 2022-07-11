using bookingapp_backend.DTOs;
using bookingapp_backend.Models;
using bookingapp_backend.Repository;
using bookingapp_backend.Repository.Interfaces;
using bookingapp_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    public class InstructorController : ControllerBase
    {

        private readonly ILogger<InstructorController> _logger;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ILoginService _loginService;

        public InstructorController(ILogger<InstructorController> logger, IInstructorRepository instructorRepository, ILoginService loginService)
        {
            _logger = logger;
            _instructorRepository = instructorRepository;
            _loginService = loginService;
        }

        // GET: api/<InstructorController>
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<Instructor>> GetInstructors()
        {
            return await _instructorRepository.Get();
        }

        // GET api/<InstructorController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Instructor>> GetSingleInstructor(int id)
        {
            return await _instructorRepository.Get(id);
        }

        // POST api/<InstructorController>
        [HttpPost]
        public async Task<ActionResult<Instructor>> Post([FromBody] Instructor user)
        {

            var newInstructor = await _instructorRepository.Create(user);
            return CreatedAtAction(nameof(GetSingleInstructor), new { id = newInstructor.Id }, newInstructor);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogInInstructor([FromBody] LoginModel login)
        {
            var loggedInUser = await _loginService.CheckLogin(login.UserId, login.Password);
            if (loggedInUser == null)
            {
                return NotFound("User not found.");
            }

            JwtModel token = new JwtModel();

            token.idToken = _loginService.GenerateJwtToken(loggedInUser);
            token.expiresAt = DateTime.Now.AddHours(1);

            return Ok(token);
        }

        // PUT api/<InstructorController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] Instructor user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _instructorRepository.Update(user);

            return NoContent();
        }

        // DELETE api/<InstructorController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var userToDelete = await _instructorRepository.Get(id);

            if (userToDelete == null)
                return NotFound();


            await _instructorRepository.Delete(id);

            return NoContent();
        }
    }
}
