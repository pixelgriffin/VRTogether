using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnStart : MonoBehaviour {

    public string scene;

    private void Start()
    {
        Load();
    }

    public void Load()
    {
        SceneManager.LoadScene(scene);
    }
}
