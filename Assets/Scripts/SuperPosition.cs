using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPosition
{
    List<int> _possibleValues = new List<int>();
    bool _observed = false;

    public SuperPosition(int maxValue)
    {
        for (int i = 0; i < maxValue; i++)
        {
            _possibleValues.Add(i);
        }
    }

    public int GetObservedValue()
    {
        return _possibleValues[0];
    }

    public int Observe()
    {
        // Choose a random index from the possible values
        int randomIndex = Random.Range(0, _possibleValues.Count);
        int chosenValue = _possibleValues[randomIndex];

        // Clear all other possible values and leave only the chosen one
        _possibleValues.Clear();
        _possibleValues.Add(chosenValue);

        // Mark this superposition as observed
        _observed = true;

        return chosenValue;
    }


    public bool IsObserved()
    {
        return _observed;
    }

    public void RemovePossibleValue(int value)
    {
        _possibleValues.Remove(value);
    }

    public int NumOptions{
        get
        {
            return _possibleValues.Count;
        }
    }

    public List<int> GetPossibleValues()
    {
        return new List<int>(_possibleValues);
    }

}
