using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Gzip
{
    public class DotnetDecompressor
    {
        public static string DecompressFile(string[] cmdArgs)
        {
            try
            {

                string pathFrom = cmdArgs[0];
                string pathTo = cmdArgs[1];
                using FileStream compressedFileStream = File.Open(pathFrom, FileMode.Open);
                using FileStream outputFileStream = File.Create(pathTo);
                using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
                decompressor.CopyTo(outputFileStream);
            } catch (Exception ex)
            {
                return ex.ToString();
            }
            return "Successfully decompressed";
   
        }
    }


    [MemoryDiagnoser]
    public class Benchmarking
    {
        private static readonly string[] cmdArgs1 = new[] { "../../../test.txt.gz", "outfile.txt" };
        private static readonly string[] cmdArgs2 = new[] { "../../../test.ico.gz", "outfile.ico" };
        private static readonly string[] cmdArgs3 = new[] { "../../../test.png.gz", "outfile.pdg" };

        [Benchmark]
        public string TxtMine() => GzipDecompress.GzipRun(cmdArgs1);

        [Benchmark]
        public string IcoMine() => GzipDecompress.GzipRun(cmdArgs2);

        [Benchmark]
        public string PngMine() => GzipDecompress.GzipRun(cmdArgs3);

        [Benchmark]
        public string TxtOg() => DotnetDecompressor.DecompressFile(cmdArgs1);

        [Benchmark]
        public string IcoOg() => DotnetDecompressor.DecompressFile(cmdArgs2);

        [Benchmark]
        public string PngOg() => DotnetDecompressor.DecompressFile(cmdArgs3);


    }
}
