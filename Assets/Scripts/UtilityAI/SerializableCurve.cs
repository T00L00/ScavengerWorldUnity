using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI.UAI
{
    [Serializable]
    public class SerializableCurve
    {
        SerializableKeyframe[] keys;
        string postWrapMode;
        string preWrapMode;

        [Serializable]
        public struct SerializableKeyframe
        {
            public Single inTangent;
            public Single outTangent;
            public Int32 tangentMode;
            public Single time;
            public Single value;

            public SerializableKeyframe(Keyframe original)
            {
                inTangent = original.inTangent;
                outTangent = original.outTangent;
                tangentMode = original.tangentMode;
                time = original.time;
                value = original.value;
            }
        }

        public SerializableCurve(AnimationCurve original)
        {
            postWrapMode = getWrapModeAsString(original.postWrapMode);
            preWrapMode = getWrapModeAsString(original.preWrapMode);
            keys = new SerializableKeyframe[original.length];
            for (int i = 0; i < original.keys.Length; i++)
            {
                keys[i] = new SerializableKeyframe(original.keys[i]);
            }
        }

        public AnimationCurve toCurve()
        {
            AnimationCurve res = new AnimationCurve();
            res.postWrapMode = getWrapMode(postWrapMode);
            res.preWrapMode = getWrapMode(preWrapMode);
            Keyframe[] newKeys = new Keyframe[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                SerializableKeyframe aux = keys[i];
                Keyframe newK = new Keyframe();
                newK.inTangent = aux.inTangent;
                newK.outTangent = aux.outTangent;
                newK.tangentMode = aux.tangentMode;
                newK.time = aux.time;
                newK.value = aux.value;
                newKeys[i] = newK;
            }
            res.keys = newKeys;
            return res;
        }

        private WrapMode getWrapMode(String mode)
        {
            if (mode.Equals("Clamp"))
            {
                return WrapMode.Clamp;
            }
            if (mode.Equals("ClampForever"))
            {
                return WrapMode.ClampForever;
            }
            if (mode.Equals("Default"))
            {
                return WrapMode.Default;
            }
            if (mode.Equals("Loop"))
            {
                return WrapMode.Loop;
            }
            if (mode.Equals("Once"))
            {
                return WrapMode.Once;
            }
            if (mode.Equals("PingPong"))
            {
                return WrapMode.PingPong;
            }
            Debug.LogError("Wat is this wrap mode???");
            return WrapMode.Default;
        }

        private string getWrapModeAsString(WrapMode mode)
        {
            if (mode.Equals(WrapMode.Clamp))
            {
                return "Clamp";
            }
            if (mode.Equals(WrapMode.ClampForever))
            {
                return "ClampForever";
            }
            if (mode.Equals(WrapMode.Default))
            {
                return "Default";
            }
            if (mode.Equals(WrapMode.Loop))
            {
                return "Loop";
            }
            if (mode.Equals(WrapMode.Once))
            {
                return "Once";
            }
            if (mode.Equals(WrapMode.PingPong))
            {
                return "PingPong";
            }
            Debug.LogError("Wat is this wrap mode???");
            return "f you";
        }
    }
}