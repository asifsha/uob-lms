using System;
using System.Data;
using lms_service.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static lms_service.Implementations.IBorrowService;

namespace lms_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "CanBorrowBooks")]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            this.borrowService = borrowService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var loans = await borrowService.GetAllAsync();
            return Ok(loans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var loan = await borrowService.GetByIdAsync(id);
            if (loan == null) return NotFound();
            return Ok(loan);
        }

        [Authorize(Policy = "CanBorrowBooks")]
        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow(int bookId, int memberId)
        {
            try
            {
                var loan = await borrowService.BorrowBookAsync(bookId, memberId);
                return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("return/{id}")]
        public async Task<IActionResult> Return(int id)
        {
            var loan = await borrowService.ReturnBookAsync(id);
            if (loan == null) return NotFound();
            return Ok(loan);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdue()
        {
            var overdue = await borrowService.GetOverdueRecordsAsync();
            return Ok(overdue);
        }
    }
}


