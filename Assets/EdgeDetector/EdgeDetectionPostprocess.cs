using UnityEngine;

[ExecuteInEditMode]
public class EdgeDetectionPostProcess : MonoBehaviour
{
    // This material should have your Sobel shader assigned.
    public Material edgeDetectionMaterial;

    // OnRenderImage is called after the camera finishes rendering.
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (edgeDetectionMaterial != null)
        {
            // Process the secondary camera's output using the Sobel shader.
            Graphics.Blit(source, destination, edgeDetectionMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}