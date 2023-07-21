using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Main6 : MonoBehaviour
{
    [SerializeField]
    string playerPrefsInts;//預設題目
    [SerializeField]
    Transform answers;
    Transform[] inputs;
    int Num = 7;
    int Dir = 6;
    int[] put_index = new int[9];//0~8
    int[] put_dir = new int[9];//0~3 應該是逆時針轉
    bool[] state = new bool[9]; //沒用過就會是true
    int[,] Ans_index = new int[60, 10];
    int[,] Ans_dir = new int[60, 10];
    int ans_index = 0;
    void Start()
    {
        SetInputs();
    }
    public void Solve()
    {
        //先將Answers全部關閉
        for (int i = 0;i<answers.childCount;i++)
            answers.GetChild(i).gameObject.SetActive(false);
        int[,] data = new int[Num, Dir];
        for (int i = 0; i < Num; i++)
        {
            //Console.WriteLine("Input num " + (i));
            for (int j = 0; j < Dir; j++)
            {
                //data[i, j] = Convert.ToInt32(Console.ReadLine());
                data[i, j] = GetInput(i, j);
                //Debug.Log("["+i+"]["+j+"]data[i, j]:" + data[i, j]);
            }
            put_index[i] = -1;
            put_dir[i] = 0;
            state[i] = true;
        }
        for (int now = 0; now < Num; now++)
        {
            for (int dir = 0; dir < Dir; dir++)
            {
                count(data, 0, now, dir);
            }
        }
    }
    void count(int[,] data, int pointer, int now, int dir)
    {
        //pointer 第幾張牌
        put_index[pointer] = now;
        put_dir[pointer] = dir;
        //Console.WriteLine("put_index: ");
        //for (int i = 0; i < Num; i++)
        //{
        //    Console.Write(put_index[i] + " ");
        //}
        state[now] = false;
        for (int i = 0; i < Num; i++)
        {
            if (state[i])
            {
                for (int j = 0; j < Dir; j++)
                {
                    switch (pointer)
                    {
                        case 0://A-B
                            //Console.WriteLine("pointer = 0");
                            if (data[now, (2 + dir) % 6] == data[i, (5 + j) % 6])
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 1://B-C
                            //Console.WriteLine("pointer = 1");
                            if (data[now, (3 + dir) % 6] == data[i, j % 6])
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 2://C-D
                            //Console.WriteLine("pointer = 2");
                            if (data[now, (4 + dir) % 6] == data[i, (1 + j) % 6])
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 3://D-E
                            //Console.WriteLine("pointer = 3");
                            if (data[now, (5 + dir) % 6] == data[i, (2 + j) % 6])
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 4://E-F
                            //Console.WriteLine("pointer = 4");
                            if ((data[now, (0 + dir) % 6] == data[i, (3 + j) % 6])
                                && (data[put_index[0], (4 + put_dir[0]) % 6] == data[i, (1 + j) % 6]))
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 5://F-G
                               //Console.WriteLine("pointer = 5");
                            if ((data[now, (2 + dir) % 6] == data[i, (5 + j) % 6])
                                && (data[put_index[0], (3 + put_dir[0]) % 6] == data[i, (0 + j) % 6]) 
                                && (data[put_index[1], (4 + put_dir[1]) % 6] == data[i, (1 + j) % 6]) 
                                && (data[put_index[2], (5 + put_dir[2]) % 6] == data[i, (2 + j) % 6]) 
                                && (data[put_index[3], (0 + put_dir[3]) % 6] == data[i, (3 + j) % 6])
                                && (data[put_index[4], (1 + put_dir[4]) % 6] == data[i, (4 + j) % 6]))
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 6:
                            break;
                        case 7:
                            //Console.WriteLine("pointer = 7");
                            break;
                        case 8:
                            //Console.WriteLine("pointer = 8");
                            break;
                    }
                }
            }
            if (pointer == 6)
            {
                if (i == 0)
                {
                    //Console.WriteLine("Ans: ");
                    answers.GetChild(ans_index).gameObject.SetActive(true);
                    //Debug.Log("Ans: ");
                }
                //Console.Write(put_index[i] + "(" + put_dir[i] + ") ");
                //Debug.Log(put_index[i] + "(" + put_dir[i] + ") ");
                answers.GetChild(ans_index).GetChild(i).GetComponentInChildren<Text>().text 
                    = IntToChar(put_index[i])+ put_dir[i] ;
                Ans_index[ans_index, i] = put_index[i];
                Ans_dir[ans_index, i] = put_dir[i];
                if (i == 6)
                {
                    //Console.WriteLine(".");
                    //Debug.Log(".");
                    ans_index++;
                }
            }
        }
        state[now] = true;
    }
    int GetInput(int i,int j)
    {
        return int.Parse(inputs[i].GetChild(j).GetComponent<InputField>().text);
    }
    void SetInputs()//設定輸入內容
    {
        inputs=new Transform[Num];
        StringToInputs();
    }
    void StringToInputs()
    {
        int serial = 0;
        for (int i = 0; i < Num; i++)
        {
            for (int j=0;j<Dir;j++)
            {
                inputs[i] = transform.GetChild(i);
                //Debug.Log(inputs[i].GetChild(j).name);
                InputField inputField 
                    = inputs[i].GetChild(j).GetComponent<InputField>();
                inputField.text = playerPrefsInts.Substring(serial,1);
                serial++;
            }
        }
    }
    string IntToChar(int i)
    {
        return Convert.ToChar(i+65).ToString();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
