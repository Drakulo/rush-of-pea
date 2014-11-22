using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public void OnTapRestart()
    {
        Application.LoadLevel("Play");
    }

    public void OnTapQuit()
    {
        Application.Quit();
    }
}
