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

## useful links

- https://www.rfc-editor.org/rfc/rfc1951.html

- https://www.nayuki.io/page/simple-deflate-decompressor

## Benchmark
- at least at first glance it seems my own implementation is faster by at least a factor x3.
- Probably because the dotnet implementation allocates more memory on the heap. (more allocations and thus more time spent on allocating and gc)

```
// my times
|     Method |     Mean |    Error |   StdDev |   Gen0 | Allocated |
|----------- |---------:|---------:|---------:|-------:|----------:|
|    TxtMine | 24.77 us | 0.137 us | 0.128 us | 0.1221 |     384 B |
|    IcoMine | 25.02 us | 0.321 us | 0.300 us | 0.1221 |     384 B |
|    PngMine | 24.75 us | 0.257 us | 0.241 us | 0.1221 |     384 B |
|     PdfMne | 24.92 us | 0.321 us | 0.301 us | 0.0916 |     368 B |
|    Mp3Mine | 24.60 us | 0.180 us | 0.168 us | 0.0916 |     368 B |
| TxtBigMine | 25.12 us | 0.322 us | 0.302 us | 0.1221 |     392 B |

// default implementation of dotnet
|      TxtOg | 80.18 us | 0.462 us | 0.432 us | 3.0518 |    9783 B |
|      IcoOg | 79.54 us | 0.239 us | 0.212 us | 3.0518 |    9783 B |
|      PngOg | 79.85 us | 0.357 us | 0.333 us | 3.0518 |    9783 B |
|      PdfOg | 80.25 us | 0.623 us | 0.552 us | 3.0518 |    9760 B |
|      Mp3Og | 81.05 us | 0.928 us | 0.868 us | 3.0518 |    9760 B |
|   TxtBigOg | 81.74 us | 0.993 us | 0.929 us | 3.0518 |    9799 B |
```