using System;
using System.Collections.Generic;
using System.Linq;

namespace Cascade.Model
{
    public class CascadeProtocolRuntimeEnvironment
    {
        public CascadeProtocolRuntimeEnvironment()
        {
            KeyLength = 32;
            AliceKey = new List<KeyItem>(CreateKey());
            BobKey = CreateBobKey();

            BlockSize = new[] { 4, 8, 16, 32 };
            BlocksCount = new[] { 8, 4, 2, 1 };

            AliceBlocks = new List<IList<ProtocolBlock>>();
            BobBlocks = new List<IList<ProtocolBlock>>();
            FillBlocksWithRandomPermutations(new[] {AliceBlocks, BobBlocks}, new[] {AliceKey, BobKey});

            OddErrorsCountBlocks = new List<ProtocolBlock>();
            BinaryEnvironment = new BinaryProtocolRuntimeEnvironment();
        }

        public int KeyLength { get; private set; }

        public IList<KeyItem> AliceKey { get; private set; }
        public IList<KeyItem> BobKey { get; private set; }

        public int[] BlockSize { get; private set; }
        public int[] BlocksCount { get; private set; }
        public IList<IList<ProtocolBlock>> AliceBlocks { get; private set; }
        public IList<IList<ProtocolBlock>> BobBlocks { get; private set; }

        public IList<ProtocolBlock> OddErrorsCountBlocks { get; set; }

        public BinaryProtocolRuntimeEnvironment BinaryEnvironment { get; set; }

        public int Pass { get; set; }

        private IEnumerable<KeyItem> CreateKey()
        {
            var list = new List<KeyItem>(KeyLength);
            var random = new Random();

            for (var i = 0; i < KeyLength; ++i)
            {
                list.Add(new KeyItem { Position = i, Value = random.Next(0, 2) });
            }

            return list;
        }

        private IList<KeyItem> CreateBobKey()
        {
            var rnd = new Random();
            const int errorsCount = 7;

            var key = AliceKey.Select(item => new KeyItem {Position = item.Position, Value = item.Value}).ToList();
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

        private void FillBlocksWithRandomPermutations(IList<IList<ProtocolBlock>>[] blocksArray,
                                                      IList<IEnumerable<KeyItem>> keysArray)
        {
            const int numberOfPasses = 4;
            var rng = new Random();
            for (var pass = 0; pass < numberOfPasses; ++pass)
            {
                var indexes = Enumerable.Range(0, KeyLength).ToList();
                indexes.Shuffle(rng);

                var blockSize = BlockSize[pass];
                var protocolBlocksArray = new List<ProtocolBlock>[blocksArray.Count()];
                for (var i = 0; i < protocolBlocksArray.Length; ++i)
                {
                    protocolBlocksArray[i] = new List<ProtocolBlock>();
                }

                for (var b = 0; b < BlocksCount[pass]; ++b)
                {
                    var keyIndexes = indexes.Skip(blockSize*b).Take(blockSize).ToList();
                    for (var i = 0; i < protocolBlocksArray.Length; ++i)
                    {
                        var items = keyIndexes.Select(index => keysArray[i].Single(item => item.Position == index)).ToList();
                        protocolBlocksArray[i].Add(new ProtocolBlock
                            {
                                KeyItems = items
                            });
                    }
                }

                for (var i = 0; i < protocolBlocksArray.Length; ++i)
                {
                    blocksArray[i].Add(protocolBlocksArray[i]);
                }
            }
        }
    }
}
