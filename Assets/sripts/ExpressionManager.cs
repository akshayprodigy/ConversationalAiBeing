using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARKit;
using System.Linq;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;
using TMPro;

public class ExpressionManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private ExpressionConfiguration[] ExpresionConfigurations;

    [SerializeField]
    private string FORMAT_DEBUG_TEXT = "{0}({1}){2}(min={3} max={4})-{5}";

    [SerializeField]
    private float detectionRate = 1.0f;

    [SerializeField]
    private Color colorOnDetection = Color.red;

    [SerializeField]
    //private FaceScanToggleMaterial faceActiveMaterial;

    private Dictionary<string, float> currentBlendShapes =
        new Dictionary<string, float>();

    private bool detected = false;

    //private Dictionary<string, TextMeshProUGUI> currentBlendShapeUIs = new Dictionary<string, TextMeshProUGUI>();

    private ARKitFaceSubsystem faceSubsystem;
    private ARFace face;
    private bool blendShapesEnabled;
    private float detectionRateTimer = 0;


    void OnEnable()
    {
        SetupInitialBlendShapeValues();

        CreateDebugOverlays();

        CleanUp();
        face = GetComponent<ARFace>();
        face.updated += OnUpdated;

        ARFaceManager faceManager = FindObjectOfType<ARFaceManager>();

        if (faceManager != null) faceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;

        
    }



    void Update()
    {
        if (!blendShapesEnabled)
        {
            Debug.Log("Not configured or blendshapes are not enabled");
            //face = GetComponent<ARFace>();
            //face.updated -= OnUpdated;
            //face.updated += OnUpdated;
            return;
        }

        if (detectionRateTimer >= detectionRate)
        {
            DetectExpressions();
            detectionRateTimer = 0;
        }
        else
        {
            detectionRateTimer += Time.deltaTime * 1.0f;
        }
    }

    void OnDisable() => face.updated -= OnUpdated;

    void OnUpdated(ARFaceUpdatedEventArgs eventArgs) => UpdateFaceFeatures();

    void UpdateFaceFeatures()
    {
        blendShapesEnabled = true;
        
        using (var blendShapes = faceSubsystem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp))
        {
            foreach (var blendShape in blendShapes)
            {
                string blendShapeName = blendShape.blendShapeLocation.ToString();

                if (currentBlendShapes.TryGetValue(blendShapeName, out float coefficient))
                {
                    currentBlendShapes[blendShapeName] = blendShape.coefficient;
                }
            }
        }
        
    }

    void SetupInitialBlendShapeValues()
    {
        foreach (var expression in ExpresionConfigurations)
        {
            foreach (var range in expression.BlendShapeRanges)
            {
                currentBlendShapes.Add(range.BlendShape.ToString(), 0);
            }
        }
    }

    void CleanUp()
    {
        foreach (var configuration in ExpresionConfigurations)
        {
            foreach (var range in configuration.BlendShapeRanges)
            {
                range.DetectionCount = 0;
            }
        }
    }

    void CreateDebugOverlays()
    {/*
        //currentBlendShapeUIs = new Dictionary<string, TextMeshProUGUI>();
        //Transform overlay = GameObject.Find("VerticalGroup").transform;

        foreach (var configuration in ExpresionConfigurations)
        {
            foreach (var range in configuration.BlendShapeRanges)
            {
                GameObject rangeGo = new GameObject(range.BlendShape.ToString());
                TextMeshProUGUI rangeGoText = rangeGo.AddComponent<TextMeshProUGUI>();

                //currentBlendShapeUIs.Add(range.BlendShape.ToString(), rangeGoText);
                rangeGoText.color = Color.white;
                rangeGoText.fontSize = 20;
                rangeGoText.alignment = TextAlignmentOptions.Center;
                rangeGoText.text = string.Format(FORMAT_DEBUG_TEXT, configuration.name, 0, range.BlendShape, range.LowBound, range.UpperBound, "0");
                //rangeGo.transform.parent = overlay;
                rangeGo.transform.localScale = new Vector3(1, 1, 1);
                rangeGo.transform.localPosition = Vector3.zero;
            }
        }*/
    }

    void DetectExpressions()
    {
        detected = true;
        foreach (var configuration in ExpresionConfigurations)
        {
            foreach (var range in configuration.BlendShapeRanges)
            {
                string blendshapeName = range.BlendShape.ToString();
                if (currentBlendShapes.ContainsKey(blendshapeName))
                {
                    //TextMeshProUGUI currentBlendshapeText = currentBlendShapeUIs[blendshapeName];
                    float currentBlendshapeValue = currentBlendShapes[blendshapeName];

                    // offset values by sensitivity
                    float newLower = range.LowBound <= 0 ? 0 : range.LowBound;
                    float newUpper = range.UpperBound <= range.LowBound ? range.LowBound : range.UpperBound;
                    //currentBlendshapeText.text = string.Format(FORMAT_DEBUG_TEXT, configuration.name, range.DetectionCount, range.BlendShape, newLower, newUpper, currentBlendshapeValue.ToString());
                    if (currentBlendshapeValue >= newLower && currentBlendshapeValue <= newUpper)
                    {
                        //currentBlendshapeText.color = colorOnDetection;
                        if(detected)
                            detected = true;
                        range.DetectionCount += 1;

                        if (range.Action != null && !string.IsNullOrEmpty(range.Action.MethodName))
                        {
                            Invoke(range.Action.MethodName, range.Action.Delay);
                        }
                    }
                    else
                    {
                        detected = false;
                        //currentBlendshapeText.color = Color.white;
                    }
                       
                }
            }
            // update face
            //Debug.Log("configuration.name: " + configuration.name+ " configuration.BlendShapeRanges: "+ configuration.BlendShapeRanges.Length+ "  "+ detected);
            if (detected)
            {
                FaceUIManager.Instance.UpdateDetectionStatus(configuration.name, AreAllSet(configuration));
                break;
            }
            else
            {
                FaceUIManager.Instance.UpdateDetectionStatus("Neutral", AreAllSet(configuration));
            }

            detected = true;
                
        }
    }

    private bool AreAllSet(ExpressionConfiguration configuration)
    {
        //Debug.Log("Allset: " + detected);
        bool areAllSet = detected;//currentBlendShapeUIs.Values.Where(v => v.color == colorOnDetection).Count() == currentBlendShapeUIs.Keys.Count();

        if (configuration.Action != null && !string.IsNullOrEmpty(configuration.Action.MethodName))
        {
            Invoke(configuration.Action.MethodName, configuration.Action.Delay);
        }
        return areAllSet;
    }

}
