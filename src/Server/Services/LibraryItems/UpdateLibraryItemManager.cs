using System.Collections.ObjectModel;
using System.Reflection;
using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Server.Services.LibraryItems.UpdateLibraryItemHandlers;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services;

internal sealed class UpdateLibraryItemManager : IUpdateLibraryItemManager
{
    private readonly IReadOnlyDictionary<string, IUpdateLibraryItemHandler> updateLibraryItemHandlers;

    public UpdateLibraryItemManager(Assembly assembly)
    {
        // Scan for all types in assembly that implements IUpdateLibraryItemHandler and create an instance as a singleton.
        var updateLibraryItemHandlerImplementations = assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IUpdateLibraryItemHandler)) && x.IsClass)
            .Select(x => (IUpdateLibraryItemHandler)Activator.CreateInstance(x)!);

        var baseDictionary = updateLibraryItemHandlerImplementations.ToDictionary(x => x.HandleForType, x => x);
        updateLibraryItemHandlers = new ReadOnlyDictionary<string, IUpdateLibraryItemHandler>(baseDictionary);
    }

    public Result UpdateItem(string type, LibraryItemRecord record, UpdateLibraryItemRequest updateLibraryItemRequest)
    {
        if (!updateLibraryItemHandlers.TryGetValue(type, out var handler))
            return Result.Fail($"'{type}' is not a valid type.");

        return handler.HandleUpdate(record, updateLibraryItemRequest);
    }
}
