using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Helpers;

namespace Requc.Models
{
    public class TransmissionAct
    {
        public TransmissionAct(TransmissionActScheme scheme)
        {
            _scheme = scheme;
        }

        public void Run()
        {
            foreach (var column in _scheme.Columns)
            {
                column.Process();
            }
        }

        private readonly TransmissionActScheme _scheme;
    }
}
