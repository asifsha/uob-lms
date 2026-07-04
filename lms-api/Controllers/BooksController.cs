using System;
using lms_data;
using lms_service.Interfaces;
using lms_service.Models;
using Microsoft.AspNetCore.Mvc;

namespace lms_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
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


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            var created = await _bookService.CreateAsync(book);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Book book)
        {
            await _bookService.UpdateAsync(id, book);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}

