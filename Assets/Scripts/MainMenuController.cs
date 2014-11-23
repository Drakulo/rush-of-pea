using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public void OnTapPlay()
    {
        Application.LoadLevel("Play");
    }

    public void OnTapExit()
    {
        Application.Quit();
    }
}
