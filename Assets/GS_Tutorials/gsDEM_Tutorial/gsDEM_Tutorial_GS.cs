using GpuScript;
using UnityEngine;

public class gsDEM_Tutorial_GS : _GS
{
  [GS_UI, AttGS("UI|User Interface")] TreeGroup group_UI;
  [GS_UI, AttGS("DEM|Distinct Element Method Group")] TreeGroup group_DEM;
  enum UInts { maxForce, maxSmp, maxSimGridVal, nearestPnt, timeChanged, dm_minMass, dm_maxStiffness, dm_minDamping, dm_Energy, N };
  uint[] uints { set => Size(UInts.N); }
  struct Node { float3 position, velocity, position0, velocity0; uint u; float mass, stiff, damp; }
  Node[] nodes;
  uint3 nodeN3, gridN3, dimension3;
  int[] forces;
  void BuildNodes() { Size(nodeN); }
  void zero_forces() { Size(nodeN); }
  void calc_forces() { Size(nodeN); }
  void move_nodes() { Size(nodeN); }
  void check_collisions() { Size(nodeN * (nodeN - 1) / 2); }
  [GS_UI, AttGS("Display Nodes|Show or hide the Nodes")] bool displayNodes;
  [GS_UI, AttGS("Node N|The number of nodes", UI.OnValueChanged, "GenerateNodes();", UI.ValRange, 10, 0, 10000, UI.Format, "#,##0", UI.Pow2_Slider, UI.ShowIf, nameof(displayNodes))] uint nodeN;
  [GS_UI, AttGS("Resolution|Voxel size", UI.OnValueChanged, "GenerateNodes();", UI.ValRange, 0.2f, 0.001f, 1, siUnit.m, UI.Format, "0.000", UI.Pow2_Slider, UI.ShowIf, nameof(displayNodes))] float resolution;
  [GS_UI, AttGS("GenerateNodes", UI.ShowIf, false)] void GenerateNodes() { }
  [GS_UI, AttGS("Run|Run continuously", UI.OnValueChanged, "GenerateNodes();")] bool runContinuously;
  [GS_UI, AttGS("DEM|Distinct Element Method Group")] TreeGroupEnd groupEnd_DEM;
  [GS_UI, AttGS("UI|User Interface")] TreeGroupEnd group_UI_End;
  [GS_UI, AttGS(GS_Render.Quads, UI.ShowIf, nameof(displayNodes))] void vert_Nodes() { Size(nodeN); }

  gsADraw ADraw;
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
