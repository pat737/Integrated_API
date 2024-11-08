using System;
using System.IO;

namespace APIProject
{
    public class Helper
    {
        public void CreateLog(string Message)
        {
            try
            {
                string logFilePath = "mylog.txt";

                // create or append to the log file
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    // write the timestamp and log message to the file
                    string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sw.WriteLine(timeStamp + " - " + Message);
                }
            }
            catch (Exception ex)
            {
                // logging failed
                Console.WriteLine("Logging failed: " + ex.Message);
            }
        }
    }
}
