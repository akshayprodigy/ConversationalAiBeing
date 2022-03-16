using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuObject;
    public GameObject CameraView;

    // Start is called before the first frame update
    void Start()
    {
        HideCameraView();
        HideMenuView();
    }

   public void HideCameraView()
    {
        CameraView.SetActive(false);
    }

    public void ShowCameraView()
    {
        CameraView.SetActive(true);
    }

    public void HideMenuView()
    {
        MenuObject.SetActive(false);

    }

    public void ShowMenuView()
    {
        MenuObject.SetActive(true);
    }

    public void ClickonCameraView()
    {
        if (CameraView.active)
            HideCameraView();
        else
            ShowCameraView();
    }
}
