using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using Unity.Collections;
using Unity.XR.CoreUtils;

public class QrCodeRecenter : MonoBehaviour {
    [SerializeField]
    private ARSession session;
    [SerializeField]
    private XROrigin sessionOrigin;
    [SerializeField]
    private ARCameraManager cameraManager;
    [SerializeField]
    private TargetHandler targetHandler;
    [SerializeField]
    private GameObject qrCodeScanningPanel;


    private Texture2D cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader();
    private bool scanningEnabled = false;

    private void OnEnable() {
        cameraManager.frameReceived += OnCameraFrameReceived;
        Debug.Log("QR Code scanning enabled.");
    }

    private void OnDisable() {
        cameraManager.frameReceived -= OnCameraFrameReceived;
        Debug.Log("QR Code scanning disabled.");
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs) {
        if (!scanningEnabled) {
            return;
        }

        Debug.Log("Scanning for QR Codes...");
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)) {
            Debug.LogWarning("Failed to acquire latest CPU image.");
            return;
        }

        // [Image conversion and barcode reading logic remains the same]

        if (result != null) {
            Debug.Log($"QR Code detected: {result.Text}");
            SetQrCodeRecenterTarget(result.Text);
            ToggleScanning();
        } else {
            Debug.Log("No QR Code detected in the current frame.");
        }

        // Dispose of the texture if it's being recreated every frame
        if (cameraImageTexture != null) {
            Destroy(cameraImageTexture);
        }
    }

    private void SetQrCodeRecenterTarget(string targetText) {
        Debug.Log($"Setting QR Code recenter target: {targetText}");
        TargetFacade currentTarget = targetHandler.GetCurrentTargetByTargetText(targetText);
        if (currentTarget != null) {
            session.Reset();
            sessionOrigin.transform.position = currentTarget.transform.position;
            sessionOrigin.transform.rotation = currentTarget.transform.rotation;
            Debug.Log("AR Session recentered based on QR Code.");
        } else {
            Debug.LogWarning("Target for the QR Code not found.");
        }
    }

    public void ChangeActiveFloor(string floorEntrance) {
        Debug.Log($"Changing active floor to: {floorEntrance}");
        SetQrCodeRecenterTarget(floorEntrance);
    }

    public void ToggleScanning() {
        scanningEnabled = !scanningEnabled;
        qrCodeScanningPanel.SetActive(scanningEnabled);
        Debug.Log($"Scanning toggled. Scanning enabled: {scanningEnabled}");
    }
}
