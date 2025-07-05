using DevOpsTaskApp.Application.WorkItemDefinitions.Commands;
using DevOpsTaskApp.Application.WorkItemDefinitions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsTaskApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkItemsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetWorkItemDefinitionsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkItemDefinitionCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(new { Id = id });
        }
    }



}
