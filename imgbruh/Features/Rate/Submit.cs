using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace imgbruh.Features.Rate
{
    public class Submit
    {
        public class Command : SignableCommand, IAsyncRequest
        {
            public string CodeName { get; set; }
            public string Rating { get; set; }
            public int OnOff { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.OnOff).Must(o => o == 1 || o == 0);
                RuleFor(c => c.Rating).Must(r => r == "like" || r == "dislike");
                RuleFor(c => c.CodeName).NotNull().NotEmpty();
            }
        }

        //public class Handler : AsyncRequestHandler<Command>
        //{
        //    protected override Task HandleCore(Command message)
        //    {
                
        //    }
        //}
    }
}