using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class GetAnswer : MonoBehaviour
{
    [SerializeField]
    Text[] texts;
    [SerializeField]
    Text message;
    [SerializeField]
    string playerPrefsInts;//預設題目
    Card[] orginCards;
    bool isError;
    Card[] tempCards;
    List<int> usedCards;
    int[] positions;
    int tempSerial=0;
    //Ints 輸入數列
    private void Start()
    {
        //PlayerPrefs.DeleteKey("Ints");
        playerPrefsInts = PlayerPrefs.GetString("Ints", playerPrefsInts);
        StringToInputs(playerPrefsInts);
        InitCards();
    }
    private void InitCards()
    {
        positions=new int[9];
        orginCards = new Card[9];
        for (int i = 0; i < orginCards.Length; i++)
            orginCards[i] = new Card();
        tempCards = new Card[50];
        for (int i = 0; i < tempCards.Length; i++)
            tempCards[i] = new Card();
        usedCards = new List<int>();
    }
    public void Solve()
    {
        isError = false;
        for (int i = 0; i < texts.Length; i++)
            SetInts(i, texts[i].text);
        if (isError) 
        { 
            message.text = "輸入有誤";
            return;
        }
        string input = IntsToString();
        PlayerPrefs.SetString("Ints", input);
        //Solve0();
        RecursiveAnswer(0);
    }
    void RecursiveAnswer(int position)
    {
        if (position >= 2)
        {
            DebugList();
            return;
        }
        for (int i = 0; i < 9; i++)
        {
            if (usedCards.Contains(i))
                continue;
            if (position != 0)
                if (!CanLink(position-1,i))
                    continue;
            usedCards.Add(i);
            positions[position] = i;
            RecursiveAnswer(position + 1);
            usedCards.Remove(i);
            //Debug.Log("position:" + position+"i:"+i);
        }
    }
    void DebugList()
    {
        string temp = "";
        foreach (int i in usedCards)
            temp+=i + ",";
        temp = temp.Substring(0, temp.Length - 1) + ";";
        Debug.Log(temp);
    }
    bool CanLink(int sourcePosition,int targetSerial)
    {
        Card target = orginCards[targetSerial];
        Card source;
        switch (sourcePosition)
        {
            case 0:case 1:
                source = orginCards[sourcePosition];
                return CardMatch(source, target, 1);
            case 2:case 5:
                source = orginCards[sourcePosition-2];
                return CardMatch(source, target, 2);
            case 3:case 4:case 6:case 7:
                source = orginCards[sourcePosition];
                if (!CardMatch(source, target, 1))
                    return false;
                source = orginCards[sourcePosition-2];
                return CardMatch(source, target, 2);
            default:
                print("Incorrect SourcePosition.");
                break;
        }
        return false;
    }
    void Solve0()
    {
        for (int i = 0; i < 9; i++)
        {
            tempCards[i].ints = orginCards[i].ints;
            tempCards[i].orgin = i;
            tempCards[i].target = 0;
            tempSerial++;
            usedCards.Add(i);
            Solve1(tempCards[0]);
            DebugTempCards();
        } 
    }
    void DebugTempCards()
    {
        for (int i = 0; i < tempSerial; i++)
            DebugInts(tempCards[i].ints);
    }
    void DebugInts(int[] ints)
    {
        string debugInts = "";
        for (int i = 0; i < ints.Length; i++)
            debugInts+=ints[i]+",";
        debugInts = debugInts.Substring(0, debugInts.Length - 1) + ";";
        Debug.Log(debugInts);
    }
    void Solve1(Card card)//左邊要跟 card 配合，且不能使用過 
    {
        for (int i = 0; i < 9; i++)
        {
            if (usedCards.Contains(i))
                continue;
            if (CardMatch(card, orginCards[i], 1))
            {
                tempCards[i+tempSerial].ints = orginCards[i].ints;
                tempCards[i + tempSerial].orgin = i;
                tempCards[i + tempSerial].target = 1;
                tempSerial++;
            }
        }
    }
    bool CardMatch(Card source,Card target,int direction)
    {
        int sourceInt = CardInt(source, direction);
        int reverserDirection = (direction + 2) % 4;
        int targetInt = CardInt(target, reverserDirection);
        if (sourceInt == -targetInt)
            return true;
        else
            return false;
    }
    int CardInt(Card source, int direction)
    {
        //先不考慮旋轉
        //direction += source.rotation;
        //direction = direction % 4;
        return source.ints[direction];
    }
    string IntsToString()
    {
        string value = "";
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 4; j++)
                value += orginCards[i].ints[j].ToString() + ",";
            value = value.Substring(0, value.Length - 1);
            value += ":";
        }
        value=value.Substring(0, value.Length - 1);
        return value;
    }
    void StringToInputs(string playerPrefsInts)
    {
        string[] subs = playerPrefsInts.Split(":");
        for (int i = 0; i < texts.Length; i++)
        {
            InputField inputField= texts[i].GetComponentInParent<InputField>();
            inputField.text = subs[i];
        }
    }
    void SetInts(int serial,string one)
    {
        string[] subs = one.Split(",");
        if (subs.Length != 4 )
            {
            Debug.Log("subs.Length:"+ subs.Length);
            isError = true;
            return;
            }
        for (int i=0;i<4;i++)
        {
            int number;
            bool success = int.TryParse(subs[i], out number);
            if (!success)
            {
                Debug.Log("subs[i]:" + subs[i]);
                isError = true;
                return;
            }
            orginCards[serial].ints[i]= number;
        }
    }
}
