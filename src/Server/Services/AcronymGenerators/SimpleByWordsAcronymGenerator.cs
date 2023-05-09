namespace ConsiderBorrow.Server.Services;

/// <summary>
/// Will generate the following acronyms:
/// 
/// Hello, world! -> HW
/// my title of _ -> MTO_
/// ASP.NET   101 -> A1
/// </summary>
internal sealed class SimpleByWordsAcronymGenerator : IAcronymGenerator
{
    public string CreateAcronym(string text, string? separator = null)
    {
        return string.Join(separator, text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(str => str[0])).ToUpperInvariant();
    }
}
