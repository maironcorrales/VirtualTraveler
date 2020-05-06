using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeControl : MonoBehaviour
{
    public int id;
    public CanvasGroup canvasGroup;
    public int nextID;
    public MainUIControl uiControl;

    public IEnumerator FadeOut()
    {
        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.01f; 
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.01f;
            yield return null;
        }
    }

    public void SetNext(int next)
    {
        uiControl.activeID = next;
        nextID = next;
        uiControl.Fade(id,nextID);
    }
}
