using MudBlazor;

namespace ConsiderBorrow.Client;

internal static class ColorThemes
{
    public static MudTheme LightTheme { get; } = new MudTheme()
    {
        Typography = new Typography()
        {
            Default = new()
            {
                FontWeight = 400,
                FontSize = "0.875rem",
            },

            H1 = new()
            {
                FontWeight = 500,
                FontSize = "clamp(2rem, 3.7vw, 4rem)",
            },

            H2 = new()
            {
                FontWeight = 500,
                FontSize = "clamp(2rem, 5vw, 3rem)",
            },

            H3 = new()
            {
                FontWeight = 500,
                FontSize = "clamp(1.8rem, 5vw, 3rem)",
            },

            H4 = new()
            {
                FontWeight = 500,
                FontSize = "clamp(1rem, 5vw, 1.8rem)",
            },

            H5 = new()
            {
                FontWeight = 400,
                FontSize = "clamp(1rem, 5vw, 1.5rem)",
            },

            H6 = new()
            {
                FontWeight = 500,
                FontSize = "clamp(1rem, 5vw, 1.3rem)",
            },

            Subtitle1 = new()
            {
                FontWeight = 400,
                FontSize = "clamp(0.8rem, 2vw, 0.875rem)",
            },

            Subtitle2 = new()
            {
                FontWeight = 500,
                FontSize = "0.875rem",
            },

            Body1 = new()
            {
                FontWeight = 400,
                FontSize = "clamp(1rem, 2vw, 1.25rem)",
            },

            Body2 = new()
            {
                FontWeight = 400,
                FontSize = "clamp(0.9rem, 2vw, 1rem)",
            },

            Button = new()
            {
                FontWeight = 500,
                FontSize = "clamp(0.7rem, 2vw, 1rem)",
            },


            Caption = new()
            {
                FontWeight = 400,
                FontSize = "0.675rem",
            },

            Overline = new()
            {
                FontWeight = 400,
                FontSize = "0.75rem",
            }
        },
        Palette = new PaletteLight()
        {
            Primary = "#FE424D",
            Secondary = "#022D41",
            Tertiary = "#F1F7FC",

            TextPrimary = "#000000",
            TextSecondary = "#00000089",
            TextDisabled = "rgba(0, 0, 0, 0.2)",

            Surface = "#FBFBFB",
            Background = "#FBFBFB",
            BackgroundGrey = "#27272f",

            DrawerBackground = "#ffffff",
            DrawerText = "#000000",
            DrawerIcon = "#000000b8",
            AppbarBackground = "#022D41",
            AppbarText = "#ffffff",

            ActionDefault = "#00000089",
            ActionDisabled = "#00000042",
            ActionDisabledBackground = "#0000001e",

            Divider = "#C2C2C2",
            DividerLight = "rgba(255,255,255, 0.06)",
            LinesDefault = "#0000001e",
            LinesInputs = "#bdbdbdff",

            Black = "#272c34",
            DarkLighten = "#2e2f30",
        }
    };
}
