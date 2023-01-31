using UnityEngine;

namespace WindTurbineVR.Object.Scriptable
{
    [CreateAssetMenu(fileName = "New Info Card", menuName = "Info Card")]
    public class InfoCard : ScriptableObject
    {
        public string title;
        public string description;
    }
}
