using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requc.Models
{
    public class ProtocolParams
    {
        public ProtocolParams(int laserPhotonNumberMin, int laserPhotonNumberMax, double phase0, double phase1)
        {
            Phase1 = phase1;
            Phase0 = phase0;
            LaserPhotonNumberMax = laserPhotonNumberMax;
            LaserPhotonNumberMin = laserPhotonNumberMin;
        }

        public int LaserPhotonNumberMin { get; private set; }
        public int LaserPhotonNumberMax { get; private set; }
        public double Phase0 { get; private set; }
        public double Phase1 { get; private set; }
    }
}
