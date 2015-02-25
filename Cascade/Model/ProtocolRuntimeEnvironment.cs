using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cascade.Model
{
    public class ProtocolRuntimeEnvironment
    {
        public ProtocolRuntimeEnvironment()
        {
            KeyLength = 32;
            AliceKey = new List<KeyItem>(CreateKey());
            BobKey = CreateBobKey();

            BlockSize = new[] { 4, 8, 16, 32 };
            BlocksCount = new[] { 8, 4, 2, 1 };

            const int numberOfPasses = 4;
            AliceBlocks = new List<IEnumerable<ProtocolBlock>>(numberOfPasses);
            for (var i = 0; i < numberOfPasses; ++i)
            {
                AliceBlocks.Add(new List<ProtocolBlock>(BlocksCount[i]));
            }

            for (var p = 0; p < numberOfPasses; ++p)
            {
                var list = new List<ProtocolBlock>();
                for (var i = 0; i < BlocksCount[p]; ++i)
                {
                    list.Add(new ProtocolBlock
                        {
                            KeyItems = new ObservableCollection<KeyItem>(AliceKey.Skip(BlockSize[p]*i).Take(BlockSize[p]))
                        });
                }

                AliceBlocks[p] = list;
            }

            BobBlocks = new List<IEnumerable<ProtocolBlock>>(numberOfPasses);
            for (var i = 0; i < numberOfPasses; ++i)
            {
                BobBlocks.Add(new List<ProtocolBlock>(BlocksCount[i]));
            }

            for (var p = 0; p < numberOfPasses; ++p)
            {
                var list = new List<ProtocolBlock>();
                for (var i = 0; i < BlocksCount[p]; ++i)
                {
                    var bobKey = BobKey as KeyItem[] ?? BobKey.ToArray();
                    list.Add(new ProtocolBlock
                    {
                        KeyItems = new ObservableCollection<KeyItem>(bobKey.Skip(BlockSize[p] * i).Take(BlockSize[p]))
                    });
                }

                BobBlocks[p] = list;
            }
        }

        public int KeyLength { get; private set; }

        public IEnumerable<KeyItem> AliceKey { get; private set; }
        public IEnumerable<KeyItem> BobKey { get; private set; }

        public int[] BlockSize { get; private set; }
        public int[] BlocksCount { get; private set; }
        public IList<IEnumerable<ProtocolBlock>> AliceBlocks { get; private set; }
        public IList<IEnumerable<ProtocolBlock>> BobBlocks { get; private set; }

        private IEnumerable<KeyItem> CreateKey()
        {
            var list = new List<KeyItem>(KeyLength);
            var random = new Random();

            for (var i = 0; i < KeyLength; ++i)
            {
                list.Add(new KeyItem() { Position = i, Value = random.Next(0, 2) });
            }

            return list;
        }

        private IEnumerable<KeyItem> CreateBobKey()
        {
            var rnd = new Random();
            const int errorsCount = 7;

            var key = AliceKey.Select(item => new KeyItem() {Position = item.Position, Value = item.Value}).ToList();
            var errorPositions = new List<int>();
            while (errorPositions.Count < errorsCount)
            {
                var pos = rnd.Next(0, KeyLength);
                if (errorPositions.Contains(pos))
                {
                    continue;
                }

                var value = key[pos];
                key[pos].Value = value.Value == 0 ? 1 : 0;
                errorPositions.Add(pos);
            }

            return key;
        }
    }
}
