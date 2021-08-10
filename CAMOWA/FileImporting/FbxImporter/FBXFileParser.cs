using System;
using System.Collections.Generic;
using System.IO;
using CAMOWA.FBXRuntimeImporter.AnimationRead;

namespace CAMOWA.FBXRuntimeImporter
{
    public class FBXFileParser
    {
        //Main nodes
        public FBXRecordNode FBXHeaderExtension;
        public FBXRecordNode FileId;
        public FBXRecordNode CreationTime;
        public FBXRecordNode Creator;
        public FBXRecordNode GlobalSettings;
        public FBXRecordNode Documents;
        public FBXRecordNode References;
        public FBXRecordNode Definitions;
        public FBXRecordNode Objects;
        public FBXRecordNode Connections;
        public FBXRecordNode Takes;

        //EX: 7400 (7.4)
        public int FileVersion;
        
        public FBXFileParser(string filePath)
        {
            if (!filePath.EndsWith(".fbx"))
                throw new Exception("Incorrect File Type");

            List<byte> fbxFile = new List<byte>(File.ReadAllBytes(filePath));
            FileVersion = BitConverter.ToInt32(fbxFile.GetRange(23, 26).ToArray(), 0);

            FBXHeaderExtension = new FBXRecordNode(ref fbxFile, 27);
            FileId = new FBXRecordNode(ref fbxFile, (int)FBXHeaderExtension.EndOffset);
            CreationTime = new FBXRecordNode(ref fbxFile, (int)FileId.EndOffset);
            Creator = new FBXRecordNode(ref fbxFile, (int)CreationTime.EndOffset);
            GlobalSettings = new FBXRecordNode(ref fbxFile, (int)Creator.EndOffset);
            Documents = new FBXRecordNode(ref fbxFile, (int)GlobalSettings.EndOffset);
            References = new FBXRecordNode(ref fbxFile, (int)Documents.EndOffset);
            Definitions = new FBXRecordNode(ref fbxFile, (int)References.EndOffset);
            Objects = new FBXRecordNode(ref fbxFile, (int)Definitions.EndOffset);
            Connections = new FBXRecordNode(ref fbxFile, (int)Objects.EndOffset);
            Takes = new FBXRecordNode(ref fbxFile, (int)Connections.EndOffset);
        }

        public string AllNodesToString()
        {
            return FBXHeaderExtension.ToString()
                    + "\n\n" + FileId.ToString()
                    + "\n\n" + CreationTime.ToString()
                    + "\n\n" + Creator.ToString()
                    + "\n\n" + GlobalSettings.ToString()
                    + "\n\n" + Documents.ToString()
                    + "\n\n" + References.ToString()
                    + "\n\n" + Definitions.ToString()
                    + "\n\n" + Objects.ToString()
                    + "\n\n" + Connections.ToString()
                    + "\n\n" + Takes.ToString();
        }

        public FBXAnimation[] ReadAnimations()
        {
            List<FBXAnimationGroup> animationsInNodes = new List<FBXAnimationGroup>();
            List<string> boneNames = new List<string>();
            int animToGiveNodes = -1; //1- - none, 0 - animationOne, 1 - //Two ...
            int nodeToGiveCurve = -1; // ^ but for each node of each animation
            for (int i = 0; i < Objects.NestedRecords.Count; i++)
            {
                //Finding the /BONES/
                if (Objects.NestedRecords[i].Name == "Model") 
                {
                    if (FBXProperty.FromHexToChar(Objects.NestedRecords[i].PropertyList[2].stringProperty) == "LimbNode")
                    {
                        string s = FBXProperty.FromHexToChar(Objects.NestedRecords[i].PropertyList[1].stringProperty);
                        boneNames.Add(s.Substring(0, s.LastIndexOf('\0')));
                    }
                }
                //Marks the beggining of a new animation
                else if (Objects.NestedRecords[i].Name == "AnimationStack" && Objects.NestedRecords[i + 1].Name == "AnimationLayer")
                {
                    animToGiveNodes++;
                    nodeToGiveCurve = -1;
                    animationsInNodes.Add(new FBXAnimationGroup(Objects.NestedRecords[i], Objects.NestedRecords[i + 1]));
                }
                //Marks the beggining of a new animation array(a single or trio (x , y , z)
                else if (Objects.NestedRecords[i].Name == "AnimationCurveNode")
                {
                    nodeToGiveCurve++;
                    animationsInNodes[animToGiveNodes].AnimationNodes.Add(new FBXAnimationNode(Objects.NestedRecords[i]));
                }
                //The curves
                else if (animToGiveNodes > -1 && animToGiveNodes < animationsInNodes.Count && Objects.NestedRecords[i].Name == "AnimationCurve")
                    animationsInNodes[animToGiveNodes].AnimationNodes[nodeToGiveCurve].AnimationCurves.Add(Objects.NestedRecords[i]);
            }

            //Parse the nodes to a more usable format
            List<FBXAnimation> animations = new List<FBXAnimation>();
            foreach (var nodes in animationsInNodes)
                animations.Add(new FBXAnimation(nodes, boneNames.ToArray()));

            return animations.ToArray();
        }
    }
}
