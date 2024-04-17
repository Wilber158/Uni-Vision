using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEngine.UI;

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
    [SerializeField]
    private Slider floorSlider; // Slider component for selecting floors.

    private List<Target> navigationTargetObjects = new List<Target>();
    private Texture2D cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader();
    private float scanInterval = 2.0f; // Time in seconds between scans
    private float lastScanTime = 0;

    private void Awake()
    {
        cameraImageTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        foreach (Transform child in targetParent.transform)
        {
            Target target = new Target(child.name, child);
            navigationTargetObjects.Add(target);
        }
    }

    private void Start()
    {
        floorSlider.onValueChanged.AddListener(delegate { GetSliderValue(); });
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
        if (Time.time - lastScanTime < scanInterval)
            return;

        lastScanTime = Time.time;
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

        if (cameraImageTexture.width != conversionParams.outputDimensions.x || cameraImageTexture.height != conversionParams.outputDimensions.y)
        {
            cameraImageTexture.Reinitialize(conversionParams.outputDimensions.x, conversionParams.outputDimensions.y);
        }
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

    private void GetSliderValue()
    {
        int floorIndex = Mathf.RoundToInt(floorSlider.value);
        ChangeActiveFloor($"MainEntrance_{floorIndex}");
    }

    public void ChangeActiveFloor(string floorEntrance)
    {
        Debug.Log($"Changing active floor to: {floorEntrance}");
        SetQrCodeRecenterTarget(floorEntrance);
    }
}
