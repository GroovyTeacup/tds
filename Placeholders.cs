using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TDS
{
    public class Placeholders : MonoBehaviour
    {
        private void Awake()
        {
            TDSPlugin.Instance.LogInfo("test placeholders");

            TexturePlaceholder = new Texture2D(4, 4, TextureFormat.RGB24, true, false);
            TDSPlugin.Instance.LogInfo("crteated tex2d");
            TextureScript.ApplyTextSettings(TexturePlaceholder);
            TDSPlugin.Instance.LogInfo("applied settings");
            TexturePlaceholder.SetPixel(1, 1, Color.red);
            TexturePlaceholder.SetPixel(2, 1, Color.red);
            TexturePlaceholder.SetPixel(1, 2, Color.magenta);
            TexturePlaceholder.SetPixel(2, 2, Color.magenta);
            TDSPlugin.Instance.LogInfo("set pixels");
            TexturePlaceholder.Apply(true, false);
            TDSPlugin.Instance.LogInfo("apply");

            AudioPlaceholder = AudioClip.Create("TDS_Placeholder_Audio", 1, 1, 44100, false);
            TDSPlugin.Instance.LogInfo("audio");
        }

        public Texture2D TexturePlaceholder;
        public AudioClip AudioPlaceholder;
    }
}
