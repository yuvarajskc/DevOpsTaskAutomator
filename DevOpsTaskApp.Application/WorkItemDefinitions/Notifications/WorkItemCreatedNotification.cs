using MediatR;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Notifications;

public class WorkItemCreatedNotification : INotification
{
    public int WorkItemId { get; }
    public string Title { get; }

    public WorkItemCreatedNotification(int workItemId, string title)
    {
        WorkItemId = workItemId;
        Title = title;
    }
}
