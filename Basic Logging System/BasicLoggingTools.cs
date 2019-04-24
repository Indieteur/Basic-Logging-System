using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indieteur.BasicLoggingSystem
{
    public static class BasicLoggingTools
    {
        public const string DEFAULT_LOG_SEARCH_PATTERN = "*.log";
        public enum LimitByEnum
        {
            Days,
            Count
        }
        public static void LimitLogsInFolder(string directoryPath, int number = 30, LimitByEnum limitBy = LimitByEnum.Days, string searchPattern = DEFAULT_LOG_SEARCH_PATTERN)
        {
            if (limitBy == LimitByEnum.Count)
                LimitLogsInFolderByCount(directoryPath, searchPattern, number);
            else
                LimitLogsInFolderByDays(directoryPath, searchPattern, number);
        }
        static void LimitLogsInFolderByDays(string directoryPath, string searchPattern, int days) //https://stackoverflow.com/questions/2222348/delete-files-older-than-3-months-old-in-a-directory-using-net
        {
            string[] filesInDir = Directory.GetFiles(directoryPath, searchPattern);
            foreach (string file in filesInDir)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.CreationTime < DateTime.Now.AddDays(-days))
                    fi.Delete();
            }
        }
        static void LimitLogsInFolderByCount(string directoryPath, string searchPattern, int count) 
        {
            string[] filesInDir = Directory.GetFiles(directoryPath, searchPattern);
            if (filesInDir.Length > count)
            {
                List<string> listOfFiles = new List<string>(filesInDir);
                while (listOfFiles.Count > count)
                {
                    int itemWithLowestDate = 0;
                    FileInfo fiWithLowestDate = new FileInfo(listOfFiles[0]);
                    for (int i = 0; i < listOfFiles.Count; i++)
                    {
                        string file = listOfFiles[i];
                        FileInfo fi = new FileInfo(file);
                        if (fi.CreationTime < fiWithLowestDate.CreationTime)
                        {
                            itemWithLowestDate = i;
                            fiWithLowestDate = fi;
                        }
                    }
                    fiWithLowestDate.Delete();
                    listOfFiles.RemoveAt(itemWithLowestDate);
                }
            }

        }
    }
}
