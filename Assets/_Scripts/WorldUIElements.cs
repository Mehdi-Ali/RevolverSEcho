using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIElement : MonoBehaviour
{
    public static WorldUIElement Elements { get; private set; }

    public Image RightReticle;
    public Image LeftReticle;

    private void Awake()
    {
        if (Elements == null)
        {
            Elements = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}