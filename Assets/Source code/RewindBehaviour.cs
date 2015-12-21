using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewindBehaviour : MonoBehaviour
{
    public float totalRewindTime = 5f;

    ObjectData[] storedData;
    Vector3 prevVelocity;
    int i = 0;
    int rewindIndex;
    int maxIndex;
    bool b_rewind;
    Rigidbody2D m_rigidbody;
    RigidbodyConstraints2D initalConstraint;
    MANIPULATION_TYPE currentManipulationType;

    void Start()
    {
        storedData = new ObjectData[(int)(totalRewindTime/Time.fixedDeltaTime)];
        maxIndex = storedData.Length;
        Messenger.AddListener<MANIPULATION_TYPE>(EVENTID.EVENT_REWIND_TIME, OnEventRewindTime);
        b_rewind = false;
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (b_rewind)
            RewindTime();
        else if(currentManipulationType != MANIPULATION_TYPE.PAUSE)
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
        {
            m_rigidbody.velocity = Vector2.zero;
            Messenger.Broadcast<MANIPULATION_TYPE>(EVENTID.EVENT_REWIND_TIME, MANIPULATION_TYPE.NONE);
        }
    }

    void OnEventRewindTime(MANIPULATION_TYPE type)
    {
        switch(type)
        {
            case MANIPULATION_TYPE.REWIND:
                currentManipulationType = currentManipulationType == MANIPULATION_TYPE.REWIND ? MANIPULATION_TYPE.NONE : MANIPULATION_TYPE.REWIND;
                rewindIndex = i;
                b_rewind = !b_rewind;
                break;

            case MANIPULATION_TYPE.NONE:
                b_rewind = !b_rewind;
                break;

            case MANIPULATION_TYPE.PAUSE:
                pauseTime();
                break;
        }
    }

    void pauseTime()
    {
        currentManipulationType = currentManipulationType == MANIPULATION_TYPE.PAUSE ? MANIPULATION_TYPE.NONE : MANIPULATION_TYPE.PAUSE;
        if (m_rigidbody.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            m_rigidbody.velocity = prevVelocity;
            m_rigidbody.constraints = initalConstraint;
        }
        else
        {
            prevVelocity = m_rigidbody.velocity;
            initalConstraint = m_rigidbody.constraints;
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
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
