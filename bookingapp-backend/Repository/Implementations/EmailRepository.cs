using bookingapp_backend.Models;
using bookingapp_backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace bookingapp_backend.Repository.Implementations
{
    public class EmailRepository : IEmailRepository
    {

        public readonly DBContext dbContext;

        public EmailRepository(DBContext context)
        {
            dbContext = context; 
        }
        public async Task<Email> Create(Email email)
        {
            email.DateAdded = DateTime.Now;
            email.DateUpdated = email.DateAdded;

            dbContext.Emails.Add(email);
            await dbContext.SaveChangesAsync();
            return email;
        }

        public async Task Delete(int id)
        {
            var emailToDelete = await dbContext.Emails.FindAsync(id);
            dbContext.Emails.Remove(emailToDelete);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Email>> Get()
        {
            return await dbContext.Emails.ToListAsync();
        }

        public async Task<Email> Get(int id)
        {
            return await dbContext.Emails.FindAsync(id);
        }

        public async Task Update(Email email)
        {
            email.DateUpdated = DateTime.Now;
            dbContext.Entry(email).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
