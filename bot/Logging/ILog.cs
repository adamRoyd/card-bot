using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace bot.Logging
{
    public interface ILog
    {
        void WriteLine(string strMessage);
        void Flush();

        void WriteError(string strMessage, [CallerMemberName] string functionName = "", [CallerFilePath] string filePath = "", [CallerLineNumber]int lineNumber = -1);
    }
}
