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
using static Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bookingapp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {

        private readonly ILogger<BookingController> _logger;
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingService _bookingService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public BookingController(ILogger<BookingController> logger , IBookingRepository bookingRepository,IEmailService emailService,IUserRepository userRepository,IBookingService bookingService)
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _bookingService = bookingService;

        }

        // GET: api/<BookingController>
        [HttpGet]
        public async Task<IEnumerable<Booking>> GetBookings()
        {
           return await _bookingRepository.Get();   
        }

        // GET api/<BookingController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetSingleBooking(int id)
        {
            return await _bookingRepository.Get(id);
        }

        // POST api/<BookingController>
        [HttpPost]
        public async Task<ActionResult<Booking>> Post([FromBody] BookingEvent booking)
        {
            // Confirm provided user exists
            var user =  _userRepository.CheckUser(booking.uid, booking.email);

            if (user == null)
            {
                return BadRequest("The provided user information does not belong to any user");
            }

            var newBooking = new Booking { StartTime = booking.start, EndTime = booking.end, Title = booking.title, UserId = user.Id, LabId = booking.labId, Uid = booking.uid };

            var bookingMessage = await _bookingService.IsValidBooking(newBooking);

            if (bookingMessage != null)
            {
                return BadRequest(bookingMessage);
            }
        
            newBooking = await _bookingRepository.Create(newBooking);

            var email = _emailService.CreateEmail(booking.email, user.Name, BookingConstants.BookingCreated, newBooking);

            try
            {
                await _emailService.SendEmailAsync(email);

            }
            catch(Exception ex)
            {
                Console.WriteLine($"EXCEPTION {ex}");
            }
          

            return CreatedAtAction(nameof(GetSingleBooking), new { id = newBooking.Id }, newBooking);
        }

        // PUT api/<BookingController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            var bookingMessage = await _bookingService.IsValidBooking(booking, true);

            if (bookingMessage != null)
            {
                return BadRequest(bookingMessage);
            }

            await _bookingRepository.Update(booking);

            var user = await _userRepository.Get(booking.UserId);

            var email = _emailService.CreateEmail(user.Email,user.Name, BookingConstants.BookingUpdated, booking);

            try
            {
                await _emailService.SendEmailAsync(email);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION {ex}");
            }
            return NoContent();
        }

        // DELETE api/<BookingController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var bookingToDelete = await _bookingRepository.Get(id);

            if(bookingToDelete == null)
            {
                return NotFound();
            }

            var user = await _userRepository.Get(bookingToDelete.UserId);
            var email = _emailService.CreateEmail(user.Email, user.Name, BookingConstants.BookingDeleted, bookingToDelete);

            await _bookingRepository.Delete(id);

            try
            {
                await _emailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION {ex}");
            }
            return NoContent();
        }
    }
}
