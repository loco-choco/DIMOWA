using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DIMOWAModLoader
{
    public class ClassSerializer
    {
        public static void WriteToFile<T>(string filePath, T save) where T : DIMOWASerializable
        {
            BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
            save.Serialize(ref writer);
            writer.Close();
        }
        public static void ReadFromFile<T>(string filePath, T deserializedObject) where T : DIMOWASerializable
        {
            BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            deserializedObject.Deserialize(ref reader);
            reader.Close();
        }
    }
    public abstract class DIMOWASerializable
    {
        public abstract void Serialize(ref BinaryWriter writer);
        public abstract void Deserialize(ref BinaryReader reader);
    }
}
