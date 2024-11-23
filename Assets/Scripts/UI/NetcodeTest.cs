using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetcodeTest : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    void Awake()
    {
        startHostButton.onClick.AddListener(() =>
        {
            Debug.Log("Host started");
            NetworkManager.Singleton.StartHost();
            gameObject.SetActive(false);
        });
        startClientButton.onClick.AddListener(() =>
        {
            Debug.Log("Client started");
            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false);
        });
    }
}
