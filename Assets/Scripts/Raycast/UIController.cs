using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour {

    public Color colorForUI;
    public GameObject objetoApuntado;
    public Color actualColor;
    public Color appliedColor;
    public Color UItuto;

    public void GetActualImageColor(GameObject objeto)
    {
        
        objetoApuntado = objeto;
        if(objetoApuntado != null)
            actualColor = objetoApuntado.GetComponent<Image>().color;
    }

    public void ColorApply()
    {
        if (objetoApuntado != null)
            objetoApuntado.GetComponent<Image>().color = appliedColor;
    }

    public void ColorDiapply()
    {
        if (objetoApuntado != null)
            objetoApuntado.GetComponent<Image>().color = actualColor;
    }

    public void ColorApplyToUI()
    {
        if (objetoApuntado != null)
            objetoApuntado.GetComponent<Image>().color = colorForUI;
    }
    public void ColorApplyToUITuto()
    {
        if (objetoApuntado != null)
            objetoApuntado.GetComponent<Image>().color = UItuto;
    }

}
