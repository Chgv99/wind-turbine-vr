using UnityEngine;

namespace WindTurbineVR.Data.Scriptable
{
    [CreateAssetMenu(fileName = "New Info Card", menuName = "Info Card")]
    public class InfoCard : ScriptableObject
    {
        public string title;
        public string description;
        public string url;
    }
}
