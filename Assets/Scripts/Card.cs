public class Card 
{
    public int[] ints;
    public int rotation;
    public int orgin;//原本卡片
    public int target;//要填位置
    public Card() 
    {
        ints = new int[4];
        rotation = 0;
        orgin = -1;
        target = -1;
    }
}
