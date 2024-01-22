using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLibControl : MonoBehaviour
{
    public static SpriteLibControl Instance;

    [SerializeField] 
    private List<Sprite> _sprite;
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start ()
    {
        foreach(var sprite in _sprite)
        {
            //Debug.Log(sprite.name.ToString ());
            spriteDict.Add(sprite.name, sprite);
        }
    }

    public Sprite GetSpriteByName(string name)
    {
        return spriteDict[name];
    }
    public string GetSpriteName(int type ,int id)
    {
        if (type == 0) //SLICE SPRITE
        {
            string name = GetSliceSprite(id);
            return name;    
        }
        else if(type == 1) //FRUIT SPRITE
        {
            string name = GetFruitSprite(id);
            return name;
        }
        else
        {
            return null;
        }
    }
    public string GetSliceSprite(int id)
    {
        switch (id)
        {
            case 1:
                return "01_Orange__Section";
            case 2:
                return "02_Strawberry_Section";
            case 3:
                return "03_WATERMELON_Section";
            case 4:
                return "04_Mangosteen_Section";
            case 5:
                return "05_Lemon_Section";
            case 6:
                return "06_Hami Melon_Section";
            case 7:
                return "07_Apple_Section";
            case 8:
                return "08_Peach_Section";
            case 9:
                return "09_Tomato_Section";
            case 10:
                return "10_Pomegranate_Section";
            case 11:
                return "11_grape_section new";
            default:
                return null;
        }
    }
    public string GetFruitSprite(int id)
    {
        switch (id)
        {
            case 1:
                return "Apple";
            case 2:
                return "Graphe";
            case 3:
                return "Hami melon";
            case 4:
                return "Lemon";
            case 5:
                return "Mangosteen";
            case 6:
                return "Orgrane";
            case 7:
                return "Peach";
            case 8:
                return "Pomegranate";
            case 9:
                return "Strawberry";
            case 10:
                return "Tomoto";
            case 11:
                return "WATERMELON";
            default:
                return null;
        }
    }
}
