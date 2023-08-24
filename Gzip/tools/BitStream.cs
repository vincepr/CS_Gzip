﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Gzip.Gzip.tools
{
    /// <summary>
    /// Bit input stream, wraps around a normal stream but instead of bytes it can read single bits.
    ///
    /// - can read either starting from left or right. (bool BigEndian)
    /// </summary>
    internal class BitStream
    {
        public BitStream(Stream stream)
        {
            _stream = stream;
            _nextIdx = 8;   // state for freshly starting a bit next time ReadBit() is called
        }

        private Stream _stream;
        private int _nextIdx;
        private byte _currentByte;

        /// <summary>
        /// Reads one BIT (not byte) null when empty
        /// 
        /// - it does so by reading a byte-sized chunk and indexing over it
        /// </summary>
        /// <param name="bigEndian"> LSB vs MSB wise reading of bits</param>
        /// <returns></returns>
        private bool? ReadBit(bool bigEndian = false)
        {
            if (_nextIdx == 8)
            {
                int r = _stream.ReadByte();
                if (r == -1) return null; // end of stream
                _nextIdx = 0;
                _currentByte = (byte)r;
            }
            bool result;
            if (!bigEndian) result = (_currentByte & 1 << _nextIdx) > 0;
            else result = (_currentByte & 1 << 7 - _nextIdx) > 0;
            _nextIdx++;
            return result;
        }

        /// <summary>
        /// reads numBits amount of bits and packs them into an uint 
        /// </summary>
        /// <param name="numBits"></param>
        public uint ReadUint(uint numBits)
        {
            if (numBits < 0 || numBits > 32)    // we assume 32bit here for now
                throw new InvalidDataException("Number of bits out of range.");

            uint result = 0;
            for (int i = 0; i < numBits; i++)
            {
                bool? bit = ReadBit();
                if (bit is null) throw new InvalidDataException("Number of bits out of range");
                if ((bool)bit) result |= (uint)1 << i;
                //else result |= (uint)0 << i;    // we can skipp since we init it as row of 0s i guess
            }
            return result;
        }

        /// <summary>
        /// disregards bits to align freshly with byte boundary.
        /// </summary>
        public void AlignToByteBoundary()
        {
            while (_nextIdx != 8)
            {
                _ = ReadUint(1);
            }
        }
    }
}
