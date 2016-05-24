using UnityEngine;
using System.Collections;

namespace StrikeMonster
{

    public class ReflectClusterRayComponent : ClusterRayComponent {
        private int m_ReflectNum = 3;


        public override void CastRay(System.Collections.Generic.List<UnitComponent> targets, System.Collections.Generic.List<GameObject> intersectObjs)
        {
//            base.CastRay(targets, intersectObjs);
        }

    }


}