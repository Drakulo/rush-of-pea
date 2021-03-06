﻿using UnityEngine;
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

    private bool isSwipe = false;
    private float fingerStartTime = 0.0f;
    private float maxSwipeTime = 0.5f;
    private float minSwipeDist = 50.0f;

    public AudioClip SlideLeft;
    public AudioClip SlideRight;
    public AudioClip TacleSound;
    public AudioClip[] JumpSounds;

    void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Q)) PlaySound(SlideLeft);
        else if(Input.GetKeyDown(KeyCode.D)) PlaySound(SlideRight);
        if(Input.GetKeyDown(KeyCode.Space)) PlaySound(JumpSounds[Random.Range(0, JumpSounds.Length)]);
        if (Input.GetKeyDown(KeyCode.S)) PlaySound(TacleSound);
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
            case TouchPhase.Began :
                /* this is a new touch */
                isSwipe = true;
                fingerStartTime = Time.time;
                _startPos = touch.position;
                break;
                     
            case TouchPhase.Canceled :
                /* The touch is being canceled */
                isSwipe = false;
                break;
                     
            case TouchPhase.Ended :
 
                float gestureTime = Time.time - fingerStartTime;
                float gestureDist = (touch.position - _startPos).magnitude;
                         
                if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist){
                    Vector2 direction = touch.position - _startPos;
                    Vector2 swipeType = Vector2.zero;
                         
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
                        // the swipe is horizontal:
                        swipeType = Vector2.right * Mathf.Sign(direction.x);
                    }else{
                        // the swipe is vertical:
                        swipeType = Vector2.up * Mathf.Sign(direction.y);
                    }
 
                    if(swipeType.x != 0.0f){
                        if(swipeType.x > 0.0f){
                            // MOVE RIGHT
                            FsmVariables.GlobalVariables.GetFsmBool("Strafe_To_RIGHT").Value = true;
                            PlaySound(SlideLeft);
                        }else{
                            // MOVE LEFT
                            FsmVariables.GlobalVariables.GetFsmBool("Strafe_To_LEFT").Value = true;
                            PlaySound(SlideRight);
                        }
                    }
 
                    if(swipeType.y != 0.0f ){
                        if(swipeType.y > 0.0f){
                            // MOVE UP
                            FsmVariables.GlobalVariables.GetFsmBool("JUMP").Value = true;
                            PlaySound(JumpSounds[Random.Range(0, JumpSounds.Length)]);
                        }else{
                            // MOVE DOWN
                            FsmVariables.GlobalVariables.GetFsmBool("TACLE").Value = true;
                            PlaySound(TacleSound);
                        }
                    }
 
                }
                         
                break;
            }
        }
#endif
    }

    void OnGUI()
    {
        GUILayout.Label(_debugVector.ToString());
    }
    private void CheckGesture(Vector3 delta)
    {
        _debugVector = delta;
        _debug = "";
        if (delta.x > MobileSwipeDetection)
        {
            // Swipe gauche
            //print("swipe gauche");
            _debug = "gauche";

            FsmVariables.GlobalVariables.GetFsmBool("Strafe_To_LEFT").Value = true;
        }
        else if (delta.x < 0 && delta.x < -MobileSwipeDetection)
        {
            // Swipe droit
            //print("swipe droit");
            _debug = "droit";
            FsmVariables.GlobalVariables.GetFsmBool("Strafe_To_RIGHT").Value = true;
        }

        else if (delta.y > MobileSwipeDetection)
        {
            // Swipe bas
            //print("swipe bas");
            _debug += " bas";
            FsmVariables.GlobalVariables.GetFsmBool("TACLE").Value = true;
        }
        else if (delta.y < 0 && delta.y < -MobileSwipeDetection)
        {
            // Swipe haut
            //print("swipe haut");
            _debug += " haut";
            FsmVariables.GlobalVariables.GetFsmBool("JUMP").Value = true;
        }

        _waitForRelease = true;
    }

    private void PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }
}
