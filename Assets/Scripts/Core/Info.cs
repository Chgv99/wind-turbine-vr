using UnityEngine;
//using WindTurbineVR.Core.Scriptable;

namespace WindTurbineVR.Core
{
    public class Info : MonoBehaviour
    {
        public bool useInfoCard = false;

        [SerializeField] Scriptable.InfoCard infoCard;

        [SerializeField] string title;
        [SerializeField] string description;

        public string Title 
        { 
            get
            {
                return useInfoCard ? infoCard.title : title;
            }
            set => title = value; 
        }

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
