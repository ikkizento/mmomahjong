using System;
using System.Collections.Generic;
using System.Text;

namespace Lidgren.Library.Network
{
	internal sealed class BigEndianBitWriter : BitWriter
	{
		private LittleEndianBitWriter m_littleWriter = new LittleEndianBitWriter();

		public override unsafe uint ReadUInt32(byte[] fromBuffer, int numberOfBits, int readBitOffset)
		{
			uint a = m_littleWriter.ReadUInt32(fromBuffer, numberOfBits, readBitOffset);

			// reorder bytes
			return
				((a & 0xff000000) >> 24) |
				((a & 0x00ff0000) >> 8) |
				((a & 0x0000ff00) << 8) |
				((a & 0x000000ff) << 24);
		}

		public override unsafe ulong ReadUInt64(byte[] fromBuffer, int numberOfBits, int readBitOffset)
		{
			ulong a = m_littleWriter.ReadUInt64(fromBuffer, numberOfBits, readBitOffset);

			// reorder bytes
			return ((a & 0xff00000000000000L) >> 56) |
				((a & 0x00ff000000000000L) >> 40) |
				((a & 0x0000ff0000000000L) >> 24) |
				((a & 0x000000ff00000000L) >> 8) |
				((a & 0x00000000ff000000L) << 8) |
				((a & 0x0000000000ff0000L) << 24) |
				((a & 0x000000000000ff00L) << 40) |
				((a & 0x00000000000000ffL) << 56);
		}

		public override int WriteUInt32(uint source, int numberOfBits, byte[] destination, int destBitOffset)
		{
			// reorder bytes
			uint reverse = ((source & 0xff000000) >> 24) |
				((source & 0x00ff0000) >> 8) |
				((source & 0x0000ff00) << 8) |
				((source & 0x000000ff) << 24);

			return m_littleWriter.WriteUInt32(reverse, numberOfBits, destination, destBitOffset);
		}

		public override int WriteUInt64(ulong source, int numberOfBits, byte[] destination, int destBitOffset)
		{
			ulong reverse = ((source & 0xff00000000000000L) >> 56) |
				((source & 0x00ff000000000000L) >> 40) |
				((source & 0x0000ff0000000000L) >> 24) |
				((source & 0x000000ff00000000L) >> 8) |
				((source & 0x00000000ff000000L) << 8) |
				((source & 0x0000000000ff0000L) << 24) |
				((source & 0x000000000000ff00L) << 40) |
				((source & 0x00000000000000ffL) << 56);
			return m_littleWriter.WriteUInt64(reverse, numberOfBits, destination, destBitOffset);
		}
	}
}
