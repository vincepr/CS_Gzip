// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using CS_Gzip;

Console.WriteLine("Starting benchmarks");


//string[] cmdArgs1 = new[] { "../../../test.txt.gz", "outfile.txt" };
//string[] cmdArgs2 = new[] { "../../../test.ico.gz", "outfile.ico" };
//string[] cmdArgs3 = new[] { "../../../test.png.gz", "outfile.pdg" };

//string result = GzipDecompress.GzipRun(cmdArgs3);
//Console.WriteLine(result);

BenchmarkRunner.Run<Benchmarking>();