﻿using System.IO;

namespace IMOWA
{
    public class DirectorySearchTools 
    {
        public static string GetFilePathInDirectory(string fileName, params string[] folders)
        {
            for (int i = 0; i < folders.Length; i++)
            {
                var possibleFiles = Directory.GetFiles(folders[i], fileName, SearchOption.AllDirectories);
                if (possibleFiles.Length > 0)
                    return possibleFiles[0];
            }
            throw new FileNotFoundException($"{fileName} was not found inside in any of the given folder or inside theirs children directories");
        }

        public static string GetDirectoryInDirectory(string directoryName, params string[] folders)
        {
            for (int i = 0; i < folders.Length; i++)
            {
                var possibleDirectories = Directory.GetDirectories(folders[i], directoryName, SearchOption.AllDirectories);
                if (possibleDirectories.Length > 0)
                    return possibleDirectories[0];
            }
            throw new DirectoryNotFoundException($"{directoryName}  was not found inside in any of the given folder or inside theirs children directories");
        }

        //TODO Local da implementacao para pegar as referencias
        //public static string[] ParseAssemblyReferences(System.Reflection.AssemblyName[] assemblyNames)
        //{
        //    return;
        //}
    }
}
