
namespace WindTurbineVR.Core
{
    public class TurbineData
    {
        float _wind_speed;
        float _wind_direction;

        public float WindSpeed { get => _wind_speed; }
        public float WindDirection { get => _wind_direction; }

        public TurbineData(float speed, float direction)
        {
            _wind_speed = speed;
            _wind_direction = direction;
        }
    }
}
