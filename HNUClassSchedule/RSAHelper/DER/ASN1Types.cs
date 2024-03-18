namespace HNUClassSchedule.RSAHelper.DER;

public enum ASN1Type : byte
{
    EOC,
    INTEGER = 0x02,
    BIT_STRING = 0x03,
    OCTET_STRING = 0x04,
    NULL = 0x05,
    OBJECT_IDENTIFIER = 0x06,
    SEQUENCE = 0x30
}
