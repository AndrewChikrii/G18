using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour, IActivatable
{

    [SerializeField] GameObject fuse;

    [SerializeField] float dist;

    [SerializeField] Item item;

    private bool questDone;

    HintDisplay hintDisplay;

    AudioSource audio;

    [SerializeField] GameObject doorAudio;
    [SerializeField] GameObject partSys;

    void Start()
    {
        hintDisplay = GameObject.Find("Canvas").GetComponent<HintDisplay>();
        audio = GetComponent<AudioSource>();
    }
    void FixedUpdate()
    {
        dist = Vector3.Distance(fuse.transform.position, transform.position);

        if (dist < 0.5f && !questDone)
        {
            questDone = true;

            fuse.transform.SetParent(transform);

            GameObject.Find("PlayerCamera").GetComponent<PRaycast>().CancelAction();
            fuse.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.20f);
            Destroy(fuse.GetComponent<Grabbable>());
            Destroy(GetComponent<BoxCollider>());
            Destroy(fuse.GetComponent<Rigidbody>());

            fuse.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            audio.Play();
            doorAudio.GetComponent<AudioSource>().Play();
            partSys.GetComponent<ParticleSystem>().Play();//.enableEmission = true;
            //partSys.enableEmmision = true;
            SC_FPSController.inventory.AddItem(item);
            this.enabled = false;
        }
    }

    public void ActPrimary()
    {
        hintDisplay.interaction.text = "Fuse is missing";
        StartCoroutine(hintDisplay.HintCoroutine());
    }
    public void ActSecondary() { }
    public void Deact() { }
}
