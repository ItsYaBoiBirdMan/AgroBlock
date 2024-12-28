using UnityEngine;
using UnityEngine.UI;

namespace Script.Gamification.Tutorials{
    public class TutorialHandler : MonoBehaviour {
        private Tutorial tutorial;
        GameObject menu;
        GameObject notification;
        private Transform UiPanel;

        public void SetTutorial(Tutorial newTutorial, GameObject _menu, GameObject _notification, Transform _UiPanel){
            menu = _menu;
            notification = _notification;
            tutorial = newTutorial;
            UiPanel = _UiPanel;
            Debug.Log($"Assigned ScriptableObject: {tutorial.name}");
            gameObject.GetComponent<Button>().onClick.AddListener(LoadTutorial);
        }
        

        private void LoadTutorial(){
            notification.SetActive(false);
            menu.SetActive(false);
            Instantiate(tutorial.GetMenu().gameObject, UiPanel);

        }

    }
}
