using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        GetComponent<UILabel>().text = PlayerPrefs.GetInt("HighScore").ToString();
	}
}
