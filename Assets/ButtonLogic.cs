using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    public int numberSelected;
    private bool _isNumberSelected = false;
    private int[,] _numberBoard = new int[10,10];
    
    public GameObject gameBoard;


    void Start()
    {
        
    }
    
    
    public void NumberButtonClicked(String text)
    {
        if (numberSelected == int.Parse(text))
        {
            _isNumberSelected = false;
            return;
        }
        numberSelected = int.Parse(text);
        _isNumberSelected = true;
    }

    void Update()
    {
        
    }

    public void InputButtonClicked(int position)
    {
        string sector = position.ToString().Substring(0, 1);
        string squareInSector = position.ToString().Substring(1, 1);
        GameObject inputSquare = gameBoard.transform.Find("Sector" + sector).transform.Find(squareInSector + "Input").gameObject;
        int x = ((int.Parse(sector) + 2) % 3) * 3 + 1 + ((int.Parse(squareInSector) + 2) % 3);
        int y = 11 - (int) (Math.Ceiling( (float)int.Parse(sector)/3)-1)*3-1
                - (int) Math.Ceiling((float)int.Parse(squareInSector)/3);
        if (CheckForNumberInRowAndColumn(x, y, int.Parse(sector)))
        {
            var color = inputSquare.GetComponent<Image>().color = new Color(255,0,0,255);
        }
        else
        {
            var color = inputSquare.GetComponent<Image>().color = new Color(255,255,255,255);
        }
        inputSquare.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=numberSelected.ToString();
        _numberBoard[x, y]=numberSelected;
        
    }

    bool CheckForNumberInRowAndColumn(int x, int y, int sector)
    {
        for (var i = 0; i < 10; i++)
        {
            if (_numberBoard[i, y] == numberSelected || _numberBoard[x, i] == numberSelected) return true;
        }

        var otherNumbers = gameBoard.transform.Find("Sector" + sector).GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var t in otherNumbers)
        {
            if (t.text == numberSelected.ToString()) return true;
        }
        return false;
    }
}
