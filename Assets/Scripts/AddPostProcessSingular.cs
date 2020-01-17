using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPostProcessSingular : MonoBehaviour {

    public Material postProcessingMat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        Graphics.Blit(source, destination, postProcessingMat);
    }
}
