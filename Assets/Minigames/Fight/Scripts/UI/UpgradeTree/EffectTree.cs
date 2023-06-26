using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Minigames.Fight
{
    public class EffectTree
    {
        public EffectNode RootNode;
        public static Dictionary<Category, int> CategoryDepthMap = new()
        {
            { Category.UpgradeCategory, 1 },
            { Category.EffectCategory, 2 },
            { Category.TierCategory, 3 },
            { Category.Name, 4 },
        };

        public EffectTree(string rootNode)
        {
            RootNode = new(rootNode);
        }

        public void Add(Effect effect)
        {
            var splitPath = effect.UpgradePath.Split('/');

            EffectNode parentNode = RootNode;

            var length = splitPath.Length;

            for (int depth = 1; depth < length; depth++)
            {
                string nodeName = splitPath[depth];

                var child = parentNode.Children.FirstOrDefault(node => node.Name == nodeName);
                if (child == null)
                {
                    EffectNode newNode = new EffectNode(nodeName);
                    parentNode.Children.Add(newNode);
                    parentNode = newNode;

                    if (depth == CategoryDepthMap[Category.TierCategory])
                    {
                        newNode.TierCategory = effect.TierCategory;
                    }
                    else if (depth == CategoryDepthMap[Category.Name])
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
        public TierCategory TierCategory;
        public bool IsInteractable => Effect != null || TierCategory != TierCategory.None;

        public EffectNode(string name)
        {
            Name = name;
            Children = new List<EffectNode>();
            TierCategory = TierCategory.None;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

