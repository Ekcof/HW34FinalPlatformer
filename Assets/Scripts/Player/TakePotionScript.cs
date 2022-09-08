using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePotionScript : MonoBehaviour
{
    [SerializeField] private int minHealPoints;
    [SerializeField] private int maxHealPoints;
    private Health health;
    private Health selfHealth;

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
                health = other.transform.parent.GetComponent<Health>();
                health.SetDamage(maxHealPoints*-1, minHealPoints*-1);
                selfHealth.Death();
            }
        }
    }
}
