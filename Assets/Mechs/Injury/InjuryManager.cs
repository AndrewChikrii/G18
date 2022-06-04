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

    [SerializeField] float dist;
    float playerWalkSpeed;
    float playerRunSpeed;

    void Start()
    {
        if (GameObject.Find("Creep"))
        {
            volume = GameObject.Find("Global Volume").GetComponent<Volume>();
            creep = GameObject.Find("Creep").GetComponent<CreepController>();
            playerCont = GetComponent<SC_FPSController>();
            playerWalkSpeed = playerCont.walkingSpeed;
            playerRunSpeed = playerCont.runningSpeed;
        }
        else
        {
            Destroy(this);
        }

    }

    void Update()
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
            ca.contrast.Override(Mathf.Lerp(ca.contrast.value, creep.aggroSpoolUp * 0.25f, 15f * Time.deltaTime));
            //ca.contrast.Override(creep.aggroSpoolUp * 0.98f); 
            ca.postExposure.Override(creep.aggroSpoolUp * -0.005f);
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

        if (dist <= 1.5f)
        {
            Debug.Log("Death.");
        }
    }
}
