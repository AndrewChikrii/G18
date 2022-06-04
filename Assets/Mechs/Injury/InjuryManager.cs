using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InjuryManager : MonoBehaviour
{
    Volume volume;
    CreepController creep;
    SC_FPSController playerCont;
    Vignette vig;
    FilmGrain grain;
    DepthOfField dof;
    ColorAdjustments ca;
    MotionBlur mb;
    [SerializeField] float dist;
    float playerWalkSpeed;
    float playerRunSpeed;
    RaycastHit upHit;
    bool rayUpShot;

    void Start()
    {
        if (GameObject.Find("Creep"))
        {
            creep = GameObject.Find("Creep").GetComponent<CreepController>();
        }
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        playerCont = GetComponent<SC_FPSController>();
        playerWalkSpeed = playerCont.walkingSpeed;
        playerRunSpeed = playerCont.runningSpeed;
    }

    void Update()
    {
        if (creep)
        {
            dist = Vector3.Distance(transform.position, creep.transform.position);

            if (volume.profile.TryGet<Vignette>(out vig))
            {
                if (creep.currState == "aggro")
                {
                    vig.intensity.Override(Mathf.Lerp(vig.intensity.value, 0.45f, 5f * Time.deltaTime));
                }
                else
                {
                    vig.intensity.Override(Mathf.Lerp(vig.intensity.value, 0f, 1f * Time.deltaTime));
                }

            }
            if (volume.profile.TryGet<FilmGrain>(out grain))
            {
                grain.intensity.Override(creep.aggroSpoolUp * 0.01f);
            }
            if (volume.profile.TryGet<DepthOfField>(out dof))
            {
                dof.focalLength.Override(creep.aggroSpoolUp * 2f);
                dof.focusDistance.Override(dist);
            }
            if (volume.profile.TryGet<ColorAdjustments>(out ca))
            {
                ca.contrast.Override(Mathf.Lerp(ca.contrast.value, creep.aggroSpoolUp * 0.3f, 15f * Time.deltaTime));
                ca.postExposure.Override(creep.aggroSpoolUp * 0.015f);
            }
            if (volume.profile.TryGet<MotionBlur>(out mb))
            {
                mb.intensity.Override(Mathf.Lerp(mb.intensity.value, creep.aggroSpoolUp * 0.03f, 15f * Time.deltaTime));
            }

            if (creep.aggroSpoolUp >= 100f)
            {
                playerCont.walkingSpeed = Mathf.Lerp(playerCont.walkingSpeed, playerWalkSpeed - (creep.aggroSpoolUp / 100f), 2f * Time.deltaTime);
                playerCont.runningSpeed = Mathf.Lerp(playerCont.runningSpeed, playerRunSpeed - (creep.aggroSpoolUp / 100f), 2f * Time.deltaTime);
            }
            else
            {
                playerCont.walkingSpeed = Mathf.Lerp(playerCont.walkingSpeed, playerWalkSpeed, 2f * Time.deltaTime);
                playerCont.runningSpeed = Mathf.Lerp(playerCont.runningSpeed, playerRunSpeed, 2f * Time.deltaTime);
            }
        }
        else
        {
            dist = 100f;
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 1.25f, Color.red);
        rayUpShot = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out upHit, 1.25f);

        if (upHit.collider)
        {
            if (upHit.collider.gameObject.GetComponent<WheelDoor>())
            {
                Die();
            }
        }

        if (dist <= 1.5f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Death.");
    }
}