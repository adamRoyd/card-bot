using Engine.Models;

namespace ICM
{
    public interface IIcmService
    {
        double GetExpectedValue(BoardState state);
    }
}