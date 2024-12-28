using UnityEngine;

namespace Script.Gamification.Tutorials
{
    [CreateAssetMenu(menuName = "TutorialsLoaders/Tutorials")]
    public class Tutorial : ScriptableObject
    {
    
        [SerializeField] private string tutorialName;
        [SerializeField] private Transform taskMenu;
        [SerializeField] private int pointsRewardAmount;
    
        public string GetName(){
            return tutorialName;
        }
        public Transform GetMenu(){
            return taskMenu;
        }
        public int GetRewardAmount(){
            return pointsRewardAmount;
        }
    

    
    }
}
