using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPanel : MonoBehaviour
{
    private int _index = 0;


    public void ChangeColorRight()
    {
        _index++;
        _index = _index % ColorChanger.Instance.CarMaterials.Length;

        ColorChanger.Instance.RPC_ChangeCarColor(_index);
    }

    public void ChangeColorLeft()
    {
        if (_index > 0)
        {
            _index--;
            _index = _index % ColorChanger.Instance.CarMaterials.Length;

            ColorChanger.Instance.RPC_ChangeCarColor(_index);
        }
    }
}
