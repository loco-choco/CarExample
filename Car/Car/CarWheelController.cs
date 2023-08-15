using UnityEngine;
using SlateShipyard.Modules.Wheels;

namespace CarExample.Car
{
    public class CarWheelController : BasicWheelController
    {
        public CarConsole carConsole;
        public Transform steeringWheel;

        public void Awake()
        {
            carConsole.OnEnterCarConsole += OnEnterCarConsole;
            carConsole.OnExitCarConsole += OnExitCarConsole;
            enabled = false;
        }
        public  void OnDestroy()
        {
            carConsole.OnEnterCarConsole -= OnEnterCarConsole;
            carConsole.OnExitCarConsole -= OnExitCarConsole;
        }

        private bool isPuppet;
        public float externalAccelerationValue = 0f;
        public float externalSteerValue = 0f;
        public void IsPuppet(bool isPuppet)
        {
            this.isPuppet = isPuppet;
            enabled = isPuppet;

            frontRWheel.enablePhysics = !isPuppet;
            frontLWheel.enablePhysics = !isPuppet;
        }
        private float GetTranslationInput()
        {
            return isPuppet ? externalAccelerationValue : OWInput.GetValue(InputLibrary.thrustZ);
        }
        private float GetSteerInput()
        {
            return isPuppet ? externalSteerValue : OWInput.GetValue(InputLibrary.thrustX);
        }

        public override void Update()
        {
            float accelerationValue = GetTranslationInput();
            steeringWheel.localPosition = - Vector3.forward * accelerationValue * 0.5f;

            float steerValue = GetSteerInput();
            float steerAngle = maxSteerAngle * steerValue;

            steeringWheel.localRotation = Quaternion.Euler(Vector3.forward * steerAngle);

            frontRWheel.steerAngle = steerAngle;
            frontLWheel.steerAngle = steerAngle;
        }

        private void OnEnterCarConsole()
        {
            enabled = true;
            OWInput.ChangeInputMode(InputMode.ShipCockpit);
        }

        private void OnExitCarConsole()
        {
            OWInput.RestorePreviousInputs();
            enabled = false;
        }
    }
}
