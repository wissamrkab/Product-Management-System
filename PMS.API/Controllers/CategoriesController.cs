using PMS.Application.Features.Products.Commands.CreateProduct;
using PMS.Application.Features.Products.Commands.DeleteProduct;
using PMS.Application.Features.Products.Queries.GetAllProducts;
using PMS.Application.Features.Products.Queries.GetProduct;
using PMS.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Dtos.Category;
using PMS.Application.Dtos.Proudct;
using PMS.Application.Features.Categories.Commands.CreateCategory;
using PMS.Application.Features.Categories.Commands.DeleteCategory;
using PMS.Application.Features.Categories.Commands.UpdateCategory;
using PMS.Application.Features.Categories.Queries.GetAllCategories;
using PMS.Application.Features.Categories.Queries.GetCategoriesWithProductCount;

namespace PMS.API.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<List<CategoryDto>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    public async Task<IActionResult> GetAllCategories(int page = 1, int pageSize = 10, Guid? parentId = null, bool allCategories = true)
    {
        var getAllCategoriesQuery = new GetAllCategoriesQuery()
        {
            Page = page,
            PageSize = pageSize,
            ParentId = parentId,
            AllCategories = allCategories
        };
       
        var result = await mediator.Send(getAllCategoriesQuery);
            
        if(!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }
    
    [HttpGet("withProductsCount")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<List<CategoryDto>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    public async Task<IActionResult> GetCategoriesWithProductCount()
    {
        var getAllCategoriesQuery = new GetCategoriesWithProductCountQuery();
       
        var result = await mediator.Send(getAllCategoriesQuery);
            
        if(!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }
    
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand createCategoryCommand)
    {
        var result = await mediator.Send(createCategoryCommand);
            
        if(!result.IsSuccess) return BadRequest(result);
        return Created($"/api/products/{result.Data!.Id}",result);
    }
    
    [HttpPut]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateCategoryCommand updateCategoryCommand)
    {
        var result = await mediator.Send(updateCategoryCommand);
            
        if(!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var deleteCategoryCommand = new DeleteCategoryCommand()
        {
            Id = id
        };
       
        var result = await mediator.Send(deleteCategoryCommand);
            
        if(!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }
}