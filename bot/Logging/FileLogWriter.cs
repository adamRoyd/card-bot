using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace bot.Logging
{
    public class FileLogWriter : ILog
    {
        private string Filename { get; set; }

        public FileLogWriter(string strPath)
        {
            Filename = $@"{strPath}\\log.txt";
        }

        public void WriteLine(string strMessage)
        {
            WriteLine(strMessage, "", "", -1);
        }

        private void WriteLine(string strMessage, string strFunctionName = "", string strFileName = "",
            int intLineNumber = -1, string strLogType="")
        {
            using (StreamWriter sw = File.AppendText(Filename))
            {
                LogMessage(strMessage, sw, strLogType);
            }
        }

        private void LogMessage(string strMessage, TextWriter streamWriter, string strLogType)
        {
            string strInfoLine;
            string strDebugInfo;

            strDebugInfo = string.Empty;


            strInfoLine = $"{strLogType}{strMessage}{strDebugInfo}";
            Console.WriteLine(strInfoLine);
            streamWriter.WriteLine(strInfoLine);
        }

        public void Flush()
        {
            // Nothing to do here
        }

        public void WriteError(string strMessage, [CallerMemberName] string functionName = "", [CallerFilePath] string filePath = "", [CallerLineNumber]int lineNumber=-1)
        {
            WriteLine(strMessage, functionName, filePath, lineNumber, "ERROR");
        }
    }
}
