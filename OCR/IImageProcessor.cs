using Engine.Enums;
using OCR.Objects;
using System.Collections.Generic;
using System.Drawing;
using Tesseract;

namespace OCR
{
    public interface IImageProcessor
    {
        CardValue GetCardValueFromImage(Image image);
        string GetImageCharacters(Image image, PageSegMode segMode);
        string GetNumbers(Image img, PageSegMode mode);
        List<BoardImage> SliceBoardScreenShot(string path, List<BoardImage> boardImageList);
    }
}