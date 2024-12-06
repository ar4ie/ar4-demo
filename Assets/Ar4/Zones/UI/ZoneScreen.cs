using Ar4.Base.UI;
using Ar4.Entities.Players;
using UnityEngine;

namespace Ar4.Zones.UI
{
    public class ZoneScreen : Screen<Zone>
    {
        [SerializeField] ZoneScreenView view;
        Player _localPlayer;
        
        public override void Init(Zone model)
        {
            base.Init(model);
            Model.PlayerSpawned += OnPlayerSpawned;
            Model.PlayerDespawned += OnPlayerDespawned;
            foreach (var player in Model.Players.Values)
                OnPlayerSpawned(player);
        }

        public void OnDestroy()
        {
            foreach (var player in Model.Players.Values)
                OnPlayerDespawned(player);
        }

        void OnPlayerSpawned(Player player)
        {
            if (player.Object.InputAuthority != player.Runner.LocalPlayer)
                return;
            _localPlayer = player;
            _localPlayer.RenderScoreChanged += OnRenderScoreChanged;
            OnRenderScoreChanged();
        }

        void OnPlayerDespawned(Player player)
        {
            if (player != _localPlayer)
                return;
            _localPlayer.RenderScoreChanged -= OnRenderScoreChanged;
            _localPlayer = null;
        }

        void OnRenderScoreChanged()
        {
            view.UpdateScore(_localPlayer.Score);
        }
    }
}