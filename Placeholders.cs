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

            TexturePlaceholder = new Texture2D(4, 4, TextureFormat.RGB24, true, false);
            TextureScript.ApplyTextSettings(TexturePlaceholder);
            TexturePlaceholder.SetPixel(1, 1, Color.red);
            TexturePlaceholder.SetPixel(2, 1, Color.red);
            TexturePlaceholder.SetPixel(1, 2, Color.magenta);
            TexturePlaceholder.SetPixel(2, 2, Color.magenta);
            TexturePlaceholder.Apply(true, false);

            AudioPlaceholder = AudioClip.Create("TDS_Placeholder_Audio", 1, 1, 44100, false);
        }

        public Texture2D TexturePlaceholder;
        public AudioClip AudioPlaceholder;
    }
}
