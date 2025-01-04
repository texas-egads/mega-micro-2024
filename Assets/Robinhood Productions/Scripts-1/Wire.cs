using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Wire : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool Active;
    SpriteRenderer sprite;
    public Sprite[] Wires, Cutwires;
    public Sprite CutWire, ActiveWire;
    public KeyCode keyCode;
    public Color code;
    Dictionary<Color, KeyCode> WireType = new Dictionary<Color, KeyCode>();
    List<Color> colors = new List<Color>();
    void Start()
    {
        createDictionary(WireType);
        createColorList(colors);
        Active = true;
        sprite = GetComponent<SpriteRenderer>();
        int randInt = Random.Range(0, 4);
        ActiveWire = Wires[randInt];
        CutWire = Cutwires[randInt];
        code = colors[randInt];
        keyCode = WireType[code];
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active)
        {
            sprite.sprite = CutWire;
        }
        else if (Active)
        {
            sprite.sprite = ActiveWire;
        }
    }

    void createDictionary(Dictionary<Color, KeyCode> WireType)
    {
        WireType.Add(Color.red, KeyCode.W);
        WireType.Add(Color.blue, KeyCode.A);
        WireType.Add(Color.green, KeyCode.S);
        WireType.Add(Color.yellow, KeyCode.D);
    }

    void createColorList(List<Color> colorTypes)
    {
        colorTypes.Add(Color.red);
        colorTypes.Add(Color.blue);
        colorTypes.Add(Color.green);
        colorTypes.Add(Color.yellow);
    }




}
