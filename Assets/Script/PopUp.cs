using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PopUpText
{
    public string Title;
    public string Msg;
    public string LText;
    public string RText;

    public Action NegativeButtonClickAction = null;
    public Action PositiveButtonClickAction = null;
}

public class PopUp : MonoBehaviour
{
    [SerializeField] Canvas PopupCanvas;
    [SerializeField] TextMeshProUGUI PopUpTitleText;
    [SerializeField] TextMeshProUGUI MsgText;
    public Button LeftButton;
    public Button RightButton;
    Action<bool> Callback;

    PopUpText popUpWindow = new PopUpText();

    public static PopUp Instance;

    private void Awake()
    {
        Instance = this;

        LeftButton.onClick.RemoveAllListeners();
        LeftButton.onClick.AddListener(Hide);

        RightButton.onClick.RemoveAllListeners();
        RightButton.onClick.AddListener(Hide);
    }

    public PopUp SetTitle(string PopUpTitle)
    {
        popUpWindow.Title = PopUpTitle;
        return Instance;
    }

    public PopUp SetMsg(string PopUpMsg)
    {
        popUpWindow.Msg = PopUpMsg;
        return Instance;
    }

    public PopUp SetLeftButtonText(string PopUpLText)
    {
        popUpWindow.LText = PopUpLText;
        return Instance;
    }

    public PopUp SetRightButtonText(string PopUpRText)
    {
        popUpWindow.RText = PopUpRText;
        return Instance;
    }

    public PopUp OnPositiveButtonClick(Action Positive)
    {
        popUpWindow.PositiveButtonClickAction = Positive;
        return Instance;
    }

    public PopUp OnNagetiveButtonClick(Action Nagetive)
    {
        popUpWindow.NegativeButtonClickAction = Nagetive;
        return Instance;
    }

    public void Show(Action<bool>Value = null)
    {
        PopUpTitleText.text = popUpWindow.Title;

        MsgText.text = popUpWindow.Msg;

        LeftButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = popUpWindow.LText;

        RightButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = popUpWindow.RText;

        Callback = Value; 
        //LeftButton.onClick.RemoveAllListeners();
        //LeftButton.onClick.AddListener(() => popUpWindow.PositiveButtonClickAction());

        //RightButton.onClick.RemoveAllListeners();
        //RightButton.onClick.AddListener(() => popUpWindow.NegativeButtonClickAction());

        PopupCanvas.enabled = true;
    }

    public void Leftb()
    {
        Application.Quit();
        //Callback(true);
    }

    public void Rightb()
    {
        Callback(false);
    }
    public void Hide()
    {
        popUpWindow = new PopUpText();

        PopupCanvas.enabled = false;
    }
        
}
