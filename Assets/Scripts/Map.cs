using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public enum CellType:byte
{
    none = 0,
    rabbit = 1,
    wolf_m = 2,
    wolf_w = 3
}

public class Map : MonoBehaviour
{
    public int MapSize = 20;
    public bool isSimulated = false;

    [Header("Игровый объекты")]
    public GameObject cell;

    public byte[,] byteMap = new byte[0, 0];
    public Cell[] allCells = new Cell[0];

    public static Map Init;

    private void Start()
    {
        Init = this;
    }

    public void StartSimulation()
    {
        isSimulated = true;
        StartCoroutine(Simulation());
    }

    public int GetIndex(int x, int y) => x * MapSize + y;
    public Vector2Int GetPosition(int index) => new Vector2Int((int)(index / MapSize), (int)(index % MapSize));
    public List<Vector2Int> GetIndexsFreeCellsAround(int index)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int pos = GetPosition(index);

        if (is_valid(pos.x - 1, pos.y + 1)) result.Add(new Vector2Int(pos.x - 1, pos.y + 1));
        if (is_valid(pos.x - 1, pos.y - 1)) result.Add(new Vector2Int(pos.x - 1, pos.y - 1));
        if (is_valid(pos.x + 1, pos.y + 1)) result.Add(new Vector2Int(pos.x + 1, pos.y + 1));
        if (is_valid(pos.x + 1, pos.y - 1)) result.Add(new Vector2Int(pos.x + 1, pos.y - 1));

        if (is_valid(pos.x, pos.y + 1)) result.Add(new Vector2Int(pos.x, pos.y + 1));
        if (is_valid(pos.x, pos.y - 1)) result.Add(new Vector2Int(pos.x, pos.y - 1));
        if (is_valid(pos.x - 1, pos.y)) result.Add(new Vector2Int(pos.x - 1, pos.y));
        if (is_valid(pos.x + 1, pos.y)) result.Add(new Vector2Int(pos.x + 1, pos.y));

        return result;
    }
    public List<Vector2Int> GetIndexsRabbitCellsAround(int index)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int pos = GetPosition(index);

        if (is_valid(pos.x - 1, pos.y + 1, 1)) result.Add(new Vector2Int(pos.x - 1, pos.y + 1));
        if (is_valid(pos.x - 1, pos.y - 1, 1)) result.Add(new Vector2Int(pos.x - 1, pos.y - 1));
        if (is_valid(pos.x + 1, pos.y + 1, 1)) result.Add(new Vector2Int(pos.x + 1, pos.y + 1));
        if (is_valid(pos.x + 1, pos.y - 1, 1)) result.Add(new Vector2Int(pos.x + 1, pos.y - 1));

        if (is_valid(pos.x, pos.y + 1, 1)) result.Add(new Vector2Int(pos.x, pos.y + 1));
        if (is_valid(pos.x, pos.y - 1, 1)) result.Add(new Vector2Int(pos.x, pos.y - 1));
        if (is_valid(pos.x - 1, pos.y, 1)) result.Add(new Vector2Int(pos.x - 1, pos.y));
        if (is_valid(pos.x + 1, pos.y, 1)) result.Add(new Vector2Int(pos.x + 1, pos.y));

        return result;
    }
    public List<Vector2Int> GetIndexsWolfWCellsAround(int index)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int pos = GetPosition(index);

        if (is_valid(pos.x - 1, pos.y + 1, 3)) result.Add(new Vector2Int(pos.x - 1, pos.y + 1));
        if (is_valid(pos.x - 1, pos.y - 1, 3)) result.Add(new Vector2Int(pos.x - 1, pos.y - 1));
        if (is_valid(pos.x + 1, pos.y + 1, 3)) result.Add(new Vector2Int(pos.x + 1, pos.y + 1));
        if (is_valid(pos.x + 1, pos.y - 1, 3)) result.Add(new Vector2Int(pos.x + 1, pos.y - 1));

        if (is_valid(pos.x, pos.y + 1, 3)) result.Add(new Vector2Int(pos.x, pos.y + 1));
        if (is_valid(pos.x, pos.y - 1, 3)) result.Add(new Vector2Int(pos.x, pos.y - 1));
        if (is_valid(pos.x - 1, pos.y, 3)) result.Add(new Vector2Int(pos.x - 1, pos.y));
        if (is_valid(pos.x + 1, pos.y, 3)) result.Add(new Vector2Int(pos.x + 1, pos.y));

        return result;
    }

    public void GenerateMap(int size = 20)
    {
        RemoveAllCells();
        MapSize = size;
        allCells = new Cell[size * size];
        byteMap = new byte[size, size];

        StartCoroutine(GenerateMap());
        return;

        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                CreateCell(i, j, CellType.none);
            }
        }
    }
    private void CreateCell(int x, int y, CellType type)
    {
        var obj = Instantiate(cell);
        obj.transform.parent = this.transform;
        obj.transform.localScale = Vector3.one;
        allCells[GetIndex(x, y)] = obj.GetComponent<Cell>();
        allCells[GetIndex(x, y)].OnCreate(x, y, CellType.none);
    }

    //args - количество существ того или итого типа:
    //args[0] - кролик
    //args[1] - волк (самец)
    //args[2] - волк (самка)
    public void GenerationOfLivingCreatures(params int[] args)
    {
        RemoveEnimals();
        int index,count = 0;
        int countRabbits = 0, countWolf_m = 0, countWolf_w = 0;

        while (countRabbits < args[0] && count < 100)
        {
            index = Random.Range(0, MapSize * MapSize);

            Vector2Int pos = GetPosition(index);
            allCells[index].OnCreate(pos.x, pos.y, CellType.rabbit);
            byteMap[pos.x, pos.y] = 1;

            countRabbits++;
            count++;
        }
        count = 0;
        while (countWolf_m < args[1] && count < 100)
        {
            index = Random.Range(0, MapSize * MapSize);
            if (allCells[index].type != CellType.none) continue;

            Vector2Int pos = GetPosition(index);
            allCells[index].OnCreate(pos.x, pos.y, CellType.wolf_m);
            byteMap[pos.x, pos.y] = 2;

            countWolf_m++;
            count++;
        }
        count = 0;
        while (countWolf_w < args[2] && count < 100)
        {
            index = Random.Range(0, MapSize * MapSize);
            if (allCells[index].type != CellType.none) continue;

            Vector2Int pos = GetPosition(index);
            allCells[index].OnCreate(pos.x, pos.y, CellType.wolf_w);
            byteMap[pos.x, pos.y] = 3;

            countWolf_w++;
            count++;
        }
    }

    private void RemoveAllCells()
    {
        for (int i = 0; i < allCells.Length; i++)
        {
            Destroy(allCells[i].gameObject);
        }
    }
    private void RemoveEnimals()
    {
        for (int i = 0; i < allCells.Length; i++)
        {
            allCells[i].ReDraw(CellType.none);
        }
    }
    private bool is_valid(int x, int y, int find = 0)
    {
        if (x < 0 || x >= MapSize || y < 0 || y >= MapSize) return false;
        if (byteMap[x, y] == find) return true;
        return false;
    }

    IEnumerator Simulation()
    {
        while (isSimulated)
        {
            int countFreeCells = 0;
            for (int i = 0; i < allCells.Length; i++)
            {
                switch (allCells[i].type)
                {
                    case CellType.rabbit: InstructionsEnimals.StepRabbit(i); break;
                    case CellType.wolf_w: InstructionsEnimals.StepWolfW(i); break;
                    case CellType.wolf_m: InstructionsEnimals.StepWolfM(i); break;
                    case CellType.none: countFreeCells++; break;
                }
            }
            yield return new WaitForSeconds(0.1f);
            if (countFreeCells >= MapSize * MapSize)
            {
                isSimulated = false;
                UIdata.Init.EndSimulation();
            }
        }
    }

    IEnumerator GenerateMap()
    {
        UIdata.Init.loading.SetActive(true);
        UIdata.Init.buttonGenerateEnimals.interactable = false;
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                CreateCell(i, j, CellType.none);
            }
            yield return new WaitForSeconds(0.05f);
        }
        UIdata.Init.loading.SetActive(false);
        UIdata.Init.buttonGenerateEnimals.interactable = true;
    }
}
