namespace HNUClassSchedule.RSAHelper.DER.Tags;

public interface IASN1Tag
{
    public ASN1Type ASN1Type { get; }
    public byte[]? Content { get; }
    public List<IASN1Tag>? Children { get; }

    public abstract long ParseDER(Stream stream);
}
