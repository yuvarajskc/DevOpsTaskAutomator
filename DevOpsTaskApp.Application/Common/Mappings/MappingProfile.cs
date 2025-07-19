using AutoMapper;
using DevOpsTaskApp.Application.WorkItemDefinitions.Queries;
using DevOpsTaskApp.Domain.Entities;

namespace DevOpsTaskApp.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<WorkItemDefinition, WorkItemDefinitionDto>();
    }
}
