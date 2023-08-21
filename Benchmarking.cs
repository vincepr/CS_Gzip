using BenchmarkDotNet.Attributes;
using CS_Gzip.Gzip;
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

    [Config(typeof(BenchmarkingAntivirFriendlyConfig))]
    [MemoryDiagnoser]
    public class Benchmarking
    {
        private static readonly string[] cmdArgs1 = new[] { "../../../testfiles/test.txt.gz", "outfile.txt" };
        private static readonly string[] cmdArgs2 = new[] { "../../../testfiles/test.ico.gz", "outfile.ico" };
        private static readonly string[] cmdArgs3 = new[] { "../../../testfiles/test.png.gz", "outfile.png" };
        private static readonly string[] cmdArgs4 = new[] { "../../../testfiles/pdf.gz", "outfile.pdf" };
        private static readonly string[] cmdArgs5 = new[] { "../../../testfiles/mp3.gz", "outfile.mp3" };
        private static readonly string[] cmdArgs6 = new[] { "../../../testfiles/bigtxt.txt.gz", "outfilebig.txt" };

        //[Benchmark]
        //public string TxtMine() => GzipDecompress.GzipRun(cmdArgs1);

        //[Benchmark]
        //public string IcoMine() => GzipDecompress.GzipRun(cmdArgs2);

        //[Benchmark]
        //public string PngMine() => GzipDecompress.GzipRun(cmdArgs3);
        //[Benchmark]
        //public string PdfMne() => GzipDecompress.GzipRun(cmdArgs4);

        //[Benchmark]
        //public string Mp3Mine() => GzipDecompress.GzipRun(cmdArgs5);

        [Benchmark]
        public string TxtBigMine() => GzipDecompress.GzipRun(cmdArgs6);


        //[Benchmark]
        //public string TxtOg() => DotnetDecompressor.DecompressFile(cmdArgs1);

        //[Benchmark]
        //public string IcoOg() => DotnetDecompressor.DecompressFile(cmdArgs2);

        //[Benchmark]
        //public string PngOg() => DotnetDecompressor.DecompressFile(cmdArgs3);
        //[Benchmark]

        //public string PdfOg() => DotnetDecompressor.DecompressFile(cmdArgs4);

        //[Benchmark]
        //public string Mp3Og() => DotnetDecompressor.DecompressFile(cmdArgs5);

        [Benchmark]
        public string TxtBigOg() => DotnetDecompressor.DecompressFile(cmdArgs6);
        }
}
