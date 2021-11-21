using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{

    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //public Transform pfItemWorld;

    public Sprite NormalPaintSprite;
    public Sprite GrapplePaintSprite;
    public Sprite DamagePaintSprite;
    public Sprite PoisonPaintSprite;
    public Sprite KillPaintSprite;
    public Sprite FreeformPaintSprite;


}
