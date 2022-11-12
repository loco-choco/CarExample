using UnityEngine;

namespace CarExample.Car
{
    internal class CarBody : OWRigidbody
    {
        private bool _isPlayerAtCarConsole;
        public CarConsole carConsole;

        public void Init() 
        {
            carConsole.OnEnterCarConsole += OnEnterFlightConsole;
            carConsole.OnExitCarConsole += OnExitFlightConsole;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            carConsole.OnEnterCarConsole -= OnEnterFlightConsole;
            carConsole.OnExitCarConsole -= OnExitFlightConsole;
        }
        private void OnEnterFlightConsole()
        {
            _isPlayerAtCarConsole = true;
        }

        private void OnExitFlightConsole()
        {
            _isPlayerAtCarConsole = false;
        }

		public override void SetPosition(Vector3 worldPosition)
		{
			if (_isPlayerAtCarConsole)
			{
				base.SetPosition(worldPosition);
				GlobalMessenger.FireEvent("PlayerRepositioned");
				return;
			}
			base.SetPosition(worldPosition);
		}

		public override void SetRotation(Quaternion rotation)
		{
			base.SetRotation(rotation);
		}

		public override void SetVelocity(Vector3 newVelocity)
		{
			base.SetVelocity(newVelocity);
		}
	}
}
