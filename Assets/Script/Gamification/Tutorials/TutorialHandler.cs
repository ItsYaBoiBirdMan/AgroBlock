using UnityEngine;
using UnityEngine.UI;

namespace Script.Gamification.Tutorials{
    public class TutorialHandler : MonoBehaviour {
        private Tutorial tutorial;

        public void SetTutorial(Tutorial newTutorial){
            tutorial = newTutorial;
            Debug.Log($"Assigned ScriptableObject: {tutorial.name}");
            gameObject.GetComponent<Button>().onClick.AddListener(LoadTutorial);
        }

        private void LoadTutorial(){
            tutorial.GetMenu().SetActive(true);
        }

    }
}
