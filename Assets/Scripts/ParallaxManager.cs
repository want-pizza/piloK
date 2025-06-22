using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public Material[] parallaxMaterials;
    public Camera mainCamera;

    void Update()
    {
        Vector2 camPos = mainCamera.transform.position;

        foreach (Material mat in parallaxMaterials)
        {
            if (mat.HasProperty("_CameraPos"))
            {
                mat.SetVector("_CameraPos", camPos);
            }
        }
    }
}
