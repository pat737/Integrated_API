﻿using System;
using System.IO;

namespace APIProject
{
    public class Helper // this class is from our labs samples and it's tool to help log any activities such as errors and save them in a text file
    {
        public void CreateLog(string message)
        {
            try
            {
                string log_file_patch = "mylog.txt";

                
                using (StreamWriter sw = File.AppendText(log_file_patch))
                {
                    
                    string time_stamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sw.WriteLine(time_stamp + " - " + message);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Logging failed: " + ex.Message);
            }
        }
    }
}
