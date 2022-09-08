using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject panel;
    [SerializeField] private string panelAction;
    [SerializeField] private float delay;
    private Image image;
    private float opacity;
    private float timer = 0f;
    private Color color;
    private IEnumerator coroutine;
    private float current;
    private void Awake()
    {
        image = panel.GetComponent<Image>();
        color = image.color;
        switch (panelAction)
        {
            case "fadein": opacity = 1f; break;
            case "fadeout": opacity = 0f; break;
            default: opacity = 0f; break;
        }
        coroutine = DeleteObject(delay);
        StartCoroutine(coroutine);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        current = Mathf.Abs(opacity - (timer / delay));
        color.a = current;
        image.color = color;
    }

    private IEnumerator DeleteObject(float delay)
    {
        yield return new WaitForSeconds(delay + 0.1f);
        Destroy(gameObject);
    }
}
