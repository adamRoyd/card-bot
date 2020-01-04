using Engine.Models;
using OCR.Objects;
using System;
using System.Collections.Generic;

namespace bot
{
    public class BoardStateHelper
    {
        public void SaveBoardImages(List<BoardImage> boardImages)
        {
            foreach(var boardImage in boardImages)
            {
                var boardImagePath = $"..\\..\\..\\images\\spliced\\{boardImage.Name}.png";
                boardImage.Image.Save(boardImagePath);
            }

        }
    }
}
