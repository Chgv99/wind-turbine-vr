using UnityEngine;
using WindTurbineVR.Object.Scriptable;

namespace WindTurbineVR.Object
{
    public class Info : MonoBehaviour
    {
        public bool useInfoCard = false;

        [SerializeField] InfoCard infoCard;

        [SerializeField] string title;
        [SerializeField] string description;

        public string Title 
        { 
            get
            {
                return useInfoCard ? infoCard.title : title;
            }
            set => title = value; }
        public string Description 
        { 
            get 
            {
                return useInfoCard ? infoCard.description : description;
            } 
            set => description = value; 
        }
    }
}
