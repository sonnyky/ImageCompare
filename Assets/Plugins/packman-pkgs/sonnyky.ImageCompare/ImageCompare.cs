using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageCompare : MonoBehaviour {

    [SerializeField]
    string m_ImagesPath;

    string[] m_ImageFilePaths;

	// Use this for initialization
	void Start () {
       
    }

    public static float CompareImages(string imA, string imB)
    {
        byte[] firstImage = File.ReadAllBytes(imA);
        byte[] secondImage = File.ReadAllBytes(imB);

        Texture2D texA = new Texture2D(320, 240);
        texA.LoadImage(firstImage);
        Texture2D resizedA = Resize(texA, 16, 16);
        Color[] colorsA = resizedA.GetPixels();

        Texture2D texB = new Texture2D(320, 240);
        texB.LoadImage(secondImage);
        Texture2D resizedB = Resize(texB, 16, 16);
        Color[] colorsB = resizedB.GetPixels();

        float diff = 0;

        for (int j = 0; j < colorsA.Length; j++)
        {
            Color color = colorsB[j] - colorsA[j];
            diff += Mathf.Sqrt((color.r * color.r) + (color.g * color.g) + (color.b * color.b));
        }

        float score = diff / 256;

        return score;
    }

    public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Point;
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D nTex = new Texture2D(newWidth, newHeight);
        nTex.ReadPixels(new Rect(0, 0, newWidth, newWidth), 0, 0);
        nTex.Apply();
        RenderTexture.active = null;
        return nTex;
    }
}
