using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Gzip.Gzip.tools
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
    internal class CanonicalHuffmanCodeSortedList
    {
        private const int MaxCodeLength = 15;
        //private readonly Dictionary<uint, uint> _bitToSymbol = new Dictionary<uint, uint>(MaxCodeLength);
        private readonly SortedList<uint, uint> _codes;

        public CanonicalHuffmanCodeSortedList(in uint[] codeLengths)
        {
            // check if params are of valid state:
            foreach (var l in codeLengths)
            {

                if (l < 0) throw new ArgumentOutOfRangeException("Negative code length");
                if (l > MaxCodeLength) throw new ArgumentOutOfRangeException("Maximum code length exceeded.");
            }
            _codes = new SortedList<uint, uint>(codeLengths.Length/2);

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

                    _codes.Add( startBit | nextCode, symbol );
                    nextCode++;
                }
            }
            if (nextCode != 1 << MaxCodeLength) throw new Exception("Canonical code produces illegal UNDER-full Huffman-code-tree.");
            _codes.TrimExcess();

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

                bool exists = _codes.TryGetValue(codeBits, out uint value);
                if (exists) return value;
                //int idx = Array.BinarySearch(_codes, codeBits);
                //Console.WriteLine($"searching {Convert.ToString(codeBits, 2)} == {codeBits}  \t->idx={idx}");
            }

            //for (int i = 0; i < _codes.Length; i++)
            //    Console.WriteLine($"[ {Convert.ToString(_codes[i], 2)} == {_codes[i]}]");

            throw new Exception("Unreachable! for");
        }
    }
}
