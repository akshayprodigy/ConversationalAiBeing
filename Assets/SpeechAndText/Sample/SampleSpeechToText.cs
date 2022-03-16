using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using TMPro;

public class SampleSpeechToText : MonoBehaviour
{
    //public GameObject loading;
    public InputField inputLocale;
    public TMP_InputField inputText;
    public GameObject enterText;
    public GameObject IMPV;
    public float pitch;
    public float rate;
    float clicked = 0;
    float clickedTime = 0;
    float clickDelay = 0.5f;
    public Text txtLocale;
    public Text txtPitch;
    public Text txtRate;
    public CallRestHandler callRestHandler;
    public ZeeAnimationController AnimationController;

    public delegate void AudioPaused();
    public static event AudioPaused OnAudioPaused;

    private void OnEnable()
    {
        CallRestHandler.OnAudioLoaded += PlayAudioAnimation;
        CallRestHandler.OnFileUploadedLoaded += FileUploaded;
        SpeechToText.onResultCallback += OnResultSpeech;
    }

    private void OnDisable()
    {
        CallRestHandler.OnAudioLoaded -= PlayAudioAnimation;
        CallRestHandler.OnFileUploadedLoaded -= FileUploaded;
        SpeechToText.onResultCallback -= OnResultSpeech;
    }

    void Start()
    {
        Setting("en-US");
        //loading.SetActive(false);
        
        callRestHandler = FindObjectOfType<CallRestHandler>();
        enterText.SetActive(false);
        IMPV.SetActive(false);
        AnimationController = FindObjectOfType<ZeeAnimationController>();

        pdfFileType = NativeFilePicker.ConvertExtensionToFileType("pdf"); // Returns "application/pdf" on Android and "com.adobe.pdf" on iOS
        textFileType = NativeFilePicker.ConvertExtensionToFileType("rtf");
        textFileDocxType = NativeFilePicker.ConvertExtensionToFileType("docx");
        textFileDocType = NativeFilePicker.ConvertExtensionToFileType("doc");
        Debug.Log("pdf's MIME/UTI is: " + pdfFileType);
    }

    public void OnStopNoding()
    {
        if (AnimationController == null)
            AnimationController = FindObjectOfType<ZeeAnimationController>();
        if (listing)
        {
            listing = false;
            AnimationController.UnlistenPose();
        }
        else if (nodding)
        {
            nodding = false;
            AnimationController.UnyesPose();
        }
        
    }

    public void FileUploaded(bool success)
    {
        if(success)
            inputText.text = "Uploaded successfully";
        else
            inputText.text = "Uploaded failed";
    }

    public void PlayAudioAnimation()
    {
        //if(AnimationController == null)
        //    AnimationController = FindObjectOfType<ZeeAnimationController>();
        //stopAllAnimation();
        //AnimationController.isSpeeking();
        Invoke("StopAudioAnimation", (CallRestHandler.audioLength+1));
    }

    public void StopAudioAnimation()
    {
        if (AnimationController == null)
            AnimationController = FindObjectOfType<ZeeAnimationController>();
        AnimationController.resetAnime();
        AnimationController.notSpeaking();
        //AnimationController.randomAnime(Random.Range(1, 9));
        //Invoke("stopAllAnimation", 2);
        OnAudioFinished();
    }

    public void stopAllAnimation()
    {
        if (AnimationController == null)
            AnimationController = FindObjectOfType<ZeeAnimationController>();
        AnimationController.resetAnime();
    }

    bool listing = false,nodding = false;
    public void StartRecording()
    {
        //AnimationController.ListenPose();
        if (AnimationController == null)
            AnimationController = FindObjectOfType<ZeeAnimationController>();
        //AnimationController.resetAnime();
        int val = Random.Range(0, 4);
        if (val == 3){
            listing = true;
            nodding = false;
            //AnimationController.ListingAnimation(false);
        }else if(val == 1){
            nodding = true;
            listing = false;
            //AnimationController.yesPose();
            //AnimationController.ListingAnimation(true);
        }
        //Invoke("OnStopNoding", 0.3f);
#if UNITY_EDITOR
#else
        SpeechToText.instance.StartRecording("Speak any");
#endif
    }

    public void StopRecording()
    {
        
            
#if UNITY_EDITOR
        //OnResultSpeech(inputText.text);
#else
        SpeechToText.instance.StopRecording();
#endif
#if UNITY_IOS
        //loading.SetActive(true);
#endif
    }


    private string pdfFileType;
    private string textFileType;
    private string textFileDocType;
    private string textFileDocxType;
    public void OpenFileBrowser()
    {
        // load gallery
        if (NativeFilePicker.IsFilePickerBusy())
            return;


        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                Debug.Log("Operation cancelled");
            else
            {
                Debug.Log("Picked file: " + path);
                inputText.text = "file selected: "+ path;
                Utils.fileName = path;
                Utils.isFileSelected = true;

            }

        }, new string[] { pdfFileType, textFileType, textFileDocType, textFileDocxType });

        Debug.Log("Permission result: " + permission);

    }

    public void CallResultForDebug()
    {
        if (Utils.isFileSelected)
        {
            callRestHandler.callfile();
            Utils.isFileSelected = false;
            Debug.Log(" file: " + Utils.fileName);
        }
        else
        {
            if (!string.Equals("", inputText.text))
                OnResultSpeech(inputText.text);
        }
        
    }

    public void OnResultSpeech(string _data)
    {
        inputText.text = _data;
        //Debug.LogError("zee OnResultSpeech: " + _data);
#if UNITY_IOS
        if(_data!=null && _data!= "nil")
        {
            if(string.Equals(_data.ToLower(),"how am i"))
            {
                Utils.checkEmotion = true;
            }
            else if (_data.ToLower().Contains("when someone is looking sad you should say"))
            {
                string newsadtext = _data.Substring(_data.IndexOf("say") + 4);
                //Debug.Log("newsadtext: " + newsadtext);
                PlayerPrefs.SetString(Utils.sadTextkey, newsadtext);
                //Utils.sadText = newsadtext;
                callRestHandler.callChat("mimic Ok, i will remember this", true);
            }
            else if (_data.ToLower().Contains("when someone is looking happy you should say"))
            {
                string newhappytext = _data.Substring(_data.IndexOf("say") + 4);
                //Debug.Log("newsadtext: " + newsadtext);
                PlayerPrefs.SetString(Utils.happyTextkey, newhappytext);
                //Utils.sadText = newsadtext;
                callRestHandler.callChat("mimic Ok, i will remember this", true);
            }
            else if(_data.ToLower().Contains("forget about emotion"))
            {
                PlayerPrefs.SetString(Utils.sadTextkey, Utils.sadText);
                PlayerPrefs.SetString(Utils.happyTextkey, Utils.happyText);
                //Utils.sadText = newsadtext;
                callRestHandler.callChat("mimic Ok, will do", true);
            }
            else
            {
                if (CallRestHandler.isChat)
                    callRestHandler.callChat(_data, true);
                else
                    callRestHandler.callMimic(_data);
            }
            
        }

#else
        if (_data != null)
        {
            if(CallRestHandler.isChat)
                callRestHandler.callChat(_data, true);
            else
                callRestHandler.callMimic(_data);

        }
            //callRestHandler.callMimic(_data);
#endif

        //if(loading.activeSelf)
        //    loading.SetActive(false);
        ////#if UNITY_IOS
        //        loading.SetActive(false);
        //#endif
    }

    public void OnClickSpeak()
    {
        TextToSpeech.instance.StartSpeak(inputText.text);
    }

    public void  OnClickStopSpeak()
    {
        //TextToSpeech.instance.StopSpeak();
        if (OnAudioPaused != null)
            OnAudioPaused();
        if(ZeeAnimationController.isZeeSpeaking)
        OnResultSpeech("MidInterrupt");

    }

    public void OnAudioFinished()
    {
        //OnResultSpeech("FinishedSpeaking");
    }

    public void OnImproveResponce()
    {
        OnResultSpeech("IMPV");
    }

    public void Setting(string code)
    {
        TextToSpeech.instance.Setting(code, pitch, rate);
        SpeechToText.instance.Setting(code);
        txtLocale.text = "Locale: " + code;
        txtPitch.text = "Pitch: " + pitch;
        txtRate.text = "Rate: " + rate;
    }

    public void OnClickApply()
    {
        Setting(inputLocale.text);
    }

    public void onPointerDown()
    {

        if (PinchDetection.Zooming)
            return;
        clicked++;
        if (clicked == 1)
        {
            if (IMPV.active)
                IMPV.SetActive(false);
            else
                IMPV.SetActive(true);

            clickedTime = Time.time;
        }
            
        if (clicked > 1 && Time.time - clickedTime < clickDelay)
        {
            IMPV.SetActive(false);
            clicked = 0;
            clickedTime = 0;
            if (enterText.active)
                enterText.SetActive(false);
            else
                enterText.SetActive(true);
        }
        else if (clicked > 2 || Time.time - clickedTime > 1)
            clicked = 0;
    }
}
