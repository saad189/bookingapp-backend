using bookingapp_backend.Configurations;
using bookingapp_backend.Helpers;
using bookingapp_backend.Models;
using bookingapp_backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Repository.Implementations
{
    public class InstructorRepository : IInstructorRepository
    {
        public readonly DBContext dbContext;
        public readonly EncryptionKey _encryptionKey;
        public InstructorRepository(DBContext context, IOptions<EncryptionKey> encryptionKey)
        {
            dbContext = context;
            _encryptionKey = encryptionKey.Value;
        }
        
        

        public async Task<Instructor> Create(Instructor user)
        {
            user.Password = Security.SecurePassword(user.Password,_encryptionKey.Key);
            user.DateAdded = DateTime.Now;
            user.DateUpdated = user.DateAdded;

            dbContext.Instructors.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task Delete(int id)
        {
            var userToDelete = await dbContext.Instructors.FindAsync(id);
            var bookingsToDelete = dbContext.Bookings.Where(booking => booking.UserId == id);
            // Need to test deletion of booking
            await bookingsToDelete.ForEachAsync(booking => dbContext.Bookings.Remove(booking));
            dbContext.Instructors.Remove(userToDelete);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Instructor>> Get()
        {
            var userList = dbContext.Instructors.Include(u => u.Bookings).AsNoTracking();
            return await userList.ToListAsync();
        }

        public async Task<Instructor> Get(int id)
        {
            var user = dbContext.Instructors.Where(u => u.Id == id).Include(u => u.Bookings).AsNoTracking();
            return await user.FirstOrDefaultAsync();
        }

        public async Task Update(Instructor user)
        {

            user.DateUpdated = DateTime.Now;

            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
