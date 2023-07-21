using UnityEngine;

public class Select46 : MonoBehaviour
{
    [SerializeField]
    GameObject edage4;
    [SerializeField]
    GameObject edage6;
    public void Select(bool isEdage4)
    {
        if (isEdage4)
        {
            edage4.SetActive(true);
            edage6.SetActive(false);
        }
        else
        {
            edage4.SetActive(false);
            edage6.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
