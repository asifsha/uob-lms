using System;
using FluentValidation;
using lms_service.Models;

namespace lms_service.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.ISBN).NotEmpty();
            RuleFor(x => x.PublishedYear)
                .InclusiveBetween(1500, DateTime.UtcNow.Year);
        }
    }
}

