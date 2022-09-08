using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPlatformMoveScript : MonoBehaviour
{
    [SerializeField] private new string tag;
    private SliderJoint2D sliderJoint;

    private void Awake()
    {
        sliderJoint = GetComponent<SliderJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            sliderJoint.angle += 180;
        }
    }

}
