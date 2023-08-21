// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using CS_Gzip;

Console.WriteLine("Starting benchmarks");

BenchmarkRunner.Run<Benchmarking>();