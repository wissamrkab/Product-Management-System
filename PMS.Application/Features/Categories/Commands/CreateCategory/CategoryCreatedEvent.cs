using PMS.Domain.Common;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Categories.Commands.CreateCategory;

public class CategoryCreatedEvent(Category category) : BaseEvent
{
    public Category GetCategory() => category;
}