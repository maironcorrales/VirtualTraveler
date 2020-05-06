using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllLocations : MonoBehaviour
{
    public List<GameObject> locations;
    public List<GameObject> instanceLocs;
    public GameObject parent;
    public int cont;
    public Location onSightLocation;
    public MainControl mainCtrl;

    public void LoadLocations()
    {
        foreach (GameObject go in locations)
        {
            GameObject location = Instantiate(go, parent.transform);
            instanceLocs.Add(location);
            location.SetActive(false);
        }

        instanceLocs.ElementAt(0).SetActive(true);
        onSightLocation = instanceLocs.ElementAt(0).GetComponent<Location>();
        mainCtrl.onsightLoc = onSightLocation;
    }

    public void Next()
    {
        Debug.Log(instanceLocs.Count);
        if (cont < instanceLocs.Count - 1)
        {
            instanceLocs.ElementAt(cont).SetActive(false);
            cont++;
            instanceLocs.ElementAt(cont).SetActive(true);
            onSightLocation = instanceLocs.ElementAt(cont).GetComponent<Location>();
            mainCtrl.onsightLoc = onSightLocation;
        }
    }

    public void Prev()
    {
        if (cont > 0)
        {
            instanceLocs.ElementAt(cont).SetActive(false);
            cont--;
            instanceLocs.ElementAt(cont).SetActive(true);
            onSightLocation = instanceLocs.ElementAt(cont).GetComponent<Location>();
            mainCtrl.onsightLoc = onSightLocation;
        }
    }
}
