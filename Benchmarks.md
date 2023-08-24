
## Benchmarks

```
-ArraySearchFromStart
|     Method |       Mean |    Error |   StdDev |      Gen0 | Allocated |
|----------- |-----------:|---------:|---------:|----------:|----------:|
|     PdfMne | 2,717.7 ms | 42.02 ms | 21.98 ms | 1000.0000 |   2.65 MB |
|    Mp3Mine | 2,342.9 ms | 15.90 ms |  8.31 ms |         - |   1.61 MB |
| TxtBigMine |   584.2 ms |  6.78 ms |  4.48 ms |         - |   1.53 MB |

// dictionary
|     Method |       Mean |    Error |  StdDev |      Gen0 | Allocated |
|----------- |-----------:|---------:|--------:|----------:|----------:|
|     PdfMne | 1,410.3 ms |  9.57 ms | 5.70 ms | 3000.0000 |   6.59 MB |
|    Mp3Mine | 1,266.9 ms | 11.96 ms | 7.12 ms | 2000.0000 |   4.15 MB |
| TxtBigMine |   542.7 ms |  8.04 ms | 4.21 ms | 1000.0000 |   2.13 MB |

// CustomSearch
|     Method |       Mean |    Error |   StdDev |      Gen0 | Allocated |
|----------- |-----------:|---------:|---------:|----------:|----------:|
|     PdfMne | 1,999.7 ms | 79.75 ms | 47.46 ms | 1000.0000 |   2.65 MB |
|    Mp3Mine | 1,897.1 ms | 72.60 ms | 43.20 ms |         - |   1.61 MB |
| TxtBigMine |   517.5 ms | 12.88 ms |  8.52 ms |         - |   1.53 MB |

// Array (with Array.BinarySearch())
|     Method |       Mean |    Error |   StdDev |      Gen0 | Allocated |
|----------- |-----------:|---------:|---------:|----------:|----------:|
|     PdfMne | 2,561.3 ms | 25.05 ms | 14.90 ms | 1000.0000 |   2.66 MB |
|    Mp3Mine | 2,377.8 ms | 17.12 ms | 11.33 ms |         - |   1.62 MB |
| TxtBigMine |   697.6 ms |  5.15 ms |  3.06 ms |         - |   1.54 MB |

```


```
|            Method |           Mean |         Error |        StdDev |      Gen0 |  Allocated |
|------------------ |---------------:|--------------:|--------------:|----------:|-----------:|
|      Dict_TxtMine |     7,750.4 us |      64.94 us |      42.96 us |   15.6250 |   50.67 KB |
|      Dict_IcoMine |     2,026.5 us |      20.68 us |      12.31 us |   23.4375 |    55.7 KB |
|      Dict_PngMine |     8,721.6 us |      75.87 us |      50.19 us |   31.2500 |   85.33 KB |
|       Dict_PdfMne | 1,454,005.8 us |  48,132.85 us |  25,174.41 us | 3000.0000 | 6745.36 KB |
|      Dict_Mp3Mine | 1,284,445.2 us |  15,653.92 us |  10,354.10 us | 2000.0000 |  4248.1 KB |
|   Dict_TxtBigMine |   541,191.4 us |   7,965.33 us |   5,268.58 us | 1000.0000 | 2179.56 KB |

|    Custom_TxtMine |       533.2 us |      45.23 us |      29.92 us |   23.4375 |    50.3 KB |
|    Custom_IcoMine |       519.0 us |      44.29 us |      29.30 us |   24.9023 |    51.2 KB |
|    Custom_PngMine |    10,320.7 us |     157.16 us |     103.95 us |   15.6250 |   60.11 KB |
|     Custom_PdfMne | 1,794,751.3 us |  65,840.30 us |  43,549.28 us | 1000.0000 | 2712.45 KB |
|    Custom_Mp3Mine | 1,685,800.7 us | 165,663.28 us | 109,576.00 us |         - | 1653.17 KB |
| Custom_TxtBigMine |   486,706.9 us |   7,099.50 us |   3,713.18 us |         - | 1566.65 KB |

|             TxtOg |       421.8 us |      54.22 us |      35.86 us |    2.4414 |    5.08 KB |
|             IcoOg |       396.0 us |      23.50 us |      15.54 us |    2.4414 |    5.08 KB |
|             PngOg |       630.7 us |      60.72 us |      40.16 us |         - |    1.06 KB |
|             PdfOg |    44,721.1 us |     796.11 us |     473.75 us |         - |    5.15 KB |
|             Mp3Og |    39,679.8 us |   3,317.49 us |   2,194.31 us |         - |    5.13 KB |
|          TxtBigOg |    42,749.3 us |     438.07 us |     260.69 us |         - |    5.17 KB |

```
