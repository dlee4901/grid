using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetcodeTest : MonoBehaviour
{
    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;

    private void Awake()
    {
        _startHostButton.onClick.AddListener(() =>
        {
            Debug.Log("Host started");
            NetworkManager.Singleton.StartHost();
            gameObject.SetActive(false);
        });
        _startClientButton.onClick.AddListener(() =>
        {
            Debug.Log("Client started");
            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false);
        });
    }
}
