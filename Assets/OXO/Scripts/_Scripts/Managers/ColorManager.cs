using System.Collections.Generic;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;

public class ColorManager : LazySingleton<ColorManager>
{
    public List<Color> cageColorList;
}
