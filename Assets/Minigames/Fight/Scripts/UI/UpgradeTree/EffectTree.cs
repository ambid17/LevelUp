using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Minigames.Fight
{
    public class EffectTree
    {
        public EffectNode RootNode;

        public EffectTree()
        {
            RootNode = new EffectNode();
            RootNode.Name = "upgrades";
        }
        public void Add(Effect effect)
        {
            var splitPath = effect.UpgradePath.Split('/');

            foreach (var folder in splitPath)
            {
                if(RootNode.Children)
            }
        }
    }

    public class EffectNode
    {
        public string Name;
        public List<EffectNode> Children;
    }
}

