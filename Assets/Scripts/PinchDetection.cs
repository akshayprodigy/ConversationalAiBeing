using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    public Canvas canvas; // The canvas
    public float zoomSpeed = 0.5f;        // The rate of change of the canvas scale factor
    public GameObject camera, zoomout, zoomin;
    Vector3 newPosition;
    float prevDiff = 0;
    float deltaIncrease = 0.1f;
    float interpolate = 0;
    public bool isAR = false;
    public static bool Zooming = false;
    void Update()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // ... change the canvas size based on the change in distance between the touches.
            Debug.Log("zoom: " + deltaMagnitudeDiff * zoomSpeed+ " deltaMagnitudeDiff: "+ deltaMagnitudeDiff);
            //canvas.scaleFactor -= deltaMagnitudeDiff * zoomSpeed;
            if (deltaMagnitudeDiff > 0)
            {
                Debug.Log("zoom in: " );
                if(prevDiff > 0)
                {
                    interpolate += deltaIncrease;
                }
                else
                {
                    interpolate = 0;
                }
                if(!isAR)
                    newPosition = Vector3.Lerp(camera.transform.position, zoomin.transform.position, interpolate);
            }
            else
            {
                Debug.Log("zoom out: ");
                if (prevDiff < 0)
                {
                    interpolate += deltaIncrease;
                }
                else
                {
                    interpolate = 0;
                }
                if (!isAR)
                    newPosition = Vector3.Lerp(camera.transform.position, zoomout.transform.position, interpolate);
            }
            prevDiff = deltaMagnitudeDiff;
            if (!isAR)
                camera.transform.position = newPosition;
            //// Make sure the canvas size never drops below 0.1
            //canvas.scaleFactor = Mathf.Max(canvas.scaleFactor, 0.1f);
            Zooming = true;
        }
        else
        {
            Zooming = false;

        }
    }


}
