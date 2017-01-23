using imgbruh.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace imgbruh.Features.Imgs
{
    public class Details
    {
        public class Query : IAsyncRequest<Model>
        {
            public string CodeName { get; set; }
        }

        public class Model
        {            
            public int Id { get; set; }
            public string CodeName { get; set; }
            public string Url { get; set; }
            public string UserName { get; set; }
            public IEnumerable<Rating> Ratings { get; set; }
            public class Rating
            {
                public int Count { get; set; }
                public string Emotion { get; set; }
            }
            public class Discussion
            {
                public int Count { get; set; }
                public IEnumerable<Comment> Comments {get;set;}
                public class Comment
                {
                    public string UserName { get; set; }
                    public string Message { get; set; }
                    public DateTime TimeCreatedUtc { get; set; }
                }
            }

            public static Model Create(Img img)
            {
                return new Model
                {
                    Id = img.Id,
                    CodeName = img.CodeName,
                    Url = img.Url
                };
            }
        }

        public class Handler : IAsyncRequestHandler<Query, Model>
        {
            private readonly ImgbruhContext _db;

            public Handler(ImgbruhContext db)
            {
                _db = db;
            }

            public async Task<Model> Handle(Query message)
            {
                var img = await _db.Imgs.SingleOrDefaultAsync(i => i.CodeName == message.CodeName);
                return Model.Create(img);
            }
        }
    }
}