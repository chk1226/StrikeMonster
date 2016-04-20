using UnityEngine;
using System.Collections;

namespace StrikeMonster
{

    public static class InterfaceMehodName
    {
        public static readonly string ReduceCD = "ReduceCD";
        public static readonly string RestFriendlySkill = "RestFriendlySkill";
    }


    public interface IReduceCD {

        void ReduceCD();
    }

    public interface IRestFriendlySkill
    {
        void RestFriendlySkill();
    }

}