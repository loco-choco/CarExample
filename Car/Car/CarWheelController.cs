using UnityEngine;
using SlateShipyard.Modules.Wheels;

namespace CarExample.Car
{
    internal class CarWheelController : BasicWheelController
    {
        public CarConsole carConsole;
        public Transform steeringWheel;

        public void Awake()
        {
            enabled = false;
        }
        public void Innit() 
        {
            carConsole.OnEnterCarConsole += OnEnterCarConsole;
            carConsole.OnExitCarConsole += OnExitCarConsole;
        }
        public  void OnDestroy()
        {
            carConsole.OnEnterCarConsole -= OnEnterCarConsole;
            carConsole.OnExitCarConsole -= OnExitCarConsole;
        }

        public override void Update()
        {
            float accelerationValue = OWInput.GetValue(InputLibrary.thrustZ);
            steeringWheel.localPosition = - Vector3.forward * accelerationValue * 0.5f;

            float steerValue = OWInput.GetValue(InputLibrary.thrustX);
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
