using DigitalRubyShared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    float screenWidth = Screen.width;
    float screenHeight = Screen.height;

    public float buttonMoveScreenheightPercent = 0.4f;

    [SerializeField]
    PlayerMotor playerMotor;

    public float minSwipeDelta = 100f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetTap(GestureRecognizer gesture)
    {
        float x = gesture.FocusX;
        float y = gesture.FocusY;

        Debug.Log("Get tap x: " + x);
        Debug.Log("Get tap y: " + y);
        Debug.Log("Get tap screenHeight * buttonMoveScreenheightPercent: " + screenHeight * buttonMoveScreenheightPercent);

        if (y < screenHeight * buttonMoveScreenheightPercent)
        {
            //Clicked on the Leftside of the screen
            if (x < screenWidth / 2)
            {
               // playerMotor.InitiateLaneSwitch(-1f);
            }
            else
            {
              //  playerMotor.InitiateLaneSwitch(1f);
            }
        }
    }

    public void GetSwipe(GestureRecognizer gesture, SwipeGestureRecognizer swipeGesture)
    {

        DebugText("Swiped from {0},{1} to {2},{3}; velocity: {4}, {5}", gesture.StartFocusX, gesture.StartFocusY, gesture.FocusX, gesture.FocusY, swipeGesture.VelocityX, swipeGesture.VelocityY);
        DebugText("Swipe Delta:\n {0}", Mathf.Abs(gesture.StartFocusX - gesture.FocusX));

        //Swiped right or left
        if (Mathf.Abs(gesture.StartFocusX - gesture.FocusX) > minSwipeDelta)
        {
            if((gesture.StartFocusX - gesture.FocusX) < 0)
            {
                //Swiped Right
                playerMotor.InitiateLaneSwitch(1f);
            } else {
                //Swiped Left
                playerMotor.InitiateLaneSwitch(-1f);
            }
        }
    }


    private void DebugText(string text, params object[] format)
    {
        //bottomLabel.text = string.Format(text, format);
        Debug.Log(string.Format(text, format));
    }

    public void GetSwipe(Vector2 swipeDelta)
    {
        if (swipeDelta.magnitude > minSwipeDelta)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or Right
                if (x < 0)
                {
                    //Swiped Left
                    playerMotor.InitiateLaneSwitch(-1f);
                }
                else
                {
                    //Swiped Right
                    playerMotor.InitiateLaneSwitch(1f);
                }
            }
            else
            {
                //Up or down
                if (y < 0)
                {
                    //swipeDown = true;
                }
                else
                {
                    //swipeUp = true;
                }
            }
        }
    }
}
