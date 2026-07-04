using System;
using lms_service.Models;

namespace lms_service.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetAllAsync();
        Task<Member> GetByIdAsync(int id);
        Task<Member> CreateAsync(Member member);
        Task UpdateAsync(int id, Member member);
        Task DeleteAsync(int id);
    }
}

