using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupScript : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image background;
    private Action _onClicked;

    public void Open(string content, Color color, Action action)
    {
        text.text = content;
        background.color = color;
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
