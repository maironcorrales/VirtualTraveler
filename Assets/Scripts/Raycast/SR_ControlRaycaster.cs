using UnityEngine;
using UnityEngine.UI;

public class SR_ControlRaycaster : MonoBehaviour
{

    RaycastHit hit;
    GameObject objetoApuntado = null;
    GameObject objetoApuntadoAnterior = null;
    public SR_VrRecticle recticle;
    GameObject ObjetoUi;



    private void Update()
    {
        if (objetoApuntado != null && objetoApuntado.GetComponent<SR_VrInteractable>())
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.A))
                objetoApuntado.GetComponent<SR_VrInteractable>().OnTriggerPressed();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {


        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit))
        {
            recticle.SetPosition(hit);
            objetoApuntado = hit.collider.gameObject;




            if (objetoApuntado != objetoApuntadoAnterior)
            {
                if (objetoApuntadoAnterior != null)
                {
                    if (objetoApuntadoAnterior.GetComponent<SR_VrInteractable>())
                    {
                        objetoApuntadoAnterior.GetComponent<SR_VrInteractable>().RayOut();
                    }
                }
                if (objetoApuntado != null)
                {
                    if (objetoApuntado.GetComponent<SR_VrInteractable>())
                    {
                        objetoApuntado.GetComponent<SR_VrInteractable>().OnHit();

                    }

                }
                objetoApuntadoAnterior = objetoApuntado;
            }



            if (hit.transform.tag == "UI")
            {
                ObjetoUi = hit.collider.gameObject;
                ObjetoUi.GetComponent<UIController>();
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.A))
                    ObjetoUi.GetComponent<Button>().onClick.Invoke();
            }
        }
        else
        {
            recticle.SetPosition();

            if (objetoApuntadoAnterior != null && objetoApuntadoAnterior.GetComponent<SR_VrInteractable>())
            {
                objetoApuntadoAnterior.GetComponent<SR_VrInteractable>().RayOut();
            }
            objetoApuntado = null;
            objetoApuntadoAnterior = null;
        }
    }


}
