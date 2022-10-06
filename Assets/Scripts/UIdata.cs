using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIdata : MonoBehaviour
{
    [Header("Объекты сцены для регулирования правил игры")]
    [Description("Размер карты (матрицы)")] public Slider sliderSizeMap;
    [Description("Количество кроликов")] public Slider sliderCountRabbits;
    [Description("Количество волков (самцов)")] public Slider sliderCountWolfM;
    [Description("Количество волков (самок)")] public Slider sliderCountWolfW;
    [Description("Шанс размножения кроликов")] public Slider sliderChanseRabbitMult;
    [Description("Начальные очки волков")] public Slider sliderWolfStartPoint;
    [Description("Очки за съедание кролика")] public Slider sliderPrizeForEatRabbit;

    [Header("Объекты текстов для показа текущих значений слайдеров")]
    public TMP_Text currSizeMap;
    public TMP_Text currCountRabbits;
    public TMP_Text currCountWolfM;
    public TMP_Text currCountWolfW;
    public TMP_Text currChanseRabbitMult;
    public TMP_Text currWolfStartPoint;
    public TMP_Text currentPrizeForEatRabbit;

    [Header("Иные объекты сцены для правильного функционирования")]
    public Button buttonGenerateMap;
    public Button buttonGenerateEnimals;
    public Button buttonStartSimulation;
    public Button buttonStopSimulation;
    public GridLayoutGroup group;
    public Toggle toggleIsAveragePointWolf;
    public GameObject loading;

    [HideInInspector]
    public float chanse;
    [HideInInspector]
    public float startPoints;
    [HideInInspector]
    public float prizePointsEatRabbit;

    public static UIdata Init;

    private void Start()
    {
        Init = this;
        buttonGenerateMap.interactable = true;
        buttonGenerateEnimals.interactable = false;
        buttonStartSimulation.interactable = false;
        buttonStopSimulation.interactable= false;
        loading.SetActive(false);
    }

    private void FixedUpdate()
    {
        currSizeMap.text = sliderSizeMap.value.ToString();
        currCountRabbits.text = sliderCountRabbits.value.ToString();
        currCountWolfM.text = sliderCountWolfM.value.ToString();
        currCountWolfW.text = sliderCountWolfW.value.ToString();

        startPoints = (float)Math.Round(sliderWolfStartPoint.value,2);
        currWolfStartPoint.text = startPoints.ToString();

        chanse = sliderChanseRabbitMult.value / 100f;
        currChanseRabbitMult.text = (chanse * 100).ToString() + "%";

        prizePointsEatRabbit = (float)Math.Round(sliderPrizeForEatRabbit.value, 1);
        currentPrizeForEatRabbit.text = prizePointsEatRabbit.ToString();
    }

    public void GenerateMap()
    {
        if (Map.Init.isSimulated) return;

        Map.Init.GenerateMap((int)sliderSizeMap.value);
        group.constraintCount = Map.Init.MapSize;
        float size = sizeCell(Map.Init.MapSize);
        group.cellSize = new Vector2(size, size);

        buttonStartSimulation.interactable = false;
        buttonStopSimulation.interactable = false;
    }

    public void GenerateEnimals()
    {
        if (Map.Init.isSimulated) return;
        Map.Init.GenerationOfLivingCreatures((int)sliderCountRabbits.value, (int)sliderCountWolfM.value, (int)sliderCountWolfW.value);

        buttonStartSimulation.interactable = true;
    }

    public void StartSimulation()
    {
        Map.Init.StartSimulation();

        buttonGenerateMap.interactable = false;
        buttonGenerateEnimals.interactable = false;
        buttonStartSimulation.interactable = false;
        buttonStopSimulation.interactable = true;
    }

    public void StopSimulation()
    {
        Map.Init.isSimulated = false;

        buttonGenerateMap.interactable = true;
        buttonGenerateEnimals.interactable = true;
        buttonStartSimulation.interactable = true;
        buttonStopSimulation.interactable = false;
    }

    public void SetAveragePointNewWolf()
    {
        InstructionsEnimals.isAverage = toggleIsAveragePointWolf.isOn;
    }

    public void EndSimulation()
    {
        buttonGenerateMap.interactable = true;
        buttonGenerateEnimals.interactable = true;
        buttonStartSimulation.interactable = false;
        buttonStopSimulation.interactable = false;
    }

    private float sizeCell(int countInRow)
    {
        float mul = (countInRow < 20) ? 4 : (countInRow < 30) ? 1.75f : (countInRow < 40) ? .5f : .15f;
        float start = (countInRow < 20) ? 110 : (countInRow < 30) ? 70 : (countInRow < 40) ? 38f : 22f;
        return start - countInRow * mul;
    }
}
