using System;
using System.Collections.Generic;

using System.Diagnostics;

namespace Lidgren.Library.Network
{
	internal abstract class BitWriter
	{
		/// <summary>
		/// Read 1-8 bits from a buffer into a byte
		/// </summary>
		public byte ReadByte(byte[] fromBuffer, int numberOfBits, int readBitOffset)
		{
			Debug.Assert(((numberOfBits > 0) && (numberOfBits < 9)), "Read() can only read between 1 and 8 bits");

			int bytePtr = readBitOffset >> 3;
			int startReadAtIndex = (readBitOffset % 8);

			if (startReadAtIndex == 0 && numberOfBits == 8)
				return fromBuffer[bytePtr];

			// "mask" away unused bits lower than (right of) relevant bits in first byte
			byte returnValue = (byte)(fromBuffer[bytePtr] >> startReadAtIndex);

			int numberOfBitsInSecondByte = numberOfBits - (8 - startReadAtIndex);

			if (numberOfBitsInSecondByte < 1)
			{
				// we don't need to read from the second byte, but we DO need
				// to mask away unused bits higher than (left of) relevant bits
				returnValue &= (byte)(255 >> (8 - numberOfBits));
				return returnValue;
			}

			byte second = fromBuffer[bytePtr + 1];

			// mask away unused bits higher than (left of) relevant bits in second byte
			second &= (byte)(255 >> (8 - numberOfBitsInSecondByte));

			returnValue |= (byte)(second << (numberOfBits - numberOfBitsInSecondByte));

			return returnValue;
		}

		/// <summary>
		/// Read 1-32 bits from a buffer into an unsigned integer
		/// </summary>
		public abstract unsafe uint ReadUInt32(byte[] fromBuffer, int numberOfBits, int readBitOffset);

		/// <summary>
		/// Read 1-64 bits from a buffer into an unsigned long
		/// </summary>
		public abstract unsafe ulong ReadUInt64(byte[] fromBuffer, int numberOfBits, int readBitOffset);

		/// <summary>
		/// Read several bytes from a buffer
		/// </summary>
		public void ReadBytes(byte[] fromBuffer, int numberOfBytes, int readBitOffset, byte[] destination, int destinationByteOffset)
		{
			int firstPartLen = (readBitOffset % 8);
			int readPtr = readBitOffset >> 3;

			if (firstPartLen == 0)
			{
				for (int i = 0; i < numberOfBytes; i++)
					destination[destinationByteOffset++] = fromBuffer[readPtr++];
				return;
			}

			int secondPartLen = 8 - firstPartLen;
			int secondMask = 255 >> secondPartLen;

			for (int i = 0; i < numberOfBytes; i++)
			{
				// "mask" away unused bits lower than (right of) relevant bits in byte
				int b = fromBuffer[readPtr] >> firstPartLen;

				readPtr++;

				// mask away unused bits higher than (left of) relevant bits in second byte
				int second = fromBuffer[readPtr] & secondMask;

				// combine
				b |= second << secondPartLen;

				destination[destinationByteOffset++] = (byte)b;
			}

			return;
		}

		/// <summary>
		/// Write a byte consisting of 1-8 bits to a buffer; assumes buffer is previously allocated
		/// </summary>
		public void WriteByte(byte source, int numberOfBits, byte[] destination, int destBitOffset)
		{
			Debug.Assert(((numberOfBits >= 1) && (numberOfBits <= 8)), "Must write between 1 and 8 bits!");

			// mask out unwanted bits in the source
			uint isrc = (uint)source & ((~(uint)0) >> (8 - numberOfBits));

			int bytePtr = destBitOffset >> 3;

			int localBitLen = (destBitOffset % 8);
			if (localBitLen == 0)
			{
				destination[bytePtr] = (byte)isrc;
				return;
			}

			destination[bytePtr] &= (byte)(255 >> (8 - localBitLen)); // clear before writing
			destination[bytePtr] |= (byte)(isrc << localBitLen); // write first half

			// need write into next byte?
			if (localBitLen + numberOfBits > 8)
			{
				destination[bytePtr + 1] &= (byte)(255 << localBitLen); // clear before writing
				destination[bytePtr + 1] |= (byte)(isrc >> (8 - localBitLen)); // write second half
			}

			return;
		}

		/// <summary>
		/// Write an unsigned integer consisting of 1-32 bits to a buffer; assumes buffer is previously allocated
		/// </summary>
		public abstract int WriteUInt32(uint source, int numberOfBits, byte[] destination, int destBitOffset);

		/// <summary>
		/// Write an unsigned long consisting of 1-64 bits to a buffer; assumes buffer is previously allocated
		/// </summary>
		public abstract int WriteUInt64(ulong source, int numberOfBits, byte[] destination, int destBitOffset);

		/// <summary>
		/// Write several whole bytes
		/// </summary>
		public void WriteBytes(byte[] source, int sourceByteOffset, int numberOfBytes, byte[] destination, int destBitOffset)
		{
			int dstBytePtr = destBitOffset >> 3;
			int firstPartLen = (destBitOffset % 8);

			if (firstPartLen == 0)
			{
				// optimized; TODO: write 64 bit chunks if possible
				for (int i = 0; i < numberOfBytes; i++)
					destination[dstBytePtr++] = source[sourceByteOffset + i];
				return;
			}

			int lastPartLen = 8 - firstPartLen;

			for (int i = 0; i < numberOfBytes; i++)
			{
				byte src = source[sourceByteOffset + i];

				// write last part of this byte
				destination[dstBytePtr] &= (byte)(255 >> lastPartLen); // clear before writing
				destination[dstBytePtr] |= (byte)(src << firstPartLen); // write first half

				dstBytePtr++;

				// write first part of next byte
				destination[dstBytePtr] &= (byte)(255 << firstPartLen); // clear before writing
				destination[dstBytePtr] |= (byte)(src >> lastPartLen); // write second half
			}

			return;
		}
	}
}
