using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour 
{
    public void ToggleSound()
    {
        AudioListener.pause = !AudioListener.pause;
        GetComponent<Image>().color = (AudioListener.pause) ? Color.gray : Color.white;
    }
}
