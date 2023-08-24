using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS_Gzip.Gzip.tools;
using CS_Gzip.Gzip.tools.HuffmanCodeImplementations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;

namespace CS_Gzip.Gzip.Deflate
{
    /// <summary>
    /// This is the decompression-process of the Deflate data (previously stripped of by its gzip-headers previously)
    /// </summary>
    internal class Decompressor
    {
        // constants these are static -> only get calculated once. (represent a 'default'-kind of huffmantree)
        private static CanonicalHuffmanCodeArray FIXED_LENGTH_CODE = makeFixedLenCode();
        private static CanonicalHuffmanCodeArray FIXED_DIST_CODE = makeFixedDistCode();

        private const int SizeOfHistoryInBytes = 32 * 1024;

        private BitStream _input;
        private Stream _output;
        private ByteHistory _history;
        private ContinousHashingCrc32 _crc;

        /// <summary>
        /// starts the decompression process for data in the input stream and writes it to the output stream.
        /// - returns the crc32 checksum of all the bytes written
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static byte[] Decompress(BitStream input, Stream output)
        {
            var d = new Decompressor(input, output);
            return d._crc.CollectCrc32();
        }

        /// <summary>
        /// Constructor, that immediately starts the decompression.
        /// </summary>
        private Decompressor(BitStream input, Stream output)
        {

            _input = input;
            _output = output;
            _history = new ByteHistory(SizeOfHistoryInBytes);
            _crc = new ContinousHashingCrc32();

            // Process of decompression:
            bool isFinal;

            do
            {
                // Header Block
                isFinal = input.ReadUint(1) != 0;       // BFINAL
                uint bType = input.ReadUint(2);          // BTYPE
                // decompress rest of the block depending on type
                if (bType == 0)
                {
                    decompressUncompressedBlock();
                }
                else if (bType == 1)
                {
                    decompressHuffmanBlock(FIXED_LENGTH_CODE, FIXED_DIST_CODE);
                }
                else if (bType == 2)
                {
                    var (huffLenCode, huffDistCode) = readHuffmanCodes();
                    decompressHuffmanBlock(huffLenCode, huffDistCode);
                }
                else if (bType == 3)
                {
                    throw new InvalidDataException("Reserved block type.");
                }
                else throw new InvalidDataException("Unreachable");
            } while (!isFinal);

        }

        private void decompressUncompressedBlock()
        {
            // discard bits to align to clean byte boundary
            _input.AlignToByteBoundary();

            // read length of block:
            uint len = _input.ReadUint(16);
            uint nlen = _input.ReadUint(16);
            if ((len ^ 0xFFFF) != nlen)
                throw new InvalidDataException("Invalid lengh of uncompressed data block");
            // copy bytes to stream directly
            for (int i = 0; i < len; i++)
            {
                byte b = (byte)_input.ReadUint(8);
                _output.WriteByte(b);
                _history.append(b);
                _crc.NextByte(b);
            }
        }

        private void decompressHuffmanBlock(CanonicalHuffmanCodeArray lenCode, CanonicalHuffmanCodeArray? distCode)
        {
            uint sym = lenCode.DecodeNextSymbol(_input);
            while (sym != 256)
            {
                
                if (sym == 256) break;  // reached end of this block
                if (sym < 256)
                {
                    // literal byte
                    _output.WriteByte((byte)sym);
                    _history.append((byte)sym);
                    _crc.NextByte((byte)sym);
                }
                else
                {
                    // length and distance used for copying
                    uint run = decodeRunLength(sym);
                    if (!(3 <= run && run <= 258)) throw new Exception("Invalid run length");
                    if (distCode is null) throw new Exception("Length symbol encountered with empty distance code.");
                    uint distSym = distCode.DecodeNextSymbol(_input);
                    uint dist = decodeDistance(distSym);
                    if (!(1 <= dist && dist <= 32768)) throw new Exception("Invalid distance");
                    _history.copy(dist, run, _output, _crc);

                }
                sym = lenCode.DecodeNextSymbol(_input);
            }
        }

        private uint decodeRunLength(uint sym)
        {
            if (257 > sym || sym > 287)
                throw new ArgumentOutOfRangeException(nameof(sym), "Invalid run length symbol");
            if (sym <= 264) return sym - 254;
            else if (sym <= 284)
            {
                uint numExtraBits = (sym - 261) / 4;
                return ((sym - 265) % 4 + 4 << (int)numExtraBits) + 3 + _input.ReadUint(numExtraBits);
            }
            else if (sym == 285) return 258;
            else throw new InvalidDataException("Reserved length symbol: " + sym);
        }

        /// <summary>
        /// reads bits from the input stream to build the huffman-code that will be used for the following block
        /// </summary>
        /// <returns></returns>
        private (CanonicalHuffmanCodeArray lenCode, CanonicalHuffmanCodeArray? distCode) readHuffmanCodes()
        {
            uint numLenCodes = _input.ReadUint(5) + 257;   // hlit + 257
            uint numDisCodes = _input.ReadUint(5) + 1;     // hdist + 1

            // read length the huffman-code takes in the stream
            uint numCodeLenCodes = _input.ReadUint(4) + 4; // hclen + 4
            List<uint> codeLenCodeLen = new();
            // fill in fixed values:
            for (int i = 0; i < 19; i++)
                codeLenCodeLen.Add(0);
            codeLenCodeLen[16] = _input.ReadUint(3);
            codeLenCodeLen[17] = _input.ReadUint(3);
            codeLenCodeLen[18] = _input.ReadUint(3);
            codeLenCodeLen[0] = _input.ReadUint(3);
            for (int i = 0; i < numCodeLenCodes - 4; i++)
            {
                int j = i % 2 == 0 ? 8 + i / 2 : 7 - i / 2;
                codeLenCodeLen[j] = _input.ReadUint(3);
            }
            var codeLenCode = new CanonicalHuffmanCodeArray(codeLenCodeLen.ToArray());

            // Read the main code lengths
            uint[] codeLens = new uint[numLenCodes + numDisCodes];
            for (uint codeLensIdx = 0; codeLensIdx < codeLens.Length;)
            {
                uint sym = codeLenCode.DecodeNextSymbol(_input);
                if (0 <= sym && sym <= 15)
                {
                    codeLens[codeLensIdx] = sym;
                    codeLensIdx++;
                }
                else
                {
                    uint runLen;
                    uint runValue = 0;
                    if (sym == 16)
                    {
                        if (codeLensIdx == 0)
                            throw new FormatException("No code length value to copy.");
                        runLen = _input.ReadUint(2) + 3;
                        runValue = codeLens[codeLensIdx - 1];
                    }
                    else if (sym == 17)
                    {
                        runLen = _input.ReadUint(3) + 3;
                    }
                    else if (sym == 18)
                    {
                        runLen = _input.ReadUint(7) + 11;
                    }
                    else throw new InvalidDataException("Symbol out allowed of range.");
                    uint end = codeLensIdx + runLen;
                    if (end > codeLens.Length)
                        throw new InvalidDataException("Run exceeds numer of codes.");
                    Array.Fill(codeLens, runValue, (int)codeLensIdx, (int)(end - codeLensIdx));
                    codeLensIdx = end;
                }
            }

            // create literal-length-code-tree
            uint[] lenCodeLen = codeLens[0..(int)numLenCodes];
            if (lenCodeLen[256] == 0)
                throw new InvalidDataException("End of block symbol has zero code length.");
            CanonicalHuffmanCodeArray huffLenCode = new CanonicalHuffmanCodeArray(lenCodeLen);

            //create distance-code-tree
            uint[] distCodesLen = codeLens[(int)numLenCodes..codeLens.Length];
            CanonicalHuffmanCodeArray? huffDistCode;
            if (distCodesLen.Length == 1 && distCodesLen[0] == 0)
                huffDistCode = null;    // no distance code -> the block will be all literal symbols
            else
            {
                // build statistics
                uint oneCount = 0;
                uint otherPositiveCOunt = 0;
                foreach (var x in distCodesLen)
                {
                    if (x == 1) oneCount++;
                    else if (x > 1) otherPositiveCOunt++;
                }
                // handle case: only one distance code is defined
                if (oneCount == 1 && otherPositiveCOunt == 0)
                {
                    // we need to fill dummy data in to make a complete huffman-tree (tree MUST always be valid)
                    // since uint initializes with 0 we can just leave all but the last in place
                    distCodesLen[31] = 1;
                }
                huffDistCode = new CanonicalHuffmanCodeArray(distCodesLen);
            }

            return (huffLenCode, huffDistCode);
        }

        private uint decodeDistance(uint sym)
        {
            if (!(0 <= sym && sym <= 31))
                throw new ArgumentOutOfRangeException(nameof(sym), "Invalid run length symbol");
            if (sym <= 3) return sym + 1;
            else if (sym <= 29)
            {
                uint numExtraBits = sym / 2 - 1;
                return (sym % 2 + 2 << (int)numExtraBits) + 1 + _input.ReadUint(numExtraBits);
            }
            else throw new InvalidDataException("Reserved length symbol: " + sym);
        }


        // building the huffman codes depending on BTYPE
        // BTYPE==1 -> static values
        private static CanonicalHuffmanCodeArray makeFixedLenCode()
        {
            List<uint> codeLengths = new List<uint>(288);
            for (uint i = 0; i < 144; i++) codeLengths.Add(8);
            for (uint i = 0; i < 112; i++) codeLengths.Add(9);
            for (uint i = 0; i < 24; i++) codeLengths.Add(7);
            for (uint i = 0; i < 8; i++) codeLengths.Add(8);
            return new CanonicalHuffmanCodeArray(codeLengths.ToArray());
        }

        private static CanonicalHuffmanCodeArray makeFixedDistCode()
        {
            List<uint> codeLengths = new List<uint>(32);
            for (uint i = 0; i < 32; i++) codeLengths.Add(5);
            return new CanonicalHuffmanCodeArray(codeLengths.ToArray());
        }
    }
}
