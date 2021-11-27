using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

  public void OnButtonClick(int i)
  {
    switch (i)
    {
      case 1:
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        break;
      case 2:
        SceneLoader.LoadScene(SceneLoader.Scene.CardCreator);
        break;
      case 3:
        SceneLoader.LoadScene(SceneLoader.Scene.SettingsScene);
        break;
      case 4:
        Application.Quit();
        break;
    }
  }
    
    
}
