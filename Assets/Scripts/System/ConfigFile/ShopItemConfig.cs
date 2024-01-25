using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ShopConfigRecord
{
    [SerializeField]
    private int id;
    [SerializeField]
    private ItemType type;
    [SerializeField]
    private int  amount;
    [SerializeField]
    private int cost;
    [SerializeField]
    private Sprite image;

    public int ID { get { return id; } }
    public float Amount { get { return amount; } }
    public float Cost { get { return cost; } }
    public Sprite Image { get { return image; } }
}


public class ShopGoldConfig : BYDataTable<CircleTypeConfigRecord>
{
    public override ConfigCompare<CircleTypeConfigRecord> DefineConfigCompare()
    {
        configCompare = new ConfigCompare<CircleTypeConfigRecord>("id");
        return configCompare;
    }
}
