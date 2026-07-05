using System;
using FluentValidation;
using lms_service.Models;

namespace lms_service.Validators
{
    public class BorrowRequestValidator : AbstractValidator<BorrowRecord>
    {
        public BorrowRequestValidator()
        {
            RuleFor(x => x.BookId).GreaterThan(0);
            RuleFor(x => x.MemberId).GreaterThan(0);
        }

    }
}

