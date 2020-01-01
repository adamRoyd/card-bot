using Engine.Enums;

namespace Engine.Models
{
    public class State
    {
        public StartingHand StartingHand { get; set; }
        public SharedCards SharedCards { get; set; }
        public HandType Hand { get; set; }
        public int Position { get; set; }
        public int Pot { get; set; }
        public Stage Stage { get; set; }
        public Action Action { get; set; }
    }
}
