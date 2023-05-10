﻿using ConsiderBorrow.Server.Services;
using ConsiderBorrow.Shared.Models.Categories;
using ConsiderBorrow.Shared.Results;
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

    [HttpPost]
    public async Task<ActionResult<Result<CategoryResponse>>> CreateCategory(CreateCategoryRequest createCategoryRequest)
    {
        var result = await _categoryService.CreateCategoryAsync(createCategoryRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpGet]
    public async Task<IEnumerable<CategoryResponse>> GetCategories()
    {
        return await _categoryService.GetCategoriesAsync();
    }

    [HttpPatch]
    public async Task<ActionResult<Result>> UpdateCategory(UpdateCategoryRequest updateCategoryRequest)
    {
        var result = await _categoryService.UpdateCategoryAsync(updateCategoryRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }
}
