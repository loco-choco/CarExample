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
            enabled = false;
        }
        public void Init() 
        {
            carConsole.OnEnterCarConsole += OnEnterCarConsole;
            carConsole.OnExitCarConsole += OnExitCarConsole;
        }
        public  void OnDestroy()
        {
            carConsole.OnEnterCarConsole -= OnEnterCarConsole;
            carConsole.OnExitCarConsole -= OnExitCarConsole;
        }

        private bool isPuppet;
        public float externalAccelerationValue = 0f;
        public void IsPuppet(bool isPuppet)
        {
            this.isPuppet = isPuppet;
        }
        private float GetTranslationInput()
        {
            return isPuppet ? externalAccelerationValue : OWInput.GetValue(InputLibrary.thrustZ);
        }

        public override void Update()
        {
            float accelerationValue = GetTranslationInput();
            steeringWheel.localPosition = - Vector3.forward * accelerationValue * 0.5f;

            float steerValue = OWInput.GetValue(InputLibrary.thrustX);
            float steerAngle = maxSteerAngle * steerValue;

            steeringWheel.localRotation = Quaternion.Euler(Vector3.forward * steerAngle);

            if (isPuppet)
                return;

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
