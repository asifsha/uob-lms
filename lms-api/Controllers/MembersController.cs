using System;
using AutoMapper;
using lms_api.Models.Requests;
using lms_data.Entities;
using lms_service.Interfaces;
using lms_service.Models;
using Microsoft.AspNetCore.Mvc;

namespace lms_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;

        public MembersController(IMemberService memberService, IMapper mapper)
        {
            _memberService = memberService;
            _mapper = mapper;
        }

        // GET: api/members
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var members = await _memberService.GetAllAsync();
            return Ok(members);
        }

        // GET: api/members/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
                return NotFound();
            return Ok(member);
        }

        // POST: api/members
        [HttpPost]
        public async Task<IActionResult> Create(MemberRequest request)
        {
            var member = _mapper.Map<Member>(request);
            var created = await _memberService.CreateAsync(member);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/members/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MemberRequest request)
        {
            var member = _mapper.Map<Member>(request);
            await _memberService.UpdateAsync(id, member);
            return NoContent();
        }

        // DELETE: api/members/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _memberService.DeleteAsync(id);
            return NoContent();
        }
    }
}

