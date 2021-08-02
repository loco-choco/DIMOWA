using UnityEngine;
using System.Collections;
using System;

namespace CAMOWA
{
    public class WWWHelper
    {
        public static string PathToUrl(string filePath)
        {
            return "file:///" + filePath.Replace(" ", "%20");
        }
        

        public IEnumerator Import2DTexture(string filePath, Texture2D texture2D)
        {
            WWW www= new WWW (PathToUrl(filePath));

            yield return www;
            
            texture2D = www.texture;
            
            yield break;
        }

        public IEnumerator ImportWav(string filePath, AudioClip audioClip)
        {
            WWW www = new WWW(PathToUrl(filePath));

            yield return www;

            audioClip = www.audioClip;

            yield break;
        }
    }
}
