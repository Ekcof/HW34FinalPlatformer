using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHit : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float radius;
    [SerializeField] private float damage;
    Transform[] array;
    private void Awake()
    {
        RaycastHit2D[] castStar = Physics2D.CircleCastAll(transform.position, radius, new Vector2(0, 0), 0);
        foreach (RaycastHit2D raycastHit in castStar)
        {
            Transform hitObject = raycastHit.transform;
            Health health = hitObject.GetComponent<Health>();
            if (health == null)
            {
                health = hitObject.GetComponentInParent<Health>();
            }
            if (health != null)
            {
                int estimatedDamage = Mathf.RoundToInt(Vector2.Distance(transform.position, hitObject.position) / radius * damage);
                if (estimatedDamage > 0) health.SetDamage(estimatedDamage, estimatedDamage);
            }
        }
    }


}
