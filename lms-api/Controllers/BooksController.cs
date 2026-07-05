using System;
using AutoMapper;
using lms_api.Models.Requests;
using lms_data;
using lms_service.Interfaces;
using lms_service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lms_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public BooksController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _bookService.GetAllAsync());


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [Authorize(Policy = "CanManageBooks")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookRequest request)
        {
            var book = _mapper.Map<Book>(request);
            var created = await _bookService.CreateAsync(book);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookRequest request)
        {
            var book = _mapper.Map<Book>(request);
            await _bookService.UpdateAsync(id, book);
            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}

