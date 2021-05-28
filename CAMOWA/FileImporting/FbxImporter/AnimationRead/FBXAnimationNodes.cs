using System;
using System.Collections.Generic;
using System.Text;

namespace CAMOWA.FBXRuntimeImporter.AnimationRead
{
    public class FBXAnimationGroup
    {
        public FBXRecordNode AnimationStack;
        public FBXRecordNode AnimationLayer;
        public List<FBXAnimationNode> AnimationNodes;

        public FBXAnimationGroup(FBXRecordNode AnimationStack, FBXRecordNode AnimationLayer)
        {
            this.AnimationStack = AnimationStack;
            this.AnimationLayer = AnimationLayer;
            AnimationNodes = new List<FBXAnimationNode>();
        }
    }

    public class FBXAnimationNode
    {
        public FBXRecordNode Node;
        public List<FBXRecordNode> AnimationCurves;

        public FBXAnimationNode(FBXRecordNode AnimationNode)
        {
            Node = AnimationNode;
            AnimationCurves = new List<FBXRecordNode>();
        }
    }
}
