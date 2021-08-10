using IMOWA.GUI;
using System;
using System.IO;

namespace IMOWAGUI.IMOWA
{
    public class ClassSerializer
    {
        public static void WriteToFile<T>(string filePath, T save) where T : DIMOWASerializable
        {
            BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
            save.Serialize(ref writer);
            writer.Close();
            writer.Dispose();
        }
        public static void ReadFromFile<T>(string filePath, T deserializedObject) where T: DIMOWASerializable
        {
            BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            deserializedObject.Deserialize(ref reader);
            reader.Close();
            reader.Dispose();
        }
    }
    public abstract class DIMOWASerializable
    {
        public abstract void Serialize(ref BinaryWriter writer);
        public abstract void Deserialize(ref BinaryReader reader);
    }

    public class ModFolderAndList : DIMOWASerializable
    {
        public string ModFolder { get; set; }
        public string[] ModList { get; set; }

        public ModFolderAndList()
        {
            ModFolder = "";
            ModList = new string[0];
        }
        public override void Deserialize(ref BinaryReader reader)
        {
            ModFolder = reader.ReadString();
            ModList = new string[reader.ReadInt32()];
            for (int i = 0; i < ModList.Length; i++)
                ModList[i] = reader.ReadString();
        }
        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(ModFolder);
            writer.Write(ModList.Length);
            for (int i = 0; i < ModList.Length; i++)
                writer.Write(ModList[i]);
        }
    }
}
