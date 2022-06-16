using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AVRIO
{
    public static class Log
    {
        private static Queue<string> qLogDataList = new Queue<string>();
        private static string strLog = null;
        private static int currentLogDataInQueue = 0;
        private static bool bLogEnable_Type0 = false;
        private static int CountDeletelog = 0;

        private static string eventlog;
        public static string Eventlog
        {
            get { return Log.eventlog; }
            set
            {
                Log.eventlog = value;
                SystemLog(value, 0);
              //  System.Console.WriteLine(value);
            }
        }

        public static void SystemLog(string data, int nType)       // 0:SystemLog, 1:CanLog
        {

            try
            {
                strLog = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " , " + data;
                qLogDataList.Enqueue(strLog);
                currentLogDataInQueue++;
            }
            catch
            {

            }
            try
            {
                if (currentLogDataInQueue == 0)
                {
                    return;
                }

                string logPath = logPath = @"D:\QuickChargeApp";
                string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".LOG";
                if (!logPath.EndsWith(@"\"))
                {
                    logPath += @"\";
                    logPath += @"Exception_LOG\";
                    int i = 0;
                    MakeLogPath(logPath);
                    if (bLogEnable_Type0 == true)
                        return;
                    bLogEnable_Type0 = true;

                    StreamWriter sw1 = new StreamWriter(logPath + logFile, true, System.Text.Encoding.UTF8);
                    try
                    {

                        bLogEnable_Type0 = true;
                        for (i = 0; i < currentLogDataInQueue; i++)
                        {
                            strLog = qLogDataList.Dequeue();
                            sw1.WriteLine(string.Format("{0}", strLog));
                            currentLogDataInQueue--;
                        }
                        bLogEnable_Type0 = false;
                        if (sw1 != null)
                            sw1.Close();
                    }
                    catch
                    {
                        if (sw1 != null)
                            sw1.Close();
                        bLogEnable_Type0 = false;
                    }
                }
                CountDeletelog++;
                if (CountDeletelog > 100)
                {
                    DeleteLogFile(logPath);
                    CountDeletelog = 0;
                }

            }
            catch
            {
                bLogEnable_Type0 = false;
            }

        }

        public static void MakeLogPath(string path)
        {
            try
            {
                // 디렉토리 체크
                string basePath = path.IndexOf(@"\\") > -1 ? path.Substring(0, path.IndexOf(@"\", 3)) : path.Substring(0, 3);
                string[] dirList = path.Substring(basePath.Length).Split('\\');
                int cnt = 0;
                for (int i = 0; i < dirList.Length; i++)
                {
                    if (dirList[i].Trim().Equals("")) continue;

                    if (!basePath.EndsWith(@"\")) basePath = basePath + @"\";
                    basePath = basePath + dirList[i];

                    if (path.IndexOf(@"\\") > -1 && cnt++ <= 0) continue;
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                }
            }
            catch { }
        }

        public static void DeleteLogFile(string strLogFilePath)
        {
            try
            {
                DateTime Now = DateTime.Now;

                DirectoryInfo LogDirectory = new DirectoryInfo(strLogFilePath);

                foreach (FileInfo FileName in LogDirectory.GetFiles("*.log"))
                {
                    string LogFile = FileName.Name;

                    string LogYearDelete = LogFile.Substring(0, 4);
                    string LogMonthDelete = LogFile.Substring(5, 2);
                    string LogDayDelete = LogFile.Substring(8, 2);

                    DateTime logDate = new DateTime(Convert.ToInt32(LogYearDelete), Convert.ToInt32(LogMonthDelete), Convert.ToInt32(LogDayDelete));
                    TimeSpan offset = Now - logDate;
                    int DeleteDay = offset.Days;

                    if (DeleteDay > 30)
                    {
                        FileName.Delete();
                    }
                }
            }
            catch { }
            return;
        }




    }

    public static class Log2
    {
        private static Queue<string> qLogDataList = new Queue<string>();
        private static string strLog = null;
        private static int currentLogDataInQueue = 0;
        private static bool bLogEnable_Type0 = false;
        private static int CountDeletelog = 0;

        private static string eventlog;
        public static string Eventlog
        {
            get { return Log2.eventlog; }
            set
            {
                Log2.eventlog = value;
                SystemLog(value, 0);
              //  System.Console.WriteLine(value);
            }
        }

        public static void SystemLog(string data, int nType)       // 0:SystemLog, 1:CanLog
        {

            try
            {
                strLog = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " , " + data;
                qLogDataList.Enqueue(strLog);
                currentLogDataInQueue++;
            }
            catch
            {

            }
            try
            {
                if (currentLogDataInQueue == 0)
                {
                    return;
                }

                string logPath = logPath = @"D:\QuickChargeApp";
                string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".LOG";
                if (!logPath.EndsWith(@"\"))
                {
                    logPath += @"\";
                    logPath += @"Touch_LOG\";
                    int i = 0;
                    MakeLogPath(logPath);
                    if (bLogEnable_Type0 == true)
                        return;
                    bLogEnable_Type0 = true;

                    StreamWriter sw1 = new StreamWriter(logPath + logFile, true, System.Text.Encoding.UTF8);
                    try
                    {

                        bLogEnable_Type0 = true;
                        for (i = 0; i < currentLogDataInQueue; i++)
                        {
                            strLog = qLogDataList.Dequeue();
                            sw1.WriteLine(string.Format("{0}", strLog));
                            currentLogDataInQueue--;
                        }
                        bLogEnable_Type0 = false;
                        if (sw1 != null)
                            sw1.Close();
                    }
                    catch
                    {
                        if (sw1 != null)
                            sw1.Close();
                        bLogEnable_Type0 = false;
                    }
                }
                CountDeletelog++;
                if (CountDeletelog > 100)
                {
                    DeleteLogFile(logPath);
                    CountDeletelog = 0;
                }

            }
            catch
            {
                bLogEnable_Type0 = false;
            }

        }

        public static void MakeLogPath(string path)
        {
            try
            {
                // 디렉토리 체크
                string basePath = path.IndexOf(@"\\") > -1 ? path.Substring(0, path.IndexOf(@"\", 3)) : path.Substring(0, 3);
                string[] dirList = path.Substring(basePath.Length).Split('\\');
                int cnt = 0;
                for (int i = 0; i < dirList.Length; i++)
                {
                    if (dirList[i].Trim().Equals("")) continue;

                    if (!basePath.EndsWith(@"\")) basePath = basePath + @"\";
                    basePath = basePath + dirList[i];

                    if (path.IndexOf(@"\\") > -1 && cnt++ <= 0) continue;
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                }
            }
            catch { }
        }

        public static void DeleteLogFile(string strLogFilePath)
        {
            try
            {
                DateTime Now = DateTime.Now;

                DirectoryInfo LogDirectory = new DirectoryInfo(strLogFilePath);

                foreach (FileInfo FileName in LogDirectory.GetFiles("*.log"))
                {
                    string LogFile = FileName.Name;

                    string LogYearDelete = LogFile.Substring(0, 4);
                    string LogMonthDelete = LogFile.Substring(5, 2);
                    string LogDayDelete = LogFile.Substring(8, 2);

                    DateTime logDate = new DateTime(Convert.ToInt32(LogYearDelete), Convert.ToInt32(LogMonthDelete), Convert.ToInt32(LogDayDelete));
                    TimeSpan offset = Now - logDate;
                    int DeleteDay = offset.Days;

                    if (DeleteDay > 30)
                    {
                        FileName.Delete();
                    }
                }
            }
            catch { }
            return;
        }




    }
}
