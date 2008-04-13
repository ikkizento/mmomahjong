using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lidgren.Library.Network
{
	internal sealed class LittleEndianBitWriter : BitWriter
	{
		public override unsafe uint ReadUInt32(byte[] fromBuffer, int numberOfBits, int readBitOffset)
		{
			Debug.Assert(((numberOfBits > 0) && (numberOfBits <= 32)), "ReadUInt32() can only read between 1 and 32 bits");

			if (numberOfBits == 32 && ((readBitOffset % 8) == 0))
			{
				fixed (byte* ptr = &(fromBuffer[readBitOffset / 8]))
				{
					return *(((uint*)ptr));
				}
			}

			uint returnValue;
			if (numberOfBits <= 8)
			{
				returnValue = ReadByte(fromBuffer, numberOfBits, readBitOffset);
				return returnValue;
			}
			returnValue = ReadByte(fromBuffer, 8, readBitOffset);
			numberOfBits -= 8;
			readBitOffset += 8;

			if (numberOfBits <= 8)
			{
				returnValue |= (uint)(ReadByte(fromBuffer, numberOfBits, readBitOffset) << 8);
				return returnValue;
			}
			returnValue |= (uint)(ReadByte(fromBuffer, 8, readBitOffset) << 8);
			numberOfBits -= 8;
			readBitOffset += 8;

			if (numberOfBits <= 8)
			{
				uint r = ReadByte(fromBuffer, numberOfBits, readBitOffset);
				r <<= 16;
				returnValue |= r;
				return returnValue;
			}
			returnValue |= (uint)(ReadByte(fromBuffer, 8, readBitOffset) << 16);
			numberOfBits -= 8;
			readBitOffset += 8;

			returnValue |= (uint)(ReadByte(fromBuffer, numberOfBits, readBitOffset) << 24);
			return returnValue;
		}

		public override unsafe ulong ReadUInt64(byte[] fromBuffer, int numberOfBits, int readBitOffset)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override int WriteUInt32(uint source, int numberOfBits, byte[] destination, int destBitOffset)
		{
			int returnValue = destBitOffset + numberOfBits;
			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)source, numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)source, 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)(source >> 8), numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)(source >> 8), 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)(source >> 16), numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)(source >> 16), 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			NetBase.BitWriter.WriteByte((byte)(source >> 24), numberOfBits, destination, destBitOffset);
			return returnValue;
		}

		public override int WriteUInt64(ulong source, int numberOfBits, byte[] destination, int destBitOffset)
		{
			int returnValue = destBitOffset + numberOfBits;
			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)source, numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)source, 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)(source >> 8), numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)(source >> 8), 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)(source >> 16), numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)(source >> 16), 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)(source >> 24), numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)(source >> 24), 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)(source >> 32), numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)(source >> 32), 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)(source >> 40), numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)(source >> 40), 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)(source >> 48), numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)(source >> 48), 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			if (numberOfBits <= 8)
			{
				NetBase.BitWriter.WriteByte((byte)(source >> 56), numberOfBits, destination, destBitOffset);
				return returnValue;
			}
			NetBase.BitWriter.WriteByte((byte)(source >> 56), 8, destination, destBitOffset);
			destBitOffset += 8;
			numberOfBits -= 8;

			return returnValue;
		}
	}
}
