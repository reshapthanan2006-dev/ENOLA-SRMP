using Microsoft.EntityFrameworkCore;
using SRMP.Data;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Repositories
{
    public class ContactRequestRepository : IContactRequestRepository
    {
        private readonly AppDbContext _context;

        public ContactRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ContactRequest?> GetByIdAsync(int contactRequestId)
        {
            return await _context.ContactRequests
                .FirstOrDefaultAsync(c =>
                    c.ContactRequestId == contactRequestId);
        }

        public async Task<ContactRequest?> GetByApplicationAsync(
            int applicationId)
        {
            return await _context.ContactRequests
                .FirstOrDefaultAsync(c =>
                    c.ApplicationId == applicationId);
        }

        public async Task<List<ContactRequest>> GetByJobSeekerAsync(
            int jobSeekerId)
        {
            return await _context.ContactRequests
                .Where(c => c.JobSeekerId == jobSeekerId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ContactRequest>> GetByEmployerAsync(
            int employerId)
        {
            return await _context.ContactRequests
                .Where(c => c.EmployerId == employerId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(ContactRequest contactRequest)
        {
            await _context.ContactRequests.AddAsync(contactRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ContactRequest contactRequest)
        {
            _context.ContactRequests.Update(contactRequest);
            await _context.SaveChangesAsync();
        }
    }
}