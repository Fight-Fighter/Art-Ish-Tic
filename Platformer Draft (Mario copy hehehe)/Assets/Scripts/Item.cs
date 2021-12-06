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
        FreeformPaint,
        Heart
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
        return (int) itemType <= 5;
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
            case ItemType.Heart: return ItemAssets.Instance.HeartSprite;
        }
    }

    public Color GetColor()
    {
        switch (itemType)
        {
            default:
            case ItemType.NormalPaint: return Color.blue;
            case ItemType.GrapplePaint: return new Color(165,42,42);
            case ItemType.DamagePaint: return Color.yellow;
            case ItemType.PoisonPaint: return Color.green;
            case ItemType.KillPaint: return Color.red;
            case ItemType.FreeformPaint: return Color.magenta;
        }
    }
}
