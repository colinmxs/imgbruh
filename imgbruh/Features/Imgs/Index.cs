using imgbruh.Models;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace imgbruh.Features.Imgs
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
            public string SearchString { get; set; }
            public string SortOrder { get; set; }
            public int? Page { get; set; }
        }

        public class Result
        {
            public string CurrentSort { get; set; }
            public string DateSortParm { get; set; }
            public IEnumerable<Model> Results { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Url { get; set; }
            [DisplayName("Comments")]
            public int CommentCount { get; set; }
            [DisplayName("Ratings")]
            public int RatingCount { get; set; }
            [DisplayName("User")]
            public string UserName { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly imgbruhContext _db;

            public QueryHandler(imgbruhContext db)
            {
                _db = db;
            }

            public Result Handle(Query message)
            {
                var model = new Result
                {
                    CurrentSort = message.SortOrder,
                    DateSortParm = message.SortOrder == "Date" ? "date_desc" : "Date",
                };

                var imgs = from i in _db.Imgs
                           select i;

                if(message.SearchString != null)
                {
                    imgs.SingleOrDefault(i => i.LookupId == message.SearchString);
                }
                else
                {
                    switch (message.SortOrder)
                    {
                        case "Date":
                            imgs = imgs.OrderBy(s => s.TimeCreatedUtc);
                            break;
                        case "date_desc":
                            imgs = imgs.OrderByDescending(s => s.TimeCreatedUtc);
                            break;
                        default: // Name ascending 
                            imgs = imgs.OrderBy(s => s.TimeCreatedUtc);
                            break;
                    }
                }

                int pageSize = 3;
                int pageNumber = (message.Page ?? 1);
                imgs.Skip(pageNumber - 1).Take(pageSize);
                model.Results = imgs.Select(i => new Model { Id = i.Id, Url = i.Url, CommentCount = i.Comments.Count, RatingCount = i.Ratings.Count, UserName = i.User.UserName });

                return model;
            }
        }
    }
}