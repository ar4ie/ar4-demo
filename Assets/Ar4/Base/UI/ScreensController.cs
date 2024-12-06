using UnityEngine;

namespace Ar4.Base.UI
{
    public class ScreensController : MonoBehaviour
    {
        Screen _currentScreen;
        
        public void OpenScreen<TModel>(Screen<TModel> screenPrefab, TModel model)
        {
            CloseScreen();
            var screen = Instantiate(screenPrefab, transform);
            screen.Init(model);
            _currentScreen = screen;
        }

        public void CloseScreen()
        {
            if (_currentScreen == null)
                return;
            Destroy(_currentScreen.gameObject);
            _currentScreen = null;
        }
    }
}
