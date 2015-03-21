using System.Collections.Generic;
using System.Linq;

namespace Cascade.Model
{
    public class ProtocolBlock
    {
        public IList<KeyItem> KeyItems { get; set; }
        public int Size { get { return KeyItems.Count(); } }

        public int Parity
        {
            get { return KeyItems.Count(item => item.Value == 1)%2; }
        }
    }
}
