// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using CS_Gzip;
using CS_Gzip.Gzip;

//TestingEquality.Run();
//decompressTestFiles();
runBenchmark();




// decompress the png test file
static void decompressTestFiles()
{
    List<string[]> cmdArgs = new List<string[]>();
    cmdArgs.Add(new[] { "../../../testfiles/test.txt.gz", "outfile.txt" });
    cmdArgs.Add(new[] { "../../../testfiles/test.ico.gz", "outfile.ico" });
    cmdArgs.Add(new[] { "../../../testfiles/test.png.gz", "outfile.png" });
    cmdArgs.Add(new[] { "../../../testfiles/pdf.gz", "outfile.pdf" });
    cmdArgs.Add(new[] { "../../../testfiles/mp3.gz", "outfile.mp3" });
    cmdArgs.Add(new[] { "../../../testfiles/bigtxt.txt.gz", "outfilebig.txt" });

    for (int i =0; i<cmdArgs.Count; i++)
    {
        string result = GzipDecompress.GzipRun(cmdArgs[i]);
        Console.WriteLine(result);
    }
}

// running The Benchmark
static void runBenchmark()
{
    BenchmarkRunner.Run<Benchmarking>();
}
