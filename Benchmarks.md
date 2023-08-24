
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
|     Method |       Mean |    Error |  StdDev |      Gen0 | Allocated |
|----------- |-----------:|---------:|--------:|----------:|----------:|
|     PdfMne | 2,514.0 ms |  5.06 ms | 3.01 ms | 1000.0000 |   2.66 MB |
|    Mp3Mine | 2,346.6 ms |  9.19 ms | 4.80 ms |         - |   1.62 MB |
| TxtBigMine |   696.4 ms | 11.07 ms | 6.59 ms |         - |   1.54 MB |
```
