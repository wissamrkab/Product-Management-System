using PMS.Domain.Common;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Categories.Commands.UpdateCategory;

public class CategoryUpdatedEvent(Category category) : BaseEvent
{
    public Category GetCategory() => category;
}