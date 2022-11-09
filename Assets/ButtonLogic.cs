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
    private int _numberSelected;
    private bool _isNumberSelected = false;
    private int _squaresWrong = 0;
    private int[,] _numberBoard = new int[10,10];
    
    public GameObject gameBoard;
    public TextMeshProUGUI youWonText;


    void Start()
    {
        PrepareForStart(1,3,3);
        PrepareForStart(1,5,6);
        PrepareForStart(1,7,4);
        PrepareForStart(1,8,9);
        PrepareForStart(2,4,9);
        PrepareForStart(2,5,8);
        PrepareForStart(2,8,3);
        PrepareForStart(2,9,1);
        PrepareForStart(3,1,2);
        PrepareForStart(3,5,4);
        PrepareForStart(3,6,3);
        PrepareForStart(3,9,6);
        PrepareForStart(4,1,9);
        PrepareForStart(4,3,7);
        PrepareForStart(4,5,4);
        PrepareForStart(4,9,5);
        PrepareForStart(5,5,9);
        PrepareForStart(5,6,8);
        PrepareForStart(5,7,4);
        PrepareForStart(5,9,7);
        PrepareForStart(6,1,8);
        PrepareForStart(6,2,6);
        PrepareForStart(6,7,1);
        PrepareForStart(6,9,9);
        PrepareForStart(7,1,6);
        PrepareForStart(7,4,5);
        PrepareForStart(7,6,8);
        PrepareForStart(7,7,2);
        PrepareForStart(7,9,9);
        PrepareForStart(8,3,3);
        PrepareForStart(8,4,1);
        PrepareForStart(8,8,5);
        PrepareForStart(8,9,6);
        PrepareForStart(9,1,9);
        PrepareForStart(9,3,5);
        PrepareForStart(9,5,7);
        PrepareForStart(9,6,2);
        PrepareForStart(9,8,3);
        PrepareForStart(9,9,8);
    }
    
    
    public void NumberButtonClicked(String text)
    {
        if (_numberSelected == int.Parse(text))
        {
            _isNumberSelected = false;
            return;
        }
        _numberSelected = int.Parse(text);
        _isNumberSelected = true;
    }

    public void InputButtonClicked(int position)
    {
        if (!_isNumberSelected) return;
        string sector = position.ToString().Substring(0, 1);
        string squareInSector = position.ToString().Substring(1, 1);
        GameObject inputSquare = gameBoard.transform.Find("Sector" + sector).transform.Find(squareInSector + "Input").gameObject;
        int x = ((int.Parse(sector) + 2) % 3) * 3 + 1 + ((int.Parse(squareInSector) + 2) % 3);
        int y = 11 - (int) (Math.Ceiling( (float)int.Parse(sector)/3)-1)*3-1
                - (int) Math.Ceiling((float)int.Parse(squareInSector)/3);
        if (inputSquare.GetComponent<Image>().color == new Color(255, 255, 255, 255) &&
            CheckForNumberInRowAndColumn(x, y, int.Parse(sector)))
        {
            _squaresWrong++;
        }

        if (inputSquare.GetComponent<Image>().color == new Color(255, 0, 0, 255) &&
            !CheckForNumberInRowAndColumn(x, y, int.Parse(sector)))
        {
            _squaresWrong--;
        }
        if (CheckForNumberInRowAndColumn(x, y, int.Parse(sector)))
        {
            var color = inputSquare.GetComponent<Image>().color = new Color(255,0,0,255);
        }
        else
        {
            var color = inputSquare.GetComponent<Image>().color = new Color(255,255,255,255);
        }
        inputSquare.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=_numberSelected.ToString();
        _numberBoard[x, y]=_numberSelected;
        for (var i = 1; i < 10; i++)
        {
            for (var j = 1; j < 10; j++)
            {
                if (_squaresWrong != 0 || gameBoard.transform.Find("Sector" + i).transform.Find(j + "Input").transform
                    .GetChild(0).GetComponent<TextMeshProUGUI>().text.Length != 1)
                {
                    return;
                }
            }
        }
        youWonText.gameObject.SetActive(true);
    }

    bool CheckForNumberInRowAndColumn(int x, int y, int sector)
    {
        for (var i = 0; i < 10; i++)
        {
            if (_numberBoard[i, y] == _numberSelected || _numberBoard[x, i] == _numberSelected) return true;
        }

        var otherNumbers = gameBoard.transform.Find("Sector" + sector).GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var t in otherNumbers)
        {
            if (t.text == _numberSelected.ToString()) return true;
        }
        return false;
    }

    void PrepareForStart(int sector, int square, int number)
    {
        gameBoard.transform.Find("Sector" + sector).transform.Find(square + "Input")
            .transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=number.ToString();
        int x = ((sector + 2) % 3) * 3 + 1 + ((square + 2) % 3);
        int y = 11 - (int) (Math.Ceiling( (float)sector/3)-1)*3-1
                - (int) Math.Ceiling((float)square/3);
        _numberBoard[x, y] = number;
        gameBoard.transform.Find("Sector" + sector).transform.
                Find(square + "Input").GetComponent<Button>().enabled = false;
    }
}
