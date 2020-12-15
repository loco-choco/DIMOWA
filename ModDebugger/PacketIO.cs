using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
//Coloquei dentro do MOdDebugger e do DIMOWAModLoader pq é um saco ter que ter + 2 dll. . .
namespace ModDebugger
{
    public class PacketWriter : BinaryWriter
    {
        private MemoryStream _memStream;

        public PacketWriter()
            : base()
        {
            _memStream = new MemoryStream();
            OutStream = _memStream;
        }

        public byte[] GetBytes()
        {
            Close();
            byte[] data = _memStream.ToArray();
            return data;
        }

    }

    public class PacketReader : BinaryReader
    {
        public PacketReader(byte[] data)
            : base(new MemoryStream(data))
        {

        }


    }
}
