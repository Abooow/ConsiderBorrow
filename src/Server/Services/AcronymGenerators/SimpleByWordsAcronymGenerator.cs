namespace ConsiderBorrow.Server.Services;

/// <summary>
/// Will generate the following acronyms:
/// <br />
/// Hello, world! -> HW <br />
/// my title of _ -> MTO_ <br />
/// ASP.NET   101 -> A1 <br />
/// </summary>
internal sealed class SimpleByWordsAcronymGenerator : IAcronymGenerator
{
    public string CreateAcronym(string text, string? separator = null)
    {
        return string.Join(separator, text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(str => str[0])).ToUpper();
    }
}
