using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour
{
    public static Darkness settings;
    SpriteRenderer darkness;

    private void Awake()
    {
        settings = this;
    }

    public void SetDarkness(float alpha)
    {
        if(darkness == null)
        {
            darkness = GetComponent<SpriteRenderer>();
        }
        MaterialPropertyBlock darknessMaterial = new MaterialPropertyBlock();  
        darkness.GetPropertyBlock(darknessMaterial);
        darknessMaterial.SetColor("_Color", new Color(0, 0, 0, alpha));
        darkness.SetPropertyBlock(darknessMaterial);
    }
}
