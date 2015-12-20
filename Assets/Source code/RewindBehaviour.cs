using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewindBehaviour : MonoBehaviour
{
    public float totalRewindTime = 5f;

    ObjectData[] storedData;
    int i = 0;
    int rewindIndex;
    int maxIndex;
    bool b_rewind;

    void Start()
    {
        storedData = new ObjectData[(int)(totalRewindTime/Time.fixedDeltaTime)];
        maxIndex = storedData.Length;
        Messenger.AddListener(EVENTID.EVENT_REWIND_TIME, OnEventRewindTime);
        b_rewind = false;
    }

    void FixedUpdate()
    {
        if (b_rewind)
            RewindTime();
        else
            StoreObjectData();
    }

    void StoreObjectData()
    {
        if (i < maxIndex)
        {
            storedData[i] = new ObjectData(transform.position, transform.eulerAngles);
            ++i;
        }
        else
            i = 0;
    }

    void RewindTime()
    {
        if (rewindIndex > 0)
        {
            --rewindIndex;
            --i;
            transform.position = storedData[rewindIndex].m_position;
            transform.eulerAngles = storedData[rewindIndex].m_eularAngle;
            storedData[rewindIndex] = null;
        }
        else
            Messenger.Broadcast(EVENTID.EVENT_REWIND_TIME);
    }

    void OnEventRewindTime()
    {
        rewindIndex = i;
        b_rewind = !b_rewind;
    }

}

public class ObjectData
{
    public ObjectData(Vector3 pos,Vector3 rot)
    {
        m_position = pos;
        m_eularAngle = rot;
    }
    public Vector3 m_position;
    public Vector3 m_eularAngle;
}
