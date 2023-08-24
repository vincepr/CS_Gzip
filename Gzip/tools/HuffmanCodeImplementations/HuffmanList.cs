using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Gzip.Gzip.tools.HuffmanCodeImplementations
{
    /// <summary>
    /// builds a dictionary of the huffmantree.
    /// - these get send before code blocks that use this exact tree as encoding.
    /// - dictionary is used to look up what a bit-sequence (ex '011') corresponds to -> (ex 'C')
    /// - deflate requires these trees be of exact size 15.
    /// </summary>
    ///
    //       /\              Symbol    Code
    //      0  1             ------    ----
    //     /    \                A      00
    //    /\     B               B       1
    //   0  1                    C     011
    //  /    \                   D     010
    // A     /\
    //      0  1
    //     /    \
    //    D      C
    internal class HuffmanList : ICanonicalHuffmanCode
    {
        public static ICanonicalHuffmanCode NewHuff(in uint[] codeLengths)
        {
            return new HuffmanList(codeLengths);
        }
        
        private const int MaxCodeLength = 15;
        //private readonly Dictionary<uint, uint> _bitToSymbol = new Dictionary<uint, uint>(MaxCodeLength);
        private readonly List<uint> _codes;
        private readonly List<uint> _values;

        private HuffmanList(in uint[] codeLengths)
        {
            // check if params are of valid state:
            foreach (var l in codeLengths)
            {

                if (l < 0) throw new ArgumentOutOfRangeException("Negative code length");
                if (l > MaxCodeLength) throw new ArgumentOutOfRangeException("Maximum code length exceeded.");
            }
            _codes = new List<uint>(codeLengths.Length / 2);
            _values = new List<uint>(codeLengths.Length / 2);

            // build the map
            uint nextCode = 0;
            for (int codeLen = 1; codeLen <= MaxCodeLength; codeLen++)
            {
                nextCode = nextCode << 1;

                uint startBit = (uint)1 << codeLen;
                for (uint symbol = 0; symbol < codeLengths.Length; symbol++)
                {
                    if (codeLengths[symbol] != codeLen) continue;
                    if (nextCode >= startBit) throw new Exception("Canonical code produces illegal OVER-full Huffman-code-tree.");

                    _codes.Add(startBit | nextCode);
                    _values.Add(symbol);
                    nextCode++;
                }
            }
            if (nextCode != 1 << MaxCodeLength) throw new Exception("Canonical code produces illegal UNDER-full Huffman-code-tree.");
            _codes.TrimExcess();
            _values.TrimExcess();

            //if (_codes.Count > 200) dbgPrintOutHuffmanTree();
            //dbgPrintOutHuffmanTree();
        }

        /// <summary>
        /// Keep reading one bit at the time from right side
        /// - until match is found in the Map of Huffman-codes.
        /// - loop terminates after at most MaxCodeLength iterations.
        /// - because the maximum tree depth is 15 (!=size)
        /// </summary>
        /// <param name="input"></param>
        public uint DecodeNextSymbol(BitStream input)
        {
            uint codeBits = 1;
            for (int i = 0; i < MaxCodeLength; i++)
            {
                codeBits = codeBits << 1 | input.ReadUint(1);
                int idx = _codes.BinarySearch(codeBits);
                //int idx = Array.BinarySearch(_codes, codeBits);
                //Console.WriteLine($"searching {Convert.ToString(codeBits, 2)} == {codeBits}  \t->idx={idx}");
                if (idx >= 0) return _values[idx];
            }

            //for (int i = 0; i < _codes.Length; i++)
            //    Console.WriteLine($"[ {Convert.ToString(_codes[i], 2)} == {_codes[i]}]");

            throw new Exception("Unreachable! for");
        }

        public void dbgPrintOutHuffmanTree()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _codes.Count; i++)
            {
                builder.AppendLine($"Code: {Convert.ToString(_codes[i], 2)} \t\t={_codes[i]}\t-> Value: {_values[i]}");
            }
            Console.WriteLine(builder.ToString());
        }
    }
}
