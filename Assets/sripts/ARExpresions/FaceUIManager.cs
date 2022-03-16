
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class FaceUIManager : Singleton<FaceUIManager>
{
    //[SerializeField]
    // private GameObject faceUI;

    //[SerializeField]
    //private Vector3 faceUIOffset = Vector3.zero;
    bool sadsaid = false;
    bool smilesaid = false;
    bool neutralsaid = false;

    [SerializeField]
    private GameObject face;

    [SerializeField]
    private Text expressionStatus;

    CallRestHandler callRestHandler;
    //private TextMeshProUGUI expressionStatus;
    

    private ARFaceManager arFaceManager;

    void Start()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        arFaceManager.facesChanged += FacesChanged;
        callRestHandler = FindObjectOfType<CallRestHandler>();
        //faceUI.transform.position = faceUIOffset;
    }

    public void UpdateDetectionStatus(string expressionName, bool detected)
    {

        //Debug.LogError("UpdateDetectionStatus: " + expressionName + " detected: " + detected);
        expressionStatus.text = detected ?
            $"<color=\"red\">{expressionName} EXPRESSION DETECTED</color>" :
            $"<color=\"white\">FACE EXPRESSION SCANNING...</color>";

        if(Utils.checkafterjoke && detected && !ZeeAnimationController.isZeeSpeaking)
        {

            if (string.Equals(expressionName.ToLower(), "sad"))
            {
                Utils.isMimicCalled = true;
                Utils.mimicCalledtext = "why are you sad it was a joke";
                callRestHandler.callChat("mimic why are you sad it was a joke", true);
            }
            else if (string.Equals(expressionName.ToLower(), "smile"))
            {
                Utils.isMimicCalled = true;
                Utils.mimicCalledtext = "Happy to see that you liked it";
                callRestHandler.callChat("mimic Happy to see that you liked it", true);// PlayerPrefs.GetString(Utils.happyTextkey, Utils.happyText)
            }
            Utils.checkjokeemotioncall = true;
            //Utils.checkafterjoke = false;
        }
        else if(Utils.checkafterjoke && Utils.nwutralafterjoke && !ZeeAnimationController.isZeeSpeaking && !neutralsaid)
        {
            Utils.isMimicCalled = true;
            Utils.mimicCalledtext = "please smile if you liked it";
            callRestHandler.callChat("mimic please smile if you liked it", true);
             //Utils.nwutralafterjoke = false;
             neutralsaid = true;
            Utils.checkjokeemotioncall = false;
        }

        if (!Utils.checkafterjoke && neutralsaid)
        {
            neutralsaid = false;
        }

        if(detected && string.Equals(expressionName.ToLower(), "sad") && !sadsaid && !ZeeAnimationController.isZeeSpeaking)
        {

            callRestHandler.callChat("mimic " + PlayerPrefs.GetString(Utils.sadTextkey,Utils.sadText), true);
            sadsaid = true;
            Invoke("Togglesaidsaid", 5);
        }

        if (detected && string.Equals(expressionName.ToLower(), "smile") && !smilesaid && !ZeeAnimationController.isZeeSpeaking)
        {

            callRestHandler.callChat("mimic " + PlayerPrefs.GetString(Utils.happyTextkey, Utils.happyText), true);
            smilesaid = true;
            Invoke("ToggleSmile", 5);
        }

        if (detected && Utils.checkEmotion)
        {
            if (string.Equals(expressionName.ToLower(), "sad"))
            {
                string sadtextdec = PlayerPrefs.GetString(Utils.sadTextkey, Utils.sadText);
                string saddec = "mimic you are looking sad now";
                if (!sadtextdec.Equals(Utils.sadText))
                {

                    saddec = saddec + " and " + sadtextdec;
                }
               
                callRestHandler.callChat(saddec , true);//PlayerPrefs.GetString(Utils.sadTextkey, Utils.sadText)
            }
            else if(string.Equals(expressionName.ToLower(), "smile"))
            {
                string happytextdec = PlayerPrefs.GetString(Utils.happyTextkey, Utils.happyText);
                string happydec = "mimic you are looking happy now";
                if (!happytextdec.Equals(Utils.happyText))
                {
                    happydec = happydec + " and " + happytextdec;
                }
                callRestHandler.callChat(happydec, true);
            }

            Utils.checkEmotion = false;
        }
    }


    void ToggleSmile()
    {
        smilesaid = false;
    }
    void Togglesaidsaid()
    {
        sadsaid = false;
    }

    void FacesChanged(ARFacesChangedEventArgs args)
    {
        if (args.updated != null && args.updated.Count > 0)
        {
           // face = args.updated[0].gameObject;
            //faceUI.transform.rotation = face.transform.rotation;
            //faceUI.transform.position = face.transform.position + faceUIOffset;
        }
    }

    void Update()
    {
#if UNITY_EDITOR
           // faceUI.transform.rotation = face.transform.rotation;
#endif
    }
}
