using BenchmarkDotNet.Attributes;
using CS_Gzip.Gzip;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS_Gzip.Gzip.tools.HuffmanCodeImplementations;

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


        
        [Benchmark]
        public string Original_Txt() => DotnetDecompressor.DecompressFile(cmdArgs1);
        [Benchmark]
        public string Original_Ico() => DotnetDecompressor.DecompressFile(cmdArgs2);
        [Benchmark]
        public string Original_Png() => DotnetDecompressor.DecompressFile(cmdArgs3);
        [Benchmark]
        public string Original_Pdf() => DotnetDecompressor.DecompressFile(cmdArgs4);
        [Benchmark]
        public string Original_Mp3() => DotnetDecompressor.DecompressFile(cmdArgs5);
        [Benchmark]
        public string Original_TxtBig() => DotnetDecompressor.DecompressFile(cmdArgs6);
        
        
        
        [Benchmark]
        public string Array_TxtMine() => GzipDecompress.GzipRun<HuffmanArray>(cmdArgs1);
        [Benchmark]
        public string Array_IcoMine() => GzipDecompress.GzipRun<HuffmanArray>(cmdArgs2);
        [Benchmark]
        public string Array_PngMine() => GzipDecompress.GzipRun<HuffmanArray>(cmdArgs3);
        [Benchmark]
        public string Array_PdfMne() => GzipDecompress.GzipRun<HuffmanArray>(cmdArgs4);
        [Benchmark]
        public string Array_Mp3Mine() => GzipDecompress.GzipRun<HuffmanArray>(cmdArgs5);
        [Benchmark]
        public string Array_TxtBigMine() => GzipDecompress.GzipRun<HuffmanArray>(cmdArgs6);

        
        
        [Benchmark]
        public string SortedList_TxtMine() => GzipDecompress.GzipRun<HuffmanSortedList>(cmdArgs1);
        [Benchmark]
        public string SortedList_IcoMine() => GzipDecompress.GzipRun<HuffmanSortedList>(cmdArgs2);
        [Benchmark]
        public string SortedList_PngMine() => GzipDecompress.GzipRun<HuffmanSortedList>(cmdArgs3);
        [Benchmark]
        public string SortedList_PdfMne() => GzipDecompress.GzipRun<HuffmanSortedList>(cmdArgs4);
        [Benchmark]
        public string SortedList_Mp3Mine() => GzipDecompress.GzipRun<HuffmanSortedList>(cmdArgs5);
        [Benchmark]
        public string SortedList_TxtBigMine() => GzipDecompress.GzipRun<HuffmanSortedList>(cmdArgs6);

         
        
        [Benchmark]
        public string SortedDict_TxtMine() => GzipDecompress.GzipRun<HuffmanSortedDict>(cmdArgs1);
        [Benchmark]
        public string SortedDict_IcoMine() => GzipDecompress.GzipRun<HuffmanSortedDict>(cmdArgs2);
        [Benchmark]
        public string SortedDict_PngMine() => GzipDecompress.GzipRun<HuffmanSortedDict>(cmdArgs3);
        [Benchmark]
        public string SortedDict_PdfMne() => GzipDecompress.GzipRun<HuffmanSortedDict>(cmdArgs4);
        [Benchmark]
        public string SortedDict_Mp3Mine() => GzipDecompress.GzipRun<HuffmanSortedDict>(cmdArgs5);
        [Benchmark]
        public string SortedDict_TxtBigMine() => GzipDecompress.GzipRun<HuffmanSortedDict>(cmdArgs6);

        
        
        [Benchmark]
        public string List_TxtMine() => GzipDecompress.GzipRun<HuffmanList>(cmdArgs1);
        [Benchmark]
        public string List_IcoMine() => GzipDecompress.GzipRun<HuffmanList>(cmdArgs2);
        [Benchmark]
        public string List_PngMine() => GzipDecompress.GzipRun<HuffmanList>(cmdArgs3);
        [Benchmark]
        public string List_PdfMne() => GzipDecompress.GzipRun<HuffmanList>(cmdArgs4);
        [Benchmark]
        public string List_Mp3Mine() => GzipDecompress.GzipRun<HuffmanList>(cmdArgs5);
        [Benchmark]
        public string List_TxtBigMine() => GzipDecompress.GzipRun<HuffmanList>(cmdArgs6);

        
        
        [Benchmark]
        public string Dict_TxtMine() => GzipDecompress.GzipRun<HuffmanDict>(cmdArgs1);
        [Benchmark]
        public string Dict_IcoMine() => GzipDecompress.GzipRun<HuffmanDict>(cmdArgs2);
        [Benchmark]
        public string Dict_PngMine() => GzipDecompress.GzipRun<HuffmanDict>(cmdArgs3);
        [Benchmark]
        public string Dict_PdfMne() => GzipDecompress.GzipRun<HuffmanDict>(cmdArgs4);
        [Benchmark]
        public string Dict_Mp3Mine() => GzipDecompress.GzipRun<HuffmanDict>(cmdArgs5);
        [Benchmark]
        public string Dict_TxtBigMine() => GzipDecompress.GzipRun<HuffmanDict>(cmdArgs6);


        
        [Benchmark]
        public string Custom_TxtMine() => GzipDecompress.GzipRun<HuffmanArrayCustomSearch>(cmdArgs1);
        [Benchmark]
        public string Custom_IcoMine() => GzipDecompress.GzipRun<HuffmanArrayCustomSearch>(cmdArgs2);
        [Benchmark]
        public string Custom_PngMine() => GzipDecompress.GzipRun<HuffmanArrayCustomSearch>(cmdArgs3);
        [Benchmark]
        public string Custom_PdfMne() => GzipDecompress.GzipRun<HuffmanArrayCustomSearch>(cmdArgs4);
        [Benchmark]
        public string Custom_Mp3Mine() => GzipDecompress.GzipRun<HuffmanArrayCustomSearch>(cmdArgs5);
        [Benchmark]
        public string Custom_TxtBigMine() => GzipDecompress.GzipRun<HuffmanArrayCustomSearch>(cmdArgs6);
        }
}
