namespace Db4objects.Db4o.Foundation
{
    public class CRC32
    {
        private static uint[] crcTable;

        static CRC32()
        {
            BuildCRCTable();
        }

        private static void BuildCRCTable()
        {
            uint Crc32Polynomial = 0xEDB88320;
            uint i;
            uint j;
            uint crc;
            crcTable = new uint[256];
            for (i = 0; i <= 255; i++)
            {
                crc = i;
                for (j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = ((crc) >> (1 & 0x1f)) ^ Crc32Polynomial;
                    }
                    else
                    {
                        crc = crc >> (1 & 0x1f);
                    }
                }
                crcTable[i] = crc;
            }
        }

        public static long CheckSum(byte[] buffer, int start, int count)
        {
            uint temp1;
            uint temp2;
            int i = start;
            uint crc = 0xFFFFFFFF;
            while (count-- != 0)
            {
                temp1 = (crc) >> (8 & 0x1f);
                temp2 = crcTable[(crc ^ buffer[i++]) & 0xFF];
                crc = temp1 ^ temp2;
            }
            return (long)~crc & 0xFFFFFFFFL;
        }
    }
}
