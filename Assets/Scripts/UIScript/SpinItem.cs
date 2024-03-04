using UnityEngine;
using UnityEngine.UI;

public class SpinItem : MonoBehaviour
{

    [SerializeField] int id;
    [SerializeField] ItemType type;
    [SerializeField] int amount;
    [SerializeField] Image itemImg;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void InitItem(SpinConfigRecord record)
    {
        Debug.Log(record.Type + " " + record.Id);
        id = record.Id;
        type = record.Type;
        amount = record.Amount;
        itemImg.sprite = SpriteLibControl.Instance.GetSpriteByName(record.ItemImg);
    }
    public void OnRewardItem()
    {
        if (type == ItemType.GOLD)
        {
            DataAPIController.instance.SaveGold(amount, () =>
            {
                Debug.Log($"Added to data {amount} gold ");
            });
        }
        else if( type  ==ItemType.CHANGE  && type == ItemType.HAMMER && type == ItemType.ROTATE)
        {
            DataAPIController.instance.AddItemTotal(type.ToString(), amount);   
        }
        else if(type == ItemType.FRUITSKIN)
        {
            DataAPIController.instance.SaveFruitSkin(amount);
        }
    }
}
