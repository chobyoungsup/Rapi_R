using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Microsoft.Win32;

namespace QuickChargeConfig
{
    public static class ChargeConfig
    {
        static XElement xroot;
        static XElement xsubAVRID;

        static string checkMessage = "";

        public static void SetConfig(string element, string node, string value)
        {
            try
            {


                // reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey(element);

                RegistryKey regKey;
                RegistryKey reg;
                if (Environment.Is64BitOperatingSystem)
                {
                    regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\" + element, true);
                }
                else
                {
                    regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                    reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\" + element, true);
                }
                reg.SetValue(node, value);

            }catch(Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }

        /// <summary>
        /// 시간은 자동저장할것
        /// </summary>
        /// <param name="logname"></param>
        /// <param name="msg"></param>
        public static void ConfigLoad()
        {
            try
            { 
            xroot = XElement.Load("Config.xml");
            xsubAVRID = xroot.Element("AVRIO");
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }

        }
        public static void SetConfig1(string element, string node, string value)
        {
            try
            { 
            XElement root = XElement.Load(@"D:\Config.xml");
            XElement sub = root.Element(element);
            XElement val = sub.Element(node);

            if (val == null)
            {
                sub.Add(new XElement(node, value));
            }
            else
            {
                val.Value = value;
            }

            root.Save(@"D:\Config.xml");
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }

        public static string GetConfig1(string element, string node, string basevalue)
        {
            try
            {
                FileInfo fi = new FileInfo(@"D:\Config.xml");
                // AVRIO.avrio.EventMsg = "config.xml";
                if (fi.Exists)
                {
                    //AVRIO.avrio.EventMsg = "config.xml";

                    XElement root = XElement.Load(@"D:\Config.xml");
                    XElement sub = root.Element(element);
                    XElement val = sub.Element(node);
                    //  AVRIO.avrio.EventMsg = "config.xml";

                    if (val == null)
                    {
                        return basevalue;
                    }
                    else
                    {
                        return val.Value;
                    }
                }

                //존재하지 않으면 기본값을 반환
                return basevalue;
            }catch(Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
                return basevalue;
            }
        }

        public static void SetLog(string subdir, uint nSeqNum, string msg, DateTime dt)
        {

            if (subdir == "FAULT" && checkMessage == msg)
            {
                return;
            }

            DirectoryInfo dinfo = new DirectoryInfo(@"D:\QuickChargeApp");
           // DirectoryInfo dinfo = new @"D:\QuickChargeApp";
            string dname = dinfo.FullName + "\\" + subdir.ToUpper();

            DirectoryInfo subinfo = new DirectoryInfo(dname);

            // 디렉토리가 존재하지 않으면
            if (!subinfo.Exists)
            {
                subinfo.Create();
            }

            try
            {
               // string filename = subinfo.Name + "\\" + subdir.Substring(0, 1).ToUpper() + dt.Year.ToString("0000") + dt.Month.ToString("00") + dt.Day.ToString("00") + ".log";
                string filename = dinfo + "\\"+subinfo.Name + "\\" + subdir.Substring(0, 1).ToUpper() + dt.Year.ToString("0000") + dt.Month.ToString("00") + dt.Day.ToString("00") + ".log";

                FileStream file = new FileStream(filename, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(file);

                DeleteLogFile(dinfo.FullName + "\\" + subinfo.Name + "\\");
                sw.WriteLine(nSeqNum + "," + dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + msg);
                sw.Flush();
                file.Close();
                SetConfig("SEQ", subdir, nSeqNum.ToString());

                checkMessage = msg;
            }
            catch (Exception err)
            {
               // System.Diagnostics.Debug.WriteLine(e.Message);
                AVRIO.Log.Eventlog = err.ToString();
            }
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

                    string LogYearDelete = LogFile.Substring(1, 4);
                    string LogMonthDelete = LogFile.Substring(5, 2);
                    string LogDayDelete = LogFile.Substring(7, 2);

                    DateTime logDate = new DateTime(Convert.ToInt32(LogYearDelete), Convert.ToInt32(LogMonthDelete), Convert.ToInt32(LogDayDelete));
                    TimeSpan offset = Now - logDate;
                    int DeleteDay = offset.Days;

                    if (DeleteDay > 30)
                    {
                        FileName.Delete();
                    }
                }
            }catch(Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            return;
        }
    }
}
