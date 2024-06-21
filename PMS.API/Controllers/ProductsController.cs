using PMS.Application.Features.Products.Commands.CreateProduct;
using PMS.Application.Features.Products.Commands.DeleteProduct;
using PMS.Application.Features.Products.Queries.GetAllProducts;
using PMS.Application.Features.Products.Queries.GetProduct;
using PMS.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Dtos.Proudct;

namespace PMS.API.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var getProductQuery = new GetProductQuery
        {
            Id = id
        };
        var result = await mediator.Send(getProductQuery);
            
        if(!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }
    
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    public async Task<IActionResult> GetAllProducts(int page = 1, int pageSize = 10, string searchCriteria = "")
    {
        var getAllProductsQuery = new GetAllProductsQuery
        {
            Page = page,
            PageSize = pageSize,
            SearchCriteria = searchCriteria
        };
        getAllProductsQuery.SetEmail(User.Identity?.Name);
        var result = await mediator.Send(getAllProductsQuery);
            
        if(!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }
    
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommand createProductCommand)
    {
        createProductCommand.SetEmail(User.Identity?.Name);
        var result = await mediator.Send(createProductCommand);
            
        if(!result.IsSuccess) return BadRequest(result);
        return Created($"/api/products/{result.Data!.Id}",result);
    }
    
    // [HttpPut]
    // [Produces("application/json")]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Product>))]
    // [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    // public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductCommand updateProductCommand)
    // {
    //     updateProductCommand.SetEmail(User.Identity?.Name);
    //     var result = await mediator.Send(updateProductCommand);
    //         
    //     if(!result.IsSuccess) return BadRequest(result);
    //     return Ok(result);
    // }
    
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var deleteProductCommand = new DeleteProductCommand
        {
            Id = id
        };
        deleteProductCommand.SetEmail(User.Identity?.Name);
        var result = await mediator.Send(deleteProductCommand);
            
        if(!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }
}