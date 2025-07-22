using MediatR;
using DevOpsTaskApp.Application.WorkItemDefinitions.Notifications;
using Microsoft.Extensions.Logging;

public class LogWorkItemCreatedHandler : INotificationHandler<WorkItemCreatedNotification>
{
    private readonly ILogger<LogWorkItemCreatedHandler> _logger;

    public LogWorkItemCreatedHandler(ILogger<LogWorkItemCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(WorkItemCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📝 Work Item Created: {Id} - {Title}", notification.WorkItemId, notification.Title);
        return Task.CompletedTask;
    }
}
