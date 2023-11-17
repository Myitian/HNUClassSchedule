using HNUCurriculum.RSAHelper.DER.Tags;

namespace HNUCurriculum.RSAHelper.DER;

public static class ASN1TypeExt
{
    public static IASN1Tag CreateInstance(this ASN1Type type)
    {
        return type switch
        {
            ASN1Type.SEQUENCE => new ASN1Sequence(),
            ASN1Type.BIT_STRING => new ASN1BitString(),
            _ => new ASN1Tag() { ASN1Type = type },
        };
    }
}
