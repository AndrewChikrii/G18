using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRaycast : MonoBehaviour
{
    [SerializeField] float dist;
    [SerializeField] bool actionFrozen;
    RaycastHit hit;
    bool rayShot;
    IActivatable colInterface;

    void LateUpdate()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * dist, Color.green);
        rayShot = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, dist);

        if (hit.collider)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0) && !actionFrozen)
            {
                Component[] hitComponents = hit.collider.gameObject.GetComponents(typeof(IActivatable));
                if (hitComponents.Length > 0)
                {
                    colInterface = hitComponents[0] as IActivatable;
                    colInterface.ActPrimary();
                }
                else
                {
                    CancelAction();
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Component[] hitComponents = hit.collider.gameObject.GetComponents(typeof(IActivatable));
                if (hitComponents.Length > 0)
                {
                    colInterface = hitComponents[0] as IActivatable;
                    colInterface.ActSecondary();
                }
                else
                {
                    CancelAction();
                }
            }
            if ((hit.collider.gameObject.GetComponent<IActivatable>() == null || actionFrozen))
            {
                CancelAction();
            }
        }
        else
        {
            CancelAction();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            CancelAction();
        }
    }

    public void FreezeAction(bool f)
    {
        actionFrozen = f;
    }

    public void CancelAction()
    {
        if (colInterface != null)
        {
            colInterface.Deact();
            colInterface = null;
        }
    }



}

/*
    assign to camera
    set dist by hand (5)
*/