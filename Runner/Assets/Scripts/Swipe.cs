using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {
    private bool tap, swipeleft, swipeRight, swipeUp, swipeDown, isDragging;
    private Vector2 startTouch, swipeDelta;

    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeleft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    InputHandler handler;

    private void Start()
    {
        handler = GetComponent<InputHandler>();
    }


    private void Update()
    {
        tap = false;
        swipeleft = false;
        swipeRight = false;
        swipeUp = false;
        swipeDown = false;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDragging = true;
            startTouch = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(0))
        {
            Reset();
        }
        #endregion

        #region Mobile Inputs
        if(Input.touches.Length > 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDragging = true;
                startTouch = Input.touches[0].position;
            } else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                Reset();
            }
        }
        #endregion

        //Calculate the distance
        swipeDelta = Vector2.zero;
        if(isDragging)
        {
            if(Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            } else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        #region Depecrated Code
        /*
        //Did we cross the deadzone?

        if(swipeDelta.magnitude > 80)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or Right
                if (x < 0)
                {
                    swipeleft = true;
                } else
                {
                    swipeRight = true;
                }
            } else
            {
                //Up or down
                if (y < 0)
                {
                    swipeDown = true;
                } else
                {
                    swipeUp = true;
                }
            }

            Reset();
        }
        */
        #endregion
        if (swipeDelta.magnitude > handler.minSwipeDelta)
        {
            handler.GetSwipe(swipeDelta);
            Reset();
        }

    }

    private void Reset()
    {
        isDragging = false;
        startTouch = Vector2.zero;
        swipeDelta = Vector2.zero;
    }
}
