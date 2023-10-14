using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace Cheat
{
    [BepInPlugin("me.id.mengtl", "盖娅迷航内置修改器", "1.0.0")]
    public class GaiaTrekCheatMenu : BaseUnityPlugin
    {
        private bool windowShow = false;
        private Rect windowRect = new Rect(50, 50, 200, 100);
        private static bool RangeBool = false;
        private static bool SpeedBool = false;
        private static bool ResourceBool = false;
        private static bool MoveSpeedBool = false;
        void Start()
        {
            Harmony.CreateAndPatchAll(typeof(GaiaTrekCheatMenu));
            Debug.Log("插件加载完成！");
        }
        
        void Update()
        {
            var key = new KeyboardShortcut(KeyCode.F9);

            if (key.IsDown())
            {
                windowShow = !windowShow;
            }
        }
        void OnGUI()
        {
            if (windowShow)
            {
                windowRect = GUILayout.Window(123, windowRect, WindowFunc, "内置修改器");
            }
        }
        public void WindowFunc(int id)
        {
            GUILayout.FlexibleSpace();
            RangeBool = GUILayout.Toggle(RangeBool, "无限距离");
            SpeedBool = GUILayout.Toggle(SpeedBool, "超级攻速");
            ResourceBool = GUILayout.Toggle(ResourceBool, "10倍资源");
            MoveSpeedBool = GUILayout.Toggle(MoveSpeedBool, "敌人龟速");
            GUI.DragWindow();
        }
        //无限攻击距离(开启一次，整局游戏有效)
        [HarmonyPrefix, HarmonyPatch(typeof(MonsterCard), "GetAttackRange")]
        public static bool MonsterCard_GetAttackRange_Prefix(MonsterCard __instance)
        {
            Debug.Log("无限距离");
            if (RangeBool)
            {
                __instance.attackRange = 5;
                return true;
            }
            else
            {
                return true;
            }
        }
        //5倍攻速(开启一次，整局游戏有效)
        [HarmonyPrefix, HarmonyPatch(typeof(MonsterCard), "GetAttackSpeed")]
        public static bool MonsterCard_GetAttackSpeed_Prefix(MonsterCard __instance)
        {
            Debug.Log("5倍攻速");
            if (SpeedBool)
            {
                __instance.attackSpeed = 0.2f;
                return true;
            }
            else
            {
                return true;
            }
        }
        //敌人龟速
        [HarmonyPrefix, HarmonyPatch(typeof(MonsterCard), "GetSpeed")]
        public static bool MonsterCard_GetSpeed_Prefix(MonsterCard __instance)
        {
            if (MoveSpeedBool)
            {
                __instance.moveSpeed = 0.1f;
                return true;
            }
            else
            {
                return true;
            }
        }
        //10倍获取资源(随开随关)
        [HarmonyPrefix, HarmonyPatch(typeof(ResourceCard), "AddCard")]
        public static bool ResourceCard_AddCard_Prefix(ResourceCard __instance,ResourceCard card)
        {
            Debug.Log("10倍资源");
            if (ResourceBool)
            {
                __instance.count = card.count * 10 + __instance.count;
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
