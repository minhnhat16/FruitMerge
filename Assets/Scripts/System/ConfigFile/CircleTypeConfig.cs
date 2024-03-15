using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CircleTypeConfigRecord
{
    [SerializeField]
    private int id;
    [SerializeField]
    private List<CircleType> _circleTypes;

    public int ID { get { return id; } }
    public List<CircleType> _CircleType { get { return _circleTypes; } }

    public CircleType GetTypeByID(int id)
    {
        //Debug.Log($"Get Type By ID {id}");
        if (_CircleType[id] != null) return _CircleType[id];
        return null;
    }
}
[Serializable]
public class CircleType
{
    [SerializeField]
    private int id;
    [SerializeField]
    private float scale;
    [SerializeField]
    private float radius;
    [SerializeField]
    private Color color;

    public int ID { get { return id; } }    
    public float Scale { get { return scale; } }
    public float Radius { get { return radius; } }
    public Color Color { get { return color; } }
}

public class CircleTypeConfig : BYDataTable<CircleTypeConfigRecord>
{
    public override ConfigCompare<CircleTypeConfigRecord> DefineConfigCompare()
    {
        configCompare = new ConfigCompare<CircleTypeConfigRecord>("id");
        return configCompare;
    
}
}