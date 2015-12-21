using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float maxVelocity = 5f;
    public float acceleration = 3f;
    public float forceMagnitude = 300f;
    public GameObject bulletPrefab;

    Rigidbody2D m_rigidbody;
    Vector2 movementVec;
    bool b_flipped;
    bool b_rewind;
    Transform bulletSpawnPos;
    MANIPULATION_TYPE currentTimeManipulation;

	void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        movementVec = new Vector2(maxVelocity, 0f);
        b_flipped = false;
        b_rewind = false;
        bulletSpawnPos = transform.FindChild("ShootingPos");
        Messenger.AddListener<MANIPULATION_TYPE>(EVENTID.EVENT_REWIND_TIME, OnEventRewind);
	}
	
	void Update ()
    {
        if (currentTimeManipulation != MANIPULATION_TYPE.REWIND)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-movementVec * Time.deltaTime);
                if (!b_flipped)
                {
                    b_flipped = true;
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(movementVec * Time.deltaTime);
                if (b_flipped)
                {
                    b_flipped = false;
                    transform.localScale = Vector3.one;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                m_rigidbody.velocity = Vector3.MoveTowards(m_rigidbody.velocity, Vector3.zero, acceleration * Time.fixedDeltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Mathf.Approximately(0f, m_rigidbody.velocity.y))
                    m_rigidbody.AddForce(new Vector2(0f, forceMagnitude));
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                GameObject bulletObj = GameObject.Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.identity) as GameObject;
                Rigidbody2D bulletRigidbody = bulletObj.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(bulletSpawnPos.right * 50f, ForceMode2D.Impulse);
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && (currentTimeManipulation == MANIPULATION_TYPE.NONE || currentTimeManipulation == MANIPULATION_TYPE.REWIND))
        {
            Messenger.Broadcast<MANIPULATION_TYPE>(EVENTID.EVENT_REWIND_TIME, MANIPULATION_TYPE.REWIND);
            currentTimeManipulation = (currentTimeManipulation == MANIPULATION_TYPE.NONE) ? MANIPULATION_TYPE.REWIND : MANIPULATION_TYPE.NONE;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) && (currentTimeManipulation == MANIPULATION_TYPE.NONE || currentTimeManipulation == MANIPULATION_TYPE.PAUSE))
        {
            Messenger.Broadcast<MANIPULATION_TYPE>(EVENTID.EVENT_REWIND_TIME, MANIPULATION_TYPE.PAUSE);
            currentTimeManipulation = (currentTimeManipulation == MANIPULATION_TYPE.NONE) ? MANIPULATION_TYPE.PAUSE : MANIPULATION_TYPE.NONE;
        }
    }

    void OnEventRewind(MANIPULATION_TYPE type)
    {
        if(type == MANIPULATION_TYPE.NONE)
            currentTimeManipulation = currentTimeManipulation == MANIPULATION_TYPE.REWIND ? MANIPULATION_TYPE.NONE : MANIPULATION_TYPE.REWIND;
        b_rewind = !b_rewind;
    }
}
