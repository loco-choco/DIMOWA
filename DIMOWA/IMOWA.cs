using dnlib.DotNet.Emit;
using dnpatch;
using System;
using System.IO;

namespace IMOWA
{
    public class IMOWA
    {

        /// <summary>
        /// Index of the installed mod, returns -1 if not installed
        /// </summary>
        /// <param name="modInnitTarget"></param>
        /// <param name="modPatcher"></param>
        /// <param name="occurence"></param>
        /// <returns></returns>
        public static int IndexOfInstalledMod(Target modInnitTarget, Patcher modPatcher, int occurence = 0)
        {
            Instruction[] instructionOfMethod = modPatcher.GetInstructions(modInnitTarget);

            int comparingIndex = 0; // Do method para conferir
            int instructionIndex = 0; // Do mod
            int occurenceCounter = 0;

            while (instructionIndex < modInnitTarget.Instructions.Length && comparingIndex < instructionOfMethod.Length)
            {
                if (modInnitTarget.Instructions[instructionIndex].Operand == null && instructionOfMethod[comparingIndex].Operand == null &&
                    modInnitTarget.Instructions[instructionIndex].OpCode.Name == instructionOfMethod[comparingIndex].OpCode.Name)
                {
                    comparingIndex++;
                    instructionIndex++;
                    if (instructionIndex == (modInnitTarget.Instructions.Length - 1))
                    {
                        if (occurenceCounter == occurence)
                            return comparingIndex - instructionIndex;
                        else
                            occurenceCounter++;
                    }
                }
                else if (modInnitTarget.Instructions[instructionIndex].OpCode.Name == instructionOfMethod[comparingIndex].OpCode.Name && 
                    modInnitTarget.Instructions[instructionIndex].Operand.ToString() == instructionOfMethod[comparingIndex].Operand.ToString())
                {
                    comparingIndex++;
                    instructionIndex++;
                    if (instructionIndex == (modInnitTarget.Instructions.Length - 1))
                    {
                        if (occurenceCounter == occurence)
                            return comparingIndex - instructionIndex;
                        else
                            occurenceCounter++;
                    }
                }
                else
                {
                    comparingIndex = comparingIndex - instructionIndex + 1;
                    instructionIndex = 0;
                }
            }
            return -1;
        }

        /// <summary>
        /// Uninstalls given mod, returns true it was succesfull
        /// </summary>
        /// <param name="modInnitTarget"></param>
        /// <param name="modPatcher"></param>
        /// <param name="modAlreadyInstalledIndex"></param>
        /// <returns></returns>
        public static bool UninstallMod(Target modInnitTarget, Patcher modPatcher, int modAlreadyInstalledIndex)
        {
            for (int i = 0; i < modInnitTarget.Indices.Length; i++)
                modInnitTarget.Indices[i] += modAlreadyInstalledIndex;
            modPatcher.RemoveInstruction(modInnitTarget);
            return true;
        }

        /// <summary>
        /// Installs given mod, returns true it was succesfull
        /// </summary>
        /// <param name="modInnitTarget"></param>
        /// <param name="modPatcher"></param>
        /// <returns></returns>
        public static bool InstallMod(Target modInnitTarget, Patcher modPatcher)
        {
            try
            {
                modPatcher.Patch(modInnitTarget);
            }
            catch (Exception exp)
            {
                throw new Exception($"Erro no Patching | Patching error: {exp}");
            }
            return true;

        }

        //public static int InstallOrUninstallMod(Target modInnitTarget, Patcher modPatcher, int modIndex, string modDllFile, string directoryPath)
        //{
        //    int modStatus = -1;
        //    if (modIndex > -1)//Desinstalar
        //    {
        //        Console.WriteLine('\n' + "Desinstalando| Uninstalling . . .");
        //        modStatus = UninstallMod(modInnitTarget, modPatcher, modIndex);
        //        if (File.Exists(directoryPath + '/' + modDllFile))
        //        {
        //            Console.WriteLine("O arquivo do mod ainda existe na pasta do jogo, deletando-o . . .");
        //            File.Delete(directoryPath + '/' + modDllFile);
        //        }

        //        if (modStatus == 0)
        //        {
        //            Console.WriteLine("O mod foi desinstalado com sucesso");
        //            Console.WriteLine("The mod was sucesffuly unistalled");
        //        }
        //        else
        //        {
        //            Console.WriteLine("O mod não foi desinstalado");
        //            Console.WriteLine("The mod was not uninstalled");
        //        }
        //    }
        //    else//Instalar
        //    {
        //        Console.WriteLine($"Instalando o mod | Instaling the mod. . .");
        //        modStatus = InstallMod(modInnitTarget, modPatcher);
        //        if (!File.Exists(directoryPath + '/' + modDllFile))
        //        {
        //            Console.WriteLine("O arquivo do mod não existe na pasta do jogo, copiando para la . . .");
        //            File.Copy(directoryPath + "/mods/" + modDllFile, directoryPath + '/' + modDllFile);
        //        }

        //        if (modStatus == 0)
        //        {
        //            Console.WriteLine("O mod foi instalado com sucesso");
        //            Console.WriteLine("The mod was sucesffuly installed");
        //        }
        //        else
        //        {
        //            Console.WriteLine("O mod não foi instalado");
        //            Console.WriteLine("The mod was not istalled");
        //        }
        //    }
        //    return modStatus;
        //}

    }
}
