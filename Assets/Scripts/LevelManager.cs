using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameSettings defaultSettings;
    public static GameSettings Settings {get; private set;}

    // Start is called before the first frame update
    void Awake()
    {
        Settings = defaultSettings;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
