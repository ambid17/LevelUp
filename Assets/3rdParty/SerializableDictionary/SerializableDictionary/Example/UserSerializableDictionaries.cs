using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Minigames.Fight;

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> {}

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class ResourceTypeSpriteDictionary : SerializableDictionary<ResourceType, Sprite> {}

[Serializable]
public class ResourceTypeFloatDictionary : SerializableDictionary<ResourceType, float> {}

[Serializable]
public class PropTypeSpriteListDictionary : SerializableDictionary<PropType, List<Sprite>> {}
[Serializable]
public class SpriteShadowSpritedataDictionary : SerializableDictionary<Sprite, ShadowSpriteData> { }

[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}

#if NET_4_6 || NET_STANDARD_2_0
[Serializable]
public class StringHashSet : SerializableHashSet<string> {}
#endif
