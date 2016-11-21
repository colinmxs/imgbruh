using FluentValidation;
using FluentValidation.Attributes;
using imgbruh.Models;
using MediatR;
using System;
using System.Threading.Tasks;

namespace imgbruh.Features.Imgs
{
    public class Create
    {
        [Validator(typeof(Validator))]
        public class Command : SignableCommand, IAsyncRequest
        {
            public string Name { get; internal set; }
            public string Url { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Url)
                    .Must(curl => Uri.IsWellFormedUriString(curl, UriKind.Absolute))
                    .WithMessage("Urls only, bro ;)");
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly imgbruhContext _db;

            public Handler(imgbruhContext db)
            {
                _db = db;
            }
                        
            protected async override Task HandleCore(Command message)
            {
                var img = new Img(message.Url, message.Name, message.User);
                _db.Imgs.Add(img);
                await _db.SaveChangesAsync();                
            }
        }
    }
}