using UnityEditor;
using UnityEngine;

namespace BaseProtection
{
    [CreateAssetMenu(fileName = "newPlayer", menuName = "Configs/Player/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public PlayerAttribute MoveSpeed;
        public PlayerAttribute AttackSpeed;
        public PlayerAttribute AttackDamage;

#if UNITY_EDITOR
        [ContextMenu("Copy JSON")]
        public void SerializeToJsonAndCopy()
        {   
            string jsonString = JsonUtility.ToJson(this, true);
            EditorGUIUtility.systemCopyBuffer = jsonString;
        }
#endif
    }
}