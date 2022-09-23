using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapGenerator : MonoBehaviour
{
    public static LevelMapGenerator Instance;
    public List<SpriteData> spriteDatas;

    private int[,] levelMap = {
     {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
     {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
     {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
     {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
     {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
     {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
     {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
     {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
     {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
     {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
     {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
     {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
     {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
     {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
     {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };


    private int[,] globalLevelMap;
    private Vector3 zeroPointPosition;


    private void Awake()
    {
        Instance = this;
        CreateGlobalLevelMap(); 
    }


    public void CreateLevelMap()
    {
        ClearLevelMap();
        InitializeGlobalLevelMapArray();
        Vector3 zeroPointPos = new Vector3(-levelMap.GetLength(0), levelMap.GetLength(1), 0);
  
        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                SpriteType spriteType = (SpriteType)globalLevelMap[i, j];
                GameObject obj = new GameObject(i + "_" + j);
                obj.transform.SetParent(transform);
                obj.transform.position = zeroPointPos + Vector3.right * i - Vector3.up * j;
                obj.transform.rotation = GetRotation(i, j, spriteType);
                SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GetSprite(spriteType);
            }
        }
    }


    public void CreateGlobalLevelMap()
    {
        ClearLevelMap();
        InitializeGlobalLevelMapArray();

        Vector3 zeroPointPos = new Vector3(-levelMap.GetLength(0), levelMap.GetLength(1), 0);
        zeroPointPosition = zeroPointPos;
        for (int i = 0; i < globalLevelMap.GetLength(0); i++)
        {
            for (int j = 0; j < globalLevelMap.GetLength(1); j++)
            {
                SpriteType spriteType = (SpriteType)globalLevelMap[i, j];
                GameObject obj = new GameObject(i + "_" + j);
                obj.transform.SetParent(transform);
                obj.transform.position = zeroPointPos + Vector3.right * i - Vector3.up * j;
                obj.transform.rotation = GetRotation(i, j, spriteType);
                SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GetSprite(spriteType);
            }
        }
    }


    private void InitializeGlobalLevelMapArray()
    {

        globalLevelMap = new int[levelMap.GetLength(0) * 2, levelMap.GetLength(1) * 2];

        for (int i = 0; i < globalLevelMap.GetLength(0); i++)
        {
            for (int j = 0; j < globalLevelMap.GetLength(1); j++)
            {
                if (i < levelMap.GetLength(0) && j < levelMap.GetLength(1)) //左上象限
                {
                    globalLevelMap[i, j] = levelMap[i, j];
                }
                else if (i >= levelMap.GetLength(0) && j < levelMap.GetLength(1)) //右上象限
                {
                    globalLevelMap[i, j] = levelMap[globalLevelMap.GetLength(0) - 1 - i, j];
                }
                else if (i < levelMap.GetLength(0) && j >= levelMap.GetLength(1)) //左下象限
                {
                    globalLevelMap[i, j] = levelMap[i, globalLevelMap.GetLength(1) - 1 - j];
                }
                else if (i >= levelMap.GetLength(0) && j >= levelMap.GetLength(1))
                {
                    globalLevelMap[i, j] = levelMap[globalLevelMap.GetLength(0) - 1 - i, globalLevelMap.GetLength(1) - 1 - j];
                }
            }
        }

    }

    public void ClearLevelMap()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }


    private Sprite GetSprite(SpriteType spriteType)
    {
        SpriteData data = spriteDatas.Find(s => s.Type == spriteType);
        if (data == null)
        {
            print(spriteType);
        }
        return data.Sprite;
    }


    private Quaternion GetRotation(int x, int y, SpriteType spriteType)
    {
        SpriteType top = GetSpriteType(x, y - 1);
        SpriteType bottom = GetSpriteType(x, y + 1);
        SpriteType left = GetSpriteType(x - 1, y);
        SpriteType right = GetSpriteType(x + 1, y);
        SpriteType topRight = GetSpriteType(x + 1, y - 1);
        SpriteType topLeft = GetSpriteType(x - 1, y - 1);
        SpriteType bottomRight = GetSpriteType(x + 1, y + 1);
        SpriteType bottomLeft = GetSpriteType(x - 1, y + 1);
        Quaternion rotaiton = Quaternion.identity;

        switch (spriteType)
        {
            case SpriteType.Empty:
                break;
            case SpriteType.CornerDouble:
                if (IsDouble(top) && IsDouble(right)) rotaiton = Quaternion.AngleAxis(90, Vector3.forward);
                else if (IsDouble(top) && IsDouble(left)) rotaiton = Quaternion.AngleAxis(180, Vector3.forward);
                else if (IsDouble(bottom) && IsDouble(right)) rotaiton = Quaternion.AngleAxis(0, Vector3.forward);
                else if (IsDouble(bottom) && IsDouble(left)) rotaiton = Quaternion.AngleAxis(-90, Vector3.forward);
                break;
            case SpriteType.WallDouble:
                if (IsDouble(top) && IsDouble(bottom)) rotaiton = Quaternion.AngleAxis(0, Vector3.forward);
                else if (IsDouble(left) && IsDouble(right)) rotaiton = Quaternion.AngleAxis(90, Vector3.forward);
                break;
            case SpriteType.CornerSingle:
                if (IsSingleOrT(top) && IsSingleOrT(right) && left != SpriteType.WallSingle && bottom != SpriteType.WallSingle) rotaiton = Quaternion.AngleAxis(90, Vector3.forward);
                else if (IsSingleOrT(top) && IsSingleOrT(left) && right != SpriteType.WallSingle && bottom != SpriteType.WallSingle) rotaiton = Quaternion.AngleAxis(180, Vector3.forward);
                else if (IsSingleOrT(bottom) && IsSingleOrT(right) && left != SpriteType.WallSingle && top != SpriteType.WallSingle) rotaiton = Quaternion.AngleAxis(0, Vector3.forward);
                else if (IsSingleOrT(bottom) && IsSingleOrT(left) && right != SpriteType.WallSingle && top != SpriteType.WallSingle) rotaiton = Quaternion.AngleAxis(-90, Vector3.forward);
                else if (!IsSingleOrT(topLeft)) rotaiton = Quaternion.AngleAxis(180, Vector3.forward);
                else if (!IsSingleOrT(topRight)) rotaiton = Quaternion.AngleAxis(90, Vector3.forward);
                else if (!IsSingleOrT(bottomLeft)) rotaiton = Quaternion.AngleAxis(-90, Vector3.forward);
                else if (!IsSingleOrT(bottomRight)) rotaiton = Quaternion.AngleAxis(0, Vector3.forward);

                break;
            case SpriteType.WallSingle:
                if (IsSingleOrT(top) && IsSingleOrT(bottom)) rotaiton = Quaternion.AngleAxis(0, Vector3.forward);
                else if (IsSingleOrT(left) && IsSingleOrT(right)) rotaiton = Quaternion.AngleAxis(90, Vector3.forward);
                break;
            case SpriteType.NormalCircle:
                break;
            case SpriteType.PowerCircle:
                break;
            case SpriteType.JunctionPiece:
                if (IsDouble(left) && IsDouble(right) && IsSingleOrT(bottom)) rotaiton = Quaternion.AngleAxis(0, Vector3.forward);
                else if (IsDouble(left)&& IsDouble(right) && IsSingleOrT(top)) rotaiton = Quaternion.AngleAxis(180, Vector3.forward);
                else if (IsDouble(top)&& IsDouble(bottom) && IsSingleOrT(right)) rotaiton = Quaternion.AngleAxis(90, Vector3.forward);
                else if (IsDouble(top) && IsDouble(bottom) && IsSingleOrT(left)) rotaiton = Quaternion.AngleAxis(-90, Vector3.forward);
                break;
            default:
                break;
        }
        return rotaiton;
    }

    private SpriteType GetSpriteType(int x, int y)
    {
        if (x < 0 || x >= globalLevelMap.GetLength(0) || y < 0 || y >= globalLevelMap.GetLength(1))
        {
            return SpriteType.OutRange;
        }
        return (SpriteType)globalLevelMap[x, y];
    }


    private bool IsDouble(SpriteType spriteType)
    {
        return spriteType == SpriteType.WallDouble || spriteType == SpriteType.CornerDouble || spriteType == SpriteType.JunctionPiece;
    }

    private bool IsSingleOrT(SpriteType spriteType)
    {
        return spriteType == SpriteType.WallSingle || spriteType == SpriteType.CornerSingle || spriteType == SpriteType.JunctionPiece;
    }

    public bool CanMove(int x,int y)
    {
        SpriteType spriteType = GetSpriteType(x, y);
        return spriteType != SpriteType.WallDouble && spriteType != SpriteType.CornerDouble && spriteType != SpriteType.CornerSingle && spriteType != SpriteType.WallSingle
            && spriteType != SpriteType.JunctionPiece;
    }

    public Vector3 GetPosition(int x,int y)
    {
        return zeroPointPosition + Vector3.right * x - Vector3.up * y;
    }
}
