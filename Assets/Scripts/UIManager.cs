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
    [SerializeField] GameObject pause, waitingOthers;
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

    public void SetVictoryScreen(bool IsHunter)
    {
        if(IsHunter)
        {
            //_victoryMeshTMP.text = winner.GetComponent<Hunter>() != null ? "El hunter Wins" : "Los Objetos Wins";
            _victoryMeshTMP.text =  "El hunter Wins";
        }
        else
        {
            _victoryMeshTMP.text = "Los Objetos Wins";
        }
        _victoryTextObject.SetActive(true);
    }
    //public void SetVictoryScreen()
    //{
    //    _victoryTextObject.SetActive(true);
    //    _victoryMeshTMP.text = "Los Objetos Wins";
    //}
    public void HunterWait(bool isWaiting)
    {
        _hunterWait.gameObject.SetActive(isWaiting);
        _hunterWaitTMP.gameObject.SetActive(isWaiting);
    }
    //public void HunterStart()
    //{
    //    _hunterWait.gameObject.SetActive(false);
    //    _hunterWaitTMP.gameObject.SetActive(false);
    //}
    public void HunterButtonClick()
    {
        hunterButton.gameObject.SetActive(false);
        playerButton.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        //GameManager.instance.RPCAddHunter();
        //Como hacer para decirle al spawner que haga un hunter o un player cuando toco esto
        //
        //GameManager.instance
    }
    public void PlayerButtonClick()
    {
        hunterButton.gameObject.SetActive(false);
        playerButton.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
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
    public void Pause(bool isPause)
    {
        pause.SetActive(isPause);
    }
    public void WaitingOthers(bool isWaiting)
    {
        waitingOthers.SetActive(isWaiting);
    }
    //public async void BackToMenu()
    //{
    //    await Disconnect();
    //
    //    SceneManager.LoadScene(0);
    //}
    //public async Task Disconnect()
    //{
    //    if (_runnerInstance == null)
    //        return;
    //
    //    StatusText.text = "Disconnecting...";
    //    PanelGroup.interactable = false;
    //
    //    // Remove shutdown listener since we are disconnecting deliberately
    //    var events = _runnerInstance.GetComponent<NetworkEvents>();
    //    events.OnShutdown.RemoveListener(OnShutdown);
    //
    //    await _runnerInstance.Shutdown();
    //    _runnerInstance = null;
    //
    //    // Reset of scene network objects is needed, reload the whole scene
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}
}
