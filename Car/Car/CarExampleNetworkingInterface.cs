using SlateShipyard.Modules.Wheels;
using SlateShipyard.NetworkingInterface;

namespace CarExample.Car
{
    public class CarExampleNetworkingInterface : SimpleNetworkingInterface
    {
        public OWSimpleRaycastWheel frWheel;
        public OWSimpleRaycastWheel flWheel;

        public CarWheelController carWheelController;

        [SyncableProperty]
        public float frWheelAngle
        {
            get => frWheel.steerAngle;
            set => frWheel.steerAngle = value;
        }

        [SyncableProperty]
        public float flWheelAngle
        {
            get => flWheel.steerAngle;
            set => flWheel.steerAngle = value;
        }

        [SyncableProperty]
        public float steeringAngle
        {
            get => OWInput.GetValue(InputLibrary.thrustZ);
            set => carWheelController.externalAccelerationValue = value;
        }
        public override void OnIsPuppetChange(bool isPuppet)
        {
            base.OnIsPuppetChange(isPuppet);

            flWheel.enablePhysics = !isPuppet;
            frWheel.enablePhysics = !isPuppet;

            carWheelController.IsPuppet(isPuppet);
        }
    }
}
