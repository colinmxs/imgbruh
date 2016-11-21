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
                var ratings = new List<Rating>();
                var comments = new List<Discussion.Comment>();

                foreach (var comment in img.Comments)
                {
                    comments.Add(new Discussion.Comment
                    {
                        UserName = comment.User.UserName,
                        Message = comment.Message,
                        TimeCreatedUtc = comment.TimeCreatedUtc                                                                                               
                    });
                }               

                //TODO: figure this ish out
                foreach (var rating in img.Ratings)
                {

                }

                return new Model
                {
                    Id = img.Id,
                    CodeName = img.CodeName,
                    Url = img.Url,
                    Ratings = ratings
                };
            }
        }

        public class Handler : IAsyncRequestHandler<Query, Model>
        {
            private readonly imgbruhContext _db;

            public Handler(imgbruhContext db)
            {
                _db = db;
            }

            public async Task<Model> Handle(Query message)
            {
                var img = await _db.Imgs.Include(i => i.Ratings).Include(i => i.Comments.Select(c => c.User)).SingleOrDefaultAsync(i => i.CodeName == message.CodeName);
                return Model.Create(img);
            }
        }
    }
}