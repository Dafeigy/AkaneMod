using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;
using System.Net.Sockets;

namespace AkaneMod
{
    [BepInPlugin("cn.cybershit.plugin.AkaneMod", "AkaneMod","1.0.0")]
    public class AkaneMod : BaseUnityPlugin
    {
        static  ConfigEntry<KeyCode> hotkey;
        static ConfigEntry<string> remoteIP;
        static ConfigEntry<int> remotePortEnemy; // 不同类型的Enemy 共享同一个Port
        static ConfigEntry<int> remotePortSelf;  // 自身的Port和Enemy的进行区分 以防出现自身Position被drop的情况
        static ConfigEntry<float> reloadTime;  // 自身的Port和Enemy的进行区分 以防出现自身Position被drop的情况

        void Start()
        {
            remoteIP = Config.Bind<string>("config", "remoteIP", "127.0.0.1","Remote IP address");
            remotePortEnemy = Config.Bind<int>("config", "remotePortEnemy", 1224, "Remote IP port");
            remotePortSelf = Config.Bind<int>("config", "remotePortSekf", 1225, "Remote IP port");
            hotkey = Config.Bind<KeyCode>("config", "hotkey", KeyCode.G, "Activating Mod");
            reloadTime = Config.Bind<float>("config", "timeToReload", 1f, "Time to reload your bullets of your gun");
            Logger.LogInfo("Configuration Done."); // Better use this to print log.

            Harmony.CreateAndPatchAll(typeof(AkaneMod));
        }

        void Update()
        {
            if (Input.GetKeyDown(hotkey.Value))
            {
                Logger.LogInfo("G is pressed");
            }

        }


        // Regular Enemy Position
        [HarmonyPostfix, HarmonyPatch(typeof(EnemyRegular), "Move")]
        public static void EnemyRegular_Move_Patch(EnemyRegular __instance)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(remoteIP.Value, remotePortEnemy.Value);
            Vector3 vector = Camera.main.WorldToScreenPoint(__instance.transform.position);
            byte[] bytes = Encoding.ASCII.GetBytes(string.Format("Enemey Regular pos:{0},{1},{2}", new object[] { __instance.GetInstanceID(), vector.x, vector.y }));
            udpClient.Send(bytes, bytes.Length);
            udpClient.Close();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Enemy), "Move")]
        public static void Enemy_Move_Patch(Enemy __instance)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(remoteIP.Value, remotePortEnemy.Value);
            Vector3 vector = Camera.main.WorldToScreenPoint(__instance.transform.position);
            byte[] bytes = Encoding.ASCII.GetBytes(string.Format("Enemey BigDude pos:{0},{1},{2}", new object[] { __instance.GetInstanceID(), vector.x, vector.y }));
            udpClient.Send(bytes, bytes.Length);
            udpClient.Close();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(EnemyShooter), "Move")]
        public static void EnemyShooter_Move_Patch(EnemyShooter __instance)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(remoteIP.Value, remotePortEnemy.Value);
            Vector3 vector = Camera.main.WorldToScreenPoint(__instance.transform.position);
            byte[] bytes = Encoding.ASCII.GetBytes(string.Format("Enemey Shooter pos:{0},{1},{2}", new object[] { __instance.GetInstanceID(), vector.x, vector.y }));
            udpClient.Send(bytes, bytes.Length);
            udpClient.Close();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(EnemyAssassin), "Move")]
        public static void EnemyAssassin_Move_Patch(EnemyAssassin __instance)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(remoteIP.Value, remotePortEnemy.Value);
            Vector3 vector = Camera.main.WorldToScreenPoint(__instance.transform.position);
            byte[] bytes = Encoding.ASCII.GetBytes(string.Format("Enemy Assassin pos:{0},{1},{2}", new object[] { __instance.GetInstanceID(), vector.x, vector.y }));
            udpClient.Send(bytes, bytes.Length);
            udpClient.Close();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Player), "Move")]
        public static void Player_Move_Patch(Player __instance)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(remoteIP.Value, remotePortSelf.Value);
            Vector3 vector = Camera.main.WorldToScreenPoint(__instance.transform.position);
            byte[] bytes = Encoding.ASCII.GetBytes(string.Format("Self pos:{0},{1},{2}", new object[] { __instance.GetInstanceID(), vector.x, vector.y }));
            udpClient.Send(bytes, bytes.Length);
            udpClient.Close();
        }

    }
}
 