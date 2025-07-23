using DevOpsTaskApp.Application.AzureDevOps.Commands.CreateAzureDevOpsTask;
using DevOpsTaskApp.Application.WorkItemDefinitions.Commands;
using DevOpsTaskApp.Application.WorkItemDefinitions.Commands.DeleteWorkItemDefinition;
using DevOpsTaskApp.Application.WorkItemDefinitions.Commands.UpdateWorkItemDefinition;
using DevOpsTaskApp.Application.WorkItemDefinitions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsTaskApp.WebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
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

        /// <summary>
        /// Creates a new work item definition.
        /// This endpoint accepts a CreateWorkItemDefinitionCommand and returns the ID of the created work item
        /// </summary>
        /// <param name="command">The command containing the details of the work item to create.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteWorkItemDefinitionCommand(id));
            return NoContent();
        }

        [HttpPost("CreateAzureTask")]
        public async Task<IActionResult> CreateAzureTask([FromBody] CreateAzureDevOpsTaskCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CreateBatch(List<CreateWorkItemDefinitionCommand> commands, CancellationToken cancellationToken)
        {
            var results = new List<int>();

            foreach (var command in commands)
            {
                var id = await _mediator.Send(command, cancellationToken);
                results.Add(id);
            }

            return Ok(results);
        }

    }



}
