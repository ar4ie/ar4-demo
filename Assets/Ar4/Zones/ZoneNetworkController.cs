using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ar4.Zones
{
    public class ZoneNetworkController : MonoBehaviour
    {
        NetworkRunner _runner;
        [SerializeField] Zone zonePrefab;
        string _roomName = "Test Room";

        void OnGUI()
        {
            if (_runner == null)
            {
                _roomName = GUI.TextField(new Rect(8, 8, 192, 32), _roomName);
                if (GUI.Button(new Rect(8,48,192, 32), "Host"))
                    StartGame(GameMode.Host);
                if (GUI.Button(new Rect(8,88,192, 32), "Server"))
                    StartGame(GameMode.Server);
                if (GUI.Button(new Rect(8,128,192, 32), "Client"))
                    StartGame(GameMode.Client);
            }
        }

        async void StartGame(GameMode mode)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = _roomName,
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
            if (_runner.IsServer)
                _runner.Spawn(zonePrefab);
        }
    }
}