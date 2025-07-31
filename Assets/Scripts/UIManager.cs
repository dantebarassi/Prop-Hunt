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
    [SerializeField] Button hunterButton;
    [SerializeField] Button playerButton;
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
    public void HunterButtonClick()
    {
        hunterButton.gameObject.SetActive(false);
        playerButton.gameObject.SetActive(false);
        //GameManager.instance.RPCAddHunter();
        //Como hacer para decirle al spawner que haga un hunter o un player cuando toco esto
        //
        //GameManager.instance
    }
    public void PlayerButtonClick()
    {
        hunterButton.gameObject.SetActive(false);
        playerButton.gameObject.SetActive(false);
        //GameManager.instance.RPCAddPlayer();
        //Como hacer para decirle al spawner que haga un hunter o un player cuando toco esto
        //
        //GameManager.instance
    }
    public void PrenderSelector(UnityEngine.Events.UnityAction hunterAction, UnityEngine.Events.UnityAction playerAction)
    {
        hunterButton.gameObject.SetActive(true);
        playerButton.gameObject.SetActive(true);
        hunterButton.onClick.AddListener(hunterAction);
        playerButton.onClick.AddListener(playerAction);
        //GameManager.instance.RPCAddPlayer();
        //Como hacer para decirle al spawner que haga un hunter o un player cuando toco esto
        //
        //GameManager.instance
    }
}
