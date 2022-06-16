using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace QuickChargeDisplay
{
    public static class PageManager
    {
        private static Dictionary<int,Page> pages;

        static PageManager()
        {
            pages = new Dictionary<int, Page>();

            pages.Add((int)PageId._00_멈춤화면, new StopPage2());                   //2
            pages.Add((int)PageId._01_대기화면, new StandByPage1());                //1
            pages.Add((int)PageId._02_카드체크, new CardCheckStartPage3());         //3
            pages.Add((int)PageId._02_4_카드에러, new CardErrorStartPage4());       //4
            pages.Add((int)PageId._02_5_카드에러, new CardErrorStartPage5());       //5
            pages.Add((int)PageId._02_6_카드에러, new CardErrorStartPage6());       //6
            pages.Add((int)PageId._02_7_시작패스워드, new ChargePassPage());        //7
            pages.Add((int)PageId._03_커넥터연결, new ConnectorPage7());            //8
            pages.Add((int)PageId._03_접속확인중, new ChargeStartPage());           //9
            pages.Add((int)PageId._05_0_절연저항측정화면, new IsolationPage());     //10
            pages.Add((int)PageId._05_1_충전중화면, new ChargingPage());            //11
            pages.Add((int)PageId._05_2_충전종료처리중, new ChargeStopPage());        //12
            pages.Add((int)PageId._06_커넥터분리, new SeparateConnectorPage());     //13
            pages.Add((int)PageId._07_결재확인, new ConfirmChargePage());           //14
            pages.Add((int)PageId._08_충전완료, new CompleteChargePage());          //15 
            pages.Add((int)PageId._09_패스워드입력, new PasswordPage());        
            pages.Add((int)PageId._10_0_관리자메뉴, new AdminMenuPage());
            pages.Add((int)PageId._10_1_관리설정, new ManagementPage());
            pages.Add((int)PageId._10_2_수전전력설정, new SetupPowerPage());
            pages.Add((int)PageId._10_3_일시설정, new SetupMomentlPage());
            pages.Add((int)PageId._10_4_언어설정, new SetupLanguagePage());
            pages.Add((int)PageId._10_5_패쓰워드, new SetupPasswordPage());
            pages.Add((int)PageId._10_6_충전이력, new ChargeHistoryPage());
            pages.Add((int)PageId._10_7_에러이력, new TroubleHistoryPage());
            pages.Add((int)PageId._10_8_Log추출, new LogSamplingPage());
            pages.Add((int)PageId._10_9_최대전력설정, new SetupMaxiumCurrentPage());
            pages.Add((int)PageId._10_10_강제충전, new CompulsionChargePage());
            pages.Add((int)PageId._09_10_전압설정, new _09_10_SetupVoltagePage());
        }

        public static Page GetPage(PageId id)
        {
            int index = (int)id;

            if (index < pages.Count)
                return pages[index];

            return pages[0];
        }
    }
}
