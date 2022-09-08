using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float rightRange;
    [SerializeField] private float velocityLimit;
    private float leftRange;
    private Rigidbody2D rigidBody;
    private void Awake()
    {
        leftRange = rightRange * -1;
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.angularVelocity = velocityLimit;

    }

    // Update is called once per frame
    void Update()
    {
        Push();
    }

    private void Push()
    {
        float zFloat = transform.rotation.z;
        if (zFloat > 0
        && zFloat < rightRange
        && rigidBody.angularVelocity > 0
        && rigidBody.angularVelocity < velocityLimit)
        { rigidBody.angularVelocity = velocityLimit; }
        else if (zFloat < 0
        && zFloat > leftRange
        && rigidBody.angularVelocity < 0
        && rigidBody.angularVelocity > velocityLimit * -1)
        {
            rigidBody.angularVelocity = velocityLimit * -1;
        }

    }


}
