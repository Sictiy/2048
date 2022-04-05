using UnityEngine;
using System;
using Assets.Scripts;

public enum State
{
    SwipeNotStarted,
    SwipeStarted
}

public class SwipeDetector : IInputDetector
{

    private State state = State.SwipeNotStarted;
    private Vector2 startPoint;
    private DateTime timeSwipeStarted;
    private TimeSpan maxSwipeDuration = TimeSpan.FromSeconds(1);
    private TimeSpan minSwipeDuration = TimeSpan.FromMilliseconds(100);

    public InputDirection? DetectInputDirection()
    {
#if UNITY_IPHONE || UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (state == State.SwipeNotStarted)
                {
                    state = State.SwipeStarted;
                    timeSwipeStarted = DateTime.Now;
                    startPoint = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (state == State.SwipeStarted)
                {
                    state = State.SwipeNotStarted;
                    return checkSwipeDirection(touch.position);
                }
            }
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            if (state == State.SwipeNotStarted)
            {
                state = State.SwipeStarted;
                timeSwipeStarted = DateTime.Now;
                startPoint = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (state == State.SwipeStarted)
            {
                state = State.SwipeNotStarted;
                return checkSwipeDirection(Input.mousePosition);
            }
        }
#endif
        return null;
    }

    private InputDirection? checkSwipeDirection(Vector2 mousePosition)
    {
        TimeSpan timeDifference = DateTime.Now - timeSwipeStarted;
        if (timeDifference > maxSwipeDuration || timeDifference < minSwipeDuration)
        {
            return null;
        }
        Vector2 differenceVector = mousePosition - startPoint;
        float angle = Vector2.Angle(differenceVector, Vector2.right);
        Vector3 cross = Vector3.Cross(differenceVector, Vector2.right);

        if (cross.z > 0)
            angle = 360 - angle;

        if ((angle >= 315 && angle < 360) || (angle >= 0 && angle <= 45))
            return InputDirection.Right;
        else if (angle > 45 && angle <= 135)
            return InputDirection.Top;
        else if (angle > 135 && angle <= 225)
            return InputDirection.Left;
        else
            return InputDirection.Bottom;

    }

}
