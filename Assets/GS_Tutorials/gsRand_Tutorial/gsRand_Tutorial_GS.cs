using GpuScript;
using UnityEngine;

public class gsARand_Tutorial_GS : _GS
{
  [GS_UI, AttGS("UI|ARand Tutorial")] TreeGroup group_UI;
  [GS_UI, AttGS("Thickness|Line thickness", UI.Format, "0.000", UI.ValRange, 0.004f, 0.001f, 0.01f)] float lineThickness;

  [GS_UI, AttGS("ARand|ARand test")] TreeGroup group_ARand;
  [GS_UI, AttGS("Random Number N|Number of random numbers", UI.Format, "#,##0", UI.ValRange, 128, 8, 33554432, UI.IsPow2, UI.Pow2_Slider, UI.OnValueChanged, "Init_randomNumbers(); Avg();")] uint randomNumberN;

  [GS_UI, AttGS("Avg|Calculate random number average")] TreeGroup group_Avg;
  [GS_UI, AttGS("Init|Initialize Random Number Buffer")] void Init_randomNumbers() { }
  [GS_UI, AttGS("Calc Average|Calculate the average of an array of random numbers")] void Avg() { }
  [GS_UI, AttGS("Average|Calculated average, should be close to zero", UI.Format, "0.000000", UI.ReadOnly)] float Avg_Val;
  [GS_UI, AttGS("Runtime|Time to generate a single random number", Unit.ns, UI.Format, "#,##0.000000", UI.ReadOnly)] float Avg_Val_Runtime;
  [GS_UI, AttGS("TFlops|Tera-Flops per second", UI.ReadOnly, UI.Format, "#,##0.000")] float Avg_Val_TFlops;
  int[] ints { set => Size(1); }
  void Calc_Random_Numbers() { Size(randomNumberN); }
  void Calc_Average() { Size(randomNumberN); }
  uint4[] randomNumbers { set => Size(randomNumberN); }
  [GS_UI, AttGS(GS_Render.Quads)] void vert_Draw_Random_Signal() { Size(1); }
  [GS_UI, AttGS(GS_Render.Quads)] void vert_Draw_Calc_Avg() { Size(1); }
  [GS_UI, AttGS(GS_Render.Quads)] void vert_Draw_Avg() { Size(1); }
  [GS_UI, AttGS(GS_Render.Quads)] void vert_Draw_Pnts() { Size(randomNumberN); }
  [GS_UI, AttGS(GS_Render.Quads)] void vert_Draw_Border() { Size(12); }
  [GS_UI, AttGS("Avg|Calculate random number average")] TreeGroupEnd groupEnd_Avg;

  [GS_UI, AttGS("ARand|ARand test")] TreeGroupEnd groupEnd_ARand;

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

  gsARand ARand;
  #region <ARand>
  uint ARand_N, ARand_I, ARand_J;
  uint4 ARand_seed4;
  uint4[] ARand_rs { set => Size(ARand_N); }
  void ARand_initSeed() { Size(ARand_N); }
  void ARand_initState() { Size(ARand_I); }
  [GS_UI, AttGS(GS_Buffer.GroupShared)] uint4[] ARand_grp { set => Size(1024); }
  void ARand_grp_init_1M() { Size(ARand_N / 1024 / 1024); Sync(); }
  void ARand_grp_init_1K() { Size(ARand_N / 1024); Sync(); }
  void ARand_grp_fill_1K() { Size(ARand_N); Sync(); }
  #endregion <ARand>

  [GS_UI, AttGS("UI|ARand Tutorial")] TreeGroupEnd groupEnd_UI;
}
