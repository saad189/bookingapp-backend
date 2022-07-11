using bookingapp_backend.Models;
using bookingapp_backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingapp_backend.Repository.Implementations
{
    public class LabRepository : ILabRepository
    {
        public readonly DBContext dbContext;

        public LabRepository(DBContext context)
        {
            dbContext = context;
        }

        public async Task<Lab> Create(Lab lab)
        {
            lab.DateAdded = DateTime.Now;
            lab.DateUpdated = lab.DateAdded;

            dbContext.Labs.Add(lab);
            await dbContext.SaveChangesAsync();
            return lab;
        }

        public async Task Delete(int id)
        {
            var labToDelete = await dbContext.Labs.FindAsync(id);
            dbContext.Labs.Remove(labToDelete);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Lab>> Get()
        {
            return await dbContext.Labs.ToListAsync();
        }

        public async Task<Lab> Get(int id)
        {
            return await dbContext.Labs.FindAsync(id);
        }

        public async Task Update(Lab lab)
        {
            lab.DateUpdated = DateTime.Now;
            dbContext.Entry(lab).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
