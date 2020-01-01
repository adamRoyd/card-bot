using System;
using Tesseract;

namespace OCR
{
    public interface IImageProcessor
    {
        string GetTextFromImage(Pix img);
    }
}
