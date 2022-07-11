using bookingapp_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {

        public readonly DBContext dbContext;

        public UserRepository(DBContext context)
        {
            dbContext = context;
        }

        public async Task<User> Create(User user)
        {
            user.DateAdded = DateTime.Now;
            user.DateUpdated = user.DateAdded;

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task Delete(int id)
        {
            var userToDelete = await dbContext.Users.FindAsync(id);
            var bookingsToDelete =  dbContext.Bookings.Where(booking => booking.UserId == id);
            // Need to test deletion of booking
            await bookingsToDelete.ForEachAsync(booking => dbContext.Bookings.Remove(booking));
            dbContext.Users.Remove(userToDelete);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> Get()
        {
            var userList = dbContext.Users.Include(u => u.Bookings).AsNoTracking();
            return await userList.ToListAsync();
        }

        public User CheckUser(string UId, string email)
        {
            return  dbContext.Users.Where(user => user.Uid == UId && user.Email == email).FirstOrDefault();
        }
        public async Task<User> Get(int id)
        {
            var user = dbContext.Users.Where(u => u.Id == id).Include(u => u.Bookings).AsNoTracking();
            return await user.FirstOrDefaultAsync();
        }

        public async Task Update(User user)
        {

            user.DateUpdated = DateTime.Now;

            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
