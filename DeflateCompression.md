# Deflate Implementation in Csharp

Working decompressor for single-file gzip-compressed files or Datastreams. 
- Along the DEFLATE Compressed Data Format Specification version 1.3
- as described in **RFC 1951**

##  Usage
```
dotnet run ./compressed-input.gz outputfile.png

>Last modified - 07/08/2023 12.09.15 +00:00
>Compression - maximal Compression and slowest algorithm.
>File-System-Info - value: 0 => FAT filesystem (MS-DOS, OS/2, NT/Win32)
>Flag3 FNAME- Indicating File name: Screenshot 2023-08-07 140912.png
>successfully decompressed outfile.png
```

## Benchmark
### Initial values without any optimisations
- with calculating the crc32: (over the whole file)

```
|     Method |           Mean |       Error |       StdDev |         Median |        Gen0 |      Gen1 |      Gen2 |    Allocated |
|----------- |---------------:|------------:|-------------:|---------------:|------------:|----------:|----------:|-------------:|
|    TxtMine |     7,609.6 us |    27.64 us |     38.74 us |     7,608.1 us |     31.2500 |         - |         - |     70.14 KB |
|    IcoMine |     2,091.2 us |    29.16 us |     42.74 us |     2,089.2 us |     42.9688 |         - |         - |     89.31 KB |
|    PngMine |     8,858.8 us |    27.48 us |     40.27 us |     8,861.6 us |    140.6250 |         - |         - |    298.62 KB |
|     PdfMne | 1,429,486.8 us | 1,333.16 us |  1,954.13 us | 1,429,293.1 us |  28000.0000 | 4000.0000 | 4000.0000 |   95988.1 KB |
|    Mp3Mine | 1,278,488.9 us | 8,522.75 us | 11,947.71 us | 1,272,936.5 us |   5000.0000 | 2000.0000 | 2000.0000 |  50863.38 KB |
| TxtBigMine |   663,653.7 us | 8,706.52 us | 13,031.51 us |   664,465.6 us | 192000.0000 | 6000.0000 | 4000.0000 | 431320.95 KB |
|      TxtOg |     5,326.7 us | 2,109.08 us |  3,156.77 us |     7,353.5 us |           - |         - |         - |      5.08 KB |
|      IcoOg |     1,836.9 us |    11.33 us |     16.25 us |     1,834.3 us |      1.9531 |         - |         - |      5.07 KB |
|      PngOg |       668.8 us |    11.16 us |     16.01 us |       664.5 us |           - |         - |         - |      1.05 KB |
|      PdfOg |    44,070.3 us |   602.63 us |    864.27 us |    43,720.7 us |           - |         - |         - |      5.15 KB |
|      Mp3Og |    37,184.5 us |   635.53 us |    869.92 us |    36,864.6 us |           - |         - |         - |      5.13 KB |
|   TxtBigOg |    42,304.7 us |   881.34 us |  1,145.99 us |    41,997.3 us |           - |         - |         - |      5.16 KB |
```


## useful links

- https://www.rfc-editor.org/rfc/rfc1951.html

- https://www.nayuki.io/page/simple-deflate-decompressor
