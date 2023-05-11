namespace ConsiderBorrow.Client;

internal static class DefaultImageProvider
{
    public static string GetDefaultImage(string type)
    {
        return type switch
        {
            "DVD" => "images/no-image-circle.png",
            "Audio Book" => "images/no-image-square.png",
            _ => "images/no-image-rectangle.png"
        };
    }
}
