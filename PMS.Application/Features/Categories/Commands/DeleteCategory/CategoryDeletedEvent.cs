using PMS.Domain.Common;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Categories.Commands.DeleteCategory;

public class CategoryDeletedEvent(Category category) : BaseEvent
{
    public Category GetCategory() => category;
}