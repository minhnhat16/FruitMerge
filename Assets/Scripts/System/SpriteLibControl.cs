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
}
