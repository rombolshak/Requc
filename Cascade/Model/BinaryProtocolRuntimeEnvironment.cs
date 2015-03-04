using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cascade.Model
{
    public class BinaryProtocolRuntimeEnvironment
    {
        public ProtocolBlock WorkingBlock { get; set; }
        public ProtocolBlock SampleBlock { get; set; }

        public int StartPosition { get; set; }

        public int PositionsCount { get; set; }

        public int WorkingParity { get { return CalculateParity(WorkingBlock); } }

        public int SampleParity { get { return CalculateParity(SampleBlock); } }

        private int CalculateParity(ProtocolBlock block)
        {
            return block.KeyItems.Skip(StartPosition).Take(PositionsCount).Count(item => item.Value == 1) % 2;
        }
    }
}
