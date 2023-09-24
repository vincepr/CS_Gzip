using BenchmarkDotNet.Running;
using CS_Gzip;
using CS_Gzip.Gzip;

Console.WriteLine(args.Length);
if (args.Length != 2 )
{
    Console.WriteLine("Expected 2 arguments: \ndotnetgzip [path to .gz] [outfile.pdf]");
    Environment.Exit(1);
}
string result = GzipDecompress.GzipRun(args);
Console.WriteLine(result);


// // Performance benchmarking
//TestingEquality.Run();
//for (int i = 0; i < 3; i++)
//    decompressTestFiles();
// runBenchmark();


// decompress the png test file
static void decompressTestFiles()
{
    List<string[]> cmdArgs = new List<string[]>
    {
        new[] { "../../../testfiles/test.txt.gz", "outfile.txt" },
        new[] { "../../../testfiles/test.ico.gz", "outfile.ico" },
        new[] { "../../../testfiles/test.png.gz", "outfile.png" },
        new[] { "../../../testfiles/pdf.gz", "outfile.pdf" },
        new[] { "../../../testfiles/mp3.gz", "outfile.mp3" },
        new[] { "../../../testfiles/bigtxt.txt.gz", "outfilebig.txt" }
    };

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
