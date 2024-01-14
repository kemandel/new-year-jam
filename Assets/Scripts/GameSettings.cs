using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public float musicVolume = .5f;
    public float charMoveTime = .5f;
    public float baseFadeSpeed = 1f;
    public float baseAudioFadeSpeed;
    public KeyCode interactKey = KeyCode.Space;
    public KeyCode[] characterSwapKeys = new KeyCode[5] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5};
}
