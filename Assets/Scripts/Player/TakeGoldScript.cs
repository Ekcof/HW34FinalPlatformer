using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeGoldScript : MonoBehaviour
{
    [SerializeField] private int goldPoints;
    private Health selfHealth;
    private PlayerBehavior playerBehavior;

    private void Awake()
    {
        selfHealth = GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null)
        {
            if (other.transform.parent.gameObject.layer == 8)
            {
                playerBehavior = other.transform.parent.GetComponent<PlayerBehavior>();
                playerBehavior.AddGold(goldPoints);
                selfHealth.Death();
            }
        }
    }
}
