using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogMessage : MonoBehaviour
{
    public Canvas canvas;
    public Sprite sprite;
    public string setName;
    public string messageText;
    public float time;
    public float delay = 0.02f;
    private Transform dialogPanel;
    private IEnumerator coroutine;
    private Text dialogText;
    private Text dialogName;
    private Image dialogImage;
    private DialogWindowParametersScript dialogParameters;
    private bool isCalledByTrigger = false;


    private void Awake()
    {
        if (canvas == null) { canvas = GameObject.Find("UICanvas").GetComponent<Canvas>(); }
        dialogPanel = canvas.transform.Find("DialogPanel");
        dialogText = dialogPanel.Find("Text").GetComponent<Text>();
        dialogName = dialogPanel.Find("DialogName").GetComponent<Text>();
        dialogImage = dialogPanel.Find("DialogImage").GetComponent<Image>();
        dialogParameters = dialogPanel.GetComponent<DialogWindowParametersScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCalledByTrigger)
        {
            if (other.transform.parent != null)
            {
                if (other.transform.parent.gameObject.layer == 8)
                {
                    isCalledByTrigger = true;
                    SendMessage(setName, messageText, sprite);
                }
            }
        }
    }

    public void SendMessage(string setName, string newText, Sprite sprite)
    {
        messageText = newText;
        dialogName.text = setName;
        dialogImage.sprite = sprite;
        coroutine = TypeText(delay, time);
        dialogPanel.gameObject.SetActive(true);
        StartCoroutine(coroutine);
    }

    public void SendMessageSignal(string setName, string newText, Sprite sprite)
    {
        messageText = newText;
        dialogName.text = setName;
        dialogImage.sprite = sprite;
        coroutine = TypeText(delay, time);
        dialogPanel.gameObject.SetActive(true);
        StartCoroutine(coroutine);
    }

    IEnumerator TypeText(float delayPerLetter, float commonTime)
    {
        dialogParameters.AddMessageInQueue();
        for (int i = 0; i < messageText.Length + 1; i++)
        {
            string messageCurrent = messageText.Substring(0, i);
            dialogText.text = messageCurrent;
            yield return new WaitForSeconds(delayPerLetter);
            commonTime -= delayPerLetter;
            if (dialogText.text.Length < messageCurrent.Length - 1) { break; };
        }
        if (commonTime < 0) { commonTime = 0; }
        yield return new WaitForSeconds(commonTime);
        dialogParameters.RemoveMessageInQueue();
        if (dialogParameters.GetMessagesInQueue() <= 0) dialogPanel.gameObject.SetActive(false);
        if (isCalledByTrigger) { Destroy(gameObject); }
    }
}
