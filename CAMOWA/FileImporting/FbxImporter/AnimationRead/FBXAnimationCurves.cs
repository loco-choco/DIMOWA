using System;
using System.Collections.Generic;
using System.Text;

namespace CAMOWA.FBXRuntimeImporter.AnimationRead
{
    public struct FBXAnimationCurve
    {
        public FBXKeyFrame[] KeyFrames;
        public float DefaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimationCurve">The AnimationCurve node not the AnimationCurveNode node</param>
        public FBXAnimationCurve(FBXRecordNode AnimationCurve)
        {
            if (AnimationCurve.Name != "AnimationCurve")
                throw new Exception("Incorrect Node Type");

            //Default node
            DefaultValue = AnimationCurve.NestedRecords[0].PropertyList[0].singleProperty;

            int keyFrameAmount = AnimationCurve.NestedRecords[2].PropertyList[0].int64ArrayProperty.Length;
            KeyFrames = new FBXKeyFrame[keyFrameAmount];
            for (int i = 0; i < keyFrameAmount; i++)
                KeyFrames[i] = new FBXKeyFrame(AnimationCurve.NestedRecords[3].PropertyList[0].singleArrayProperty[i]
                    , AnimationCurve.NestedRecords[2].PropertyList[0].int64ArrayProperty[i]);

        }
    }
    public struct FBXKeyFrame
    {
        public long Time;
        public float TimeInSeconds;
        public float Value;
        public FBXKeyFrame(float Value, long Time)
        {
            this.Value = Value;
            //Grows in 1924423250 increments
            this.Time = Time;
            TimeInSeconds = Time / (1924423250 * 24f);
        }
        override public string ToString()
        {
            return $"{TimeInSeconds}s - {Value}";
        }
    }
}
