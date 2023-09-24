
## Benchmarks

```
|                Method |           Mean |         Error |       StdDev |         Median |      Gen0 |  Allocated |
|---------------------- |---------------:|--------------:|-------------:|---------------:|----------:|-----------:|

- the dotnet-Deflate to compare against what is possible
|          Original_Txt |     7,623.4 us |      36.95 us |     19.32 us |     7,627.7 us |         - |    5.09 KB |
|          Original_Ico |     1,884.5 us |      32.09 us |     21.22 us |     1,879.5 us |    1.9531 |    5.08 KB |
|          Original_Png |       563.2 us |      21.48 us |     11.23 us |       566.5 us |         - |    1.06 KB |
|          Original_Pdf |    44,893.9 us |   2,510.86 us |  1,660.78 us |    44,041.5 us |         - |    5.15 KB |
|          Original_Mp3 |    36,417.7 us |     398.94 us |    237.40 us |    36,362.3 us |         - |    5.13 KB |
|       Original_TxtBig |    42,332.1 us |     372.78 us |    221.83 us |    42,387.3 us |         - |    5.17 KB |

- SortedDictionary (really bad with preordered data)
|    SortedDict_TxtMine |       647.4 us |      32.83 us |     21.72 us |       641.1 us |   24.4141 |   52.01 KB |
|    SortedDict_IcoMine |       658.9 us |      18.23 us |     12.06 us |       657.0 us |   26.3672 |   55.33 KB |
|    SortedDict_PngMine |    34,445.9 us |     237.20 us |    156.89 us |    34,438.0 us |         - |    80.5 KB |
|     SortedDict_PdfMne | 6,324,740.3 us | 109,946.12 us | 72,722.55 us | 6,349,796.4 us | 2000.0000 | 5998.41 KB |
|    SortedDict_Mp3Mine | 5,922,015.7 us |  20,085.28 us | 11,952.43 us | 5,925,411.2 us | 1000.0000 | 3728.54 KB |
| SortedDict_TxtBigMine | 1,239,244.3 us |  17,856.43 us | 11,810.93 us | 1,238,842.1 us | 1000.0000 | 2138.91 KB |

- SortedList with tryFindValue
|    SortedList_TxtMine |       502.5 us |      27.08 us |     17.91 us |       499.9 us |   23.4375 |   49.62 KB |
|    SortedList_IcoMine |       518.0 us |      14.22 us |      9.41 us |       517.1 us |   25.3906 |   52.52 KB |
|    SortedList_PngMine |    14,572.5 us |      49.76 us |     32.92 us |    14,573.3 us |   31.2500 |   66.88 KB |
|     SortedList_PdfMne | 2,761,571.7 us |  98,079.77 us | 58,365.70 us | 2,751,980.8 us | 1000.0000 | 2889.98 KB |
|    SortedList_Mp3Mine | 2,696,320.3 us |  64,485.60 us | 42,653.23 us | 2,709,161.2 us |         - | 1801.12 KB |
| SortedList_TxtBigMine |   779,798.8 us |  31,717.49 us | 18,874.57 us |   776,256.5 us |         - | 1374.59 KB |

- list with BinarySearch
|          List_TxtMine |       498.3 us |      17.75 us |     10.56 us |       499.0 us |   23.4375 |   49.64 KB |
|          List_IcoMine |       517.2 us |      15.25 us |     10.09 us |       518.3 us |   25.3906 |   52.54 KB |
|          List_PngMine |    14,076.2 us |      67.09 us |     39.92 us |    14,057.1 us |   31.2500 |   66.93 KB |
|           List_PdfMne | 2,568,570.1 us |  13,480.61 us |  8,022.10 us | 2,570,997.3 us | 1000.0000 | 2898.77 KB |
|          List_Mp3Mine | 2,388,815.2 us |   7,848.62 us |  5,191.37 us | 2,390,000.8 us |         - |  1806.2 KB |
|       List_TxtBigMine |   701,231.5 us |   3,121.25 us |  2,064.51 us |   701,485.2 us |         - | 1379.79 KB |

- Array with Array.BinarySearch() for the search algorithm
|         Array_TxtMine |       484.1 us |       9.32 us |      5.54 us |       483.6 us |   23.9258 |   50.32 KB |
|         Array_IcoMine |       913.9 us |   1,016.66 us |    672.46 us |       506.5 us |   24.4141 |   51.22 KB |
|         Array_PngMine |    14,763.4 us |      64.10 us |     42.40 us |    14,759.1 us |   15.6250 |   60.16 KB |
|          Array_PdfMne | 2,529,709.0 us |  13,506.47 us |  8,933.69 us | 2,529,425.7 us | 1000.0000 | 2720.72 KB |
|         Array_Mp3Mine | 2,351,602.3 us |   7,494.54 us |  4,459.88 us | 2,350,858.5 us |         - |  1658.3 KB |
|      Array_TxtBigMine |   695,907.0 us |   5,319.47 us |  3,165.53 us |   695,432.7 us |         - | 1572.18 KB |

- default-dictionary (with key=huffmanCodes, value=char)
|          Dict_TxtMine |       471.1 us |      11.17 us |      6.64 us |       472.0 us |   23.9258 |   50.66 KB |
|          Dict_IcoMine |       495.0 us |      17.77 us |     11.75 us |       491.2 us |   26.3672 |    55.7 KB |
|          Dict_PngMine |     7,748.0 us |      41.91 us |     24.94 us |     7,761.9 us |   39.0625 |   85.34 KB |
|           Dict_PdfMne | 1,415,230.9 us |   1,802.66 us |    942.82 us | 1,415,121.8 us | 3000.0000 | 6745.41 KB |
|          Dict_Mp3Mine | 1,267,611.4 us |   3,679.05 us |  2,433.46 us | 1,267,498.1 us | 2000.0000 | 4248.43 KB |
|       Dict_TxtBigMine |   557,862.4 us |   4,700.23 us |  2,797.03 us |   557,149.8 us | 1000.0000 | 2180.17 KB |

- Array with custom search algorithm (but still linear-ish)
|        Custom_TxtMine |       445.4 us |       8.30 us |      5.49 us |       444.6 us |   23.9258 |    50.3 KB |
|        Custom_IcoMine |       468.7 us |       5.09 us |      3.37 us |       468.3 us |   24.9023 |    51.2 KB |
|        Custom_PngMine |     9,490.9 us |      69.76 us |     36.48 us |     9,489.8 us |   15.6250 |   60.12 KB |
|         Custom_PdfMne | 1,761,919.0 us |  63,652.48 us | 37,878.57 us | 1,758,809.4 us | 1000.0000 | 2712.49 KB |
|        Custom_Mp3Mine | 1,620,604.3 us | 121,037.30 us | 80,058.68 us | 1,589,793.6 us |         - | 1653.22 KB |
|     Custom_TxtBigMine |   494,891.6 us |   7,001.92 us |  4,166.73 us |   495,872.8 us |         - | 1566.41 KB |

```
