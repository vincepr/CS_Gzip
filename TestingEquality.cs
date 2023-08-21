// See https://aka.ms/new-console-template for more information
using CS_Gzip.Gzip;
using System.IO.Compression;
using System.Text;
public static class TestingEquality
{
    /// <summary>
    /// runs the dotnet implemetation against my implementation with randomized bytes and randomized text.
    /// then compares decompressed files for equality
    /// </summary>
    public static void Run()
    {
        CompressionLevel[] levels = new CompressionLevel[] {
        CompressionLevel.Fastest,
        CompressionLevel.Optimal,
        CompressionLevel.SmallestSize,
        CompressionLevel.SmallestSize,
    };
        foreach (CompressionLevel level in levels)
        {
            TestingEquality.RandomizeMessage();
            TestingEquality.CreateFileToCompress();
            TestingEquality.CompressFile(level);
            TestingEquality.DecompressFileOg();
            TestingEquality.DecompressFileMine();
            TestingEquality.PrintResults();
            TestingEquality.DeleteFiles();

            TestingEquality.RandomizeTxt();
            TestingEquality.CreateTextFileToCompress();
            TestingEquality.CompressFile(level);
            TestingEquality.DecompressFileOg();
            TestingEquality.DecompressFileMine();
            TestingEquality.PrintResults();
            TestingEquality.DeleteFiles();
        }
    }

    public static void RandomizeMessage()
    {
        Random rng = new Random();
        int len = rng.Next(10000);
        byte[] bytes = new byte[len];
        rng.NextBytes(bytes);
        RngBytes = bytes;
    }

    public static void RandomizeTxt()
    {
        Random random = new Random();
        int len = random.Next(10000);

        // creating a StringBuilder object()
        StringBuilder str_build = new StringBuilder();

        char letter;

        for (int i = 0; i < len; i++)
        {
            double flt = random.NextDouble();
            int shift = Convert.ToInt32(Math.Floor(25 * flt));
            letter = Convert.ToChar(shift + 65);
            str_build.Append(letter);
        }
        Message = str_build.ToString();
    }



    static string Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
    static byte[] RngBytes = new byte[] { 12, 32, 12 };
    private const string OriginalFileName = "original.txt";
    private const string CompressedFileName = "compressed.gz";
    private const string MyDecompressedFileName = "mydecompressed.gz";
    private const string DecompressedFileName = "decompressed.txt";
    public static void CreateTextFileToCompress() => File.WriteAllText(OriginalFileName, Message);
    public static void CreateFileToCompress() => File.WriteAllBytes(OriginalFileName, RngBytes);

    public static void CompressFile(CompressionLevel compressionLevel)
    {
        using FileStream originalFileStream = File.Open(OriginalFileName, FileMode.Open);
        using FileStream compressedFileStream = File.Create(CompressedFileName);
        using var compressor = new GZipStream(compressedFileStream, compressionLevel);
        originalFileStream.CopyTo(compressor);
    }

    public static void DecompressFileOg()
    {
        using FileStream compressedFileStream = File.Open(CompressedFileName, FileMode.Open);
        using FileStream outputFileStream = File.Create(DecompressedFileName);
        using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
        decompressor.CopyTo(outputFileStream);
    }

    public static void DecompressFileMine()
    {
        string[] cmdArgs = new[] { "./"+ CompressedFileName, MyDecompressedFileName };
        GzipDecompress.GzipRun(cmdArgs);
    }

    public static void PrintResults()
    {
        long originalSize = new FileInfo(OriginalFileName).Length;
        long myDecompressedSize = new FileInfo(MyDecompressedFileName).Length;
        long decompressedSize = new FileInfo(DecompressedFileName).Length;

        var isEqual = File.ReadAllText(OriginalFileName) == File.ReadAllText(DecompressedFileName);
        if (!isEqual) 
        { 
            Console.WriteLine($"The decompressed and orignal file NOT equal: {isEqual}");
            Console.WriteLine($"The original file '{OriginalFileName}' weighs {originalSize} bytes.");
            Console.WriteLine($"The decompressed file '{DecompressedFileName}' weighs {decompressedSize} bytes. Contents: \"{File.ReadAllText(DecompressedFileName)}\"");
            throw new Exception("compressed and original are NOT equal");
        }

        isEqual = File.ReadAllText(MyDecompressedFileName) == File.ReadAllText(DecompressedFileName);
        Console.WriteLine($"Randomized Data -> dotnet-decompressed and my-decompressed are equal {isEqual}");
        if (!isEqual)
        {
            Console.WriteLine($"The original file '{OriginalFileName}' weighs {originalSize} bytes.");
            Console.WriteLine($"The decompressed file '{DecompressedFileName}' weighs {decompressedSize} bytes. Contents: \"{File.ReadAllText(DecompressedFileName)}\"");
            Console.WriteLine($"My  decompressed file '{MyDecompressedFileName}' weighs {myDecompressedSize} bytes. Contents: \"{File.ReadAllText(DecompressedFileName)}\"");
            throw new Exception("my decompressed file NOT equal to default implementation");
        }
    }

    public static void DeleteFiles()
    {
        File.Delete(OriginalFileName);
        File.Delete(CompressedFileName);
        File.Delete(DecompressedFileName);
    }
}
