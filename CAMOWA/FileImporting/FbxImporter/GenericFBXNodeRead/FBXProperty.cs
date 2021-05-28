using System;
using System.Collections.Generic;
using System.Text;

namespace CAMOWA.FBXRuntimeImporter
{
    public class FBXProperty
    {
        //I'm going to regret doing this
        public FBXPropertyType propertyType;
        //PLS, THIS CAN'T BE THE ONLY WAY WITHOUT REFLECTION 
        public bool boolProperty;
        public short int16Property;
        public int int32Property;
        public long int64Property;
        public float singleProperty;
        public double doubleProperty;

        public bool[] boolArrayProperty;
        public int[] int32ArrayProperty;
        public long[] int64ArrayProperty;
        public float[] singleArrayProperty;
        public double[] doubleArrayProperty;

        public string stringProperty;
        public byte[] rawBytesProperty;

        public FBXProperty(bool boolProperty){propertyType = FBXPropertyType.BOOL; this.boolProperty = boolProperty;}
        public FBXProperty(short int16Property) { propertyType = FBXPropertyType.INT16; this.int16Property = int16Property;}
        public FBXProperty(int int32Property) { propertyType = FBXPropertyType.INT32; this.int32Property = int32Property;}
        public FBXProperty(long int64Property) { propertyType = FBXPropertyType.INT64;this.int64Property = int64Property;}
        public FBXProperty(float singleProperty) { propertyType = FBXPropertyType.SINGLE; this.singleProperty = singleProperty;}
        public FBXProperty(double doubleProperty) { propertyType = FBXPropertyType.DOUBLE; this.doubleProperty = doubleProperty;}

        public FBXProperty(bool[] boolArrayProperty) { propertyType = FBXPropertyType.BOOL_ARRAY; this.boolArrayProperty = boolArrayProperty; }
        public FBXProperty(int[] int32ArrayProperty) { propertyType = FBXPropertyType.INT32_ARRAY; this.int32ArrayProperty = int32ArrayProperty;}
        public FBXProperty(long[] int64ArrayProperty) { propertyType = FBXPropertyType.INT64_ARRAY; this.int64ArrayProperty = int64ArrayProperty;}
        public FBXProperty(float[] singleArrayProperty) { propertyType = FBXPropertyType.SINGLE_ARRAY; this.singleArrayProperty= singleArrayProperty;}
        public FBXProperty(double[] doubleArrayProperty) { propertyType = FBXPropertyType.DOUBLE_ARRAY; this.doubleArrayProperty = doubleArrayProperty;}

        public FBXProperty(string stringProperty) { propertyType = FBXPropertyType.STRING; this.stringProperty = stringProperty;}
        public FBXProperty(byte[] rawBytesProperty) { propertyType = FBXPropertyType.RAW_BYTES; this.rawBytesProperty = rawBytesProperty;}

        override public string ToString()
        {
            string s = "";
            switch (propertyType)
            {
                case FBXPropertyType.BOOL:
                    return "Bool: " + boolProperty.ToString();
                case FBXPropertyType.INT16:
                    return "Int 16: " + int16Property.ToString();
                case FBXPropertyType.INT32:
                    return "Int32: " + int32Property.ToString();
                case FBXPropertyType.INT64:
                    return "Int64: " + int64Property.ToString();
                case FBXPropertyType.SINGLE:
                    return "Single: " + singleProperty.ToString();
                case FBXPropertyType.DOUBLE:
                    return "Double: " + doubleProperty.ToString();

                case FBXPropertyType.BOOL_ARRAY:
                    foreach (bool b in boolArrayProperty)
                        s += b.ToString() + " , ";
                    return $"Bool Array[{boolArrayProperty.Length}]: " + s;
                case FBXPropertyType.INT32_ARRAY:
                    foreach (int i in int32ArrayProperty)
                        s += i + " , ";
                    return $"Int32 Array[{int32ArrayProperty.Length}]: "+ s;
                case FBXPropertyType.INT64_ARRAY:
                    foreach (long l in int64ArrayProperty)
                        s += l + " , ";
                    return $"Int64 Array[{int64ArrayProperty.Length}]: "+ s;
                case FBXPropertyType.SINGLE_ARRAY:
                    foreach (float f in singleArrayProperty)
                        s += f + " , ";
                    return $"Single Array[{singleArrayProperty.Length}]: "+ s;
                case FBXPropertyType.DOUBLE_ARRAY:
                    foreach (int d in doubleArrayProperty)
                        s += d + " , ";
                    return $"Double Array[{doubleArrayProperty.Length}]: " + s;

                case FBXPropertyType.STRING:
                    return $"String[{stringProperty.Length}]: " + stringProperty + $" (In ASCII: {FromHexToChar(stringProperty)})";
                case FBXPropertyType.RAW_BYTES:
                    for (int i = 0; i < rawBytesProperty.Length; i++)
                        s += rawBytesProperty[i] + ((i%8==0 && i>0) ? " " : "");
                    return $"Raw Bytes[{rawBytesProperty.Length}]: " + s;
            }
            return s;
        }

        /// <summary>
        /// Only for the strings that come from the FBX file
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static public string FromHexToChar(string s)
        {
            string parsedString = "";
            try
            {
                for (int i = 0; i < s.Length; i += 3)
                    parsedString += Convert.ToChar(Convert.ToByte(s.Substring(i, 2), 16));
            }
            catch { }

            return parsedString;
        }
    }
    public enum FBXPropertyType : byte
    {
        BOOL,
        INT16,
        INT32,
        INT64,
        SINGLE,
        DOUBLE,
        BOOL_ARRAY,
        INT64_ARRAY,
        INT32_ARRAY,
        SINGLE_ARRAY,
        DOUBLE_ARRAY,
        STRING,
        RAW_BYTES
    }
}
