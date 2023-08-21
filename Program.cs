// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using CS_Gzip;
using CS_Gzip.Gzip;

//TestingEquality.Run();
decompressTestFiles();
//runBenchmark();




// decompress the png test file
static void decompressTestFiles()
{
    string[] cmdArgs1 = new[] { "../../../testfiles/test.txt.gz", "outfile.txt" };
    string[] cmdArgs2 = new[] { "../../../testfiles/test.ico.gz", "outfile.ico" };
    string[] cmdArgs3 = new[] { "../../../testfiles/test.png.gz", "outfile.png" };
    string[] cmdArgs4 = new[] { "../../../testfiles/pdf.gz", "outfile.pdf" };           // ERRORS
    string[] cmdArgs5 = new[] { "../../../testfiles/mp3.gz", "outfile.mp3" };
    string[] cmdArgs6 = new[] { "../../../testfiles/bigtxt.txt.gz", "outfilebig.txt" };
    string[] cmdArgs = new[] { "../../../testfiles/test.png.gz", "outfile.pngg" };
    string result = GzipDecompress.GzipRun(cmdArgs4);
    Console.WriteLine(result);
}

// running The Benchmark
static void runBenchmark()
{
    BenchmarkRunner.Run<Benchmarking>();
}
