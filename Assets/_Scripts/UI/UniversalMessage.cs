using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UniversalMessage : MonoBehaviour
{
    public static UniversalMessage Message;
    private TextMeshProUGUI _text;


    private void Awake()
    {
        if (Message == null)
        {
            Message = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SendText(string text)
    {
        _text.text += "\n /n" + text;
        // add a times to deletes the text or make it transparent ?
    }
}
