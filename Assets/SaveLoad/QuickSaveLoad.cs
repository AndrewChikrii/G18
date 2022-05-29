using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().isLoaded) {
            transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPositionX"), PlayerPrefs.GetFloat("PlayerPositionY"), PlayerPrefs.GetFloat("PlayerPositionZ"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("f5")) {
            QuickSaveHandler();
        }
        else if(Input.GetKeyDown("f9")) {
            StartCoroutine(QuickLoadHandler());
        }
        else if(Input.GetKeyDown("f7")){
            PlayerPrefs.DeleteAll();
        }
        else {
            return;
        }
    }

    private IEnumerator QuickLoadHandler() {
        if(PlayerPrefs.HasKey("SceneSaved")) {
            string sceneToLoad = PlayerPrefs.GetString("SceneSaved");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
            Debug.Log("Load: " + PlayerPrefs.GetString("SceneSaved"));
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }

    private void QuickSaveHandler() {
        string activeScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetFloat("PlayerPositionX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", transform.position.z);
        PlayerPrefs.SetString("SceneSaved", activeScene);
        PlayerPrefs.Save();
        Debug.Log(transform.position);
        Debug.Log("Level Saved: " + PlayerPrefs.GetString("SceneSaved"));
    }
}
