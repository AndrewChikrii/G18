using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLighter : MonoBehaviour
{
    [SerializeField] GameObject matchSpotPrefab;
    [SerializeField] GameObject matchPointPrefab;
    bool throwMatch = true;
    IEnumerator throwCor;
    GameObject playerCamera;

    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera");
    }

    void Update()
    {
        if (SC_FPSController.inventory.MatchesCount() > 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                throwCor = ThrowCoroutine();
                StartCoroutine(throwCor);
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                if (throwMatch)
                {
                    StopCoroutine(throwCor);
                    LightThrow();
                }
                else
                {
                    StopCoroutine(throwCor);
                }
            }
        }
    }

    void LightUp()
    {
        SC_FPSController.inventory.RemoveItem(SC_FPSController.inventory.GetItemList().Find(item => item.name == "Matches"));
        GameObject match = Instantiate(matchSpotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        match.transform.SetParent(gameObject.transform);
    }

    void LightThrow()
    {
        SC_FPSController.inventory.RemoveItem(SC_FPSController.inventory.GetItemList().Find(item => item.name == "Matches"));
        GameObject match = Instantiate(matchPointPrefab, new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z), transform.rotation);
        match.GetComponent<Rigidbody>().AddForce(playerCamera.transform.TransformDirection(Vector3.forward) * 25f);
    }

    IEnumerator ThrowCoroutine()
    {
        throwMatch = true;
        yield return new WaitForSeconds(1f);
        LightUp();
        throwMatch = false;
        yield return null;
    }
}