namespace HNUCurriculum.RSAHelper.DER.Tags;

public class ASN1Sequence : IASN1Tag
{
    public ASN1Type ASN1Type => ASN1Type.SEQUENCE;

    public byte[]? Content => null;

    public List<IASN1Tag> Children { get; } = new();

    public long ParseDER(Stream stream)
    {
        long length = DERParser.ReadInt(stream, out long readCount);
        Children.AddRange(DERParser.Parse(stream, length));
        return length + readCount;
    }
}
