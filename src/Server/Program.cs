using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Data Access.
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services.
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddTransient<ILibraryItemService, LibraryItemService>();
builder.Services.AddTransient<IAcronymGenerator, SimpleByWordsAcronymGenerator>();
builder.Services.AddSingleton<IUpdateLibraryItemManager, UpdateLibraryItemManager>(x => new UpdateLibraryItemManager(typeof(Program).Assembly));

// Custom model validation response.
builder.Services.UseResultBasedValidationResponse();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Create the Database if it doesn't exist.
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreatedAsync();

    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
