using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] TextMeshProUGUI _victoryMeshTMP;
    private GameObject _victoryTextObject;
    [SerializeField] Image _hunterWait;
    [SerializeField] TextMeshProUGUI _hunterWaitTMP;
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

    public void SetVictoryScreen(Hunter winner)
    {
        _victoryTextObject.SetActive(true);
        //_victoryMeshTMP.text = winner.GetComponent<Hunter>() != null ? "El hunter Wins" : "Los Objetos Wins";
        _victoryMeshTMP.text =  "El hunter Wins";
    }
    public void SetVictoryScreen()
    {
        _victoryTextObject.SetActive(true);
        _victoryMeshTMP.text = "Los Objetos Wins";
    }
    public void HunterWait()
    {
        _hunterWait.gameObject.SetActive(true);
        _hunterWaitTMP.gameObject.SetActive(true);
    }
    public void HunterStart()
    {
        _hunterWait.gameObject.SetActive(false);
        _hunterWaitTMP.gameObject.SetActive(false);
    }
}
