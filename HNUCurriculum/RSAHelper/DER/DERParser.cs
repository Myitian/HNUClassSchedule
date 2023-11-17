using HNUCurriculum.RSAHelper.DER.Tags;

namespace HNUCurriculum.RSAHelper.DER;

/// <summary>
/// A simple DER parser for RSA keys.
/// </summary>
public class DERParser
{
    public static byte ReadByte(Stream s)
    {
        int read = s.ReadByte();
        if (read < 0)
        {
            throw new EndOfStreamException();
        }
        return (byte)read;
    }
    public static long ReadInt(Stream stream, out long readCount)
    {
        byte read = ReadByte(stream);
        readCount = 1;
        switch (read)
        {
            case 0x80:
                throw new NotImplementedException();
            case 0xFF:
                throw new NotSupportedException();
            default:
                if (read < 0x80)
                    return read;
                long result = 0;
                for (int i = read - 0x80; i > 0; i--)
                {
                    result = result << 8 | ReadByte(stream);
                    readCount++;
                }
                return result;
        }
    }
    internal static IASN1Tag ParseOneInternal(Stream stream, int read)
    {
        ASN1Type type = (ASN1Type)read;
        IASN1Tag tag = type.CreateInstance();
        tag.ParseDER(stream);
        return tag;
    }
    public static IASN1Tag? ParseOne(Stream stream)
    {
        int read = stream.ReadByte();
        return read >= 0 ? ParseOneInternal(stream, read) : null;
    }
    public static IEnumerable<IASN1Tag> Parse(Stream stream)
    {
        int read;
        while ((read = stream.ReadByte()) >= 0)
        {
            yield return ParseOneInternal(stream, read);
        }
    }
    public static IASN1Tag? ParseOne(byte[]? data)
    {
        if (data is null)
        {
            return null;
        }
        using MemoryStream ms = new(data);
        return ParseOne(ms);
    }
    public static IEnumerable<IASN1Tag> Parse(byte[]? data)
    {
        if (data is null)
        {
            return Enumerable.Empty<IASN1Tag>();
        }
        using MemoryStream ms = new(data);
        return Parse(ms, data.LongLength);
    }
    public static IEnumerable<IASN1Tag> Parse(Stream stream, long lengthLimit)
    {
        while (lengthLimit-- > 0)
        {
            ASN1Type type = (ASN1Type)ReadByte(stream);
            IASN1Tag tag = type.CreateInstance();
            lengthLimit -= tag.ParseDER(stream);
            yield return tag;
        }
        if (lengthLimit != -1)
        {
            throw new InvalidDataException();
        }
    }
}
