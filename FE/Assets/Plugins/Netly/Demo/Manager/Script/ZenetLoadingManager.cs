using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZenetLoadingManager : MonoBehaviour
{
    public void Load(string name)
    {
        try
        {
            SceneManager.LoadScene(name);
        }
        catch
        {
            Application.Quit();
        }
    }
}
