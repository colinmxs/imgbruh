﻿using FluentValidation;
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
        public class Command : IAsyncRequest
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
            private readonly ImgbruhContext _db;
            private readonly FileStorage _fs;

            public Handler(ImgbruhContext db, FileStorage fs)
            {
                _db = db;
                _fs = fs;
            }
                        
            protected override async Task HandleCore(Command message)
            {
                //TODO: plug in name generator
                await Img.CreateAsync(message.Image, message.Name, "Turtle Lover", _fs, _db);                       
            }
        }
    }
}