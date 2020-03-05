using Engine.Enums;

namespace OCR
{
    public interface ISuitFinder
    {
        string GetBlackOrWhite(string path);
        CardSuit GetSuitFromImage(string path);
        string GetTopRGBColor(string path);
        bool IsDealerButton(string path);
    }
}