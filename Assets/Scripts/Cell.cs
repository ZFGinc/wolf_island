using UnityEngine;

public class Cell : MonoBehaviour
{
    [Header("Спрайты животных")]
    public GameObject[] Sprites;

    [Header("Данный ячейки")]
    public int x, y;
    public CellType type;

    [HideInInspector] public int idEnimal = -1;
    public float life = 1f;

    public void OnCreate(int x, int y, CellType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        life = UIdata.Init.startPoints;

        DrawSprite();
    }
    public void ReDraw(CellType type)
    {
        this.type = type;
        DrawSprite();
    }
    public void ReDraw(CellType type, int idEnimal)
    {
        this.type = type;
        this.idEnimal = idEnimal;
        DrawSprite(idEnimal);
    }

    private void DrawSprite(int idenim = -1)
    {
        HideAll();
        switch (type)
        {
            case CellType.rabbit: DrawRabbit((idenim == -1) ? true : false); break;
            case CellType.wolf_m: Sprites[3].SetActive(true); break;
            case CellType.wolf_w: Sprites[4].SetActive(true); break;
        }
    }
    private void DrawRabbit(bool isRandom = true)
    {
        if (isRandom) idEnimal = Random.Range(0, 3);
        Sprites[idEnimal].SetActive(true);
    }
    private void HideAll()
    {
        foreach (var sprite in Sprites)
            sprite.SetActive(false);
    }
}
