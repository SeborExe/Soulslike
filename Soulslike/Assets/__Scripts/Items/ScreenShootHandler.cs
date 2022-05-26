using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShootHandler : MonoBehaviour
{
    Camera myCamera;

    bool takeScreenShoot;
    InputHandler inputHandler;

    private void Awake()
    {
        myCamera = Camera.main;
        inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        if (inputHandler.b_Input)
        {
            inputHandler.b_Input = false;
            TakeScreenShoot();
        }
    }

    private void OnPostRender()
    {
        if (takeScreenShoot)
        {
            takeScreenShoot = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D rendererResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            rendererResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = rendererResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Sprites/Icons/Icon.png", byteArray);
            Debug.Log("Screen Shoot has been save");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }

    public void TakeScreenShoot(int width = 256, int height = 256)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 24);
        takeScreenShoot = true;
    }
}
