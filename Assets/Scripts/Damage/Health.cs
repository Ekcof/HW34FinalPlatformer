using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private bool hasAnimator;
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool deadLikeKinematic;
    [SerializeField] private GameObject damageText;
    [SerializeField] private int currentHealthPoints;
    [SerializeField] private Image healthBar;
    [SerializeField] private AudioClip audioClip;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;
    private Collider2D[] childColliders;
    private Health health;
    private GameObject canvas;
    private CreateLoot createLoot;
    private RespawnAfterDeathScript respawnAfterDeathScript;
    private Camera mainCamera;

    public int DataHealth { get { return currentHealthPoints; } private set { currentHealthPoints = value; } }

    private void Awake()
    {
        currentHealthPoints = maxHealthPoints;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        canvas = GameObject.Find("UICanvas");
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        mainCamera = Camera.main;
    }

    public void SetDamage(int minDamage, int maxDamage)
    {
        int damage = 0;
        if (maxDamage > minDamage)
        {
            damage = Random.Range(minDamage, (maxDamage + 1));
        }
        else
        {
            damage = minDamage;
        }
        currentHealthPoints -= damage;
        if (damageText != null) { MakeDamageHint(damage); };
        if (currentHealthPoints <= 0)
        {
            currentHealthPoints = 0;
            Death();

        }
        if (currentHealthPoints > maxHealthPoints)
        {
            currentHealthPoints = maxHealthPoints;
        }
        if (healthBar != null)
        {
            ChangeHealthBar();
        }
    }

    public void Death()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder -= 1;
        }
        else
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder -= 1;
            }
        }
        Vector2 position = transform.position;
        createLoot = GetComponent<CreateLoot>();
        if (createLoot != null)
        {
            createLoot.CreatePrefab();
        }
        if (hasAnimator)
        {
            animator.SetBool("Death", true);
            if (audioClip != null)
            {
                AudioSource.PlayClipAtPoint(audioClip, position);
            }
            if (!isPlayer)
            {
                DestroyColliders();
                if (deadLikeKinematic)
                {
                    rigidbody2d.velocity = new Vector2(0, 0);
                    rigidbody2d.isKinematic = true;
                }
            }
            else
            {
                PlayerBehavior playerBehavior = GetComponent<PlayerBehavior>();
                playerBehavior.SetOnDeath();
                CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
                if (cinemachineBrain != null)
                {
                    Destroy(cinemachineBrain);
                }
            }
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name != "Death")
                {
                    animator.SetBool(parameter.name, false);
                }
            }
        }
        respawnAfterDeathScript = GetComponent<RespawnAfterDeathScript>();
        if (respawnAfterDeathScript != null && gameObject != null)
        {
            respawnAfterDeathScript.StartRespawn();
        }
    }

    private void DestroyColliders()
    {
        childColliders = gameObject.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in childColliders)
        {
            Destroy(collider);
            Destroy(health);
        }
    }

    private void MakeDamageHint(in int damage)
    {
        if (damage > 0)
        {
            GameObject newHint = Instantiate(damageText, transform.position, Quaternion.identity);
            newHint.transform.SetParent(canvas.transform);
            Text createHint = newHint.transform.GetChild(0).GetComponent<Text>();
            if (isPlayer) createHint.color = new Color(0.4f, 0.4f, 1, 1);
            createHint.text = damage.ToString();
            Vector2 UIpos = new Vector2(newHint.transform.position.x, newHint.transform.position.y + 0.5f);
            newHint.transform.position = mainCamera.WorldToScreenPoint(UIpos);
            //transform.rotation = Quaternion.Euler(0.0f, сanvas.transform.rotation.eulerAngles.y, сanvas.transform.rotation.eulerAngles.z);
        }
    }

    public int GetDamage()
    {
        return currentHealthPoints;
    }

    public float GetDamageProportion()
    {
        float currentHealthFloat = currentHealthPoints;
        float maxHealthFloat = maxHealthPoints;
        float proportion = currentHealthFloat / maxHealthFloat;
        return proportion;
    }

    private void ChangeHealthBar()
    {
        float fillAmount = GetDamageProportion();
        healthBar.fillAmount = fillAmount;
    }
}
