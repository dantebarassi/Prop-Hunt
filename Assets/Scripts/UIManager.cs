using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] TextMeshProUGUI _victoryMeshTMP;
    private GameObject _victoryTextObject;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        _victoryTextObject = _victoryMeshTMP.gameObject;
        _victoryTextObject.SetActive(false);
    }

    public void SetVictoryScreen(GameObject winner)
    {
        _victoryTextObject.SetActive(true);
        _victoryMeshTMP.text = winner.GetComponent<Healt>() == null ? "El hunter Wins" : "Los Objetos Wins";
    }
}
