using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    Transform playerT;
    [SerializeField] string nextSceneName;
    [SerializeField] float dist;
    [SerializeField] float triggerDist = 4f;

    void Start()
    {
        playerT = GameObject.Find("Player").transform;
    }

    void Update() 
    {
        if(Input.GetKey(KeyCode.F9)) 
        {
            SceneManager.LoadScene("1_Prison_entry", LoadSceneMode.Single);
        }
    }

    void FixedUpdate()
    {
        dist = Vector3.Distance(playerT.position, transform.position);
        if(dist <= triggerDist)
        {
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }
    
}
