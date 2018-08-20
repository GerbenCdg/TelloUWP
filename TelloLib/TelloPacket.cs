using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelloLib
{
    internal class TelloPacket
    {
        public byte[] Buffer { get; }

        public int Size
        {
            get => Buffer[1] + ((Buffer[2] << 8) >> 3);
        }

        public byte CRC_8 { get => Buffer[3]; }

        public bool FromDrone
        {
            get
            {
                var packetInfo = new BitArray(Buffer[4]);
                return packetInfo[0];
            }
        }

        public PacketType Type => (PacketType)((Buffer[4] >> 3) & 0x7);
        public PacketType SubType => (PacketType)(Buffer[4] & 0x7);

        public PacketMessageId MessageId => (PacketMessageId)(Buffer[5] + (Buffer[6] << 8));
        public int SequenceNumber => Buffer[7] + (Buffer[8] << 8);

        // TODO needs to be checked
        public byte[] Payload
        {
            get
            {
                int remainingLength = Buffer.Length - 9 - 2;
                
                var array = new byte[remainingLength];
                for (int i = 9; i < Buffer.Length - 2; i++)
                {
                    array[i - 9] = Buffer[i];
                }
                return array;
                // When using this, check if length of the array is 0.
            }
        }
        // TODO check this is valid
        public int CRC_16 => ((Buffer[Buffer.Length - 2] >> 8) + Buffer[Buffer.Length - 1]);


        public TelloPacket(byte[] buffer)
        {
            Buffer = buffer;
        }

        public enum PacketType : byte
        {
            Extended = 0,
            GetInfo = 1,
            Data1 = 2,
            Unknown1 = 3,
            Data2 = 4,
            SetInfo = 5,
            Flip = 6,
            Unknown2 = 7
        }

        public enum PacketMessageId
        {
            WifiStrength = 0x001a,
            TakeOff = 0x0054,
            Land = 0x0055,
        }
    }
}
