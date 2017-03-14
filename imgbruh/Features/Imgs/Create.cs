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
            const string Message = ".gifs, .jpg, and .png only...for now...";
            const string Gif = "image/gif";
            const string Jpeg = "image/jpg";
            const string Png = "image/png";

            public Validator()
            {
                RuleFor(c => c.Image)
                    .Must(i => i.ContentType == Gif || i.ContentType == Jpeg || i.ContentType == Png)
                    .WithMessage(Message);
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ImgbruhContext _db;
            private readonly FileStorage _fs;

            public Handler(ImgbruhContext db, FileStorage fs)
            {
                _db = db;
                _fs = fs;
            }
                        
            protected async override Task HandleCore(Command message)
            {
                await Img.CreateAsync(message.Image, message.Name, message.User, _fs, _db);                       
            }
        }
    }
}