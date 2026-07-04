using System;
using lms_data.Entities;
using lms_data.Repositories;
using lms_service.Models;
using lms_service.Interfaces;
using AutoMapper;

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


        public async Task<Member> CreateAsync(Member member)
        {
            MemberEntity e =_mapper.Map<MemberEntity>(member);
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            member.Id = e.Id;
            return member;
        }


        public async Task DeleteAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return;
            _repo.Remove(e);
            await _repo.SaveChangesAsync();
        }


        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return (IEnumerable<Member>)_mapper.Map<Member>(list);
        }

        public async Task<Member> GetByIdAsync(int id)
        {
            var mmember = await _repo.GetByIdAsync(id);
            return _mapper.Map<Member>(mmember);
        }

        public async Task UpdateAsync(int id, Member member)
        {
            MemberEntity entity = _mapper.Map<MemberEntity>(member);
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
        }
    }
}

