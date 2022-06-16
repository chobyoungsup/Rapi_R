using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVRIO
{
    public class amiio
    {
        private ushort readRegisters = 44;
        private ushort totalRegisters = 44;

        private static uint l유효전력량 = 0;
        private static float f유효전력 = 0;

        private static uint 유효전력량 = 0;
        private static uint 무효전력량 = 0;
        private static float A상전압 = 0;
        private static float B상전압 = 0;
        private static float C상전압 = 0;
        private static float A상전류 = 0;
        private static float B상전류 = 0;
        private static float C상전류 = 0;
        private static float A상전압전류위상각 = 0;
        private static float B상전압전류위상각 = 0;
        private static float C상전압전류위상각 = 0;
        private static float A상역률 = 0;
        private static float B상역률 = 0;
        private static float C상역률 = 0;
        private static float 전체유효전력 = 0;
        private static float 전체무효전력 = 0;
        private static float A상유효전력 = 0;
        private static float B상유효전력 = 0;
        private static float C상유효전력 = 0;
        private static float A상무효전력 = 0;
        private static float B상무효전력 = 0;
        private static float C상무효전력 = 0;

        private static uint vaildWatt = 0;

        public static uint VaildWatt
        {
            get { return amiio.vaildWatt; }
            set { amiio.vaildWatt = value; }
        }
        
        public uint[] readLongData = new uint[2];
        public float[] readFloatData = new float[20];

        public static float F유효전력량
        {
            get { return amiio.f유효전력; }
            set { amiio.f유효전력 = value; }
        }

        public static uint L유효전력량
        {
            get { return amiio.l유효전력량; }
            set { amiio.l유효전력량 = value; }
        }

        public ushort ReadRegisters
        {
            get { return readRegisters; }
            set { readRegisters = value; }
        }

        public enum READADDRESS
        {
            Start = 30001,
            유효전력량 = 30001,
            무효전력량 = 30003,
            StartFloat = 30005,
            A상전압 = 30005,
            B상전압 = 30007,
            C상전압 = 30009,
            A상전류 = 30011,
            B상전류 = 30013,
            C상전류 = 30015,
            A상전압전류위상각 = 30017,
            B상전압전류위상각 = 30019,
            C상전압전류위상각 = 30021,
            A상역률 = 30023,
            B상역률 = 30025,
            C상역률 = 30027,
            전체유효전력 = 30029,
            전체무효전력 = 30031,
            A상유효전력 = 30033,
            B상유효전력 = 30035,
            C상유효전력 = 30037,
            A상무효전력 = 30039,
            B상무효전력 = 30041,
            C상무효전력 = 30043,
        };

    }
}
