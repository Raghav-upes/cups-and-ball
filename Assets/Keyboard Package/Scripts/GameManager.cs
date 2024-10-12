using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    


    private TMP_InputField textImp;


    private void Start()
    {
        Instance = this;

    }

    public void changeKeyboardObject(TMP_InputField obj)
    {
        textImp = obj;
    }

    public void DeleteLetter()
    {
        
            if (textImp.text.Length != 0)
            {
                textImp.text = textImp.text.Remove(textImp.text.Length - 1, 1);
            }
        

    }

    public void AddLetter(string letter)
    {
        if (textImp.name == "uNameMath")
        {
            if ("1234567890".Contains(letter))
            {
                textImp.text = textImp.text + letter;
            }
            else
                return;
        }
        else
        {
            textImp.text = textImp.text + letter;
        }
        

    }




}
