using System;
using System.Collections.Generic;
using UnityEngine;

public static class InstructionsEnimals
{
    private static System.Random rand = new System.Random();
    public static bool isAverage = false;

    public static void StepRabbit(int index)
    {
        //Ходьба кроликов
        if(rand.Next(0,9) == 0)
        {
            List<Vector2Int> list = Map.Init.GetIndexsFreeCellsAround(index);
            if (list.Count == 0) return;

            int i = rand.Next(0, list.Count);
            int newIndex = Map.Init.GetIndex(list[i].x, list[i].y);
            int oldIdEnimal = Map.Init.allCells[index].idEnimal;

            Vector2Int oldPos = Map.Init.GetPosition(index);

            Map.Init.byteMap[list[i].x, list[i].y] = 1;
            Map.Init.byteMap[oldPos.x, oldPos.y] = 0;

            Map.Init.allCells[index].ReDraw(CellType.none);
            Map.Init.allCells[newIndex].ReDraw(CellType.rabbit, oldIdEnimal);
        }
        //Размножение кроликов
        if(rand.Next(0,100) < UIdata.Init.chanse*100)
        {
            List<Vector2Int> list = Map.Init.GetIndexsFreeCellsAround(index);
            if (list.Count == 0) return;

            int i = rand.Next(0, list.Count);
            int newIndex = Map.Init.GetIndex(list[i].x, list[i].y);
            int oldIdEnimal = Map.Init.allCells[index].idEnimal;

            Map.Init.byteMap[list[i].x, list[i].y] = 1;
            Map.Init.allCells[newIndex].ReDraw(CellType.rabbit, oldIdEnimal);
        }
    }

    public static void StepWolfW(int index)
    {
        //Ходьба волчицы
        bool isRabbits = true;
        List<Vector2Int> list = Map.Init.GetIndexsRabbitCellsAround(index);
        if (list.Count == 0) { list = Map.Init.GetIndexsFreeCellsAround(index); isRabbits = false; }
        if (list.Count == 0) { Map.Init.allCells[index].life -= 0.1f; return; }

        int i = rand.Next(0, list.Count);
        int newIndex = Map.Init.GetIndex(list[i].x, list[i].y);
        CellType type = Map.Init.allCells[index].type;
        Vector2Int oldPos = Map.Init.GetPosition(index);

        if (isRabbits)
        {
            Map.Init.allCells[newIndex].life = Map.Init.allCells[index].life + UIdata.Init.prizePointsEatRabbit;
            Map.Init.allCells[index].life = 0;

            Map.Init.byteMap[oldPos.x, oldPos.y] = 0;
            Map.Init.byteMap[list[i].x, list[i].y] = 3;

            Map.Init.allCells[index].ReDraw(CellType.none);
            Map.Init.allCells[newIndex].ReDraw(type);
        }
        else
        {
            Map.Init.allCells[newIndex].life = Map.Init.allCells[index].life - 0.1f;
            Map.Init.allCells[index].life = 0;

            Map.Init.byteMap[oldPos.x, oldPos.y] = 0;
            Map.Init.allCells[index].ReDraw(CellType.none);

            if (Map.Init.allCells[newIndex].life <= 0)
            {
                Map.Init.allCells[newIndex].ReDraw(CellType.none);
                Map.Init.allCells[newIndex].life = 0;
                Map.Init.byteMap[list[i].x, list[i].y] = 0;
            }
            else
            {
                Map.Init.allCells[newIndex].ReDraw(type);
                Map.Init.byteMap[list[i].x, list[i].y] = 3;
            }
        }        
    }

    public static void StepWolfM(int index)
    {
        //Ходьба волка
        bool isRabbits = true;
        bool isFindWolfW = true;
        List<Vector2Int> list = Map.Init.GetIndexsRabbitCellsAround(index);
        if (list.Count == 0) { list = Map.Init.GetIndexsWolfWCellsAround(index); isRabbits = false; }
        if (list.Count == 0) { list = Map.Init.GetIndexsFreeCellsAround(index); isFindWolfW = false; }
        if (list.Count == 0) { Map.Init.allCells[index].life -= 0.1f; return; }

        int i = rand.Next(0, list.Count);
        int newIndex = Map.Init.GetIndex(list[i].x, list[i].y);
        CellType type = Map.Init.allCells[index].type;
        Vector2Int oldPos = Map.Init.GetPosition(index);

        //Погоня за кроликов
        if (isRabbits)
        {
            Map.Init.allCells[newIndex].life = Map.Init.allCells[index].life + UIdata.Init.prizePointsEatRabbit;
            Map.Init.allCells[index].life = 0;

            Map.Init.byteMap[oldPos.x, oldPos.y] = 0;
            Map.Init.byteMap[list[i].x, list[i].y] = 2;

            Map.Init.allCells[index].ReDraw(CellType.none);
            Map.Init.allCells[newIndex].ReDraw(type);
        }
        else
        {
            //Погоня за волчицей
            if (isFindWolfW)
            {
                List<Vector2Int> freeCells = Map.Init.GetIndexsFreeCellsAround(newIndex);
                if (freeCells.Count == 0) freeCells = Map.Init.GetIndexsFreeCellsAround(index);
                if (freeCells.Count == 0)
                {
                    Map.Init.allCells[newIndex].life = Map.Init.allCells[index].life - 0.1f;
                    Map.Init.allCells[index].life = 0;

                    Map.Init.byteMap[oldPos.x, oldPos.y] = 0;
                    Map.Init.allCells[index].ReDraw(CellType.none);

                    if (Map.Init.allCells[newIndex].life <= 0)
                    {
                        Map.Init.allCells[newIndex].ReDraw(CellType.none);
                        Map.Init.allCells[newIndex].life = 0;
                        Map.Init.byteMap[list[i].x, list[i].y] = 0;
                    }
                    else
                    {
                        Map.Init.allCells[newIndex].ReDraw(type);
                        Map.Init.byteMap[list[i].x, list[i].y] = 2;
                    }
                }
                else
                {
                    int j = rand.Next(0, freeCells.Count);
                    int newWolf = Map.Init.GetIndex(freeCells[j].x, freeCells[j].y);
                    CellType newType = (CellType)rand.Next(2,4);
                    Vector2Int newPos = Map.Init.GetPosition(newWolf);

                    float newLife = (isAverage) ? ((Map.Init.allCells[newIndex].life + Map.Init.allCells[index].life) /2) : UIdata.Init.startPoints;

                    Map.Init.allCells[newWolf].life = newLife;
                    Map.Init.allCells[newWolf].ReDraw(newType);
                    Map.Init.byteMap[newPos.x, newPos.y] = (byte)newType;
                }
            }
            //Просто шаг
            else
            {
                Map.Init.allCells[newIndex].life = Map.Init.allCells[index].life - 0.1f;
                Map.Init.allCells[index].life = 0;

                Map.Init.byteMap[oldPos.x, oldPos.y] = 0;
                Map.Init.allCells[index].ReDraw(CellType.none);

                if (Map.Init.allCells[newIndex].life <= 0)
                {
                    Map.Init.allCells[newIndex].ReDraw(CellType.none);
                    Map.Init.allCells[newIndex].life = 0;
                    Map.Init.byteMap[list[i].x, list[i].y] = 0;
                }
                else
                {
                    Map.Init.allCells[newIndex].ReDraw(type);
                    Map.Init.byteMap[list[i].x, list[i].y] = 2;
                }
            }
        }
    }
}
