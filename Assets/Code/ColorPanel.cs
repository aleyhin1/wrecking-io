using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPanel : MonoBehaviour
{
    private int index = 0;

    public void ChangeColorRight()
    {
        index++;
        index = index % ColorChanger.instance.CarMaterials.Length;
        ColorChanger.instance.RPC_ChangeCarColor(index);
    }

    public void ChangeColorLeft()
    {
        if (index > 0)
        {
            index--;
            index = index % ColorChanger.instance.CarMaterials.Length;
            ColorChanger.instance.RPC_ChangeCarColor(index);
        }
    }
}
