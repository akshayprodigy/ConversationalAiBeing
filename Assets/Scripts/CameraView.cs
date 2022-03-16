using UnityEngine;
using UnityEngine.UI;

public class CameraView : MonoBehaviour
{

    RawImage rawImage;
    AspectRatioFitter fitter;
    WebCamTexture webcamTexture;
    bool ratioSet;
    [SerializeField] private bool isFrontFacing = false;
    private WebCamDevice[] devices;
    private int deviceIndex;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        fitter = GetComponent<AspectRatioFitter>();
        InitWebCam();
    }

    void Update()
    {

        if (webcamTexture.width > 100 && !ratioSet)
        {
            ratioSet = true;
            SetAspectRatio();
        }
    }

    void SetAspectRatio()
    {
        fitter.aspectRatio = (float)webcamTexture.width / (float)webcamTexture.height;
    }

    void InitWebCam()
    {
        
        string camName = WebCamTexture.devices[0].name;
        if (Application.isMobilePlatform)
        {
            var webCamDevices = WebCamTexture.devices;
            foreach (var camDevice in webCamDevices)
            {
                if (camDevice.isFrontFacing)
                {
                    camName = camDevice.name;
                    break;
                }
            }


        }
        webcamTexture = new WebCamTexture(camName, Screen.width, Screen.height, 30);
        rawImage.texture = webcamTexture;
        webcamTexture.Play();

        /*
        devices = WebCamTexture.devices;
        string cameraName = Application.isEditor
            ? editorCameraName
            : WebCamUtil.FindName(preferKind, isFrontFacing);

        WebCamDevice device = default;
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].name == cameraName)
            {
                device = devices[i];
                deviceIndex = i;
                break;
            }
        }
        StartCamera(device);*/
    }

    public WebCamTexture GetCamImage()
    {
        return webcamTexture;
    }
}