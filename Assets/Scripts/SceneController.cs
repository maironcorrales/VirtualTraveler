using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    public List<TourScene> scenes;
    public TourScene selectedScene;
    public Material sceneMat;

    public void SelectScene(int id)
    {
        selectedScene = scenes.ElementAt(id);
        sceneMat.mainTexture = selectedScene.scenetexture;
    }

    
}
