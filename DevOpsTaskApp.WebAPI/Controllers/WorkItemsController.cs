using DevOpsTaskApp.Application.WorkItemDefinitions.Commands;
using DevOpsTaskApp.Application.WorkItemDefinitions.Commands.UpdateWorkItemDefinition;
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
        public async Task<IActionResult> Get([FromQuery] string? userStoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetWorkItemDefinitionsQuery
            {
                UserStoryId = userStoryId,
                Page = page,
                PageSize = pageSize
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkItemDefinitionCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(new { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateWorkItemDefinitionCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

    }



}
