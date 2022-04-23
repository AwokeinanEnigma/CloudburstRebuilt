using EntityStates.Engi.EngiWeapon;
using System;
using UnityEngine;

namespace Cloudburst.CEntityStates.EngineerStates
{
    public class PlaceFlameTurret : PlaceTurret
    {
        public PlaceFlameTurret()
        {
            this.blueprintPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/EngiTurretBlueprints");
            this.wristDisplayPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/EngiTurretWristDisplay");
            this.turretMasterPrefab = Engineer.Engineer.flameTurretMaster;
        }
    }
}