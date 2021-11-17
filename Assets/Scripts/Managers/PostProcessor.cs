using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessor : MonoBehaviour
{
    public static PostProcessor Instance;
    private void Awake()
    {
        if (Instance != null)  // check to make sure there is not another instance of this class
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
