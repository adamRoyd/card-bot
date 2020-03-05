using Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace bot.Services
{
    public interface IBoardStateService
    {
        BoardState GetBoardStateFromImagePath(string path);
    }
}
