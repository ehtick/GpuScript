using GpuScript;
using UnityEngine;

public class gsDrawSpheres_Tutorial_GS : _GS
{
  [GS_UI, AttGS("UI|User Interface")] TreeGroup group_UI;
  [GS_UI, AttGS("Display Buttons|Show or hide the buttons")] bool displayButtons;
  [GS_UI, AttGS("Hello|Print Hello", UI.OnClicked, "print(\"Hello\");", UI.ShowIf, nameof(displayButtons))] void Hello() { }
  [GS_UI, AttGS("Normal Button|Button action specified in code", UI.ShowIf, nameof(displayButtons))] void NormalButton() { }
  [GS_UI, AttGS("Coroutine Button|Button with coroutine", UI.Sync, UI.ShowIf, nameof(displayButtons))] void CoroutineButton() { }
  [GS_UI, AttGS("UI|User Interface")] TreeGroupEnd group_UI_End;

  [GS_UI] gsADraw ADraw;
  #region <ADraw>
  enum ADraw_Draw { Point, Sphere, Line, Arrow, Signal, LineSegment, Texture_2D, Quad, WebCam, Mesh, Number, N }
  uint ADraw_ABuff_IndexN, ADraw_ABuff_BitN, ADraw_ABuff_N;
  uint[] ADraw_ABuff_Bits, ADraw_ABuff_Sums, ADraw_ABuff_Indexes;
  void ADraw_ABuff_Get_Bits() { Size(ADraw_ABuff_BitN); }
  void ADraw_ABuff_Get_Bits_Sums() { Size(ADraw_ABuff_BitN); Sync(); }
  [GS_UI, AttGS(GS_Buffer.GroupShared_Size, 1024)] uint[] ADraw_ABuff_grp, ADraw_ABuff_grp0;
  uint ADraw_ABuff_BitN1, ADraw_ABuff_BitN2;
  uint[] ADraw_ABuff_Fills1, ADraw_ABuff_Fills2;
  void ADraw_ABuff_GetSums() { Size(ADraw_ABuff_BitN); Sync(); }
  void ADraw_ABuff_GetFills1() { Size(ADraw_ABuff_BitN1); Sync(); }
  void ADraw_ABuff_GetFills2() { Size(ADraw_ABuff_BitN2); Sync(); }
  void ADraw_ABuff_IncFills1() { Size(ADraw_ABuff_BitN1); }
  void ADraw_ABuff_IncSums() { Size(ADraw_ABuff_BitN); }
  void ADraw_ABuff_GetIndexes() { Size(ADraw_ABuff_BitN); }
  struct ADraw_FontInfo { float2 uvBottomLeft, uvBottomRight, uvTopLeft, uvTopRight; int advance, bearing, minX, minY, maxX, maxY; };
  struct ADraw_TextInfo { float3 p, right, up, p0, p1; float2 size, uvSize; float4 color, backColor; uint justification, textI, quadType, axis; float height; };
  enum ADraw_TextAlignment { BottomLeft, CenterLeft, TopLeft, BottomCenter, CenterCenter, TopCenter, BottomRight, CenterRight, TopRight }
  enum ADraw_Text_QuadType { FrontOnly, FrontBack, Switch, Arrow, Billboard }
  const uint ADraw_Draw_Text3D = 12;
  const uint ADraw_LF = 10, ADraw_TB = 9, ADraw_ZERO = 0x30, ADraw_NINE = 0x39, ADraw_PERIOD = 0x2e, ADraw_COMMA = 0x2c, ADraw_PLUS = 0x2b, ADraw_MINUS = 0x2d, ADraw_SPACE = 0x20;
  bool ADraw_omitText, ADraw_includeUnicode;
  uint ADraw_fontInfoN, ADraw_textN, ADraw_textCharN, ADraw_boxEdgeN;
  float ADraw_fontSize;
  Texture2D ADraw_fontTexture;
  uint[] ADraw_tab_delimeted_text { set => Size(ADraw_textN); }
  ADraw_TextInfo[] ADraw_textInfos { set => Size(ADraw_textN); }
  ADraw_FontInfo[] ADraw_fontInfos { set => Size(ADraw_fontInfoN); }
  void ADraw_getTextInfo() { Size(ADraw_textN); }
  void ADraw_setDefaultTextInfo() { Size(ADraw_textN); }
  float ADraw_boxThickness;
  float4 ADraw_boxColor;
  [GS_UI, AttGS(GS_Render.Quads)] void vert_ADraw_Text() { Size(ADraw_textN); }
  [GS_UI, AttGS(GS_Render.Quads)] void vert_ADraw_Box() { Size(ADraw_boxEdgeN); }

  #endregion <ADraw>
}