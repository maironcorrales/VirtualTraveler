using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonControl : MonoBehaviour
{

    public Image button;
    public Image image;
    public Sprite normalBtn;
    public Sprite highlightedBtn;
    public Sprite normalImage;
    public Sprite highlithedImage;
    public Text btnText;
    
    public void Highlight()
    {
        button.sprite = highlightedBtn;
        image.sprite = highlithedImage;
        btnText.color = Color.black;
    }

    public void BackToNormal()
    {
        button.sprite = normalBtn;
        image.sprite = normalImage;
        btnText.color = Color.white;
    }

    public void JustButtonHighlight()
    {
        button.sprite = highlightedBtn;
        btnText.color = Color.black;
    }

    public void JustButtonBackToNormal()
    {
        button.sprite = normalBtn;
        btnText.color = Color.white;
    }

    public void BtnWithoutTextHighlight()
    {
        button.sprite = highlightedBtn;
    }

    public void BtnWithoutTextBackToNormal()
    {
        button.sprite = normalBtn;
    }


}
