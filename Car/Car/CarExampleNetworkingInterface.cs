using SlateShipyard.Modules.Wheels;
using SlateShipyard.NetworkingInterface;

namespace CarExample.Car
{
    public class CarExampleNetworkingInterface : SimpleNetworkingInterface
    {
        public CarWheelController carWheelController;

        [SyncableProperty]
        public float accelerationValue
        {
            get => OWInput.GetValue(InputLibrary.thrustZ) * (carWheelController.enabled ? 1f : 0f);
            set => carWheelController.externalAccelerationValue = value;
        }
        [SyncableProperty]
        public float steeringValue
        {
            get => OWInput.GetValue(InputLibrary.thrustX) * (carWheelController.enabled ? 1f : 0f);
            set => carWheelController.externalSteerValue = value;
        }
        public override void OnIsPuppetChange(bool isPuppet)
        {
            base.OnIsPuppetChange(isPuppet);

            carWheelController.IsPuppet(isPuppet);
        }
    }
}
