using AutoMapper;
using lms_data.Entities;
using lms_data.Repositories;
using lms_service.Models;
using lms_service.Interfaces;

namespace lms_service.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly IRepository<MemberEntity> _repo;
        private readonly IMapper _mapper;

        public MemberService(IRepository<MemberEntity> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // =========================
        // CREATE
        // =========================
        public async Task<Member> CreateAsync(Member member)
        {
            ValidateMember(member);
            await ValidateEmailUnique(member.Email);

            var entity = _mapper.Map<MemberEntity>(member);

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            member.Id = entity.Id;
            return member;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();

            return _mapper.Map<IEnumerable<Member>>(list);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<Member> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Invalid member id.");

            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                throw new InvalidOperationException("Member not found.");

            return _mapper.Map<Member>(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(int id, Member member)
        {
            if (id <= 0)
                throw new InvalidOperationException("Invalid member id.");

            var existing = await _repo.GetByIdAsync(id);

            if (existing == null)
                throw new InvalidOperationException("Member not found.");

            ValidateMember(member);

            // prevent ID overwrite
            member.Id = id;

            _mapper.Map(member, existing);

            _repo.Update(existing);
            await _repo.SaveChangesAsync();
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Invalid member id.");

            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                throw new InvalidOperationException("Member not found.");

            _repo.Remove(entity);
            await _repo.SaveChangesAsync();
        }

        // =========================
        // VALIDATION
        // =========================

        private void ValidateMember(Member member)
        {
            if (member == null)
                throw new InvalidOperationException("Member data is required.");

            if (string.IsNullOrWhiteSpace(member.FullName))
                throw new InvalidOperationException("Full name is required.");

            if (string.IsNullOrWhiteSpace(member.Email))
                throw new InvalidOperationException("Email is required.");

            if (!member.Email.Contains("@"))
                throw new InvalidOperationException("Invalid email format.");

            if (string.IsNullOrWhiteSpace(member.Phone))
                throw new InvalidOperationException("Phone is required.");
        }

        private async Task ValidateEmailUnique(string email)
        {
            var members = await _repo.GetAllAsync();

            if (members.Any(x => x.Email == email))
                throw new InvalidOperationException("Email already exists.");
        }
    }
}