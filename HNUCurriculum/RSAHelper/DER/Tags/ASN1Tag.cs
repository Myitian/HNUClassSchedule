namespace HNUCurriculum.RSAHelper.DER.Tags;

public class ASN1Tag : IASN1Tag
{
    public ASN1Type ASN1Type { get; set; }
    public byte[]? Content { get; set; }
    public List<IASN1Tag>? Children => null;

    public long ParseDER(Stream stream)
    {
        long length = DERParser.ReadInt(stream, out long readCount);
        Content = new byte[length];
        for (long i = 0; i < length; i++)
        {
            Content[i] = DERParser.ReadByte(stream);
        }
        return length + readCount;
    }
}
