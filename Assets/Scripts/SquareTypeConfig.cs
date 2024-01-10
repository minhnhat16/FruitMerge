using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class SquareTypeConfigRecord
{
    [SerializeField]
    private int value;
    [SerializeField]
    public string color;

    public int Value { get => value; }
    public string Color { get => color; }
}
public class SquareTypeConfig : BYDataTable<SquareTypeConfigRecord>
{
    public override ConfigCompare<SquareTypeConfigRecord> DefineConfigCompare()
    {
        configCompare = new ConfigCompare<SquareTypeConfigRecord>("id");
        return configCompare;
    }
}

