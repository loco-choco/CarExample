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

            //TODO dissable this
            CarBody carBodyRigid = carBody.AddComponent<CarBody>();
            CarWheelController carWheelController = carBody.AddComponent<CarWheelController>();
            CarControlledVanish carControlledVanish = carBody.AddComponent<CarControlledVanish>();
            ImpactSensor impactSensor = carBody.AddComponent<ImpactSensor>();

            //TODO dissable this
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
            carWheelController.Init();
            carBodyRigid.Init();
            #endregion

            #region Car_Detectors
            //Detector
            GameObject carDetector = carBody.transform.GetChild(1).gameObject;

            SphereShape shape = carDetector.AddComponent<SphereShape>();
            shape.CopySettingsFromCollider();
            shape.RecalculateLocalBounds();
            shape.SetCollisionMode(Shape.CollisionMode.Detector);

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
                //TODO make the physics in OWSimpleRaycastWheel toggleable
                //TODO add sync for this
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
            //TODO add sync for this
            carWheelController.steeringWheel = carBody.transform.GetChild(2).GetChild(0).GetChild(0);

            #region CarExample_Networking
            CarExampleNetworkingInterface carExampleNetworkingInterface = carBody.AddComponent<CarExampleNetworkingInterface>();
            carExampleNetworkingInterface.carWheelController = carWheelController;

            carExampleNetworkingInterface.RigidbodyToKinematicWhenPuppet = true;

            carExampleNetworkingInterface.gameObjectsToDisableWhenPuppet = new GameObject[]
            {
                carSeat,
                carDetector,
                carBody.transform.GetChild(0).gameObject,
            };
            carExampleNetworkingInterface.scriptsToDisableWhenPuppet = new MonoBehaviour[]
            {
                carBodyRigid,
                carControlledVanish,
                impactSensor,
            };
            #endregion

            return carBody;
        }
    }
}
