using UnityEngine;
using UnityEngine.UI;

namespace Script.Gamification.Tutorials{
    public class TutorialHandler : MonoBehaviour {
        private Tutorial tutorial;
        GameObject menu;
        GameObject notification;
        private Transform UiPanel;

        private GameObject header;
        private GameObject body;

        public void SetTutorial(Tutorial newTutorial, GameObject _menu, GameObject _notification, Transform _UiPanel){
            menu = _menu;
            notification = _notification;
            tutorial = newTutorial;
            UiPanel = _UiPanel;
            Debug.Log($"Assigned ScriptableObject: {tutorial.name}");
            gameObject.GetComponent<Button>().onClick.AddListener(LoadTutorial);
        }
        
        public void CompleteSetTutorial(Tutorial newTutorial, GameObject _header, GameObject _body ,GameObject _notification, Transform _UiPanel){
            header = _header;
            body = _body;
            notification = _notification;
            tutorial = newTutorial;
            UiPanel = _UiPanel;
            Debug.Log($"Assigned ScriptableObject: {tutorial.name}");
            gameObject.GetComponent<Button>().onClick.AddListener(CompleteLoadTutorial);
        }
        
        public void SetVideoTutorial(Tutorial newTutorial, GameObject _menu, GameObject _notification, Transform _UiPanel){
            menu = _menu;
            notification = _notification;
            tutorial = newTutorial;
            UiPanel = _UiPanel;
            Debug.Log($"Assigned ScriptableObject: {tutorial.name}");
            gameObject.GetComponent<Button>().onClick.AddListener(LoadVideoTutorial);
        }
        

        private void LoadTutorial(){
            notification.SetActive(false);
            menu.SetActive(false);
            Instantiate(tutorial.GetMenu().gameObject, UiPanel);

        }
        
        private void CompleteLoadTutorial(){
            notification.SetActive(false);
            header.SetActive(false);
            body.SetActive(false);
            Instantiate(tutorial.GetMenu().gameObject, UiPanel);

        }
        
        private void LoadVideoTutorial(){
            notification.SetActive(false);
            header.SetActive(false);
            body.SetActive(false);
            Instantiate(tutorial.GetMenu().gameObject, UiPanel);

        }

    }
}
