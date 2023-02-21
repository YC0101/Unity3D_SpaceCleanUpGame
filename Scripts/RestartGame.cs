using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Load Scene
using UnityEngine.EventSystems;

public class RestartGame : MonoBehaviour, IPointerDownHandler
{

    private void Restart()//Reload Scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Load the current scene
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Restart();
        Time.timeScale = 1;
    }

}
