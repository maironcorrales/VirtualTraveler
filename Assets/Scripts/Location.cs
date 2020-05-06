using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Location : MonoBehaviour
{
    // Start is called before the first frame update

    public Texture img;
    public string name;
    public float price;
    public bool purchased;

    public GameObject buyBtn;
    public Text priceText;
    public RawImage thumbnail;
    public Text nameText;
    public GameObject btnGo;

    public string description;

    public LocationsController controller;
    

    void Start()
    {
        controller = FindObjectOfType<LocationsController>();
        nameText.text = name;
        thumbnail.texture =img;
        if (purchased)
        {
            priceText.text = "";
            buyBtn.SetActive(false);
            btnGo.SetActive(true);
        }
        else
        {
            priceText.text += " " + price + "$";
            btnGo.SetActive(false);
        }
       
    }

    public void SelectLocation()
    {
        controller.SelectLocationtoSee(this.gameObject.GetComponent<Location>());
    }

    
        
}
