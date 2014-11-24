package com.db4o.foundation;

/**
 * @sharpen.ignore
 */
public class CRC32
{
   private static int crcTable[];

   static {
      buildCRCTable();     
   }

   private static void buildCRCTable()
   {
      final int CRC32_POLYNOMIAL = 0xEDB88320;

      int i, j;
      int crc;

      crcTable = new int[256];

      for (i = 0; i <= 255; i++)
      {
         crc = i;
         for (j = 8; j > 0; j--)
            if ((crc & 1) == 1)
               crc = (crc >>> 1) ^ CRC32_POLYNOMIAL;
            else
               crc >>>= 1;
         crcTable[i] = crc;
      }
   }

   public static long checkSum(byte buffer[], int start, int count)
   {
      int temp1, temp2;
      int i = start;

      int crc = 0xFFFFFFFF;

      while (count-- != 0)
      {
         temp1 = crc >>> 8;
         temp2 = crcTable[(crc ^ buffer[i++]) & 0xFF];
         crc = temp1 ^ temp2;
      }

      return (long) ~crc & 0xFFFFFFFFL;
   }
}
