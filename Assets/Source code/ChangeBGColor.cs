using UnityEngine;
using System.Collections;

public class ChangeBGColor : MonoBehaviour
{
    public float LerpSpeed = 2f;

    SpriteRenderer BG;
    Color endColor;
    Color initColor;

    bool b_rewind;

    void Start()
    {
        BG = GetComponent<SpriteRenderer>();
        initColor = Color.white;
        endColor = Color.black;
        Messenger.AddListener<MANIPULATION_TYPE>(EVENTID.EVENT_REWIND_TIME, OnEventRewind);
        b_rewind = false;
    }

    void Update()
    {
        if (b_rewind)
            BG.color = Color.Lerp(BG.color, endColor, LerpSpeed * Time.deltaTime);
        else
            BG.color = Color.Lerp(BG.color, initColor, LerpSpeed * Time.deltaTime);
    }

    void OnEventRewind(MANIPULATION_TYPE type)
    {
        b_rewind = !b_rewind;
    }
}
