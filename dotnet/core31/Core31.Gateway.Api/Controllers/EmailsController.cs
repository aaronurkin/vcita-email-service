using Core31.Shared.Models.Requests;
using Core31.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core31.Gateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly EmailServiceOptions options;
        private readonly IPublisher<CreateEmailRequest> publisher;

        public EmailsController(
            EmailServiceOptions options,
            IPublisher<CreateEmailRequest> publisher
        )
        {
            this.options = options ?? throw new System.ArgumentNullException(nameof(options));
            this.publisher = publisher ?? throw new System.ArgumentNullException(nameof(publisher));
        }

        [HttpPost]
        public async Task Create(CreateEmailRequest requestData)
        {
            await this.publisher.Publish(requestData, options.CreateEmailTopic);
        }
    }
}
