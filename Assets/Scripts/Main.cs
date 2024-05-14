using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField]
    string playerPrefsInts;//�w�]�D��
    [SerializeField]
    Transform answers;
    [SerializeField]
    GameObject quitButton;
    [SerializeField]
    GameObject message;
    Transform[] inputs;
    int Num = 9;
    int Dir = 4;
    int[] put_index = new int[9];//0~8
    int[] put_dir = new int[9];//0~3 ���ӬO�f�ɰw��
    bool[] state = new bool[9]; //�S�ιL�N�|�Otrue
    int[,] Ans_index = new int[10000, 10];
    int[,] Ans_dir = new int[10000, 10];
    int ans_index = 0;
    //bool[] canEqual = new bool[5];//�O�_�i�H����
    void Start()
    {
#if UNITY_WEBGL
        quitButton.SetActive(false);
#endif
        SetInputs();
    }
    public void Solve()
    {
        ans_index = 0;
        //���NAnswers��������
        for (int i = 0;i<answers.childCount;i++)
            answers.GetChild(i).gameObject.SetActive(false);
        int[,] data = new int[Num, Dir];
        //SetCanEqual();
        for (int i = 0; i < Num; i++)
        {
            //Console.WriteLine("Input num " + (i));
            for (int j = 0; j < Dir; j++)
            {
                //data[i, j] = Convert.ToInt32(Console.ReadLine());
                data[i, j] = GetInput(i, j);
                //if (data[i, j] < 0)
                //    canEqual[-data[i, j]] = false;
                //Debug.Log("["+i+"]["+j+"]data[i, j]:" + data[i, j]);
            }
            put_index[i] = -1;
            put_dir[i] = 0;
            state[i] = true;
        }
        for (int now = 0; now < Num; now++)
        {
            for (int dir = 0; dir < Dir; dir++)
                count(data, 0, now, dir);
        }
        string ansText="";
        for (int i = 0; i < ans_index - 1; i++)
            for (int j = 0; j < 9; j++)
                ansText += IntToChar(Ans_index[i, j]) + Ans_dir[i, j];
        //Debug.Log(ansText);
        DisplayAnswer();
    }
    void DisplayAnswer()
    {
        message.SetActive(true);
        if (ans_index>9000)
             message.transform.GetChild(1).GetComponent<Text>().text= "Solutions > 9000";
        else
        message.transform.GetChild(1).GetComponent<Text>().text 
                = "Solutions = "+ ans_index.ToString();
    }
    /*bool CanEqual(int i)
    {
        if (i<0)
            return false;
        return canEqual[i];
    }
    void SetCanEqual()
    {
        for (int i = 0; i < canEqual.Length; i++)
            canEqual[i] = true;
    }*/
    void count(int[,] data, int pointer, int now, int dir)
    {
        //�W�L 600 �յ��״N���ޤF
        if (ans_index > 9000)
            return;
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
                            //if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                            //     || ((data[now, (1 + dir) % 4] == data[i, (3 + j) % 4])
                            //     && (CanEqual(data[now, (1 + dir) % 4]))))
                            if (data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                                count(data, pointer + 1, i, j);
                              break;
                        case 1:
                            //Console.WriteLine("pointer = 1");
                            if (data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                            //if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                            //    || ((data[now, (1 + dir) % 4] == data[i, (3 + j) % 4] ))
                            //    &&(CanEqual(data[now, (1 + dir) % 4])))
                                  count(data, pointer + 1, i, j);
                            break;
                        case 2:
                            //Console.WriteLine("pointer = 2");
                            if (data[put_index[0], (2 + put_dir[0]) % 4] + data[i, (0 + j) % 4] == 0)
                            //    if ((data[put_index[0], (2 + put_dir[0]) % 4] + data[i, (0 + j) % 4] == 0)
                            //    || ((data[put_index[0], (2 + put_dir[0]) % 4] == data[i, (0 + j) % 4])
                            //    &&(CanEqual(data[i, (0 + j) % 4]))))
                                count(data, pointer + 1, i, j);
                            break;
                        case 3:
                            //Console.WriteLine("pointer = 3");
                            /*if (((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
    || ((data[now, (1 + dir) % 4] == data[i, (3 + j) % 4]))
    && (CanEqual(data[now, (1 + dir) % 4])))
    && ((data[put_index[1], (2 + put_dir[1]) % 4] + data[i, (0 + j) % 4] == 0)
    || ((data[put_index[1], (2 + put_dir[1]) % 4] == data[i, (0 + j) % 4])
    && (CanEqual(data[i, (0 + j) % 4])))))*/
                            if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                                && (data[put_index[1], (2 + put_dir[1]) % 4] + data[i, (0 + j) % 4] == 0))
                                count(data, pointer + 1, i, j);
                            break;
                        case 4:
                            //Console.WriteLine("pointer = 4");
                            if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                                && (data[put_index[2], (2 + put_dir[2]) % 4] 
                                + data[i, (0 + j) % 4] == 0))
                              /*  if (((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                                || ((data[now, (1 + dir) % 4] == data[i, (3 + j) % 4] )
                                &&(CanEqual(data[i, (3 + j) % 4]))))
                                && ((data[put_index[2], (2 + put_dir[2]) % 4] + data[i, (0 + j) % 4] == 0)
                                || ((data[put_index[2], (2 + put_dir[2]) % 4] == data[i, (0 + j) % 4] )
                                &&(CanEqual(data[i, (0 + j) % 4])))))*/
                                count(data, pointer + 1, i, j);
                            break;
                        case 5:
                            //Console.WriteLine("pointer = 5");
                            if (data[put_index[3], (2 + put_dir[3]) % 4] + data[i, (0 + j) % 4] == 0)
                                /*
                                if ((data[put_index[3], (2 + put_dir[3]) % 4] + data[i, (0 + j) % 4] == 0)
                                || ((data[put_index[3], (2 + put_dir[3]) % 4] == data[i, (0 + j) % 4] )
                                &&(CanEqual(data[i, (0 + j) % 4]))))*/
                                count(data, pointer + 1, i, j);
                            break;
                        case 6:
                            //Console.WriteLine("pointer = 6");
                            if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                                && (data[put_index[4], (2 + put_dir[4]) % 4] 
                                + data[i, (0 + j) % 4] == 0))
                               /* if (((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                                || ((data[now, (1 + dir) % 4] == data[i, (3 + j) % 4])
                                &&(CanEqual(data[i, (3 + j) % 4])))) 
                                && ((data[put_index[4], (2 + put_dir[4]) % 4] + data[i, (0 + j) % 4] == 0)
                                || ((data[put_index[4], (2 + put_dir[4]) % 4] == data[i, (0 + j) % 4] )
                                &&(CanEqual(data[i, (0 + j) % 4])))))*/
                                count(data, pointer + 1, i, j);
                            break;
                        case 7:
                            //Console.WriteLine("pointer = 7");
                            if ((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
                                && (data[put_index[5], (2 + put_dir[5]) % 4] 
                                + data[i, (0 + j) % 4] == 0))
                            /*if (((data[now, (1 + dir) % 4] + data[i, (3 + j) % 4] == 0)
|| ((data[now, (1 + dir) % 4] == data[i, (3 + j) % 4])
&& (CanEqual(data[i, (3 + j) % 4]))))
&& ((data[put_index[5], (2 + put_dir[5]) % 4] + data[i, (0 + j) % 4] == 0)
|| ((data[put_index[5], (2 + put_dir[5]) % 4] == data[i, (0 + j) % 4])
&& (CanEqual(data[i, (0 + j) % 4])))))*/
                            {
                                count(data, pointer + 1, i, j);
                                //Debug.Log(ans_index+"|"+ Ans_index[ans_index, i] + "|" + put_index[i]);
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
                    //Console.WriteLine("Ans: ");��ܵ���
                    if (ans_index <= 40)
                        answers.GetChild(ans_index).gameObject.SetActive(true);
                    //Debug.Log("ans_index:"+ ans_index);
                }
                //Console.Write(put_index[i] + "(" + put_dir[i] + ") ");
                //Debug.Log("ans_index:" + ans_index +":i:"+i + ":"+put_index[i] + "(" + put_dir[i] + ") ");
                if (ans_index <= 40)
                    answers.GetChild(ans_index).GetChild(i).GetComponent<Text>().text 
                    = IntToChar(put_index[i])+ put_dir[i] ;
                //Debug.Log("ans_index:" + ans_index);
                Ans_index[ans_index, i] = put_index[i];
                Ans_dir[ans_index, i] = put_dir[i];
                if (i == 8)
                {
                    //Console.WriteLine(".");
                    //Debug.Log(".");
                    if (!AnswerExist())
                        ans_index++;
                    else if (ans_index <= 40)
                        answers.GetChild(ans_index).gameObject.SetActive(false);
                }
            }
        }
        state[now] = true;
    }
    bool AnswerExist()
    {
        int[] intsA;
        //int[] intsB = new int[Num];
        int[] intsB = SetInts(ans_index);
        //DebugInts("B:",intsB);
        for (int i = 0; i < ans_index; i++)
        {
            intsA= SetInts(i);
            //DebugInts("A:", intsA);
            if (ArrayRotateEqual(intsA, intsB))
            {
                //Debug.Log("i:"+i+"ans_index:"+ans_index);
                return true;
            }
        }
        return false;
    }
    void DebugInts(string head,int[] ints)
    {
        string text=head;
        for (int i = 0; i < Num; i++)
            text += ints[i].ToString();
        //Debug.Log(text);
    }
    int[] SetInts(int index)
    {
        int[] ints = new int[Num];
        for (int i = 0; i < Num; i++)
            ints[i] = Ans_index[index, i];
        return ints;
    } 
    bool ArrayEqual(int[] intsA, int[] intsB)
    {
        for (int i = 0; i < Num; i++)
            if (intsA[i] != intsB[i])
                return false;
        return true;
    }
    bool ArrayRotateEqual(int[] intsA, int[] intsB)
    {
        for (int i = 0; i < Dir; i++)
            if (ArrayEqual(RotateAns(intsA, i), intsB))
                return true;
        return false;
    }
    int[] RotateAns(int[] ints,int rotate)
    {
        int[] returnInts = SetValue(ints);
        //DebugInts("before rotate:" + rotate.ToString() + ":", returnInts);
        //DebugInts("ints:" + rotate.ToString() + ":", ints);
        switch (rotate)
        {
            case 1:
                returnInts[0] = ints[6];
                returnInts[1] = ints[3];
                returnInts[2] = ints[0];
                returnInts[3] = ints[7];
                returnInts[5] = ints[1];
                returnInts[6] = ints[8];
                returnInts[7] = ints[5];
                returnInts[8] = ints[2];
                //DebugInts("ints294:" + rotate.ToString() + ":", ints);
                //DebugInts("rotate:" + rotate.ToString() + ":", returnInts);
                break;
            case 2:
                returnInts[0] = ints[8];
                returnInts[1] = ints[7];
                returnInts[2] = ints[6];
                returnInts[3] = ints[5];
                returnInts[5] = ints[3];
                returnInts[6] = ints[2];
                returnInts[7] = ints[1];
                returnInts[8] = ints[0];
                //Debug.Log("2");
                break;
            case 3:
                returnInts[0] = ints[2];
                returnInts[1] = ints[5];
                returnInts[2] = ints[8];
                returnInts[3] = ints[1];
                returnInts[5] = ints[7];
                returnInts[6] = ints[0];
                returnInts[7] = ints[3];
                returnInts[8] = ints[6];
                //Debug.Log("3");
                break;
            default:
                //Debug.Log("Default case");
                break;
        }
        //DebugInts("rotate:"+ rotate.ToString()+":", returnInts);
        return returnInts;
    }
    int[] SetValue(int[] intValues)
    {
        int[] returnInts = new int[9];
        for (int i = 0; i < 9; i++)
            returnInts[i] = intValues[i];
        return returnInts;
    }
    int GetInput(int i,int j)
    {
        return int.Parse(inputs[i].GetChild(InputFieldSerial(j)).GetComponent<InputField>().text);
    }
    void SetInputs()//�]�w��J���e
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
