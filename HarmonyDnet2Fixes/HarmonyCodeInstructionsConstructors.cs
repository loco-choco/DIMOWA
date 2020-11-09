using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection.Emit;


namespace HarmonyDNet2Fixes
{
    public class HarmonyCodeInstructionsConstructors
    {
        // --- CALLING

        /// <summary>Creates a CodeInstruction calling a method (CALL)</summary>
        /// <param name="type">The class/type where the method is declared</param>
        /// <param name="name">The name of the method (case sensitive)</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="generics">Optional list of types that define the generic version of the method</param>
        /// <returns>A code instruction that calls the method matching the arguments</returns>
        ///
        public static CodeInstruction Call(Type type, string name, Type[] parameters = null, Type[] generics = null)
        {
            var method = AccessTools.Method(type, name, parameters, generics);
            if (method is null) throw new ArgumentException($"No method found for type={type}, name={name}, parameters={parameters.Description()}, generics={generics.Description()}");
            return new CodeInstruction(OpCodes.Call, method);
        }

        /// <summary>Creates a CodeInstruction calling a method (CALL)</summary>
        /// <param name="typeColonMethodname">The target method in the form <c>TypeFullName:MethodName</c>, where the type name matches a form recognized by <a href="https://docs.microsoft.com/en-us/dotnet/api/system.type.gettype">Type.GetType</a> like <c>Some.Namespace.Type</c>.</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="generics">Optional list of types that define the generic version of the method</param>
        /// <returns>A code instruction that calls the method matching the arguments</returns>
        ///
        public static CodeInstruction Call(string typeColonMethodname, Type[] parameters = null, Type[] generics = null)
        {
            var method = AccessTools.Method(typeColonMethodname, parameters, generics);
            if (method is null) throw new ArgumentException($"No method found for {typeColonMethodname}, parameters={parameters.Description()}, generics={generics.Description()}");
            return new CodeInstruction(OpCodes.Call, method);
        }

        
        // --- FIELDS

        /// <summary>Creates a CodeInstruction loading a field (LD[S]FLD[A])</summary>
        /// <param name="type">The class/type where the field is defined</param>
        /// <param name="name">The name of the field (case sensitive)</param>
        /// <param name="useAddress">Use address of field</param>
        /// <returns></returns>
        public static CodeInstruction LoadField(Type type, string name, bool useAddress = false)
        {
            
            var field = AccessTools.Field(type, name);
            if (field is null) throw new ArgumentException($"No field found for {type} and {name}");
            return new CodeInstruction(useAddress ? (field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda) : (field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld), field);
        }
		
		

        /// <summary>Creates a CodeInstruction storing to a field (ST[S]FLD)</summary>
        /// <param name="type">The class/type where the field is defined</param>
        /// <param name="name">The name of the field (case sensitive)</param>
        /// <returns></returns>
        public static CodeInstruction StoreField(Type type, string name)
        {
            var field = AccessTools.Field(type, name);
            if (field is null) throw new ArgumentException($"No field found for {type} and {name}");
            return new CodeInstruction(field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field);
        }

    }
}
