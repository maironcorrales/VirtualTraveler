using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SR_VrRecticle : MonoBehaviour
{

    public float defaultDistance;

    public bool useNormal;

    public Image reticle;

    public Transform reticleTransform;

    public Transform camera;


    public Vector3 originalScale;
    public Quaternion originalRotation;

    private void Start()
    {
        originalRotation = reticleTransform.localRotation;
        originalScale = reticleTransform.localScale;
    }

    public void SetPosition(RaycastHit hit)
    {
        reticleTransform.position = hit.point;
        reticleTransform.localScale = originalScale * hit.distance;
        reticleTransform.localRotation = originalRotation;
    }

    public void SetPosition()
    {
        reticleTransform.localPosition = new Vector3(0, 0, defaultDistance);
        reticleTransform.localScale = originalScale;
        reticleTransform.localRotation = originalRotation;
    }
}
