using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using Unity.Collections;
using Unity.XR.CoreUtils;

public class QrCodeRecenter : MonoBehaviour
{
    [SerializeField]
    private ARSession session;

    [SerializeField]
    private XROrigin sessionOrigin;

    [SerializeField]
    private ARCameraManager cameraManager;

    [SerializeField]
    private GameObject targetParent; // GameObject whose children are target objects.

    private List<Target> navigationTargetObjects = new List<Target>();

    private Texture2D cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader();

    private void Awake()
    {
        // Populate navigationTargetObjects from children of targetParent
        foreach (Transform child in targetParent.transform)
        {
            // Since the Target class is now fully defined, create a new Target object
            Target target = new Target(child.name, child);  // Create a new Target instance
            navigationTargetObjects.Add(target);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetQrCodeRecenterTarget("MainEntrance_1");
        }
    }

    private void OnEnable()
    {
        cameraManager.frameReceived += OnCameraFrameReceived;
        Debug.Log("QR Code scanning enabled.");
    }

    private void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
        Debug.Log("QR Code scanning disabled.");
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        Debug.Log("Scanning for QR Codes...");
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            Debug.LogWarning("Failed to acquire latest CPU image.");
            return;
        }

        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.MirrorY
        };

        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);
        image.Convert(conversionParams, buffer);
        image.Dispose();

        cameraImageTexture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        cameraImageTexture.LoadRawTextureData(buffer);
        cameraImageTexture.Apply();
        buffer.Dispose();

        var result = reader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);
        if (result != null)
        {
            SetQrCodeRecenterTarget(result.Text);
        }
    }

    private void SetQrCodeRecenterTarget(string targetText)
    {
        Debug.Log($"Setting QR Code recenter target: {targetText}");
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(targetText.ToLower()));
        if (currentTarget != null)
        {
            session.Reset();
            sessionOrigin.transform.position = currentTarget.PositionObject.transform.position;
            sessionOrigin.transform.rotation = currentTarget.PositionObject.transform.rotation;
            Debug.Log("AR Session recentered based on QR Code.");
        }
        else
        {
            Debug.LogWarning("Target for the QR Code not found.");
        }
    }

    public void ChangeActiveFloor(string floorEntrance)
    {
        Debug.Log($"Changing active floor to: {floorEntrance}");
        SetQrCodeRecenterTarget(floorEntrance);
    }
}
