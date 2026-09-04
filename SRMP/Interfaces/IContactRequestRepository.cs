using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IContactRequestRepository
    {
        Task<ContactRequest?> GetByIdAsync(int contactRequestId);

        Task<ContactRequest?> GetByApplicationAsync(int applicationId);

        Task<List<ContactRequest>> GetByJobSeekerAsync(int jobSeekerId);

        Task<List<ContactRequest>> GetByEmployerAsync(int employerId);

        Task AddAsync(ContactRequest contactRequest);

        Task UpdateAsync(ContactRequest contactRequest);
    }
}