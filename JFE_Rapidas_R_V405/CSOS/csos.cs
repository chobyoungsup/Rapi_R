using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.IO;
using AVRIO;

namespace CSOS
{
    public struct version
    {
        public byte service;
        public byte major;
        public byte minor;
    };
    public class csos
    {
        private static Rapidas kdn;
       // private static SMARTRONet smt;

        private static ushort cpid;
        private static string serverip;
        private static string charger;
        private static int serverport;
        private static string myip;
        private static int port;
        private static version stVer;
        private static bool bClose;
        private static string company;
        private static string subRoot = "CSOS";
        private static Socket serverSock = null;
        private static Socket clientSock = null;
        private static UInt16 chargeCostReqTime = 0;

        public static UInt16 ChargeCostReqTime
        {
            get { return csos.chargeCostReqTime; }
            set { csos.chargeCostReqTime = value; }
        }

        public static Socket ClientSock
        {
            get { return csos.clientSock; }
            set { csos.clientSock = value; }
        }

        public static bool SwClose
        {
            get { return csos.bClose; }
            set { csos.bClose = value; }
        }

        public static Socket ServerSock
        {
            get { return csos.serverSock; }
            set { csos.serverSock = value; }
        }

        public static string Company
        {
            get { return csos.company; }
            set { csos.company = value; }
        }

        /// <summary>
        /// 서버인증키
        /// KDN은 CP마다 할당
        /// 세니온은 하나의 인증키를 공유함
        /// </summary>
        private static string authkey;

        /// <summary>
        /// 통신(protocol)버전
        /// </summary>
        private static string version = "0.6.0";
        public static string Version
        {
            get { return csos.version; }
            set { csos.version = value; }
        }

        #region 생성자와 쓰레드
        public csos()
        {
        }

        public static void ThreadRun()
        {
            kdn.PollFunction();
        }
        #endregion

        #region 명령전송
        public void SendCommand(CSOSCMD cmd)
        {
            kdn.SendCSOSCommand(cmd);
        }
        #endregion

        #region 기본설정
        public static ushort Cpid
        {
            get { return csos.cpid; }
            set { csos.cpid = value; }
        }

        public static string Charger
        {
            get { return csos.charger; }
            set { csos.charger = value; }
        }

        public static string Serverip
        {
            get { return csos.serverip; }
            set { csos.serverip = value; }
        }

        public static int Serverport
        {
            get { return csos.serverport; }
            set { csos.serverport = value; }
        }

        public static string Ip
        {
            get { return csos.myip; }
            set { csos.myip = value; }
        }

        public static int Port
        {
            get { return csos.port; }
            set { csos.port = value; }
        }

        public static void SetVersion(int service, int major, int minor)
        {
            stVer.service = (byte)service;
            stVer.major = (byte)major;
            stVer.minor = (byte)minor;
        }

        public static string Authkey
        {
            get { return csos.authkey; }
            set { csos.authkey = value; }
        }

        #endregion

    }
}
