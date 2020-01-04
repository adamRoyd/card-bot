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
            StartingCard1 = new BoardImage
            {
                Image = new Bitmap(300, 300),
                X = 500,
                Y = 700
            };
            StartingCard1 = new BoardImage
            {
                Image = new Bitmap(300, 300),
                X = 500,
                Y = 700
            };
        }

        public BoardImage StartingCard1 { get; set; }
        public BoardImage StartingCard2 { get; set; }
    }
}
