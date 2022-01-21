using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CertSnap : MonoBehaviour
{
    private static CertSnap instance;
    private Camera myCamera;
    private bool takeSnapNextFrame;
    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (takeSnapNextFrame)
        {
            takeSnapNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;
            
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Images/Screenshots/Certificate_VCIT.png", byteArray);
            Debug.Log("Saved Certificate");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }
    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeSnapNextFrame = true;
    }

    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }
}
