using UnityEngine;
using UnityEngine.UI;

public class LocalTest : MonoBehaviour
{
    [SerializeField] private Button _startGame;
    [SerializeField] private Button _player1;
    [SerializeField] private Button _player2;
    [SerializeField] private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _startGame.onClick.AddListener(() => {
            Debug.Log("Start Game Clicked");
            _gameManager.StartGame();
            gameObject.SetActive(false);
        });
        _player1.onClick.AddListener(() => {
            Debug.Log("Player 1 Clicked");
            _gameManager.StartPlacement(1);
        });
        _player2.onClick.AddListener(() => {
            Debug.Log("Player 2 Clicked");
            _gameManager.StartPlacement(2);
        });
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnClick()
    {

    }
}
