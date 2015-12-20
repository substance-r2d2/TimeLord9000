using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{

    public float blastMagnitude = 500f;
    public float blastRange = 2f;

    void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(this.transform.position, blastRange);
        foreach(var objectInRange in objectsInRange)
        {
            if (objectInRange.tag == "Player")
                continue;
            Vector3 dir = objectInRange.transform.position - this.transform.position;
            dir.Normalize();
            if (objectInRange.attachedRigidbody != null)
            {
                other.attachedRigidbody.AddForce(dir * blastMagnitude);
            }
        }
        Destroy(this.gameObject);
    }

}
