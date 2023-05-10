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

    [HttpPost("book")]
    public async Task<ActionResult<Result<LibraryItemResponse>>> CreateBook(CreateBookRequest createBookRequest)
    {
        var result = await _libraryItemService.CreateBookAsync(createBookRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost("dvd")]
    public async Task<ActionResult<Result<LibraryItemResponse>>> CreateDvd(CreateDvdRequest createDvdRequest)
    {
        var result = await _libraryItemService.CreateDvdAsync(createDvdRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost("audio-book")]
    public async Task<ActionResult<Result<LibraryItemResponse>>> CreateAudioBook(CreateAudioBookRequest createAudioBookRequest)
    {
        var result = await _libraryItemService.CreateAudioBookAsync(createAudioBookRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost("reference-book")]
    public async Task<ActionResult<Result<LibraryItemResponse>>> CreateReferenceBook(CreateReferenceBookRequest createReferenceBookRequest)
    {
        var result = await _libraryItemService.CreateReferenceBookAsync(createReferenceBookRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpGet]
    public async Task<IEnumerable<LibraryItemResponse>> GetLibraryItems([FromQuery] int currentPage = 0, [FromQuery] int pageSize = 16, [FromQuery] bool sortByType = false)
    {
        var libraryItems = await _libraryItemService.GetLibraryItemsAsync(currentPage, pageSize, sortByType);
        return libraryItems;
    }

    [HttpPost("check-out/{id}")]
    public async Task<ActionResult<Result>> BorrowLibraryItem(int id, BorrowLibraryItemRequest borrowLibraryItemRequest)
    {
        var result = await _libraryItemService.BorrowItemAsync(id, borrowLibraryItemRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost("check-in/{id}")]
    public async Task<ActionResult<Result>> ReturnLibraryItem(int id)
    {
        var result = await _libraryItemService.ReturnItemAsync(id);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Result>> UpdateItem(int id, UpdateLibraryItemRequest updateLibraryItemRequest)
    {
        var result = await _libraryItemService.UpdateItemAsync(id, updateLibraryItemRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> DeleteItem(int id)
    {
        var result = await _libraryItemService.DeleteItemAsync(id);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }
}
