// See https://aka.ms/new-console-template for more information
using CS_Gzip;

Console.WriteLine("Hello, World!");
string[] cmdArgs = new [] { "../../../test.txt.gz", "outfile.txt" }; 
string result = GzipDecompress.GzipRun(cmdArgs);
Console.WriteLine(result);