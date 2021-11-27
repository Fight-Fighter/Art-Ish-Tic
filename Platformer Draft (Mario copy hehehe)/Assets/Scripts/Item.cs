using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        NormalPaint,
        GrapplePaint,
        DamagePaint,
        PoisonPaint,
        KillPaint,
        FreeformPaint
    }

    public ItemType itemType;
    public float amount;
    public float maxAmount = 1000;
    public bool IsStackable()
    {
        return true;
    }

    public bool IsPaint()
    {
        return true;
    }

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.NormalPaint: return ItemAssets.Instance.NormalPaintSprite;
            case ItemType.GrapplePaint: return ItemAssets.Instance.GrapplePaintSprite;
            case ItemType.DamagePaint: return ItemAssets.Instance.DamagePaintSprite;
            case ItemType.PoisonPaint: return ItemAssets.Instance.PoisonPaintSprite;
            case ItemType.KillPaint: return ItemAssets.Instance.KillPaintSprite;
            case ItemType.FreeformPaint: return ItemAssets.Instance.FreeformPaintSprite;
        }
    }

    
}
