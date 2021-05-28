using System;
using System.IO;
using Ionic.Zlib;

namespace CAMOWA.FBXRuntimeImporter
{
    public class FBXPacketReader : BinaryReader
    {
        public FBXPacketReader(byte[] data) : base(new MemoryStream(data))
        {
        }

        //Example: https://github.com/DinoChiesa/DotNetZip/blob/master/Examples/c%23/ZLIB/ZlibStreamExample.cs
        static void CopyStream(Stream copiedStream, Stream receivingStream)
        {
            byte[] tempBuffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = copiedStream.Read(tempBuffer, 0, tempBuffer.Length)) > 0)
                receivingStream.Write(tempBuffer, 0, bytesRead);

            receivingStream.Flush();
        }

        private byte[] ReadDataArrayBytes(byte arrayDataTypeSize)
        {
            uint arrayLength = ReadUInt32();
            uint encoding = ReadUInt32();
            uint compressedLength = ReadUInt32();
            if (encoding == 0)
                return ReadBytes(checked((int)(arrayDataTypeSize * arrayLength)));

            else if (encoding == 1)//https://archive.codeplex.com/?p=dotnetzip
            {
                MemoryStream decompresedArray = new MemoryStream();
                ZlibStream ZlibStream = new ZlibStream(decompresedArray ,CompressionMode.Decompress, true);
                CopyStream(new MemoryStream(ReadBytes(checked((int)compressedLength))), ZlibStream);
                return decompresedArray.ToArray();
            }
            else
                throw new Exception("This type of enconding wasn't normally observerd, here is the number that represents it: " + encoding);
        }

        public FBXProperty ReadProperty()
        {
            char caractere = ReadChar();
            byte[] dataInBytes;
            switch (caractere)
            {
                //Simples
                case 'C':
                    return new FBXProperty(ReadBoolean());

                case 'Y':
                    return new FBXProperty(ReadInt16());
                case 'I':
                    return new FBXProperty(ReadInt32());
                case 'L':
                    return new FBXProperty(ReadInt64());
                //IEEE 754
                case 'F':
                    return new FBXProperty(ReadSingle());
                case 'D':
                    return new FBXProperty(ReadDouble());

                //Complexos (Ahhhh eu n queria copiar tudo em todos, mas n sei como faria isso dentro de uma função ;-;)
                case 'b':
                    dataInBytes = ReadDataArrayBytes(1);
                    bool[] bools = new bool[dataInBytes.Length];
                    for (int index = 0; index < bools.Length; index++)
                        bools[index] = BitConverter.ToBoolean(dataInBytes, index);

                    return new FBXProperty(bools);

                case 'l':
                    dataInBytes = ReadDataArrayBytes(1);
                    long[] longs = new long[dataInBytes.Length / 8];
                    for (int index = 0; index < longs.Length; index++)
                        longs[index] = BitConverter.ToInt64(dataInBytes, index * 8);

                    return new FBXProperty(longs);

                case 'i':
                    dataInBytes = ReadDataArrayBytes(1);
                    int[] ints = new int[dataInBytes.Length / 4];
                    for (int index = 0; index < ints.Length; index++)
                        ints[index] = BitConverter.ToInt32(dataInBytes, index * 4);

                    return new FBXProperty(ints);

                case 'f':
                    dataInBytes = ReadDataArrayBytes(1);
                    float[] floats = new float[dataInBytes.Length / 4];
                    for (int index = 0; index < floats.Length; index++)
                        floats[index] = BitConverter.ToSingle(dataInBytes, index * 4);

                    return new FBXProperty(floats);

                case 'd':
                    dataInBytes = ReadDataArrayBytes(1);
                    double[] doubles = new double[dataInBytes.Length / 8];
                    for (int index = 0; index < doubles.Length; index++)
                        doubles[index] = BitConverter.ToDouble(dataInBytes, index * 8);

                    return new FBXProperty(doubles);

                //Tipos mais "exóticos"
                case 'S':
                    dataInBytes = ReadBytes(checked((int)ReadUInt32()));
                    return new FBXProperty(BitConverter.ToString(dataInBytes));

                case 'R': //Raw Bytes
                    return new FBXProperty(ReadBytes(checked((int)ReadUInt32())));
            }

            throw new Exception("Nao foi possivel ler o tipo dessa propiedade, o char lido fora: " + caractere);
        }
    }
}
