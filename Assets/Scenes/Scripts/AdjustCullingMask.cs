using UnityEngine;

public class CullingMaskController : MonoBehaviour
{
    public Camera cameraToAdjust;
    public LayerMask environmentLayer;

    private void Start()
    {
        // Initially, remove the environment layer from the culling mask
        cameraToAdjust.cullingMask &= ~environmentLayer.value;
    }

    public void ToggleEnvironmentLayer()
    {
        // Toggle the environment layer on the camera's culling mask
        cameraToAdjust.cullingMask ^= environmentLayer.value;
    }
}