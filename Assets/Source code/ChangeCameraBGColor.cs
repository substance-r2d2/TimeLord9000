using UnityEngine;
using System.Collections;

public class ChangeCameraBGColor : MonoBehaviour {

    public Color endColor;
    Color initColor;

    bool b_rewind;

	void Start ()
    {
        initColor = Camera.main.backgroundColor;
        Messenger.AddListener(EVENTID.EVENT_REWIND_TIME, OnEventRewind);
        b_rewind = false;
	}
	
	void Update ()
    {
        if (b_rewind)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, endColor, 2f * Time.deltaTime);
        else
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, initColor, 2f * Time.deltaTime);
	}

    void OnEventRewind()
    {
        b_rewind = !b_rewind;
    }
}
