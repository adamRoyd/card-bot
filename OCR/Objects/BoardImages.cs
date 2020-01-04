using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OCR.Objects
{
    public class BoardImages
    {
        public List<BoardImage> BoardImageList { get; set; }

        public BoardImages()
        {

            BoardImageList = new List<BoardImage>
            {
                new BoardImage
                {
                    Name = ImageName.StartingCard1,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 931,
                    Y = 644
                },
                new BoardImage
                {
                    Name = ImageName.StartingCard2,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 1021,
                    Y = 644
                },
                new BoardImage
                {
                    Name = ImageName.Flop1,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 780,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.Flop2,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 873,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.Flop3,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 980,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.Turn,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 1080,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.River,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 1180,
                    Y = 357
                },
            };
        }
    }
}
