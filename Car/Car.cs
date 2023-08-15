using UnityEngine;
using OWML.ModHelper;
using OWML.Common;
using SlateShipyard.ShipSpawner;

namespace CarExample
{    
    public class CarInit : ModBehaviour
    {
        public static GameObject carPrefab;
        public static IModHelper modHelper;

        private void Start()
        {
            AssetBundle bundle = ModHelper.Assets.LoadBundle("AssetBundles/carexample");

            carPrefab = bundle.LoadAsset<GameObject>("car_body.prefab");

            modHelper = ModHelper;
            ShipSpawnerManager.AddShip(carPrefab, "Car Example");
        }
    }    
}
