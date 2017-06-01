using CodenameGenerator;
using FluentValidation;
using FluentValidation.Attributes;
using imgbruh.Infrastructure;
using imgbruh.Models;
using MediatR;
using System.Threading.Tasks;
using System.Web;
using System;

namespace imgbruh.Features.Imgs
{
    public class Create
    {
        [Validator(typeof(Validator))]
        public class Command : IAsyncRequest<string>
        {
            public HttpPostedFileBase Image { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            //if it is "...for now..." wtf did i make it constant
            const string Message = ".gifs, .jpg, and .png only...for now...";
            const string Gif = "image/gif";
            const string Jpg = "image/jpg";
            const string Jpeg = "image/jpeg";
            const string Png = "image/png";

            public Validator()
            {
                RuleFor(c => c.Image)
                    .Must(i => i.ContentType == Gif || i.ContentType == Jpeg || i.ContentType == Png || i.ContentType == Jpg)
                    .WithMessage(Message);
            }
        }

        public class Handler : IAsyncRequestHandler<Command, string>
        {
            private readonly FileStorage _fs;
            private readonly DataClient _dc;

            public Handler(DataClient dc, FileStorage fs)
            {
                _dc = dc;
                _fs = fs;                
            }

            public async Task<string> Handle(Command message)
            {
                var generator = new Generator("-", Casing.LowerCase);
                var imgName = generator.Generate();
                generator.Separator = " ";
                generator.SetParts(WordBank.FirstNames, WordBank.LastNames);
                generator.Casing = Casing.PascalCase;
                var artistName = generator.Generate();
                await Img.CreateAsync(message.Image, imgName, artistName, _fs, _dc);
                return imgName;
            }            
        }
    }
}