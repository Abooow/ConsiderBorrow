using System.Net;
using ConsiderBorrow.Server.Services;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace ConsiderBorrow.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class LibraryItemsController : ControllerBase
{
    private readonly ILibraryItemService _libraryItemService;

    public LibraryItemsController(ILibraryItemService libraryItemService)
    {
        _libraryItemService = libraryItemService;
    }

    [HttpPost]
    public async Task<ActionResult<Result<LibraryItemResponse>>> CreateLibraryItem(CreateLibraryItemRequest createLibraryItemRequest)
    {
        var result = await _libraryItemService.CreateLibraryItemAsync(createLibraryItemRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Result<LibraryItemResponse>>> GetLibraryItem(int id)
    {
        var result = await _libraryItemService.GetLibraryItemAsync(id);

        if (!result.Succeeded && result.StatusCode is HttpStatusCode.NotFound)
            return NotFound(result);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpGet]
    public async Task<IEnumerable<LibraryItemResponse>> GetLibraryItems([FromQuery] int currentPage = 0, [FromQuery] int pageSize = 16, [FromQuery] bool sortByType = false)
    {
        return await _libraryItemService.GetLibraryItemsAsync(currentPage, pageSize, sortByType);
    }

    [HttpPost("check-out/{id}")]
    public async Task<ActionResult<Result<LibraryItemResponse>>> CheckOutLibraryItem(int id, CheckOutLibraryItemRequest checkOutLibraryItemRequest)
    {
        var result = await _libraryItemService.CheckOutItemAsync(id, checkOutLibraryItemRequest);

        if (!result.Succeeded && result.StatusCode is HttpStatusCode.NotFound)
            return NotFound(result);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost("return/{id}")]
    public async Task<ActionResult<Result<LibraryItemResponse>>> ReturnLibraryItem(int id)
    {
        var result = await _libraryItemService.ReturnItemAsync(id);

        if (!result.Succeeded && result.StatusCode is HttpStatusCode.NotFound)
            return NotFound(result);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Result<LibraryItemResponse>>> UpdateItem(int id, UpdateLibraryItemRequest updateLibraryItemRequest)
    {
        var result = await _libraryItemService.UpdateItemAsync(id, updateLibraryItemRequest);

        if (!result.Succeeded && result.StatusCode is HttpStatusCode.NotFound)
            return NotFound(result);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> DeleteItem(int id)
    {
        var result = await _libraryItemService.DeleteItemAsync(id);

        if (!result.Succeeded && result.StatusCode is HttpStatusCode.NotFound)
            return NotFound(result);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }
}
