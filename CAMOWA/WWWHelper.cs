using UnityEngine;
using System.Collections;
using System;

namespace CAMOWA
{
    public class WWWHelper
    {
        private static string PathToUrl(string filePath)
        {
            return "file:///" + filePath.Replace(" ", "%20");
        }

        //Should be the one used by the mods for safety reasons
        //Starts in "\OuterWilds_Alpha_1_2_Data\Assets\"
        public static string RelativePathToUrl(string relativeFilePath)
        {
            return PathToUrl(Application.dataPath + "/Assets/" + relativeFilePath);
        }

        public IEnumerator Import2DTexture(string filePath, Texture2D texture2D)
        {
            WWW www= new WWW (RelativePathToUrl(filePath));

            yield return www;
            
            texture2D = www.texture;
            
            yield break;
        }

        public IEnumerator ImportWav(string filePath, AudioClip audioClip)
        {
            WWW www = new WWW(RelativePathToUrl(filePath));

            yield return www;

            audioClip = www.audioClip;

            yield break;
        }
    }
}
