using FluentValidation;
using FluentValidation.Attributes;
using imgbruh.Infrastructure;
using imgbruh.Models;
using MediatR;
using System.Threading.Tasks;
using System.Web;

namespace imgbruh.Features.Imgs
{
    public class Create
    {
        [Validator(typeof(Validator))]
        public class Command : SignableCommand, IAsyncRequest
        {
            public string Name { get; set; }
            public HttpPostedFileBase Image { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Image)
                    .Must(i => i.ContentType == "image/gif" || i.ContentType == "image/jpeg" || i.ContentType == "image/png")
                    .WithMessage(".gifs, .jpg, and .png only...for now...");
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly imgbruhContext _db;
            private readonly FileStorage _fs;

            public Handler(imgbruhContext db, FileStorage fs)
            {
                _db = db;
                _fs = fs;
            }
                        
            protected async override Task HandleCore(Command message)
            {

                var img = await Img.CreateAsync(message.Image, message.Name, message.User, _fs, _db);
                await _db.SaveChangesAsync();                
            }
        }
    }
}