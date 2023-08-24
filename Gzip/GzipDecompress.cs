using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using File = System.IO.File;
using System.Buffers.Binary;
using System.Security.Cryptography;
using CS_Gzip.Gzip.Deflate;
using CS_Gzip.Gzip.tools;

namespace CS_Gzip.Gzip;

/// <summary>
/// usage is GzipDecompress.GzipRun(cmdLineArgs);
/// outputs string with error-/success-info
///
/// Along the DEFLATE Compressed Data Format Specification version 1.3
/// as described in **RFC 1951**
/// </summary>
internal class GzipDecompress
{
    private GzipDecompress() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"> The file path provided is wrong </exception>
    /// <exception cref="InvalidDataException"> Illegal Formated Deflate data </exception>
    public static string GzipRun(string[] args)
    {
        StringBuilder outStrBuilder = new StringBuilder();
        if (args.Length != 2) return "Usage: GzipDecompress Inputfile.gz Outputfile";
        string inPath = args[0];
        string outPath = args[1];

        if (!File.Exists(inPath) || Directory.Exists(inPath)) throw new FileNotFoundException($"Input-File {inPath} not found!");

        using (var stream = File.OpenRead(inPath))
        {
            // StreamReader vs BinaryReader here?
            using (var reader = new BinaryReader(stream, Encoding.Latin1))
            {
                try
                {
                    // first we must read and consume header-info of variable length
                    readHeaderInfo(reader, outStrBuilder);

                    // we wrap the underlyingStream into our own BitStream that reads 1 BIT at a time.
                    Stream underlyingStream = reader.BaseStream;
                    using FileStream output = File.Create(outPath);
                    BitStream bitwiseInStream = new BitStream(underlyingStream);

                    // start the decompression process
                    var dataCrc = Decompressor.Decompress(bitwiseInStream, output);

                    // read expected-checksum and expected-length
                    byte[] crc = BitConverter.GetBytes(readLittleEndianInt32(reader));
                    int size = readLittleEndianInt32(reader);
                    if (size != output.Length)
                        throw new InvalidDataException($"Error: Size after decompression mismatched. expected: {size} got: {output.Length}");

                    // we must compare equality of each byte:
                    Array.Reverse(dataCrc);
                    if (!dataCrc.AsSpan().SequenceEqual(crc))
                        throw new InvalidDataException($"Error: Crc32 mismatch; got: {string.Join(" ", dataCrc)} expected: {string.Join(" ", crc)}");
                    // dbgPrintOutStream(output);
                }
                catch
                {
                    Console.WriteLine($"This File Errored: {inPath}\n--- --- --- --- ---\n" + outStrBuilder.ToString());
                    throw;      // rethrow to preserve stack information
                }
            };
        }
        outStrBuilder.AppendLine("Successfully decompressed " + outPath);
        return outStrBuilder.ToString();
    }

    /// <summary>
    /// Header info of various optional fields that must be read in order.
    /// We just print out encountered Headers like last-modified etc...
    /// </summary>
    /// <exception cref="InvalidDataException"></exception>
    private static void readHeaderInfo(BinaryReader reader, StringBuilder outStrBuilder)
    {
        // 2 bytes initialisation - gzip magic number: 
        var magicNr = reader.ReadUInt16();
        if (!(magicNr == 0x1F8B || magicNr == 35615)) throw new InvalidDataException("Invalid gzip magic number.");
        // 1 byte compression-method -  Deflate = 8 for deflate
        byte compressionMethod = reader.ReadByte();
        if (compressionMethod != 8) throw new InvalidDataException($"Unsupported compression method. Expected 8 got: [{compressionMethod}].");
        // 1 byte special-info - reserved-bits must be 0
        BitVector32 fileFlags = new BitVector32(reader.ReadByte());    //reader.ReadInt32() any difference?;
        if (fileFlags[5] || fileFlags[6] || fileFlags[7]) throw new InvalidDataException("Reserved flags are set. Must be 0");
        // 4 byte unixtimestamp - last modified - time is in endian byte array -> we reverse it before casting it
        int unixTime = readLittleEndianInt32(reader); //BitConverter.ToInt32(reader.ReadBytes(4).ToArray());
        var dateTimeLastModification = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        if (unixTime != 0) outStrBuilder.AppendLine($"Last modified - {dateTimeLastModification}");
        else outStrBuilder.AppendLine("last modified - N/A");
        // 1 byte additional-info - info about kompression
        BitVector32 extraFlags = new BitVector32(reader.ReadByte());
        if (extraFlags[2]) outStrBuilder.AppendLine("Compression - maximal Compression and slowest algorithm.");
        else if (extraFlags[4]) outStrBuilder.AppendLine("Compression - fastest Compression algorithm.");
        else outStrBuilder.AppendLine($"Compression unknown. Extra-flags: {extraFlags}");
        // 1 byte os-file-system - info about what OS this file was compressed on
        byte operatingSystem = reader.ReadByte();
        string os = operatingSystem switch
        {
            0 => "FAT filesystem (MS-DOS, OS/2, NT/Win32)",
            1 => "Amiga",
            2 => "VMS (or OpenVMS)",
            3 => "Unix",
            4 => "VM/CMS",
            5 => "Atari TOS",
            6 => "HPFS filesystem (OS/2, NT)",
            7 => "Macintosh",
            8 => "Z-System",
            9 => "CP/M",
            10 => "TOPS-20",
            11 => "NTFS filesystem (NT)",
            12 => "QDOS",
            13 => "Acorn RISCOS",
            255 => "Unknown",
            _ => "Could not match OperatingSystem Identifier",
        };
        outStrBuilder.AppendLine($"File-System-Info - value: {operatingSystem} => {os}");

        // next come optinal flags, denoted in fileFlags.
        //  0x01    FTEXT       file is probably ASCII text.
        //  0x04    FEXTRA      The file contains extra fields
        //  0x08    FNAME       The file contains an original file name string
        //  0x10    FCOMMENT    The file contains comment
        //  0x20                Reserved
        //  0x40                Reserved
        //  0x80                Reserved
        if (fileFlags[1]) outStrBuilder.AppendLine("Flag0 FTEXT - Indicating this is Text is set.");
        if (fileFlags[4])
        {
            byte[] u16Endian = reader.ReadBytes(2);
            var bytesToSkipp = BinaryPrimitives.ReadUInt16LittleEndian(u16Endian);
            outStrBuilder.AppendLine($"Flag2 FEXTRA - Indicating Extra");
            reader.ReadBytes(bytesToSkipp);
        }
        if (fileFlags[8]) outStrBuilder.AppendLine($"Flag3 FNAME- Indicating File name: {readNullTerminatedString(reader)}");
        if (fileFlags[16]) outStrBuilder.AppendLine($"Flag4 FCOMMENT - Indicating Comment: {readNullTerminatedString(reader)}");
        if (fileFlags[2])
        {
            reader.ReadBytes(2); // 2 byte checksum (that we just disregard)
            outStrBuilder.AppendLine("Flag1 FHCRC - Indicating this has a header-checksum is set.");
        }
    }

    /*
     *          HELPERS
     */

    private static bool byteArraysEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

    private static void writeStreamToFile(Stream output, string newFilePath)
    {
        using (var fileStream = File.Create(newFilePath))
        {
            output.Seek(0, SeekOrigin.Begin);
            output.CopyTo(fileStream);
        }

    }

    private static byte[] dbgHashBytesFromStream(Stream output)
    {
        byte[] arr;
        if (output is MemoryStream) arr = ((MemoryStream)output).ToArray();
        else
        {
            using (var memoryStream = new MemoryStream())
            {
                output.CopyTo(memoryStream);
                arr = memoryStream.ToArray();
            }
        }
        return HashingCrc32.CRC32(arr);
    }

    // Debugging only till we get anything working
    private static void dbgPrintOutStream(Stream output)
    {
        // print out contents we decoded (works for text only)
        output.Position = 0;
        StreamReader toStrReader = new StreamReader(output);
        string txt = toStrReader.ReadToEnd();
        Console.WriteLine(txt);
    }

    /// <summary>
    /// reads 32 bit in reverse-order (littleEndian) and interprets them as a int32
    /// </summary>
    private static int readLittleEndianInt32(BinaryReader reader)
    {
        return BitConverter.ToInt32(reader.ReadBytes(4).ToArray());
    }

    /// <summary>
    /// keep reading chars till Null-terminator is found.
    /// </summary>
    /// <returns> string it just fully consumed. (minus consumed Null-terminator)</returns>
    private static string readNullTerminatedString(in BinaryReader reader)
    {
        char ch = reader.ReadChar();
        string result = "";
        while (ch != '\0')
        {
            result += ch;
            try
            {
                ch = reader.ReadChar();
            }
            catch
            {
                // reader.ReadChar() might break on special characters
                // for non Encoding.Latin1 it did break on öäüß so we leave this in to secure against it
                ch = '?';
                reader.ReadByte();
            }
        }
        return result;
    }
}