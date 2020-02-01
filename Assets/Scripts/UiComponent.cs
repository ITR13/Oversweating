using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[ExecuteAlways]
public class UiComponent : MonoBehaviour
{
    private static readonly Dictionary<string, Sprite[]> SpriteSheets = new Dictionary<string, Sprite[]>();

    [SerializeField] private Texture2D spriteSheet;
    [SerializeField] private Image image;


    private int _state, _color;
    public int state, color;


    private void Start()
    {
        _state = -1;
        _color = -1;
    }

    private void Update()
    {
        if (state == _state && color == _color) return;
        _state = state;
        _color = color;

        if (
            !SpriteSheets.ContainsKey(spriteSheet.name) ||
            SpriteSheets[spriteSheet.name].Length == 0
        )
        {
            var loadedSprites =
                Resources.LoadAll<Sprite>(spriteSheet.name);
            SpriteSheets.Add(
                spriteSheet.name,
                loadedSprites
            );
        }

        var sprites = SpriteSheets[spriteSheet.name];
        if (sprites.Length == 0)
        {
            Debug.LogError(spriteSheet.name);
            return;
        }

        var index =
            (
                    _state * Constants.ColorCount + 
                    _color % Constants.ColorCount
            ) % sprites.Length;

        image.sprite = sprites[index];
    }
}
