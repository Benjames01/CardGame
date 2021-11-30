using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{

    public enum Scene
    {
        MenuScene,
        CardCreator,
        GameScene,
        SettingsScene
    }
    
    public static async void LoadScene(Scene scene)
    {
       var load = SceneManager.LoadSceneAsync(scene.ToString());
    }    
}
