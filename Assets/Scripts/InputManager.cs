using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class InputManager : MonoBehaviour
{
    public float DetectionDistance = 50;
    public float SwipeDetection = 50;
    public float MobileSwipeDetection = 10;

    private bool _wasTouching;
    private bool _waitForRelease;

    private Vector2 _startPos;

    private float _currentDistance;

    private string _debug = "";
    private Vector2 _debugVector = Vector2.zero;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if(Input.GetMouseButton(0) && !_waitForRelease)
        {
            if (!_wasTouching)
            {
                // Store start point
                _startPos = Input.mousePosition;
                _wasTouching = true;
            }

            // calcul de la distance parcourue
            _currentDistance = Vector2.Distance(_startPos, Input.mousePosition);
            if (_currentDistance >= DetectionDistance)
            {
                CheckGesture((Vector3)_startPos - Input.mousePosition);
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            _wasTouching = false;
            _waitForRelease = false;
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0 && !_waitForRelease)
        {
            var touch = Input.GetTouch(0);
            var delta = touch.deltaPosition;
            var distance = delta.x * delta.x + delta.y * delta.y;
            if (distance >= DetectionDistance)
            {
                CheckGesture(delta * -1);
            }
        }
        else if (Input.touchCount == 0)
        {
            _waitForRelease = false;
        }
#endif
    }

    void OnGUI()
    {
        GUILayout.Label(_debug.ToString());
        GUILayout.Label(_debugVector.ToString());
    }

    private void CheckGesture(Vector3 delta)
    {
        _debugVector = delta;
        _debug = "";
        if (delta.x > MobileSwipeDetection)
        {
            // Swipe gauche
            print("swipe gauche");
            _debug = "gauche";

            FsmVariables.GlobalVariables.GetFsmBool("Strafe_To_LEFT").Value = true;
        }
        else if (delta.x < 0 && delta.x < MobileSwipeDetection)
        {
            // Swipe droit
            print("swipe droit");
            _debug = "droit";
            FsmVariables.GlobalVariables.GetFsmBool("Strafe_To_RIGHT").Value = true;
        }

        if (delta.y > MobileSwipeDetection)
        {
            // Swipe bas
            print("swipe bas");
            _debug += " bas";
        }
        else if (delta.y < 0 && delta.y < MobileSwipeDetection)
        {
            // Swipe haut
            print("swipe haut");
            _debug += " haut";
        }

        _waitForRelease = true;
    }
}
