using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
  void Update()
  {
    if (Input.GetKeyDown("return"))
    {
      SceneManager.LoadScene("Main");
    }

    if (Input.GetKeyDown("escape"))
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
  }
}
