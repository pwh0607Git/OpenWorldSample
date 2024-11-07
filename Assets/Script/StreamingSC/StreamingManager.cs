using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StreamingManager : MonoBehaviour
{
    private void Start()
    {
        LoadSceneAdditive();
    }

    void LoadSceneAdditive()
    {
        SceneManager.LoadScene("Sector_0_1", LoadSceneMode.Additive);
    }
}
