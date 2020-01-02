using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OCR.Objects
{
    public class BoardImages
    {
        public BoardImages()
        {
            StartingCard1 = new Bitmap(100, 100);
        }

        public Bitmap StartingCard1 { get; set; }
        public Bitmap StartingCard2 { get; set; }
    }
}
