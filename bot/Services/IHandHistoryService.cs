using System;
using System.Collections.Generic;
using System.Text;

namespace bot.Services
{
    public interface IHandHistoryService
    {
        void GetLatestHistory(string path);
    }
}
