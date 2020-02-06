using System.Drawing;

namespace OCR.Objects
{
    public class BoardImage
    {
        public ImageName Name { get; set; }
        public Image Image { get; set; }
        public int PlayerNumber { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ImageType Type { get; set; }
    }

    public enum ImageName
    {
        StartingCard1,
        StartingCard2,
        Flop1, 
        Flop2, 
        Flop3, 
        Turn, 
        River, 
        Position1,
        Position2,
        Position3,
        Position4,
        Position5,
        Position6,
        Position7,
        Position8,
        Position9,
        Bet1,
        Bet2,
        Bet3,
        Bet4,
        Bet5,
        Bet6,
        Bet7,
        Bet8,
        Bet9,        
        Pot,
        Stack1,
        Stack2,
        Stack3,
        Stack4,
        Stack5,
        Stack6,
        Stack7,
        Stack8,
        Stack9,
        BigBlind,
        Players,
        ReadyForAction,
        FoldButton,
        CallButton,
        RaiseButton,
        CallAmount,
        RaiseAmount,
        Ante,
        GameIsFinished
    }

    public enum ImageType
    {
        Card,
        Pot,
        PlayerBet,
        Word,
        Number,
        PlayerDealerButton,
        BigBlind,
        ReadyForAction,
        PlayerStack,
        Ante,
        GameIsFinished
    }
}
