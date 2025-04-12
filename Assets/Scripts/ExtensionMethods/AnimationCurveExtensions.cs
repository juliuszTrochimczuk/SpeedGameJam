using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
    public static class AnimationCurveExtensions
    {
        public static AnimationCurve Inverse(this AnimationCurve curve )
        {
            AnimationCurve inverstCurve = new AnimationCurve();
            for (int i = 0; i < curve.length; i++)
            {
                Keyframe inverseKey = new Keyframe(curve.keys[i].value, curve.keys[i].time);
                inverstCurve.AddKey(inverseKey);
            }
            return inverstCurve;
        }
    }
}