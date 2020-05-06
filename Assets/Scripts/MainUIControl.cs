using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainUIControl : MonoBehaviour
{
    public List<FadeControl> fades;
    public LocationsController locControl;
    public GameObject youLocations;
    public GameObject allLocations;
    public int activeID;

    

    public void Fade(int outID, int inID)
    {
        StartCoroutine(StartOperation(fades.ElementAt(outID), fades.ElementAt(inID)));
    }
    
    public IEnumerator StartOperation(FadeControl fadeOutCanvas, FadeControl fadeInCanvas)
    {
        yield return StartCoroutine(fadeOutCanvas.FadeOut());
        fadeOutCanvas.gameObject.SetActive(false);
        fadeInCanvas.gameObject.SetActive(true);
        StartCoroutine(fadeInCanvas.FadeIn());
    }

    public void ShowYorLocations()
    {
        allLocations.SetActive(false);
        youLocations.SetActive(true);
        youLocations.GetComponent<YourLocations>().LoadLocations();
    }

    
}
