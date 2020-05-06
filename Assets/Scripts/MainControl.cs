using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainControl : MonoBehaviour
{

    public Location SelectedLocation;
    public Text infoText;
    public FadeControl fade;
    public GameObject sphere;
    public GameObject environment1;
    public int locationID;
    public Location onsightLoc;
    public GameObject infoBtn;
    public GameObject backBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnviorment()
    {
        infoText.text = SelectedLocation.description;
    }

    public void BackToSceneChoosing()
    {
        fade.StartCoroutine(fade.FadeIn());
        environment1.SetActive(true);
        sphere.SetActive(false);
    }
}
