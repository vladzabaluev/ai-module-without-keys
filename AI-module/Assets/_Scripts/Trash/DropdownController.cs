using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.AddOptions(InterestsContainer.Interests.ToList());
        string text = dropdown.itemText.text;
        Debug.Log(text);
    }
}