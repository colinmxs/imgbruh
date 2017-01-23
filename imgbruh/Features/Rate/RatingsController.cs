using imgbruh.Infrastructure;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

namespace imgbruh.Features.Rate
{
    [RoutePrefix("r")]
    public class RatingsController
    {
        private readonly IMediator _mediator;

        public RatingsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //[HttpPost]
        //[MustRegister]
        //[SignCommand]
        //public async Task Submit(Submit.Command command)
        //{
        //    await _mediator.SendAsync(command);
        //}
    }
}