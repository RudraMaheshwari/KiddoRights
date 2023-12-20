using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string scenename;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void changeScene(){
        SceneManager.LoadScene(scenename);
    }
}
