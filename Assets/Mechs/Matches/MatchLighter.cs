using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLighter : MonoBehaviour
{
    Inventory inv;
    [SerializeField] GameObject matchSpotPrefab;
    [SerializeField] GameObject matchPointPrefab;
    bool throwMatch = true;
    IEnumerator throwCor;
    GameObject playerCamera;

    void Start()
    {
        inv = GetComponent<Inventory>();
        playerCamera = GameObject.Find("PlayerCamera");
    }

    void Update()
    {
        if (inv.matchesCount > 0)
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
                    //Debug.Log("throw");
                }
                else
                {
                    StopCoroutine(throwCor);

                    //Debug.Log("up");
                }

            }
        }

    }

    void LightUp()
    {
        inv.RemoveFromInv(1);
        GameObject match = Instantiate(matchSpotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        match.transform.SetParent(gameObject.transform);
    }

    void LightThrow()
    {
        inv.RemoveFromInv(1);
        GameObject match = Instantiate(matchPointPrefab, new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z), transform.rotation);
        match.GetComponent<Rigidbody>().AddForce(playerCamera.transform.TransformDirection(Vector3.forward) * 25f);
    }

    IEnumerator ThrowCoroutine()
    {
        //Debug.Log("cor st");
        throwMatch = true;
        yield return new WaitForSeconds(1f);
        //Debug.Log("cor 2 sec");
        LightUp();
        throwMatch = false;
        yield return null;
    }
}
