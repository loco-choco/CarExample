using OWML.ModHelper;
using UnityEngine;
using CarExample.Car;
using OWML.Common;

namespace CarExample
{
    public class CarInnit : ModBehaviour
    {
        public static GameObject carPrefab;
        public static IModHelper modHelper;

        private void Start()
        {
            AssetBundle bundle = ModHelper.Assets.LoadBundle("AssetBundles/carexample");

            carPrefab = bundle.LoadAsset<GameObject>("car_body.prefab");

            modHelper = ModHelper;
            gameObject.AddComponent<CarSpawn>();
        }
    }
}
