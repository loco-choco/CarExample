using CarExample.Car;
using SlateShipyard.PlayerAttaching;

namespace Spaceshipinha.Navinha
{
    public class CarFreeLookablePlayerAttachPoint : FreeLookablePlayerAttachPoint
    {
        public CarWheelController carWheelController;
        public void Awake() 
        {
            GetComponent<InteractZone>().ChangePrompt("Seat On");
            AllowFreeLook = () =>
            {
                return carWheelController.enabled;
            };
        }
    }
}
