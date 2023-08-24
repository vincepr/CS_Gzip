# Deflate Implementation in Csharp

Working decompressor for single-file gzip-compressed files or Datastreams. 
- Along the DEFLATE Compressed Data Format Specification version 1.3
- as described in **RFC 1951**
- `GzipDecompress.cs` strips headers (and checksum at the end) off the file. `Decompressor.cs` Decompresses the rest of the filestream od deflate-data.

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
- the initial benchmark

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
## removed MemoryStream
instead of writing to a memory-stream, we directly write to a SileStream. (and thus allocating a bit less memory)
	- this removed the (for performance terrible) big blob of data on the gen2-heap (Large Object Heap).
```
|     Method |           Mean |       Error |      StdDev |        Gen0 |    Allocated |
|----------- |---------------:|------------:|------------:|------------:|-------------:|
|    TxtMine |     7,666.0 us |    18.91 us |    28.31 us |     31.2500 |     67.56 KB |
|    IcoMine |     2,112.9 us |    32.96 us |    47.27 us |     39.0625 |     84.38 KB |
|    PngMine |     9,052.4 us |    33.27 us |    48.77 us |     46.8750 |    117.66 KB |
|     PdfMne | 1,418,545.2 us | 2,057.14 us | 2,883.83 us |  25000.0000 |   51280.7 KB |
|    Mp3Mine | 1,258,828.2 us | 4,063.30 us | 5,955.93 us |   3000.0000 |   7849.48 KB |
| TxtBigMine |   664,977.7 us | 3,300.87 us | 4,838.38 us | 188000.0000 | 385881.54 KB |
|      TxtOg |       526.0 us |   122.34 us |   171.50 us |      1.9531 |       5.1 KB |
|      IcoOg |     1,949.1 us |    13.85 us |    19.41 us |           - |      5.11 KB |
|      PngOg |       730.6 us |    35.20 us |    51.59 us |           - |      1.08 KB |
|      PdfOg |    44,504.6 us |   385.23 us |   527.30 us |           - |      5.18 KB |
|      Mp3Og |    37,555.6 us |   716.21 us | 1,027.17 us |           - |      5.17 KB |
|   TxtBigOg |    42,365.4 us |   261.05 us |   339.44 us |           - |      5.19 KB |
```
## fixed a bug reallocating a bunch of arrays in a loop
While going trough Riders Memory-Analysis i found a bug with the following:
```csharp
// the bug:
var by = _data[readIdx];
ReadOnlySpan<byte> b = new byte[] { _data[readIdx] };
output.Write(b);
// changed to:
var b = _data[readIdx];
output.WriteByte(b)
```
- i was reallocating a array every iteration of the loop.
- And thus generating arround 500Mb of Memory of byte arrays allocat in the worst case i encountered. (assuming i read the Rider-Memory-Analysis right)
- this was most notably in the `TxtBigMine`-Benchmark.
```
|     Method |           Mean |       Error |      StdDev |         Median |      Gen0 |  Allocated |
|----------- |---------------:|------------:|------------:|---------------:|----------:|-----------:|
|    TxtMine |       716.1 us |    20.06 us |    29.40 us |       709.4 us |   24.4141 |   51.62 KB |
|    IcoMine |     1,506.5 us |   531.67 us |   762.50 us |     2,143.6 us |   27.3438 |   56.66 KB |
|    PngMine |     9,013.7 us |    60.07 us |    84.21 us |     9,022.2 us |   31.2500 |    87.2 KB |
|     PdfMne | 1,425,400.9 us | 3,112.69 us | 4,155.35 us | 1,425,818.4 us | 3000.0000 | 7056.23 KB |
|    Mp3Mine | 1,279,865.8 us | 2,381.56 us | 3,415.57 us | 1,279,756.8 us | 2000.0000 | 4441.83 KB |
| TxtBigMine |   541,804.0 us | 2,588.46 us | 3,455.53 us |   542,270.0 us | 1000.0000 | 2378.02 KB |
|      TxtOg |       599.9 us |    14.76 us |    21.64 us |       594.7 us |    1.9531 |    5.23 KB |
|      IcoOg |     2,111.1 us |    56.71 us |    81.34 us |     2,076.6 us |         - |    5.61 KB |
|      PngOg |       830.3 us |    20.48 us |    30.66 us |       830.6 us |         - |     1.2 KB |
|      PdfOg |    44,931.8 us |   610.73 us |   895.20 us |    44,584.0 us |         - |   15.85 KB |
|      Mp3Og |    37,259.6 us |   233.12 us |   311.21 us |    37,181.1 us |         - |   14.31 KB |
|   TxtBigOg |    43,341.7 us |   778.72 us | 1,141.44 us |    42,843.8 us |         - |   15.86 KB |
```


- different Computer
```
|     Method |       Mean |     Error |    StdDev |     Median |      Gen0 | Allocated |
|----------- |-----------:|----------:|----------:|-----------:|----------:|----------:|
|     PdfMne | 2,098.3 ms | 162.54 ms | 238.24 ms | 1,964.8 ms | 2000.0000 |   6.59 MB |
|    Mp3Mine | 1,718.0 ms |  47.01 ms |  62.75 ms | 1,695.8 ms | 1000.0000 |   4.15 MB |
| TxtBigMine |   880.0 ms | 152.21 ms | 213.38 ms |   781.0 ms |         - |   2.13 MB |
```

## useful links

- https://www.rfc-editor.org/rfc/rfc1951.html

- https://www.nayuki.io/page/simple-deflate-decompressor
