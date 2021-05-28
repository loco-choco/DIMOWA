using System;
using System.Collections.Generic;
using System.Text;

namespace CAMOWA.FBXRuntimeImporter.AnimationRead
{
    public class FBXAnimation
    {
        public FBXBoneAnimation[] BonesAnimation;
        public FBXAnimationNode[] AnimationNodesLeft;
        public FBXAnimation(FBXAnimationGroup AnimationGroup, string[] BoneNames)
        {
            BonesAnimation = new FBXBoneAnimation[BoneNames.Length];
            for (int i = 0; i < BonesAnimation.Length; i++)
                BonesAnimation[i] = new FBXBoneAnimation(BoneNames[i]);
            
            int boneId = 0;
            int j = 0;
            for (j = 0; j / 3 < BonesAnimation.Length && j < AnimationGroup.AnimationNodes.Count; j++)
            {
                if (boneId != j / 3)
                    boneId = j / 3;
                switch (j % 3)
                {
                    case 0: //Transform
                        BonesAnimation[boneId].PositionCurves = FBXBoneAnimation.GenerateAnimationCurveTrio(AnimationGroup.AnimationNodes[j]);
                        break;
                    case 1: //Rotation
                        BonesAnimation[boneId].RotationCurves = FBXBoneAnimation.GenerateAnimationCurveTrio(AnimationGroup.AnimationNodes[j]);
                        break;
                    case 2: //Scale
                        BonesAnimation[boneId].ScaleCurves = FBXBoneAnimation.GenerateAnimationCurveTrio(AnimationGroup.AnimationNodes[j]);
                        break;
                }
            }
            if (j < AnimationGroup.AnimationNodes.Count - 1)
                AnimationNodesLeft =  AnimationGroup.AnimationNodes.GetRange(j, AnimationGroup.AnimationNodes.Count - j).ToArray();
        }
    }
    public class FBXBoneAnimation
    {
        public string BoneName;
        public FBXAnimationCurve[] PositionCurves;
        public FBXAnimationCurve[] RotationCurves; //In euler angles :/
        public FBXAnimationCurve[] ScaleCurves;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PositionAnimationCurveNode">The AnimationCurveNode node not the AnimationCurve node</param>
        /// <param name="RotationAnimationCurveNode">The AnimationCurveNode node not the AnimationCurve node</param>
        /// <param name="ScaleAnimationCurveNode">The AnimationCurveNode node not the AnimationCurve node</param>
        public FBXBoneAnimation(string BoneName, FBXAnimationNode PositionAnimationCurveNode, FBXAnimationNode RotationAnimationCurveNode, FBXAnimationNode ScaleAnimationCurveNode)
        {
            this.BoneName = BoneName;
            PositionCurves = GenerateAnimationCurveTrio(PositionAnimationCurveNode);
            RotationCurves = GenerateAnimationCurveTrio(RotationAnimationCurveNode);
            ScaleCurves = GenerateAnimationCurveTrio(ScaleAnimationCurveNode);
        }
        public FBXBoneAnimation(string BoneName)
        {
            this.BoneName = BoneName;
        }

        static public FBXAnimationCurve[] GenerateAnimationCurveTrio(FBXAnimationNode AnimationCurveNode)
        {
            var curveNodes = AnimationCurveNode.AnimationCurves;
            if (curveNodes != null)
            {
                if (curveNodes.Count == 3)
                    return new FBXAnimationCurve[] {new FBXAnimationCurve(AnimationCurveNode.AnimationCurves[0]),
                new FBXAnimationCurve(AnimationCurveNode.AnimationCurves[1]), new FBXAnimationCurve(AnimationCurveNode.AnimationCurves[2]) };
                else if (curveNodes.Count == 2)
                    return new FBXAnimationCurve[]{ new FBXAnimationCurve(AnimationCurveNode.AnimationCurves[0]),
                new FBXAnimationCurve(AnimationCurveNode.AnimationCurves[1])};
                else if (curveNodes.Count == 1)
                    return new FBXAnimationCurve[] { new FBXAnimationCurve(AnimationCurveNode.AnimationCurves[0]) };
            }
            return new FBXAnimationCurve[] { };
        }
    }
}
