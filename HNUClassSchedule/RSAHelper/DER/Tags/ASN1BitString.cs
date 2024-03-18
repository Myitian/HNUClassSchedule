namespace HNUClassSchedule.RSAHelper.DER.Tags;

public class ASN1BitString : IASN1Tag
{
    private byte unusedBits;

    public ASN1Type ASN1Type => ASN1Type.BIT_STRING;
    public byte[]? Content { get; set; }
    public List<IASN1Tag>? Children => null;
    public byte UnusedBits
    {
        get => unusedBits;
        set
        {
            if (value > 7)
                throw new ArgumentOutOfRangeException(nameof(value));
            unusedBits = value;
        }
    }

    public long ParseDER(Stream stream)
    {
        long length = DERParser.ReadInt(stream, out long readCount);
        UnusedBits = DERParser.ReadByte(stream);
        length--;
        Content = new byte[length];
        for (long i = 0; i < length; i++)
        {
            Content[i] = DERParser.ReadByte(stream);
        }
        return length + readCount + 1;
    }
}
