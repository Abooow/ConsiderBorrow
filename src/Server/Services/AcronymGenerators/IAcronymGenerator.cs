namespace ConsiderBorrow.Server.Services;

public interface IAcronymGenerator
{
    string CreateAcronym(string text, string? separator = null);
}
