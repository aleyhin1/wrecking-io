using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorPanel : MonoBehaviour
{
    public TextMeshProUGUI CarColorText;
    public TextMeshProUGUI PlayerColorText;

    private int _carColorIndex = 0;
    private int _playerColorIndex = 0;

    public void ChangeCarColorRight()
    {
        _carColorIndex++;

        ChangeCarColor();
    }

    public void ChangeCarColorLeft()
    {
        _carColorIndex--;

        ChangeCarColor();
    }

    private void ChangeCarColor()
    {
        _carColorIndex += CarColorChanger.Instance.CarMaterials.Length;
        _carColorIndex = _carColorIndex % CarColorChanger.Instance.CarMaterials.Length;
        CarColorChanger.Instance.RPC_ChangeCarColor(_carColorIndex);
        CarColorText.text = CarColorChanger.Instance.CarMaterials[_carColorIndex].name;
    }

    public void ChangePlayerColorRight()
    {
        _playerColorIndex++;

        ChangePlayerColor();
    }

    public void ChangePlayerColorLeft()
    {
        _playerColorIndex--;

        ChangePlayerColor();
    }

    private void ChangePlayerColor()
    {
        _playerColorIndex += CarColorChanger.Instance.PlayerMaterials.Length;
        _playerColorIndex = _playerColorIndex % CarColorChanger.Instance.PlayerMaterials.Length;
        CarColorChanger.Instance.RPC_ChangePlayerColor(_playerColorIndex);
        PlayerColorText.text = CarColorChanger.Instance.PlayerMaterials[_playerColorIndex].name;
    }
}
