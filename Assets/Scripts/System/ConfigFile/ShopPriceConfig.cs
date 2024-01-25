using UnityEngine;

[System.Serializable]
public class ShopPriceConfigRecord {

    [SerializeField]
    private int id;
    [SerializeField]
    private int  idItem;
    [SerializeField]
    private int price;
    [SerializeField]
    private int amount;
    [SerializeField]
    private bool available;
    [SerializeField]
    private bool moneyPaid;
    public int Id { get => id; }
    public int IdItem { get => idItem; }
    public int Price { get => price; }
    public int Amount { get => amount; }
    public bool Available { get => available; }
    public bool MoneyPaid { get => moneyPaid; }
}

public class ShopPriceConfig : BYDataTable<ShopPriceConfigRecord>
{
    public override ConfigCompare<ShopPriceConfigRecord> DefineConfigCompare()
    {
        configCompare = new ConfigCompare<ShopPriceConfigRecord>("id");
        return configCompare;
    }
}
