using System;
using UnityEngine;

[Serializable]
public class CircleTypeConfigRecord
{
    [SerializeField]
    private int id;
    [SerializeField]
    private float scale;
    [SerializeField]
    private float radius;


    public int ID { get { return id; } }
    public float Scale { get { return scale; } }
    public float Radius { get { return radius; } }
}
public class CircleTypeConfig : BYDataTable<CircleTypeConfigRecord>
{
    public override ConfigCompare<CircleTypeConfigRecord> DefineConfigCompare()
    {
        configCompare = new ConfigCompare<CircleTypeConfigRecord>("id");
        return configCompare;
    }
}
