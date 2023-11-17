using HNUCurriculum.RSAHelper.DER;
using HNUCurriculum.RSAHelper.DER.Tags;
using System.Security.Cryptography;

namespace HNUCurriculum.RSAHelper;

public static class RSAKeyConverter
{
    // This code partially refers to the implementation of JSEncryptRSAKey.parseKey() in JSEncrypt
    public static RSAParameters ParseKey(string keyString)
    {
        IASN1Tag? asn1 = DERParser.ParseOne(Convert.FromBase64String(keyString));
        if (asn1 is not null)
        {
            if (asn1.Children?.Count == 3)
            {
                asn1 = DERParser.ParseOne(asn1.Children[2].Content);
            }
            if (asn1?.Children?.Count == 9)
            {
                return new()
                {
                    Modulus = TrimLeadingZero(asn1.Children[1].Content),
                    Exponent = TrimLeadingZero(asn1.Children[2].Content),
                    D = TrimLeadingZero(asn1.Children[3].Content),
                    P = TrimLeadingZero(asn1.Children[4].Content),
                    Q = TrimLeadingZero(asn1.Children[5].Content),
                    DP = TrimLeadingZero(asn1.Children[6].Content),
                    DQ = TrimLeadingZero(asn1.Children[7].Content),
                    InverseQ = TrimLeadingZero(asn1.Children[8].Content)
                };
            }
            else if (asn1?.Children?.Count == 2)
            {
                if (asn1.Children[0].Children?.Any() == true)
                {
                    asn1 = DERParser.ParseOne(asn1.Children[1].Content);
                    if (asn1?.Children?.Count > 1)
                    {
                        return new()
                        {
                            Modulus = TrimLeadingZero(asn1.Children[0].Content),
                            Exponent = TrimLeadingZero(asn1.Children[1].Content)
                        };
                    }
                }
                else
                {
                    return new()
                    {
                        Modulus = TrimLeadingZero(asn1.Children[0].Content),
                        Exponent = TrimLeadingZero(asn1.Children[1].Content)
                    };
                }
            }
        }
        throw new InvalidDataException();
    }

    internal static byte[] TrimLeadingZero(byte[]? data)
    {
        if (data is null)
            return Array.Empty<byte>();
        if (data.Length > 1 && data[0] == 0)
        {
            byte[] result = new byte[data.Length - 1];
            Array.Copy(data, 1, result, 0, result.Length);
            return result;
        }
        return data;
    }
}
