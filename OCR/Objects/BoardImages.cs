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
                    X = 749,
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
                    X = 949,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.Turn,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 1050,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.River,
                    Type = ImageType.Card,
                    Image = new Bitmap(51, 75),
                    X = 1152,
                    Y = 357
                },
                new BoardImage
                {
                    Name = ImageName.Position1,
                    Type = ImageType.PlayerDealerButton,
                    Image = new Bitmap(25, 25),
                    X = 831,
                    Y = 641
                },
                new BoardImage
                {
                    Name = ImageName.Position2,
                    Type = ImageType.PlayerDealerButton,
                    Image = new Bitmap(25, 25),
                    X = 590,
                    Y = 521
                },
                new BoardImage
                {
                    Name = ImageName.Position3,
                    Type = ImageType.PlayerDealerButton,
                    Image = new Bitmap(25, 25),
                    X = 505,
                    Y = 378
                },
                new BoardImage
                {
                    Name = ImageName.Position4,
                    Type = ImageType.PlayerDealerButton,
                    Image = new Bitmap(25, 25),
                    X = 670,
                    Y = 288
                },
                new BoardImage
                {
                    Name = ImageName.Position5,
                    Type = ImageType.PlayerDealerButton,
                    Image = new Bitmap(25, 25),
                    X = 900,
                    Y = 221
                },
                new BoardImage
                {
                    Name = ImageName.Position6,
                    Type = ImageType.PlayerDealerButton,
                    Image = new Bitmap(25, 25),
                    X = 1189,
                    Y = 249
                },
                new BoardImage
                {
                    Name = ImageName.Position7,
                    Type = ImageType.PlayerDealerButton,
                    Image = new Bitmap(25, 25),
                    X = 1339,
                    Y = 323
                },
                new BoardImage
                {
                    Name = ImageName.Position8,
                    Type = ImageType.PlayerDealerButton,
                    Image = new Bitmap(25, 25),
                    X = 1385,
                    Y = 517
                },
                new BoardImage
                {
                    Name = ImageName.Position9,
                    Type = ImageType.PlayerDealerButton,
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
                //new BoardImage
                //{
                //    Name = ImageName.Bet1,
                //    Type = ImageType.Bet,
                //    Image = new Bitmap(170, 40),
                //    X = 822,
                //    Y = 601
                //},
                //new BoardImage
                //{
                //    Name = ImageName.Bet2,
                //    Type = ImageType.Bet,
                //    Image = new Bitmap(170, 40),
                //    X = 561,
                //    Y = 579
                //},
                //new BoardImage
                //{
                //    Name = ImageName.Bet3,
                //    Type = ImageType.Bet,
                //    Image = new Bitmap(170, 40),
                //    X = 534,
                //    Y = 482
                //},
                //new BoardImage
                //{
                //    Name = ImageName.Bet4,
                //    Type = ImageType.Bet,
                //    Image = new Bitmap(170, 40),
                //    X = 486,
                //    Y = 321
                //},
                //new BoardImage
                //{
                //    Name = ImageName.Bet5,
                //    Type = ImageType.Bet,
                //    Image = new Bitmap(170, 40),
                //    X = 724,
                //    Y = 257
                //},
                //new BoardImage
                //{
                //    Name = ImageName.Bet6,
                //    Type = ImageType.Bet,
                //    Image = new Bitmap(170, 40),
                //    X = 1069,
                //    Y = 232
                //},
                //new BoardImage
                //{
                //    Name = ImageName.Bet7,
                //    Type = ImageType.Bet,
                //    Image = new Bitmap(170, 40),
                //    X = 1210,
                //    Y = 289
                //},
                //new BoardImage
                //{
                //    Name = ImageName.Bet8,
                //    Type = ImageType.Bet,
                //    Image = new Bitmap(170, 40),
                //    X = 1254,
                //    Y = 480
                //},
                //new BoardImage
                //{
                //    Name = ImageName.Bet9,
                //    Type = ImageType.Bet,
                //    Image = new Bitmap(170, 40),
                //    X = 1138,
                //    Y = 550
                //},
                new BoardImage
                {
                    Name = ImageName.CallAmount,
                    Type = ImageType.Bet,
                    Image = new Bitmap(170, 40),
                    X = 1352,
                    Y = 986
                },
                new BoardImage
                {
                    Name = ImageName.BigBlind,
                    Type = ImageType.BigBlind,
                    Image = new Bitmap(200, 25),
                    X = 1760,
                    Y = 110
                },
                new BoardImage
                {
                    Name = ImageName.ReadyForAction,
                    Type = ImageType.ReadyForAction,
                    Image = new Bitmap(160, 50),
                    X = 1159,
                    Y = 967
                },
                new BoardImage
                {
                    Name = ImageName.Stack1,
                    Type = ImageType.PlayerStack,
                    PlayerNumber = 1,
                    Image = new Bitmap(150, 40),
                    X = 849,
                    Y = 767
                },
                new BoardImage
                {
                    Name = ImageName.Stack2,
                    Type = ImageType.PlayerStack,
                    PlayerNumber = 2,
                    Image = new Bitmap(150, 40),
                    X = 411,
                    Y = 667
                },
                new BoardImage
                {
                    Name = ImageName.Stack3,
                    Type = ImageType.PlayerStack,
                    PlayerNumber = 3,
                    Image = new Bitmap(150, 40),
                    X = 261,
                    Y = 461
                },
                new BoardImage
                {
                    Name = ImageName.Stack4,
                    Type = ImageType.PlayerStack,
                    PlayerNumber = 4,
                    Image = new Bitmap(150, 40),
                    X = 340,
                    Y = 266
                },
                new BoardImage
                {
                    Name = ImageName.Stack5,
                    Type = ImageType.PlayerStack,
                    PlayerNumber = 5,
                    Image = new Bitmap(150, 40),
                    X = 613,
                    Y = 166
                },
                new BoardImage
                {
                    Name = ImageName.Stack6,
                    Type = ImageType.PlayerStack,
                    PlayerNumber = 6,
                    Image = new Bitmap(150, 40),
                    X = 1171,
                    Y = 162
                },
                new BoardImage
                {
                    Name = ImageName.Stack7,
                    Type = ImageType.PlayerStack,
                    PlayerNumber = 7,
                    Image = new Bitmap(170, 40),
                    X = 1455,
                    Y = 266
                },
                new BoardImage
                {
                    Name = ImageName.Stack8,
                    Type = ImageType.PlayerStack,
                    PlayerNumber = 8,
                    Image = new Bitmap(150, 40),
                    X = 1496,
                    Y = 462
                },
                new BoardImage
                {
                    Name = ImageName.Stack9,
                    Type = ImageType.PlayerStack,
                    PlayerNumber = 9,
                    Image = new Bitmap(150, 40),
                    X = 1343,
                    Y = 669
                }
            };
        }
    }
}
