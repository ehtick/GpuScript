using GpuScript;
using UnityEngine;

public class gsSwarm_GS : _GS
{
  enum ParticleShape { Point, Sphere, Line, Arrow, Text, Numbers, Letters };
  enum ParticleDistribution { onSphere, inSphere, inCube, onCircle, inCircle, onLine };
  struct Particle { float3 position, velocity; };
  Particle[] particles { set => Size(particleN); }
  float deltaTime;
  float3 mousePosition;
  uint randomArrayI;
  [GS_UI, AttGS("UI|User Interface")] TreeGroup group_UI;
  [GS_UI, AttGS("Initial Speed|Initial speed to the center of the sphere", UI.ValRange, 0, -1, 1, siUnit.mps, UI.Format, "0.000")] float initialSpeed;
  [GS_UI, AttGS("Initial Spread|Initial spread size", UI.ValRange, 0.5f, 0.1f, 5, UI.Format, "0.000", UI.Pow2_Slider)] float initialSpread;
  [GS_UI, AttGS("Mouse Strength|Velocity change from the mouse position", UI.ValRange, 10, 10, 200, UI.Pow2_Slider)] float mouseStrength;
  [GS_UI, AttGS("Particle N|Number of particles", UI.ValRange, 1000, 1, 10000000, UI.Format, "#,##0", UI.Pow2_Slider, UI.IsPow10, UI.OnValueChanged, "InitBuffers();")] uint particleN;
  [GS_UI, AttGS("Shape|Shape of each particle", UI.OnValueChanged, "InitBuffers();")] ParticleShape particleShape;
  [GS_UI, AttGS("Text|Text to display", UI.ShowIf, "particleShape == ParticleShape.Text", UI.OnValueChanged, "InitBuffers();")] string text;
  [GS_UI, AttGS("Text Height|Height of Text", siUnit.m, UI.ValRange, 0.1f, 0.01f, 0.5f, UI.ShowIf, "particleShape.IsAny(ParticleShape.Text, ParticleShape.Numbers, ParticleShape.Letters)", UI.OnValueChanged, "InitBuffers();")] float textHeight;
  [GS_UI, AttGS("Quad|Text quad type", UI.ShowIf, "particleShape.IsAny(ParticleShape.Text, ParticleShape.Numbers, ParticleShape.Letters)", UI.OnValueChanged, "InitBuffers();")] ADraw_Text_QuadType textQuadType;
  [GS_UI, AttGS("Distribution|Random distribution of points")] ParticleDistribution particleDistribution;
  [GS_UI, AttGS("Restart")] void Restart() { }

  void initParticles() { Size(ARand_N); }
  void moveParticles() { Size(particleN); }
  [GS_UI, AttGS(GS_Render.Quads, UI.ShowIf, "particleShape == ParticleShape.Sphere")] void vert_Spheres() { Size(particleN); }
  [GS_UI, AttGS(GS_Render.Quads, UI.ShowIf, "particleShape == ParticleShape.Line")] void vert_Lines() { Size(particleN); }
  [GS_UI, AttGS(GS_Render.Quads, UI.ShowIf, "particleShape == ParticleShape.Arrow")] void vert_Arrows() { Size(particleN); }
  [GS_UI, AttGS(GS_Render.Points, UI.ShowIf, "particleShape == ParticleShape.Point")] void vert_Points() { Size(particleN); }

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
  //[GS_UI, AttGS(GS_Render.Quads)] void vert_ADraw_Text() { Size(ADraw_textN); }
  [GS_UI, AttGS(GS_Render.Quads)] void vert_ADraw_Box() { Size(ADraw_boxEdgeN); }

  #endregion <ADraw>
  [GS_UI, AttGS(GS_Render.Quads, UI.ShowIf, "particleShape == ParticleShape.Text || particleShape == ParticleShape.Numbers || particleShape == ParticleShape.Letters")] void vert_ADraw_Text() { Size(particleN); }

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

  [GS_UI, AttGS(GS_Lib.External, GS_Lib.Email, "you@gmail.com", GS_Lib.Expires, "2024/12/10", GS_Lib.Key, 123456)] gsAReport_Lib AReport_Lib;
  #region <AReport_Lib>

  #endregion <AReport_Lib>
  [GS_UI, AttGS("UI|User Interface")] TreeGroupEnd group_UI_End;

  [GS_UI, AttGS(GS_Lib.External, GS_Lib.Email, "you@gmail.com", GS_Lib.Expires, "2024/12/10", GS_Lib.Key, 123456)] gsACam_Lib ACam_Lib;
  #region <ACam_Lib>

  #endregion <ACam_Lib>
}