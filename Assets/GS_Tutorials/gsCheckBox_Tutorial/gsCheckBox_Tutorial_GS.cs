using GpuScript;
using UnityEngine;

public class gsCheckBox_Tutorial_GS : _GS
{
  [GS_UI, AttGS("UI|User Interface")] TreeGroup group_UI;
  [GS_UI, AttGS("Display Buttons|Show or hide the buttons")] bool displayButtons;
  [GS_UI, AttGS("Hello|Button with click code", UI.OnClicked, "print(\"Hello\");", UI.ShowIf, nameof(displayButtons))] void Hello() { }
  [GS_UI, AttGS("Method|Button that runs a method", UI.ShowIf, nameof(displayButtons))] void RunMethod() { }
  [GS_UI, AttGS("Coroutine|Button that runs a coroutine", UI.Sync, UI.ShowIf, nameof(displayButtons))] void RunCoroutine() { }
  [GS_UI, AttGS("UI|User Interface")] TreeGroupEnd group_UI_End;
}