using GpuScript;
using UnityEngine;

public class gsBDraw_GS : _GS
{
  enum Draw { Point, Sphere, Line, Arrow, Signal, LineSegment, Texture_2D, Quad, WebCam, Mesh, Number, N }

  gsAppendBuff AppendBuff;
  #region <AppendBuff>
  uint AppendBuff_IndexN, AppendBuff_BitN, AppendBuff_N;
  uint[] AppendBuff_Bits, AppendBuff_Sums, AppendBuff_Indexes;
  void AppendBuff_Get_Bits() { Size(AppendBuff_BitN); }
  void AppendBuff_Get_Bits_Sums() { Size(AppendBuff_BitN); Sync(); }
  [GS_UI, AttGS(GS_Buffer.GroupShared_Size, 1024)] uint[] AppendBuff_grp, AppendBuff_grp0;
  uint AppendBuff_BitN1, AppendBuff_BitN2;
  uint[] AppendBuff_Fills1, AppendBuff_Fills2;
  void AppendBuff_GetSums() { Size(AppendBuff_BitN); Sync(); }
  void AppendBuff_GetFills1() { Size(AppendBuff_BitN1); Sync(); }
  void AppendBuff_GetFills2() { Size(AppendBuff_BitN2); Sync(); }
  void AppendBuff_IncFills1() { Size(AppendBuff_BitN1); }
  void AppendBuff_IncSums() { Size(AppendBuff_BitN); }
  void AppendBuff_GetIndexes() { Size(AppendBuff_BitN); }

  #endregion <AppendBuff>

  struct FontInfo { float2 uvBottomLeft, uvBottomRight, uvTopLeft, uvTopRight; int advance, bearing, minX, minY, maxX, maxY; };
  struct TextInfo { float3 p, right, up, p0, p1; float2 size, uvSize; float4 color, backColor; uint justification, textI, quadType, axis; float height; };
  enum TextAlignment { BottomLeft, CenterLeft, TopLeft, BottomCenter, CenterCenter, TopCenter, BottomRight, CenterRight, TopRight }
  enum Text_QuadType { FrontOnly, FrontBack, Switch, Arrow, Billboard }
  const uint Draw_Text3D = 12;
  //const uint maxByteN = 2097152, LF = 10, TB = 9, ZERO = 0x30, NINE = 0x39, PERIOD = 0x2e, COMMA = 0x2c, PLUS = 0x2b, MINUS = 0x2d, SPACE = 0x20;
  const uint LF = 10, TB = 9, ZERO = 0x30, NINE = 0x39, PERIOD = 0x2e, COMMA = 0x2c, PLUS = 0x2b, MINUS = 0x2d, SPACE = 0x20;
  bool omitText, includeUnicode;
  uint fontInfoN, textN, textCharN, boxEdgeN;
  float fontSize;
  Texture2D fontTexture;
  uint[] tab_delimeted_text { set => Size(textN); }
  TextInfo[] textInfos { set => Size(textN); }
  FontInfo[] fontInfos { set => Size(fontInfoN); }
  void getTextInfo() { Size(textN); }
  void setDefaultTextInfo() { Size(textN); }
  float boxThickness;
  float4 boxColor;
  [GS_UI, AttGS(GS_Render.Quads)] void vert_Text() { Size(textN); }
  [GS_UI, AttGS(GS_Render.Quads)] void vert_Box() { Size(boxEdgeN); }
}