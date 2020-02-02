using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupScript : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image background;
    private Action _onClicked;

    public void Open(
        string content,
        Color backgroundColor,
        Color textColor,
        Action action = null
    )
    {
        text.text = content;
        text.color = textColor;
        background.color = backgroundColor;
        _onClicked = action;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Click()
    {
        _onClicked?.Invoke();
    }
}
