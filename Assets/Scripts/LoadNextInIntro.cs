
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextInIntro : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject AccessPanel;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("id"))
        {
            int id = Random.Range(0, 1000);
            PlayerPrefs.SetInt("id", id);
            PlayerPrefs.Save();
        }
    }
    IEnumerator Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {


            AndroidRuntimePermissions.Permission[] result = AndroidRuntimePermissions.RequestPermissions("android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.RECORD_AUDIO", "android.permission.CAMERA");//"android.permission.ACCESS_FINE_LOCATION", "android.permission.READ_PHONE_STATE",, "android.permission.READ_CONTACTS"
            if (result[0] == AndroidRuntimePermissions.Permission.Granted && result[1] == AndroidRuntimePermissions.Permission.Granted && result[2] == AndroidRuntimePermissions.Permission.Granted)//&& result [4] == AndroidRuntimePermissions.Permission.Granted && result [5] == AndroidRuntimePermissions.Permission.Granted && result[3] == AndroidRuntimePermissions.Permission.Granted
            {
                Invoke("loadFirstScene", 2);
                AccessPanel.SetActive(false);
            }
            else
            {
                AccessPanel.SetActive(true);
            }
        }else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            findWebCams();

            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                Debug.Log("webcam found");
                Invoke("loadFirstScene", 2);
                AccessPanel.SetActive(false);
            }
            else
            {
                Debug.Log("webcam not found");
            }
            /*
            findMicrophones();

            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
            if (Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Debug.Log("Microphone found");
                Invoke("loadFirstScene", 2);
                AccessPanel.SetActive(false);
            }
            else
            {
                Debug.Log("Microphone not found");
            }
            */
        }
        else
        {
            Invoke("loadFirstScene", 2);
            AccessPanel.SetActive(false);
        }

        yield return null;
            
    }

    public void OnLoadNextScene()
    {
        Invoke("loadFirstScene", 2);
        AccessPanel.SetActive(false);
    }

    void loadFirstScene()
    {
      SceneManager.LoadScene(1);
    }


    void findWebCams()
    {
        foreach (var device in WebCamTexture.devices)
        {
            Debug.Log("Name: " + device.name);
        }
    }

    void findMicrophones()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }
}
