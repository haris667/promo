using UnityEngine;
using System.Collections;
using System.Collections.Generic;

sealed public class Squad
{
    [SerializeField] private List<ControlledUnit> units = new List<ControlledUnit>();

    public void AddUnit() => units.Add(new ControlledUnit(null));
    public void RemoveUnitByIndex(int index) => units.RemoveAt(index);
}