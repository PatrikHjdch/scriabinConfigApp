using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    internal class Logger
    {
        FileStream logFileStream;
        StreamWriter logStreamWriter;

        public Logger(string name)
        {
            logFileStream = File.Create("logs\\Scriabin_" + name + "_log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            logStreamWriter = new StreamWriter(logFileStream);
        }
        public void Log(string message)
        {
            logStreamWriter.WriteLine($"{DateTime.Now}: {message}");
            logStreamWriter.Flush();
        }

        public void Dispose()
        {
            logStreamWriter.Close();
            logFileStream.Close();
        }
    }
}
