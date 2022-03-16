using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ArtaptoplaceObject : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject zee;
    private GameObject spawnedObject;
    public GameObject canvas;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPsition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    ARPlaneManager aRPlaneManager;
    ARRaycastManager aRRaycastManager;
    public AudioSource audioSource;

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }


    private void Start()
    {
        canvas.SetActive(false);
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition) || spawnedObject != null)
            return;
        if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            //Debug.Log("zee " + "indexer raycast"+ hits.Count);
            var hitPose = hits[0].pose;
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(zee, hitPose.position, hitPose.rotation);
                //spawnedObject.transform.LookAt(transform);
                //spawnedObject.transform.position = new Vector3(spawnedObject.transform.position.x, spawnedObject.transform.position.y, (spawnedObject.transform.position.z - 10));
                //spawnedObject.transform.RotateAround(spawnedObject.transform.position, Vector3.up, 180);
                Vector3 newDirection = transform.position - spawnedObject.transform.position;
                spawnedObject.transform.rotation = Quaternion.LookRotation(newDirection,spawnedObject.transform.up);
                spawnedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                aRPlaneManager.detectionMode = PlaneDetectionMode.None;
                aRPlaneManager.enabled = false;
                aRRaycastManager.enabled = false;
                //Debug.Log("spawnedObject: " + spawnedObject.transform.eulerAngles);
                spawnedObject.transform.eulerAngles = new Vector3(0, spawnedObject.transform.eulerAngles.y, 0); //= new Quaternion(0, spawnedObject.transform.rotation.y, 0, 1);
                //var planeList = new ARPlane[];
                var planeList = FindObjectsOfType<ARPlane>();
                foreach (ARPlane plane in planeList)
                    plane.gameObject.SetActive(false);
                canvas.SetActive(true);
                zee.GetComponent<ZeeAnimationController>().arlaunghPose();
                audioSource.Play();
            }
        }
        //Debug.LogError("zee " + "raycast not happening");
    }
}
