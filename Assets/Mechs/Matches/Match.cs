using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    [SerializeField] float burnTime = 2f;
    Light light;
    float lightIntensity;

    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(GoOut());
        Destroy(gameObject, burnTime);
        lightIntensity = light.intensity;
    }

    void Update()
    {
        light.intensity = Mathf.Lerp(light.intensity, lightIntensity, 5f * Time.deltaTime);;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Water")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator GoOut() 
    {
        yield return new WaitForSeconds(burnTime * 0.9f);
        lightIntensity = 0f;
        yield return null;
    }
}