using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationsController : MonoBehaviour
{
    public YourLocations yourLocs;
    public AllLocations allLocs;

    public Material sphereMat;
    public string description;
    public string name;

    public GameObject scene1;
    public GameObject sphere;

    public FadeControl locations;
    public GameObject canvas;
    public GameObject canvas2;
    public MainControl main;

    public void SelectLocationtoSee(Location loc)
    {
        // hago lo que tengo que hacer
        sphereMat.mainTexture = loc.img;
        name = loc.name;
        description = loc.description;
        scene1.SetActive(false);
        sphere.SetActive(true);
        main.SelectedLocation = loc;
        StartCoroutine(CanvasDissapear());

    }
    IEnumerator CanvasDissapear()
    {
        yield return locations.StartCoroutine(locations.FadeOut());
        canvas2.SetActive(true);
        canvas.SetActive(false);

    }

    
}
