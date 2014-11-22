using UnityEngine;
using System.Collections;

public class GameOverScore : MonoBehaviour
{
    void Start()
    {
        GetComponent<UILabel>().text = Score.GameScore.ToString();
    }
}
