namespace CS_Gzip.Gzip.tools.HuffmanCodeImplementations;

internal interface ICanonicalHuffmanCode
{
    public static  abstract ICanonicalHuffmanCode NewHuff(in uint[] codeLengths);
    public uint DecodeNextSymbol(BitStream input);
}