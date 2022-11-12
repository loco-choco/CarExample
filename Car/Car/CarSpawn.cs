using SlateShipyard.ShipSpawner;
using SlateShipyard.PlayerAttaching;
using SlateShipyard.Modules.Wheels;

using UnityEngine;

namespace CarExample.Car
{
    internal class CarSpawn : MonoBehaviour
    {
        private string seatOnPrompt = "Seat On";

        public void Start() 
        {
            ShipSpawnerManager.AddShip(CreateCar, "Car Example");
        }
        public GameObject CreateCar()
        {
            GameObject carBody = Instantiate(CarInit.carPrefab);

            CarBody carBodyRigid = carBody.AddComponent<CarBody>();
            CarWheelController carWheelController = carBody.AddComponent<CarWheelController>();
            carBody.AddComponent<CarControlledVanish>();
            carBody.AddComponent<ImpactSensor>();

            #region Car_Seat
            //Assento
            GameObject carSeat = carBody.transform.GetChild(3).gameObject;
            carSeat.AddComponent<InteractZone>().ChangePrompt(seatOnPrompt);

            FreeLookablePlayerAttachPoint attachPoint = carSeat.AddComponent<FreeLookablePlayerAttachPoint>();
            attachPoint.AllowFreeLook = () =>
            {
                return carWheelController.enabled;
            };
            attachPoint._lockPlayerTurning = true;
            attachPoint._centerCamera = true;
            CarConsole carConsole = carSeat.AddComponent<CarConsole>();
            carConsole.carBody = carBodyRigid;
            carConsole.carWheelController = carWheelController;

            carWheelController.carConsole = carConsole;
            carBodyRigid.carConsole = carConsole;
            carWheelController.Innit();
            carBodyRigid.Innit();
            #endregion

            #region Car_Detectors
            //Detector
            GameObject carDetector = carBody.transform.GetChild(1).gameObject;

            SphereShape shape = carDetector.AddComponent<SphereShape>();
            shape.CopySettingsFromCollider();
            shape.RecalculateLocalBounds();
            shape.SetCollisionMode(Shape.CollisionMode.Detector);
            ShapeManager.AddShape(shape);

            carDetector.AddComponent<DynamicForceDetector>();
            carDetector.AddComponent<SectorDetector>();

            DynamicFluidDetector fluidDetec = carDetector.AddComponent<DynamicFluidDetector>();
            fluidDetec.SetDragFactor(2f);
            fluidDetec._dontApplyForces = false;
            carBodyRigid.RegisterAttachedFluidDetector(fluidDetec);

            carDetector.AddComponent<FogWarpDetector>();
            carDetector.AddComponent<RulesetDetector>();
            #endregion

            //WHEELS
            Transform wheels = carBody.transform.GetChild(4);
            for (int i = 0; i < 4; i++)
            {
                OWSimpleRaycastWheel wheel = wheels.GetChild(i).gameObject.AddComponent<OWSimpleRaycastWheel>();
                wheel.collisionMask = OWLayerMask.physicalMask;

                wheel.rb = carBodyRigid._rigidbody;
                wheel.wheelRadius = 0.6f;
                wheel.restLenght = 0.6f;
                wheel.springTravel = 0.4f;
                wheel.springStiffness = 30f;
                wheel.damperStiffness = 4f;
                if (i == 0)
                {
                    carWheelController.frontLWheel = wheel;
                }
                else if (i == 1)
                {
                    carWheelController.frontRWheel = wheel;
                }
            }
            carWheelController.body = carBodyRigid;
            carWheelController.carConsole = carConsole;
            carWheelController.maxAccelerationForce = 10f;

            //Steering wheel
            carWheelController.steeringWheel = carBody.transform.GetChild(2).GetChild(0).GetChild(0);


            return carBody;
        }
    }
}
