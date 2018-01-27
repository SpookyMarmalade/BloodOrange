using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSheet2 : Tile
{
    [SerializeField]
    Sprite[] textures;
    Dictionary<directionFlags, List<Sprite>> lookup;
    [System.Flags]
    public enum directionFlags
    {
        None = 0x00,    //0b00000000
        nw = 0x01,      //0b00000001
        n = 0x02,       //0b00000010
        ne = 0x04,      //0b00000100
        e = 0x08,       //0b00001000
        se = 0x10,      //0b00010000
        s = 0x20,       //0b00100000
        sw = 0x40,      //0b01000000
        w = 0x80,       //0b10000000
        All = 0xFF      //0b11111111
    }

    bool checkSideEmpty(Sprite s, int x, int y)
    {
        Rect r = s.rect;
        int stepx = (int)(r.width - 1) / 2;
        int stepy = (int)(r.height - 1) / 2;


        return s.texture.GetPixel((int)r.x + (x * stepx),(int) r.y + (y * stepy)).a == 0;
    }
    // Use this for initialization
    void setup()
    {
        lookup = new Dictionary<directionFlags, List<Sprite>>();
        foreach (Sprite s in textures)
        {
            directionFlags current = directionFlags.None;
            if (!checkSideEmpty(s, 0, 0))
            {
                Debug.Log(current);
                current |= directionFlags.nw;
                Debug.Log(current);
            }
            if (!checkSideEmpty(s, 1, 0))
            {
                current |= directionFlags.n;
            }
            if (!checkSideEmpty(s, 2, 0))
            {
                current |= directionFlags.ne;
            }
            if (!checkSideEmpty(s, 0, 1))
            {
                current |= directionFlags.w;
            }
            if (!checkSideEmpty(s, 2, 1))
            {
                current |= directionFlags.e;
            }
            if (!checkSideEmpty(s, 0, 2))
            {
                current |= directionFlags.sw;
            }
            if (!checkSideEmpty(s, 1, 2))
            {
                current |= directionFlags.s;
            }
            if (!checkSideEmpty(s, 2, 2))
            {
                current |= directionFlags.se;
            }
            if (!lookup.ContainsKey(current))
            {
                lookup[current] = new List<Sprite>();
            }
            lookup[current].Add(s);
        }
            Debug.Log("Setup");

    }

    public bool hasFlag(directionFlags value, directionFlags flag)
    {
        return (value & flag) != directionFlags.None;
    }
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        if (lookup == null)
            setup();

        directionFlags current = directionFlags.None;
        current |= hasNabour(tilemap, position, 0, -1) ? directionFlags.n : directionFlags.None;
        current |= hasNabour(tilemap, position, -1, 0) ? directionFlags.w : directionFlags.None;
        current |= hasNabour(tilemap, position, 1, 0) ? directionFlags.e : directionFlags.None;
        current |= hasNabour(tilemap, position, 0, 1) ? directionFlags.s : directionFlags.None;


        current |= (hasNabour(tilemap, position, -1, -1) && (hasFlag(current, directionFlags.n) && hasFlag(current, directionFlags.w))) ? directionFlags.nw : directionFlags.None;
        current |= (hasNabour(tilemap, position, 1, -1) && (hasFlag(current, directionFlags.n) && hasFlag(current, directionFlags.e))) ? directionFlags.ne : directionFlags.None;
        current |= (hasNabour(tilemap, position, -1, 1) && (hasFlag(current, directionFlags.s) && hasFlag(current, directionFlags.w))) ? directionFlags.sw : directionFlags.None;
        current |= (hasNabour(tilemap, position, 1, 1) && (hasFlag(current, directionFlags.s) && hasFlag(current, directionFlags.e))) ? directionFlags.se : directionFlags.None;

        if (lookup.ContainsKey(current))
        {
            Debug.Log("in lookup");
            Debug.Log(current);
            tileData.sprite = pick<Sprite>(lookup[current]);
        }
        else
        {
            Debug.Log("not in look up");
            Debug.Log(current);
            tileData.sprite = textures.Length >= 1 ? textures[0] : tileData.sprite;
        }
        
    }
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
        for (int y = -1; y <= 1; y++) //Runs through all the tile's neighbours 
        {
            for (int x = -1; x <= 1; x++)
            {
                //We store the position of the neighbour 

                if (hasNabour(tilemap, position, x, y)) //If the neighbour has water on it
                {
                    Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);
                    tilemap.RefreshTile(nPos); //Them we make sure to refresh the neighbour aswell
                }
            }
        }
    }
    private bool hasNabour(ITilemap tilemap, Vector3Int position, int dx, int dy)
    {
        return tilemap.GetTile(position + Vector3Int.right * dx + Vector3Int.up * dy) == this;
    }
    private T pick<T>(List<T> t)
    {
        if (t.Count <= 0)
            return default(T);
        return t[UnityEngine.Random.Range(0, t.Count)];
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (lookup == null)
            setup();
        return base.StartUp(position, tilemap, go);
    }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/DynamicTile")]
    public static void CreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Dynamic Tile", "New Dynamic Tile", "asset", "Save Dynamic Tile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TileSheet2>(), path);
    }

#endif
}
