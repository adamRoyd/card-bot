using Engine.Models;
using OCR.Objects;
using System.Collections.Generic;
using System.Drawing;

namespace bot.Helpers
{
    public interface IBoardStateHelper
    {
        int GetAnteFromImage(Image image, string path);
        int GetBigBlindFromImage(Image image, string path);
        Card GetCardFromImage(Image img, string path);
        object GetGameIsFinished(Image image, string path);
        bool GetIsInPlay(Image image, string path);
        int GetNumberFromImage(Image image, string path);
        bool GetReadyForAction(Image image, string path);
        string GetWordFromImage(Image image, string path);
        void SaveBoardImages(List<BoardImage> boardImages, string path);
        void SetPlayerBets(BoardState state);
        void SetPlayerIsDealer(string path, BoardImage boardImage, BoardState state);
        void SetPlayerStack(string path, BoardImage boardImage, BoardState state);
        bool GetSittingOut(Image image, string boardImagepath);
    }
}