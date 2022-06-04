using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchLighter : MonoBehaviour
{
    [SerializeField] GameObject matchSpotPrefab;
    [SerializeField] GameObject matchPointPrefab;
    [SerializeField] private Text uiMatchesCount;
    private Text matchesCount;
    bool throwMatch = true;
    IEnumerator throwCor;
    GameObject playerCamera;

    float maxAlpha = 1f;

    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera");

        matchesCount = uiMatchesCount.GetComponent<Text>();
    }

    void Update()
    {
        matchesCount.color = new Color(matchesCount.color.r, matchesCount.color.g, matchesCount.color.b,
                Mathf.Lerp(matchesCount.color.a, maxAlpha, 5f * Time.deltaTime));
        if (SC_FPSController.inventory.MatchesCount() < 1)
        {
            maxAlpha = 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            maxAlpha = 1f;
            throwCor = ThrowCoroutine();
            StartCoroutine(throwCor);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            maxAlpha = 0;
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

    void LightUp()
    {
        SC_FPSController.inventory.RemoveItem(SC_FPSController.inventory.GetItemList().Find(item => item.name == "Matches"));
        MatchesUI();
        GameObject match = Instantiate(matchSpotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        match.transform.SetParent(gameObject.transform);
    }

    void LightThrow()
    {
        SC_FPSController.inventory.RemoveItem(SC_FPSController.inventory.GetItemList().Find(item => item.name == "Matches"));
        MatchesUI();
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
    void MatchesUI()
    {
        matchesCount.text = SC_FPSController.inventory.MatchesCount().ToString();
    }
}