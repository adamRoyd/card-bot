using Engine.Models;

namespace bot.Services
{
    public interface IBoardStateService
    {
        BoardState SetGameStatus(string path, BoardState boardState);
        BoardState SetLiveHand(string path, BoardState boardState, Player[] playersFromPreviousHand);
    }
}
