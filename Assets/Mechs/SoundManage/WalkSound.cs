using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    [SerializeField] AudioSource soundConc;
    [SerializeField] AudioSource soundWater;
    [SerializeField] float volumeConc = 0;
    [SerializeField] float volumeWater = 0f;
    [SerializeField] float walkSoundVolume = 0.15f;

    bool isMoved;

    RaycastHit groundHit;
    bool groundShot;
    
    void Update()
    {
        soundConc.volume = Mathf.Lerp(soundConc.volume, 0, 20 * Time.deltaTime);
        soundWater.volume = Mathf.Lerp(soundWater.volume, 0, 5 * Time.deltaTime);
        if (isMoved)
        {
            soundConc.volume = Mathf.Lerp(soundConc.volume, volumeConc, 20 * Time.deltaTime);
            soundWater.volume = Mathf.Lerp(soundWater.volume, volumeWater, 20 * Time.deltaTime);
        }

        StartCoroutine(moveCheck());

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 2f, Color.grey);
        groundShot = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out groundHit, 2f);

        if (groundHit.collider)
        {
            if (groundHit.collider.gameObject.name == "Water")
            {
                volumeConc = Mathf.Lerp(volumeConc, 0f, 2 * Time.deltaTime);
                volumeWater = Mathf.Lerp(volumeWater, walkSoundVolume, 2 * Time.deltaTime);
            }
            else
            {
                volumeConc = Mathf.Lerp(volumeConc, walkSoundVolume, 20 * Time.deltaTime);
                volumeWater = Mathf.Lerp(volumeWater, 0f, 20 * Time.deltaTime);
            }
        }
    }

    IEnumerator moveCheck()
    {
        Vector3 p1 = transform.position;
        yield return new WaitForSeconds(0.1f);
        Vector3 p2 = transform.position;
        isMoved = !p1.Equals(p2);
        yield return null;
    }
}
