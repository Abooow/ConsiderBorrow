using ConsiderBorrow.Server.Services;
using ConsiderBorrow.Shared.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace ConsiderBorrow.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return Ok(categories);
    }
}
