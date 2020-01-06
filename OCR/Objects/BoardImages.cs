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
                    X = 906,
                    Y = 644
                },
                new BoardImage
                {
                    Name = ImageName.StartingCard2,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 996,
                    Y = 644
                },
                new BoardImage
                {
                    Name = ImageName.Flop1,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 755,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.Flop2,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 848,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.Flop3,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 955,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.Turn,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 1055,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.River,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 1155,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.Position1,
                    Type = ImageType.DealerButton,
                    Image = new Bitmap(25, 25),
                    X = 831,
                    Y = 641
                },
                new BoardImage
                {
                    Name = ImageName.Position2,
                    Type = ImageType.DealerButton,
                    Image = new Bitmap(25, 25),
                    X = 590,
                    Y = 521
                },
                new BoardImage
                {
                    Name = ImageName.Position3,
                    Type = ImageType.DealerButton,
                    Image = new Bitmap(25, 25),
                    X = 505,
                    Y = 378
                },
                new BoardImage
                {
                    Name = ImageName.Position4,
                    Type = ImageType.DealerButton,
                    Image = new Bitmap(25, 25),
                    X = 670,
                    Y = 288
                },
                new BoardImage
                {
                    Name = ImageName.Position5,
                    Type = ImageType.DealerButton,
                    Image = new Bitmap(25, 25),
                    X = 900,
                    Y = 221
                },
                new BoardImage
                {
                    Name = ImageName.Position6,
                    Type = ImageType.DealerButton,
                    Image = new Bitmap(25, 25),
                    X = 1189,
                    Y = 249
                },
                new BoardImage
                {
                    Name = ImageName.Position7,
                    Type = ImageType.DealerButton,
                    Image = new Bitmap(25, 25),
                    X = 1339,
                    Y = 323
                },
                new BoardImage
                {
                    Name = ImageName.Position8,
                    Type = ImageType.DealerButton,
                    Image = new Bitmap(25, 25),
                    X = 1385,
                    Y = 517
                },
                new BoardImage
                {
                    Name = ImageName.Position9,
                    Type = ImageType.DealerButton,
                    Image = new Bitmap(25, 25),
                    X = 1215,
                    Y = 591
                },
                new BoardImage
                {
                    Name = ImageName.Pot,
                    Type = ImageType.Pot,
                    Image = new Bitmap(200, 40),
                    X = 894,
                    Y = 318
                },
            };
        }
    }
}
