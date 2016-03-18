using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using System;
using System.Reflection;
using Object = UnityEngine.Object;


namespace StrikeMonster
{

    [ActionCategory("StrikeMonster")]
    public class CallMethod : FsmStateAction
    {
        [ObjectType(typeof(MonoBehaviour))]
        [HutongGames.PlayMaker.Tooltip("Store the component in an Object variable.\nNOTE: Set theObject variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera.")]
        public FsmObject behaviour;
        
        //[UIHint(UIHint.Method)]
        [HutongGames.PlayMaker.Tooltip("Name of the method to call on the component")]
        public FsmString methodName;
        
        [HutongGames.PlayMaker.Tooltip("Method paramters. NOTE: these must match the method's signature!")]
        public FsmVar[] parameters;
        
        [ActionSection("Branch Event")]

        [HutongGames.PlayMaker.Tooltip("Event to send if the Bool variable is True.")]
        public FsmEvent isTrue;
        
        [HutongGames.PlayMaker.Tooltip("Event to send if the Bool variable is False.")]
        public FsmEvent isFalse;

        
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        
        private Object cachedBehaviour;
        private Type cachedType;
        private MethodInfo cachedMethodInfo;
        private ParameterInfo[] cachedParameterInfo;
        private object[] parametersArray;
        private string errorString;
        private bool cachedBoolResult;
        
        public override void OnEnter()
        {
            parametersArray = new object[parameters.Length];
            
            DoMethodCall();
            
            if (!everyFrame)
            {
                Finish();
            }
        }
        
        public override void OnUpdate()
        {
            DoMethodCall();
        }
        
        private void DoMethodCall()
        {
            if (behaviour.Value == null)
            {
                Finish();
                return;
            }
            
            if (cachedBehaviour != behaviour.Value)
            {
                errorString = string.Empty;
                if(!DoCache())
                {
                    Debug.LogError(errorString);
                    Finish();
                    return;
                }
            }
            
            object result = null;
            if (cachedParameterInfo.Length == 0)
            {
                result = cachedMethodInfo.Invoke(cachedBehaviour, null);
            }
            else
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    parameter.UpdateValue();
                    parametersArray[i] = parameter.GetValue();
                }
                
                result = cachedMethodInfo.Invoke(cachedBehaviour, parametersArray);
            }

            if(result != null && result.GetType() == typeof(bool))
            {
                cachedBoolResult = (bool)result;

                Fsm.Event(cachedBoolResult ? isTrue : isFalse);

            }

        }
        
        private bool DoCache()
        {
            cachedBehaviour = behaviour.Value as MonoBehaviour;
            if (cachedBehaviour == null)
            {
                errorString += "Behaviour is invalid!\n";
                Finish();
                return false;
            }
            
            cachedType = behaviour.Value.GetType();
            
            #if NETFX_CORE
            cachedMethodInfo = cachedType.GetTypeInfo().GetDeclaredMethod(methodName.Value);
            #else
            cachedMethodInfo = cachedType.GetMethod(methodName.Value);
            #endif            
            if (cachedMethodInfo == null)
            {
                errorString += "Method Name is invalid: " + methodName.Value +"\n";
                Finish();
                return false;
            }
            
            cachedParameterInfo = cachedMethodInfo.GetParameters();
            return true;
        }
        
        public override string ErrorCheck()
        {
            errorString = string.Empty;
            DoCache();
            
            if (!string.IsNullOrEmpty(errorString))
            {
                return errorString;
            }
            
            if (parameters.Length != cachedParameterInfo.Length)
            {
                return "Parameter count does not match method.\nMethod has " + cachedParameterInfo.Length + " parameters.\nYou specified " +parameters.Length + " paramaters.";
            }
            
            for (var i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                var paramType = p.RealType;
                var paramInfoType = cachedParameterInfo[i].ParameterType;
                if (!ReferenceEquals(paramType, paramInfoType ))
                {
                    return "Parameters do not match method signature.\nParameter " + (i + 1) + " (" + paramType + ") should be of type: " + paramInfoType;
                }
            }

            
            return string.Empty;
        }
    }
}