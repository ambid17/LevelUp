using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Minigames.Fight
{
    public class EffectTree
    {
        public EffectNode RootNode;
        public bool DisplayLeaf;

        public EffectTree(string rootNode, bool displayLeaf)
        {
            RootNode = new(rootNode);
            DisplayLeaf = displayLeaf;
        }

        public void Add(Effect effect)
        {
            var splitPath = effect.UpgradePath.Split('/');

            EffectNode parentNode = RootNode;

            var length = splitPath.Length;

            if (!DisplayLeaf)
            {
                length -= 1;
            }

            for (int depth = 1; depth < length; depth++)
            {
                string nodeName = splitPath[depth];

                var child = parentNode.Children.FirstOrDefault(node => node.Name == nodeName);
                if (child == null)
                {
                    EffectNode newNode = new EffectNode(nodeName);
                    parentNode.Children.Add(newNode);
                    parentNode = newNode;

                    if (depth == splitPath.Length - 1)
                    {
                        newNode.Effect = effect;
                    }
                }
                else
                {
                    parentNode = child;
                }
            }
        }
    }

    public class EffectNode
    {
        public string Name;
        public List<EffectNode> Children;
        public Effect Effect;

        public EffectNode(string name)
        {
            Name = name;
            Children = new List<EffectNode>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

