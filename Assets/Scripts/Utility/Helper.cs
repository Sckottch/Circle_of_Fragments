using System.Collections.Generic;

public static class Helper
{
    public static bool IsSpecialElement(Element element)
    {
        return element switch
        {
            Element.Light => true,
            Element.Dark => true,
            Element.Lightning => true,

            _ => false
        };
    }
}
