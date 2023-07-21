using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Main : MonoBehaviour
{
    [SerializeField]
    string playerPrefsInts;//預設題目
    [SerializeField]
    Transform answers;
    Transform[] inputs;
    int Num = 9;
    int Dir = 4;
    int[] put_index = new int[9];//0~8
    int[] put_dir = new int[9];//0~3 應該是逆時針轉
    bool[] state = new bool[9]; //沒用過就會是true
    int[,] Ans_index = new int[60, 10];
    int[,] Ans_dir = new int[60, 10];
    int ans_index = 0;
    void Start()
    {
        PlayerPrefs.DeleteKey("Ints");
        playerPrefsInts = PlayerPrefs.GetString("Ints", playerPrefsInts);
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
                        case 0:
                            //Console.WriteLine("pointer = 0");
                            if (data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 1:
                            //Console.WriteLine("pointer = 1");
                            if (data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 2:
                            //Console.WriteLine("pointer = 2");
                            if (data[put_index[0], (2 + put_dir[0]) % 4] + data[i, (0 + j) % 4] == 0)
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 3:
                            //Console.WriteLine("pointer = 3");
                            if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0) && (data[put_index[1], (2 + put_dir[1]) % 4] + data[i, (0 + j) % 4] == 0))
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 4:
                            //Console.WriteLine("pointer = 4");
                            if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0) && (data[put_index[2], (2 + put_dir[2]) % 4] + data[i, (0 + j) % 4] == 0))
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 5:
                            //Console.WriteLine("pointer = 5");
                            if (data[put_index[3], (2 + put_dir[3]) % 4] + data[i, (0 + j) % 4] == 0)
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 6:
                            //Console.WriteLine("pointer = 6");
                            if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0) && (data[put_index[4], (2 + put_dir[4]) % 4] + data[i, (0 + j) % 4] == 0))
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 7:
                            //Console.WriteLine("pointer = 7");
                            if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0) && (data[put_index[5], (2 + put_dir[5]) % 4] + data[i, (0 + j) % 4] == 0))
                            {
                                count(data, pointer + 1, i, j);
                            }
                            break;
                        case 8:
                            //Console.WriteLine("pointer = 8");
                            break;
                    }
                }
            }
            if (pointer == 8)
            {
                if (i == 0)
                {
                    //Console.WriteLine("Ans: ");
                    answers.GetChild(ans_index).gameObject.SetActive(true);
                    //Debug.Log("Ans: ");
                }
                //Console.Write(put_index[i] + "(" + put_dir[i] + ") ");
                //Debug.Log(put_index[i] + "(" + put_dir[i] + ") ");
                answers.GetChild(ans_index).GetChild(i).GetComponent<Text>().text 
                    = IntToChar(put_index[i])+ put_dir[i] ;
                Ans_index[ans_index, i] = put_index[i];
                Ans_dir[ans_index, i] = put_dir[i];
                if (i == 8)
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
        return int.Parse(inputs[i].GetChild(InputFieldSerial(j)).GetComponent<InputField>().text);
    }
    void SetInputs()//設定輸入內容
    {
        inputs=new Transform[Num];
        SetInputChar();
        StringToInputs();
    }
    void SetInputChar()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i]=transform.GetChild(i);
            //Debug.Log(inputs[i].GetChild(4).name);
            inputs[i].GetChild(4).GetChild(0).GetComponent<Text>().text = IntToChar(i);
        }
    }
    void StringToInputs()
    {
        string[] subs = playerPrefsInts.Split(":");
        for (int i = 0; i < Num; i++)
        {
            string[] subsi = subs[i].Split(",");
            for (int j=0;j<Dir;j++)
            {
                //Debug.Log(inputs[i].GetChild(InputFieldSerial(j)).name);
                InputField inputField 
                    = inputs[i].GetChild(InputFieldSerial(j)).GetComponent<InputField>();
                inputField.text = subsi[j];
            }
        }
    }
    int InputFieldSerial(int i)
    {
        switch (i)
        {
            case 0:
                return 1;
            case 1:
                return 5;
            case 2:
                return 7;
            case 3:
                return 3;
        }
        return -1;
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
