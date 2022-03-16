using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessageController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TMP_Text textChat;
    [SerializeField]
    TMP_Text textChatParent;
    public Image chatbarImage;
    [SerializeField]
    TMP_Text timeChat;

    public void InitilizedChat(string msg)
    {
        var timeNow = System.DateTime.Now;
        textChat.text = msg;
        //textChatParent.text = msg;
        timeChat.text = timeNow.Hour.ToString("d2")+ ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2");
    }

    public void SetTextColor(Color color)
    {
        textChat.color = color;
    }

    public float GetTextRectHeight()
    {
        Debug.Log("Height : " + textChatParent.GetComponent<RectTransform>().rect.height);
        return textChatParent.GetComponent<RectTransform>().rect.height;
    }
    public float GetTextRectWidth()
    {
        return textChatParent.GetComponent<RectTransform>().rect.width;
    }

    public void SettextChat(string msg)
    {
        textChat.text = msg;
    }

}
