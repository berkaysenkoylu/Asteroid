using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	
    public void LoadByIndex(int sceneIndex)
    {
        StartCoroutine(LoadNewScene(sceneIndex));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadNewScene(int sceneIndex)
    {
        float fadeTime = GetComponent<Fading>().BeginFade(1);

        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
