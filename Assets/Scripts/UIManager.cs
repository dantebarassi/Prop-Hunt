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
    [SerializeField] TextMeshProUGUI _hunterWaitTMP, propsAliveText;
    [SerializeField] Button hunterButton;
    [SerializeField] Button playerButton;
    [SerializeField] GameObject pause, waitingOthers;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        _victoryTextObject = _victoryMeshTMP.gameObject;
        _victoryTextObject.SetActive(false);
    }

    public void SetVictoryScreen(bool IsHunter)
    {
        if(IsHunter)
        {
            _victoryMeshTMP.text =  "El hunter Wins";
        }
        else
        {
            _victoryMeshTMP.text = "Los Objetos Wins";
        }
        _victoryTextObject.SetActive(true);
    }
    public void HunterWait(bool isWaiting)
    {
        _hunterWait.gameObject.SetActive(isWaiting);
        _hunterWaitTMP.gameObject.SetActive(isWaiting);
    }
    public void HunterButtonClick()
    {
        hunterButton.gameObject.SetActive(false);
        playerButton.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void PlayerButtonClick()
    {
        hunterButton.gameObject.SetActive(false);
        playerButton.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void PrenderSelector(UnityEngine.Events.UnityAction hunterAction, UnityEngine.Events.UnityAction playerAction)
    {
        hunterButton.gameObject.SetActive(true);
        playerButton.gameObject.SetActive(true);
        hunterButton.onClick.AddListener(hunterAction);
        playerButton.onClick.AddListener(playerAction);
    }
    public void Pause(bool isPause)
    {
        pause.SetActive(isPause);
    }
    public void WaitingOthers(bool isWaiting)
    {
        waitingOthers.SetActive(isWaiting);
    }
    public void ChangePropsAlive(int propsAlive, int cantProps)
    {
        propsAliveText.text = "Props Alive: " + propsAlive + "/" + cantProps;
    }
}
