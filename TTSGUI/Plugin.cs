using BepInEx;
using UnityEngine;

namespace TTSGUI
{
    [BepInDependency("org.iidk.gorillatag.iimenu", "7.6.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Manager : BaseUnityPlugin
    {
        void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 100
            };
            labelStyle.normal.textColor = Color.white;

            smoothAnim = barOpen ? Mathf.Lerp(smoothAnim, 0.5f, Time.deltaTime) : Mathf.Lerp(smoothAnim, 0f, Time.deltaTime);
            if (Mathf.Floor(smoothAnim * 255f) != 0f)
            {
                overlay.SetPixel(0, 0, new Color(0f, 0f, 0f, smoothAnim));
                overlay.Apply();

                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), overlay);

                GUIStyle textStyle = new GUIStyle(GUI.skin.textArea);
                textStyle.fontSize = 50;
                GUI.SetNextControlName("bartext");
                barText = GUI.TextArea(new Rect(10, smoothAnim * 115 - 50, Screen.width - 20, 50), barText, textStyle);
            }
            if (barOpen)
            {
                GorillaTagger.Instance.transform.position = startPos;
                GUI.FocusControl("bartext");

                if (barText.Contains("\n"))
                {
                    barText = barText.Replace("\n", "");
                    GUI.FocusControl(null);

                    iiMenu.Menu.Main.SpeakText(barText);
                    ToggleBar();
                }
            }

            bool down = UnityInput.Current.GetKey(KeyCode.Slash) && !UnityInput.Current.GetKey(KeyCode.LeftShift);
            if (down && !oldbs)
                ToggleBar();
            
            oldbs = down;

            GUIStyle labelStyle2 = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                wordWrap = false,
                fontSize = 20
            };
            labelStyle2.normal.textColor = Color.white;
        }
        public static bool barOpen = false;
        private static string barText = "";
        private static float smoothAnim = 0f;
        private static bool oldbs = false;
        private static Vector3 startPos = Vector3.zero;
        private readonly Texture2D overlay = new Texture2D(1, 1);

        public static void ToggleBar()
        {
            barOpen = !barOpen;
            barText = "";
            startPos = GorillaTagger.Instance.transform.position;
            if (!barOpen)
                GUI.FocusControl(null);
        }
    }
}
