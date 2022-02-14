using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grabbable : MonoBehaviour, IActivatable
{
    [SerializeField] bool grabbed = false;
    [SerializeField] bool freezed = false;
    Rigidbody rb;
    float rbDrag;
    float rbAngularDrag;
    Vector3 originalScale;

    public void ActPrimary() {
        //Debug.Log("Grabbed a " + gameObject.name);
        Grab(GameObject.Find("PlayerOrient").transform);
    }
    public void ActSecondary() {
        if(!freezed) {
            //Debug.Log("Pushed a " + gameObject.name);
            Push();
        }
    }
    public void Deact() {
        //Debug.Log("Dropped a " + gameObject.name);
        Grab(null);
        grabbed = false;
    }

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        rbDrag = rb.drag;
        rbAngularDrag = rb.angularDrag;
        originalScale = transform.localScale;
    }

    void Update() {
        transform.localScale = originalScale;
    }

    void Grab(Transform t) {
        grabbed = Convert.ToBoolean(t);
        transform.SetParent(t);
        if(grabbed) {
            rb.drag = 100f / (2 * rb.mass);
            rb.angularDrag = 10f;
        }
        else {
            rb.drag = rbDrag;
            rb.angularDrag = rbAngularDrag;
        }
    }

    void Push() {
        Grab(null);
        rb.AddForce((transform.position - GameObject.Find("PlayerOrient").transform.position) * 50f);
        StartCoroutine(Freeze());
    }

    IEnumerator Freeze() {
        freezed = true;
        yield return new WaitForSeconds(1f);
        freezed = false;
    }


}
/*
    PlayerOrient is obj in cam to handle rotation
*/

