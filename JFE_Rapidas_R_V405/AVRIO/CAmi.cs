using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVRIO
{
    class CAmi
    {
        private ushort readRegisters = 44;

        private static long l유효전력량 = 0;
        private static float f유효전력 = 0;

        public static float F유효전력량
        {
            get { return CAmi.f유효전력; }
            set { CAmi.f유효전력 = value; }
        }

        public static long L유효전력량
        {
            get { return CAmi.l유효전력량; }
            set { CAmi.l유효전력량 = value; }
        }

        public ushort ReadRegisters
        {
            get { return readRegisters; }
            set { readRegisters = value; }
        }

        public enum READADDRESS
        {
            유효전력량 = 30001,
            무효전력량 = 30003,
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
