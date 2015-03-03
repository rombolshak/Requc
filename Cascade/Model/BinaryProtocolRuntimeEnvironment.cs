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

        public int StartPosition { get; set; }

        public int PositionsCount { get; set; }
    }
}
