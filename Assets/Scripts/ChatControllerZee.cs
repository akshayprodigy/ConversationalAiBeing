using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatControllerZee : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite userChatBarSprite;
    public Sprite zeeChatBarSprite;
    public Color userImageColor;
    public Color zeeImageColor;
    public Color userTextColor;
    public Color zeeTextColor;
    private VerticalLayoutGroup verticalLayoutGroup;

    [SerializeField]
    GameObject chatBarPrefab;
    [SerializeField]
    GameObject scrollContainer;
    [SerializeField]
    GameObject userChatlValue;
    [SerializeField]
    GameObject zeeChatlValue;
    [SerializeField]
    public TMP_InputField TMP_ChatInput;

    public CallRestHandler callRestHandler;

    public static ChatControllerZee instance;

    void OnEnable()
    {
        TMP_ChatInput.onSubmit.AddListener(AddToChatOutput);

    }

    void OnDisable()
    {
        TMP_ChatInput.onSubmit.RemoveListener(AddToChatOutput);

    }

    void AddToChatOutput(string msg)
    {
        TextChatReceived(msg, true);
#if UNITY_IOS
        if (msg != null && msg != "nil")
        {
            if (CallRestHandler.isChat)
                callRestHandler.callChat(msg, false);
            else
                callRestHandler.callMimic(msg);
        }

#else
        if (msg != null)
        {
            if(CallRestHandler.isChat)
                callRestHandler.callChat(msg, false);
            else
                callRestHandler.callMimic(msg);

        }
            //callRestHandler.callMimic(_data);
#endif
        TMP_ChatInput.text = string.Empty;
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void MessageFromUser()
    {
        string msg = TMP_ChatInput.text;
        TextChatReceived(msg, true);
#if UNITY_IOS
        if (msg != null && msg != "nil")
        {
            if (CallRestHandler.isChat)
                callRestHandler.callChat(msg, false);
            else
                callRestHandler.callMimic(msg);
        }

#else
        if (msg != null)
        {
            if(CallRestHandler.isChat)
                callRestHandler.callChat(msg, false);
            else
                callRestHandler.callMimic(msg);

        }
            //callRestHandler.callMimic(_data);
#endif
        TMP_ChatInput.text = string.Empty;
    }

    public void MessageFromServer(string msg)
    {
        TextChatReceived(msg, false);
    }


    public void TextChatReceived(string msg, bool isUser)
    {
        /*
        verticalLayoutGroup = scrollContainer.GetComponent<VerticalLayoutGroup>();
        StartCoroutine(ShowUserMsgCoroutine(msg, isUser));
        return;*/

        

        GameObject newChat;
        if (isUser)
        {
            newChat = Instantiate(userChatlValue) as GameObject;
        }
        else
        {
            newChat = Instantiate(zeeChatlValue) as GameObject;
        }
        //newChat.GetComponent<RectTransform>().rect. = scrollContainer.GetComponent<RectTransform>().rect.width;
        newChat.GetComponent<ChatMessageController>().InitilizedChat(msg);
        newChat.transform.parent = scrollContainer.transform;
        newChat.transform.localScale = Vector3.one;
        

    }

    IEnumerator ShowUserMsgCoroutine(string msg, bool isUser)
    {
        GameObject chatObj = Instantiate(chatBarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        chatObj.transform.SetParent(scrollContainer.transform, false);
        chatObj.SetActive(true);
        ChatMessageController clb = chatObj.GetComponent<ChatMessageController>();
        clb.InitilizedChat(msg);
        if (isUser)
        {
            clb.SetTextColor(userTextColor);
        }
        else
        {
            clb.SetTextColor(zeeTextColor);
        }
        yield return new WaitForEndOfFrame();
        float height = clb.GetTextRectHeight();
        float width = clb.GetTextRectWidth();
        clb.chatbarImage.rectTransform.sizeDelta = new Vector2(width + 5, height + 6);
        if (isUser)
        {
            clb.chatbarImage.sprite = userChatBarSprite;
            clb.chatbarImage.color = new Color(userImageColor.r, userImageColor.g, userImageColor.b, userImageColor.a);
            clb.chatbarImage.rectTransform.anchoredPosition = new Vector2(-3f, clb.chatbarImage.rectTransform.anchoredPosition.y);
        }
        else
        {
            clb.chatbarImage.sprite = zeeChatBarSprite;
            clb.chatbarImage.color = new Color(zeeImageColor.r, zeeImageColor.g, zeeImageColor.b, zeeImageColor.a);
            clb.chatbarImage.rectTransform.anchoredPosition = new Vector2(((scrollContainer.GetComponent<RectTransform>().rect.width - (verticalLayoutGroup.padding.left + verticalLayoutGroup.padding.right)) - chatObj.GetComponent<RectTransform>().rect.width), clb.chatbarImage.rectTransform.anchoredPosition.y);
        }

    }


}
