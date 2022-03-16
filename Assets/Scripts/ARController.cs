
using UnityEngine;
//using DG.Tweening;
//using com.tj.Events;

public class ArController : MonoBehaviour
{
    public enum ArState
    {
        idle,
        traking,
        objectSpawn,
    }


    private static ArController instance;
    public static ArController Instance
    {
        get
        {
            return instance;

        }
    }

    [SerializeField]
    private Material planeMaterial;

    [Header("AR")]

    [SerializeField]
    private LayerMask arLayer;
    [SerializeField]
    private Transform arMarker;
    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private Light arLight;



    private ArState State;
    private Vector3 markerStartScale;
    private bool tracking;

    //private ObjectAR objectArRef;
    //private ObjectAR currentARObject;


    private void Awake()
    {
        instance = this;
        markerStartScale = arMarker.localScale;
        ReInit();
    }

    public void ReInit()
    {
        EnableContainer(false);
        State = ArState.idle;
    }

    private void Start()
    {
        //EventManager<BackFrom3DEvent>.RegisterListener(BackFrom3DEventHandler);
    }

    //private void BackFrom3DEventHandler(BackFrom3DEvent evt)
    //{
    //    if (State != ArState.idle)
    //        EnableContainer(false);
    //}

    public void LoadArObject(string objectID)
    {
        //objectArRef = Resources.Load<ObjectAR>("ObjectAR/" + objectID);
        EnableContainer(true);
    }


    public void EnableContainer(bool enable)
    {
        arMarker.gameObject.SetActive(enable);
        arLight.enabled = enable;
        arCamera.enabled = enable;
        State = enable ? ArState.traking : ArState.idle;

        //planeMaterial.DOFloat(enable ? 1 : 0, "_Alpha", 0.5f);

        //if (currentARObject)
        //    Destroy(currentARObject.gameObject);
    }

    private void Update()
    {
        switch (State)
        {
            case ArState.traking:
                UpdateTracking();
                GetInput();
                break;
        }




    }

    private void GetInput()
    {
        if (tracking && Input.GetButtonDown("Fire1") )
            SpawnObject();
    }

    private void SpawnObject()
    {
        //arMarker.DOScale(0, 0.3f);
        ////currentARObject = Instantiate<ObjectAR>(objectArRef);
        //currentARObject.transform.position = arMarker.position;
        State = ArState.objectSpawn;

        //planeMaterial.DOFloat(0, "_Alpha", 0.5f);
    }

    private void UpdateTracking()
    {
        Ray ray = new Ray(arCamera.transform.position, arCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, arLayer))
        {
            arMarker.position = hit.point;
            arMarker.localScale += (markerStartScale - arMarker.localScale) * 0.3f;
            tracking = true;
        }
        else
        {
            tracking = false;
            arMarker.localScale += (Vector3.zero - arMarker.localScale) * 0.5f;
        }


    }

    public void Toggle3DContent()
    {
        //if (currentARObject != null)
        //{
        //    currentARObject.ToggleContent();
        //}
    }
}
