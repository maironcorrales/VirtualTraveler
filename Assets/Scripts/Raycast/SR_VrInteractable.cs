using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SR_VrInteractable : MonoBehaviour {


    bool enable;
    bool validator;

    public void OnTriggerPressed()
    {
        if(enable)
            RaycastEventTriggerPressed();
    }

    public void OnHit()
    {
        if(enable)
            RacastHitEnter();
    }

    public void RayOut()
    {
        if(enable)
            RaycastOut();
    }

    [Serializable]
    public class RayOutEvent : UnityEvent { }


    [Serializable]
    public class RaycastHitEvent : UnityEvent { }

    [Serializable]
    public class RaycastEvent : UnityEvent { }

    [SerializeField]
    private RayOutEvent OnRaycastOut= new RayOutEvent();
    public RayOutEvent OnRayOut
    {
        get
        {
            return OnRaycastOut;
        }
        set
        {
            OnRaycastOut = value;
        }
    }

    public void RaycastOut()
    {
        OnRaycastOut.Invoke();
    }

    [SerializeField]
    private RaycastHitEvent OnHitEnter = new RaycastHitEvent();
    public RaycastHitEvent OnHitDetected
    {
        get
        {
            return OnHitEnter;
        }
        set
        {
            OnHitEnter = value;
        }
    }

    public void RacastHitEnter()
    {
        OnHitEnter.Invoke();
    }


    [SerializeField]
    private RaycastEvent OntriggerPressed = new RaycastEvent();

    public RaycastEvent OnPrimaryIndexTriggerPressed
    {
        get
        {
            return OntriggerPressed;
        }
        set
        {
            OntriggerPressed = value;
        }
    }

    public void RaycastEventTriggerPressed()
    {
        OnPrimaryIndexTriggerPressed.Invoke();
    }

    private void OnDisable()
    {
       
        enable = false;
        validator = false;
    }
    private void OnEnable()
    {
        ValidateState();
        validator = true;
    }

    void ValidateState()
    {
        StartCoroutine(ValidateAgain());
    }

    IEnumerator ValidateAgain()
    {
        yield return new WaitForSeconds(1f);
        if(validator)
            enable = true;
    }

   

}
