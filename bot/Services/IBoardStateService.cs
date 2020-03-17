using Engine.Models;

namespace bot.Services
{
    public interface IBoardStateService
    {
        BoardState GetBoardStateFromImagePath(string path, Player[] playersFromPreviousHand);
    }
}
