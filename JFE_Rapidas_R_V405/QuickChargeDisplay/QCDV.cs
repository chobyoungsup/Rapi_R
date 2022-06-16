using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickChargeDisplay
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="address"></param>
    /// <param name="offset"></param>
    /// <param name="value"></param>
    /// <param name="type">1:bit, 2:long, 3:short, 4:int, 5:double</param>
    public delegate void AdminValueChangedEvent(int address, int offset, object value, int type);

    /// <summary>
    /// Quick Charge Display Variables
    /// </summary>
    public static class QCDV
    {
        #region 결제화면 변수

        /// <summary>
        /// 충전금액 화면 디스플레이 변수
        /// 03_ChargeMoneyPage.xaml
        /// </summary>
        private static ChargeMoney chargeMoney = new ChargeMoney();
        public static ChargeMoney ChargeMoney
        {
            get { return QCDV.chargeMoney; }
            set { QCDV.chargeMoney = value; }
        }
        
        /// <summary>
        /// 충전중 화면 디스플레이 변수
        /// 04_ChargingPage.xaml
        /// </summary>
        private static Charging charging = new Charging();
        public static Charging Charging
        {
            get { return QCDV.charging; }
            set { QCDV.charging = value; }
        }
        
        /// <summary>
        /// 결제확인 디스플레이 변수
        /// 06_ConfirmChargePage.xaml
        /// </summary>
        private static ConfirmCharge confirmCharge = new ConfirmCharge();
        public static ConfirmCharge ConfirmCharge
        {
            get { return QCDV.confirmCharge; }
            set { QCDV.confirmCharge = value; }
        }

        /// <summary>
        /// 충전완료 디스플레이 변수
        /// 07_CompleteChargePage.xaml
        /// </summary>
        private static CompleteCharge completeCharge = new CompleteCharge();
        public static CompleteCharge CompleteCharge
        {
            get { return QCDV.completeCharge; }
            set { QCDV.completeCharge = value; }
        }

        #endregion

        #region 관리자 화면 변수

        public static event AdminValueChangedEvent AdminValueChanged;

        public static void OnAdminValueChanged(int address, int offset, object value, int type)
        {
            if (AdminValueChanged != null)
                AdminValueChanged(address, offset, value, type);
        }

        /// <summary>
        /// 충전내역 디스플레이 변수
        /// 09_6_ChargeHistoryPage.xaml
        /// </summary>
        private static ChargeHistory chargeHistory = new ChargeHistory();
        public static ChargeHistory ChargeHistory
        {
            get { return QCDV.chargeHistory; }
            set { QCDV.chargeHistory = value; }
        }

        /// <summary>
        /// 고장내역 디스플레이 변수
        /// 09_7_TroubleHistoryPage.xaml
        /// </summary>
        private static TroubleHistory troubleHistory = new TroubleHistory();
        public static TroubleHistory TroubleHistory
        {
            get { return QCDV.troubleHistory; }
            set { QCDV.troubleHistory = value; }
        }

        /// <summary>
        /// 수동충전 디스플레이 변수
        /// _10_10_강제충전.xaml
        /// </summary>
        private static ManualCotrol manualControl = new ManualCotrol();
        public static ManualCotrol ManualControl
        {
            get { return QCDV.manualControl; }
            set { QCDV.manualControl = value; }
        }
        private static CorrectSetup correctSetup = new CorrectSetup();
        public static CorrectSetup CorrectSetup
        {
            get { return QCDV.correctSetup; }
            set { QCDV.correctSetup = value; }
        }

        private static InnerSetup innerSetup = new InnerSetup();
        public static InnerSetup InnerSetup
        {
            get { return QCDV.innerSetup; }
            set { QCDV.innerSetup = value; }
        }
        /// <summary>
        /// 기기설정 디스플레이 변수
        /// 10_3_SetupEquipmentPage.xaml
        /// </summary>
        private static SetupEquipment setupEquipment = new SetupEquipment();
        public static SetupEquipment SetupEquipment
        {
            get { return QCDV.setupEquipment; }
            set { QCDV.setupEquipment = value; }
        }
        #endregion

        #region BMS정보
        private static BmsInfo bmsInfo = new BmsInfo();
        public static BmsInfo BmsInfo
        {
            get { return QCDV.bmsInfo; }
            set { QCDV.bmsInfo = value; }
        }
        #endregion

        #region DSP교정

        private static DSPCalibration dSPCalibration = new DSPCalibration();
        public static DSPCalibration DSPCalibration
        {
            get { return QCDV.dSPCalibration; }
            set { QCDV.dSPCalibration = value; }
        }

        #endregion
    }
}
