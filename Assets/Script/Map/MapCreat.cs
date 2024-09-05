using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreat : MonoBehaviour
{
    [SerializeField]
    private TextAsset textFile;

    private string[] textData;
    private string[,] dungeonMap;


    private int WidthNumber; // �񐔂ɑ���
    private int HeightNumber; // �s���ɑ���

    [SerializeField]
    public GameObject FloorPrefab;
    public GameObject WallPrefab;
    public GameObject ClearPrefab;
    public GameObject Slope_Right;
    public GameObject Slope_Left;
    public GameObject Slope_Flont;
    public GameObject Slope_Back;

    private void Awake()
    {
        string textLines = textFile.text;
        print(textLines);


        textData = textLines.Split('\n');


        WidthNumber = textData[0].Split(',').Length;
        HeightNumber = textData.Length;

        print("tate" + HeightNumber);
        print("yoko" + WidthNumber);


        dungeonMap = new string[HeightNumber, WidthNumber];

        for (int i = 0; i < HeightNumber; i++)
        {
            string[] tempWords = textData[i].Split(',');

            for (int j = 0; j < WidthNumber; j++)
            {
                dungeonMap[i, j] = tempWords[j];

                if (dungeonMap[i, j] != null)
                {
                    switch (dungeonMap[i, j])
                    {
                        case "0":
                            //�Ȃ�ɂ��������Ȃ�
                            break;
                        case "1"://�}�b�v�^�C��
                            Instantiate(FloorPrefab, new Vector3(-4.0f * j + 100, 0, 4.0f * i - 10), Quaternion.identity);
                            break;
                        case "2"://Wall tag ��������
                            Instantiate(WallPrefab, new Vector3(-4.0f * j + 100, 1, 4.0f * i - 10), Quaternion.identity);
                            break;
                        case "3"://Wall tag �����Ă��Ȃ���
                            Instantiate(ClearPrefab, new Vector3(-4.0f * j + 100, 1, 4.0f * i - 10), Quaternion.identity);
                            break;
                        case "4"://�}�b�v�^�C��
                            Instantiate(FloorPrefab, new Vector3(-4.0f * j + 100, -5f, 4.0f * i - 10), Quaternion.identity);
                            break;
                        case "5"://����
                            //Instantiate(SlopePrefab, new Vector3(-4.0f * j + 100, -1.8f, 4.0f * i - 10), Quaternion.Euler(0, 0, -10));
                            break;
                        case "6"://����
                            //Instantiate(SlopePrefab2, new Vector3(-4.0f * j + 100, -1.8f, 4.0f * i - 10), Quaternion.Euler(10, 0, 0));
                            break;
                        case "7"://����
                            //Instantiate(SlopePrefab2, new Vector3(-4.0f * j + 100, -1.8f, 4.0f * i - 10), Quaternion.Euler(-10, 0, 0));
                            Instantiate(Slope_Back, new Vector3(-4.0f * j + 100, -2, 4.0f * i - 10), Quaternion.Euler(-20, 0, 0));
                            break;
                        case "8"://����    ���̕����̉����Ă�
                                 //Instantiate(SlopePrefab, new Vector3(-4.0f * j + 100, -1.8f, 4.0f * i - 10), Quaternion.Euler(0, 0, 10));
                            break;
                    }
                }
            }
        }
    }
}
