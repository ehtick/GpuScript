using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GpuScript;
public class gsBDraw_ : GS
{
  [HideInInspector] public uiData data;
  [HideInInspector] public GBDraw g; public GBDraw G { get { gBDraw.GetData(); return gBDraw[0]; } }
  public void g_SetData() { if (gChanged && gBDraw != null) { gBDraw[0] = g; gBDraw.SetData(); gChanged = false; } }
  public virtual void tab_delimeted_text_SetKernels(bool reallocate = false) { if (tab_delimeted_text != null && (reallocate || tab_delimeted_text.reallocated)) { SetKernelValues(tab_delimeted_text, nameof(tab_delimeted_text), kernel_getTextInfo, kernel_AppendBuff_GetSums, kernel_AppendBuff_Get_Bits); tab_delimeted_text.reallocated = false; } }
  public virtual void textInfos_SetKernels(bool reallocate = false) { if (textInfos != null && (reallocate || textInfos.reallocated)) { SetKernelValues(textInfos, nameof(textInfos), kernel_setDefaultTextInfo, kernel_getTextInfo); textInfos.reallocated = false; } }
  public virtual void fontInfos_SetKernels(bool reallocate = false) { if (fontInfos != null && (reallocate || fontInfos.reallocated)) { SetKernelValues(fontInfos, nameof(fontInfos), kernel_getTextInfo); fontInfos.reallocated = false; } }
  public virtual void AppendBuff_Bits_SetKernels(bool reallocate = false) { if (AppendBuff_Bits != null && (reallocate || AppendBuff_Bits.reallocated)) { SetKernelValues(AppendBuff_Bits, nameof(AppendBuff_Bits), kernel_AppendBuff_GetIndexes, kernel_AppendBuff_Get_Bits_Sums, kernel_AppendBuff_GetSums, kernel_AppendBuff_Get_Bits); AppendBuff_Bits.reallocated = false; } }
  public virtual void AppendBuff_Sums_SetKernels(bool reallocate = false) { if (AppendBuff_Sums != null && (reallocate || AppendBuff_Sums.reallocated)) { SetKernelValues(AppendBuff_Sums, nameof(AppendBuff_Sums), kernel_AppendBuff_GetIndexes, kernel_AppendBuff_IncSums, kernel_AppendBuff_GetFills1, kernel_AppendBuff_Get_Bits_Sums, kernel_AppendBuff_GetSums); AppendBuff_Sums.reallocated = false; } }
  public virtual void AppendBuff_Indexes_SetKernels(bool reallocate = false) { if (AppendBuff_Indexes != null && (reallocate || AppendBuff_Indexes.reallocated)) { SetKernelValues(AppendBuff_Indexes, nameof(AppendBuff_Indexes), kernel_getTextInfo, kernel_AppendBuff_GetIndexes); AppendBuff_Indexes.reallocated = false; } }
  public virtual void AppendBuff_Fills1_SetKernels(bool reallocate = false) { if (AppendBuff_Fills1 != null && (reallocate || AppendBuff_Fills1.reallocated)) { SetKernelValues(AppendBuff_Fills1, nameof(AppendBuff_Fills1), kernel_AppendBuff_IncSums, kernel_AppendBuff_IncFills1, kernel_AppendBuff_GetFills2, kernel_AppendBuff_GetFills1); AppendBuff_Fills1.reallocated = false; } }
  public virtual void AppendBuff_Fills2_SetKernels(bool reallocate = false) { if (AppendBuff_Fills2 != null && (reallocate || AppendBuff_Fills2.reallocated)) { SetKernelValues(AppendBuff_Fills2, nameof(AppendBuff_Fills2), kernel_AppendBuff_IncFills1, kernel_AppendBuff_GetFills2); AppendBuff_Fills2.reallocated = false; } }
  public virtual void Gpu_setDefaultTextInfo() { g_SetData(); textInfos?.SetCpu(); textInfos_SetKernels(); Gpu(kernel_setDefaultTextInfo, setDefaultTextInfo, textN); textInfos?.ResetWrite(); }
  public virtual void Cpu_setDefaultTextInfo() { textInfos?.GetGpu(); Cpu(setDefaultTextInfo, textN); textInfos.SetData(); }
  public virtual void Cpu_setDefaultTextInfo(uint3 id) { textInfos?.GetGpu(); setDefaultTextInfo(id); textInfos.SetData(); }
  public virtual void Gpu_getTextInfo() { g_SetData(); fontInfos?.SetCpu(); fontInfos_SetKernels(); textInfos?.SetCpu(); textInfos_SetKernels(); AppendBuff_Indexes?.SetCpu(); AppendBuff_Indexes_SetKernels(); tab_delimeted_text?.SetCpu(); tab_delimeted_text_SetKernels(); Gpu(kernel_getTextInfo, getTextInfo, textN); textInfos?.ResetWrite(); }
  public virtual void Cpu_getTextInfo() { fontInfos?.GetGpu(); textInfos?.GetGpu(); AppendBuff_Indexes?.GetGpu(); tab_delimeted_text?.GetGpu(); Cpu(getTextInfo, textN); textInfos.SetData(); }
  public virtual void Cpu_getTextInfo(uint3 id) { fontInfos?.GetGpu(); textInfos?.GetGpu(); AppendBuff_Indexes?.GetGpu(); tab_delimeted_text?.GetGpu(); getTextInfo(id); textInfos.SetData(); }
  public virtual void Gpu_AppendBuff_GetIndexes() { g_SetData(); AppendBuff_Bits?.SetCpu(); AppendBuff_Bits_SetKernels(); AppendBuff_Sums?.SetCpu(); AppendBuff_Sums_SetKernels(); AppendBuff_Indexes_SetKernels(); Gpu(kernel_AppendBuff_GetIndexes, AppendBuff_GetIndexes, AppendBuff_BitN); AppendBuff_Indexes?.ResetWrite(); }
  public virtual void Cpu_AppendBuff_GetIndexes() { AppendBuff_Bits?.GetGpu(); AppendBuff_Sums?.GetGpu(); AppendBuff_Indexes?.GetGpu(); Cpu(AppendBuff_GetIndexes, AppendBuff_BitN); AppendBuff_Indexes.SetData(); }
  public virtual void Cpu_AppendBuff_GetIndexes(uint3 id) { AppendBuff_Bits?.GetGpu(); AppendBuff_Sums?.GetGpu(); AppendBuff_Indexes?.GetGpu(); AppendBuff_GetIndexes(id); AppendBuff_Indexes.SetData(); }
  public virtual void Gpu_AppendBuff_IncSums() { g_SetData(); AppendBuff_Sums?.SetCpu(); AppendBuff_Sums_SetKernels(); AppendBuff_Fills1?.SetCpu(); AppendBuff_Fills1_SetKernels(); gBDraw?.SetCpu(); Gpu(kernel_AppendBuff_IncSums, AppendBuff_IncSums, AppendBuff_BitN); AppendBuff_Sums?.ResetWrite(); g = G; }
  public virtual void Cpu_AppendBuff_IncSums() { AppendBuff_Sums?.GetGpu(); AppendBuff_Fills1?.GetGpu(); Cpu(AppendBuff_IncSums, AppendBuff_BitN); AppendBuff_Sums.SetData(); }
  public virtual void Cpu_AppendBuff_IncSums(uint3 id) { AppendBuff_Sums?.GetGpu(); AppendBuff_Fills1?.GetGpu(); AppendBuff_IncSums(id); AppendBuff_Sums.SetData(); }
  public virtual void Gpu_AppendBuff_IncFills1() { g_SetData(); AppendBuff_Fills1?.SetCpu(); AppendBuff_Fills1_SetKernels(); AppendBuff_Fills2?.SetCpu(); AppendBuff_Fills2_SetKernels(); Gpu(kernel_AppendBuff_IncFills1, AppendBuff_IncFills1, AppendBuff_BitN1); AppendBuff_Fills1?.ResetWrite(); }
  public virtual void Cpu_AppendBuff_IncFills1() { AppendBuff_Fills1?.GetGpu(); AppendBuff_Fills2?.GetGpu(); Cpu(AppendBuff_IncFills1, AppendBuff_BitN1); AppendBuff_Fills1.SetData(); }
  public virtual void Cpu_AppendBuff_IncFills1(uint3 id) { AppendBuff_Fills1?.GetGpu(); AppendBuff_Fills2?.GetGpu(); AppendBuff_IncFills1(id); AppendBuff_Fills1.SetData(); }
  public virtual void Gpu_AppendBuff_GetFills2() { g_SetData(); AppendBuff_Fills1?.SetCpu(); AppendBuff_Fills1_SetKernels(); AppendBuff_Fills2_SetKernels(); Gpu(kernel_AppendBuff_GetFills2, AppendBuff_GetFills2, AppendBuff_BitN2); AppendBuff_Fills2?.ResetWrite(); }
  public virtual IEnumerator Cpu_AppendBuff_GetFills2() { AppendBuff_Fills1?.GetGpu(); AppendBuff_Fills2?.GetGpu(); yield return StartCoroutine(Cpu_Coroutine(kernel_AppendBuff_GetFills2, AppendBuff_GetFills2, AppendBuff_BitN2)); }
  public virtual void Cpu_AppendBuff_GetFills2(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI) { AppendBuff_Fills1?.GetGpu(); AppendBuff_Fills2?.GetGpu(); AppendBuff_GetFills2(grp_tid, grp_id, id, grpI); AppendBuff_Fills2.SetData(); }
  public virtual void Gpu_AppendBuff_GetFills1() { g_SetData(); AppendBuff_Sums?.SetCpu(); AppendBuff_Sums_SetKernels(); AppendBuff_Fills1_SetKernels(); Gpu(kernel_AppendBuff_GetFills1, AppendBuff_GetFills1, AppendBuff_BitN1); AppendBuff_Fills1?.ResetWrite(); }
  public virtual IEnumerator Cpu_AppendBuff_GetFills1() { AppendBuff_Sums?.GetGpu(); AppendBuff_Fills1?.GetGpu(); yield return StartCoroutine(Cpu_Coroutine(kernel_AppendBuff_GetFills1, AppendBuff_GetFills1, AppendBuff_BitN1)); }
  public virtual void Cpu_AppendBuff_GetFills1(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI) { AppendBuff_Sums?.GetGpu(); AppendBuff_Fills1?.GetGpu(); AppendBuff_GetFills1(grp_tid, grp_id, id, grpI); AppendBuff_Fills1.SetData(); }
  public virtual void Gpu_AppendBuff_Get_Bits_Sums() { g_SetData(); AppendBuff_Bits?.SetCpu(); AppendBuff_Bits_SetKernels(); AppendBuff_Sums_SetKernels(); Gpu(kernel_AppendBuff_Get_Bits_Sums, AppendBuff_Get_Bits_Sums, AppendBuff_BitN); AppendBuff_Sums?.ResetWrite(); }
  public virtual IEnumerator Cpu_AppendBuff_Get_Bits_Sums() { AppendBuff_Bits?.GetGpu(); AppendBuff_Sums?.GetGpu(); yield return StartCoroutine(Cpu_Coroutine(kernel_AppendBuff_Get_Bits_Sums, AppendBuff_Get_Bits_Sums, AppendBuff_BitN)); }
  public virtual void Cpu_AppendBuff_Get_Bits_Sums(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI) { AppendBuff_Bits?.GetGpu(); AppendBuff_Sums?.GetGpu(); AppendBuff_Get_Bits_Sums(grp_tid, grp_id, id, grpI); AppendBuff_Sums.SetData(); }
  public virtual void Gpu_AppendBuff_GetSums() { g_SetData(); AppendBuff_Bits_SetKernels(); AppendBuff_Sums_SetKernels(); tab_delimeted_text?.SetCpu(); tab_delimeted_text_SetKernels(); Gpu(kernel_AppendBuff_GetSums, AppendBuff_GetSums, AppendBuff_BitN); AppendBuff_Bits?.ResetWrite(); AppendBuff_Sums?.ResetWrite(); }
  public virtual IEnumerator Cpu_AppendBuff_GetSums() { AppendBuff_Bits?.GetGpu(); AppendBuff_Sums?.GetGpu(); tab_delimeted_text?.GetGpu(); yield return StartCoroutine(Cpu_Coroutine(kernel_AppendBuff_GetSums, AppendBuff_GetSums, AppendBuff_BitN)); }
  public virtual void Cpu_AppendBuff_GetSums(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI) { AppendBuff_Bits?.GetGpu(); AppendBuff_Sums?.GetGpu(); tab_delimeted_text?.GetGpu(); AppendBuff_GetSums(grp_tid, grp_id, id, grpI); AppendBuff_Bits.SetData(); AppendBuff_Sums.SetData(); }
  public virtual void Gpu_AppendBuff_Get_Bits() { g_SetData(); AppendBuff_Bits_SetKernels(); tab_delimeted_text?.SetCpu(); tab_delimeted_text_SetKernels(); Gpu(kernel_AppendBuff_Get_Bits, AppendBuff_Get_Bits, AppendBuff_BitN); AppendBuff_Bits?.ResetWrite(); }
  public virtual void Cpu_AppendBuff_Get_Bits() { AppendBuff_Bits?.GetGpu(); tab_delimeted_text?.GetGpu(); Cpu(AppendBuff_Get_Bits, AppendBuff_BitN); AppendBuff_Bits.SetData(); }
  public virtual void Cpu_AppendBuff_Get_Bits(uint3 id) { AppendBuff_Bits?.GetGpu(); tab_delimeted_text?.GetGpu(); AppendBuff_Get_Bits(id); AppendBuff_Bits.SetData(); }
  [JsonConverter(typeof(StringEnumConverter))] public enum Draw : uint { Point, Sphere, Line, Arrow, Signal, LineSegment, Texture_2D, Quad, WebCam, Mesh, Number, N }
  [JsonConverter(typeof(StringEnumConverter))] public enum TextAlignment : uint { BottomLeft, CenterLeft, TopLeft, BottomCenter, CenterCenter, TopCenter, BottomRight, CenterRight, TopRight }
  [JsonConverter(typeof(StringEnumConverter))] public enum Text_QuadType : uint { FrontOnly, FrontBack, Switch, Arrow, Billboard }
  public const uint Draw_Point = 0, Draw_Sphere = 1, Draw_Line = 2, Draw_Arrow = 3, Draw_Signal = 4, Draw_LineSegment = 5, Draw_Texture_2D = 6, Draw_Quad = 7, Draw_WebCam = 8, Draw_Mesh = 9, Draw_Number = 10, Draw_N = 11;
  public const uint TextAlignment_BottomLeft = 0, TextAlignment_CenterLeft = 1, TextAlignment_TopLeft = 2, TextAlignment_BottomCenter = 3, TextAlignment_CenterCenter = 4, TextAlignment_TopCenter = 5, TextAlignment_BottomRight = 6, TextAlignment_CenterRight = 7, TextAlignment_TopRight = 8;
  public const uint Text_QuadType_FrontOnly = 0, Text_QuadType_FrontBack = 1, Text_QuadType_Switch = 2, Text_QuadType_Arrow = 3, Text_QuadType_Billboard = 4;
  public const uint Draw_Text3D = 12, LF = 10, TB = 9, ZERO = 48, NINE = 57, PERIOD = 46, COMMA = 44, PLUS = 43, MINUS = 45, SPACE = 32;
  [HideInInspector] public bool ValuesChanged, gChanged;
  public static gsBDraw This;
  public virtual void Awake() { This = this as gsBDraw; Awake_GS(); }
  public virtual void Awake_GS()
  {
  }
  public virtual void Start() { Build_UI(); Load_UI(); Start0_GS(); InitBuffers(); Start1_GS(); }
  public virtual void Start0_GS()
  {
    AppendBuff_Start0_GS();
  }
  public virtual void Start1_GS()
  {
    AppendBuff_Start1_GS();
  }
  public override void onLoaded()
  {
    base.onLoaded();
  }
  [HideInInspector] public bool already_quited = false;
  public override void OnApplicationQuit() { Save_UI(); OnApplicationQuit_GS(); base.OnApplicationQuit(); already_quited = true; }
  public virtual void OnApplicationQuit_GS()
  {
    AppendBuff_OnApplicationQuit_GS();
  }
  public override void ui_to_data()
  {
    if (data == null) return;
    data.siUnits = siUnits;
  }
  public override void data_to_ui()
  {
    if (data == null) return;
  }
  public virtual void Save(string path, string projectName)
  {
    projectPaths = path;
    $"{projectPath}{projectName}.txt".WriteAllText(JsonConvert.SerializeObject(data, Formatting.Indented));
    foreach (var lib in GetComponents<GS>()) if (lib != this) lib.Save_UI();
    string usFile = $"{projectPath}usUnits.txt";
    if (siUnits) usFile.DeleteFile();
    else usFile.WriteAllText("usUnits");
  }
  public override bool Save_UI_As(string path, string projectName)
  {
    if (already_quited) return false;
    if (data != null) ui_to_data();
    if (lib_parent_gs == this) Save(path, projectName);
    else $"{path}{projectName}.txt".WriteAllText(JsonConvert.SerializeObject(data, Formatting.Indented));
    return true;
  }
  public override bool Load_UI_As(string path, string projectName)
  {
    if (path == appPath && SelectedProjectFile.Exists())
      path = $"{appPath}{SelectedProjectFile.ReadAllText()}/".Replace("//", "/");
    if(loadedProjectPath == path) return false;
    projectPaths = loadedProjectPath = path;
    string file = $"{projectPath}{projectName}.txt";
    data = file.Exists() ? JsonConvert.DeserializeObject<uiData>(ui_txt_str = file.ReadAllText()) : new uiData();
    if(data == null) return false;
    foreach (var fld in data.GetType().GetFields(bindings)) if (fld != null && fld.FieldType == typeof(TreeGroup) && fld.GetValue(data) == null) fld.SetValue(data, new TreeGroup() { isChecked = true });
    data_to_ui();
    if (lib_parent_gs == this)
    {
      foreach (var lib in GetComponents<GS>())
        if (lib != this && lib.GetType() != "gsProject_Lib".ToType())
        {
          lib.Build_UI();
          lib.Load_UI();
        }
    }
    ui_loaded = true;
    return true;
  }
  public virtual void OnApplicationPause(bool pause) { if (ui_loaded) Save_UI(); }
  [HideInInspector] public uint lateUpdateI = 0;
  public virtual void LateUpdate()
  {
    if (!ui_loaded) return;
    string usFile = $"{projectPath}usUnits.txt";
    if (lateUpdateI == 5 && usFile.Exists()) { usFile.DeleteFile(); siUnits = false; OnUnitsChanged(); }
    LateUpdate0_GS();
    AppendBuff_LateUpdate1_GS();
    LateUpdate1_GS();
    if(lateUpdateI++ == uint_max) lateUpdateI = 100;
  }
  public virtual void LateUpdate0_GS()
  {
    AppendBuff_LateUpdate0_GS();
  }
  public virtual void LateUpdate1_GS()
  {
    AppendBuff_LateUpdate1_GS();
  }
  public override void Update()
  {
    base.Update();
    if (!ui_loaded) return;
    Update0_GS();
    AppendBuff_Update1_GS();
    Update1_GS();
  }
  public virtual void Update0_GS()
  {
    AppendBuff_Update0_GS();
  }
  public virtual void Update1_GS()
  {
    AppendBuff_Update1_GS();
  }
  public override void OnValueChanged()
  {
    if (!ui_loaded) return;
    OnValueChanged_GS();
  }
  public override void OnValueChanged_GS()
  {
    AppendBuff_OnValueChanged_GS();
  }
  public string[] Scenes_in_Build => new string[] {  };
  public virtual uint AppendBuff_IndexN { get => g.AppendBuff_IndexN; set { if (g.AppendBuff_IndexN != value) { g.AppendBuff_IndexN = value; ValuesChanged = gChanged = true; } } }
  public virtual uint AppendBuff_BitN { get => g.AppendBuff_BitN; set { if (g.AppendBuff_BitN != value) { g.AppendBuff_BitN = value; ValuesChanged = gChanged = true; } } }
  public virtual uint AppendBuff_N { get => g.AppendBuff_N; set { if (g.AppendBuff_N != value) { g.AppendBuff_N = value; ValuesChanged = gChanged = true; } } }
  public virtual uint AppendBuff_BitN1 { get => g.AppendBuff_BitN1; set { if (g.AppendBuff_BitN1 != value) { g.AppendBuff_BitN1 = value; ValuesChanged = gChanged = true; } } }
  public virtual uint AppendBuff_BitN2 { get => g.AppendBuff_BitN2; set { if (g.AppendBuff_BitN2 != value) { g.AppendBuff_BitN2 = value; ValuesChanged = gChanged = true; } } }
  public virtual bool omitText { get => Is(g.omitText); set { if (g.omitText != Is(value)) { g.omitText = Is(value); ValuesChanged = gChanged = true; } } }
  public virtual bool includeUnicode { get => Is(g.includeUnicode); set { if (g.includeUnicode != Is(value)) { g.includeUnicode = Is(value); ValuesChanged = gChanged = true; } } }
  public virtual uint fontInfoN { get => g.fontInfoN; set { if (g.fontInfoN != value) { g.fontInfoN = value; ValuesChanged = gChanged = true; } } }
  public virtual uint textN { get => g.textN; set { if (g.textN != value) { g.textN = value; ValuesChanged = gChanged = true; } } }
  public virtual uint textCharN { get => g.textCharN; set { if (g.textCharN != value) { g.textCharN = value; ValuesChanged = gChanged = true; } } }
  public virtual uint boxEdgeN { get => g.boxEdgeN; set { if (g.boxEdgeN != value) { g.boxEdgeN = value; ValuesChanged = gChanged = true; } } }
  public virtual float fontSize { get => g.fontSize; set { if (any(g.fontSize != value)) { g.fontSize = value; ValuesChanged = gChanged = true; } } }
  public virtual float boxThickness { get => g.boxThickness; set { if (any(g.boxThickness != value)) { g.boxThickness = value; ValuesChanged = gChanged = true; } } }
  public virtual float4 boxColor { get => g.boxColor; set { if (any(g.boxColor != value)) { g.boxColor = value; ValuesChanged = gChanged = true; } } }
  [Serializable]
  public class uiData
  {
    public bool siUnits;
  }
  public virtual void InitBuffers()
  {
    InitBuffers0_GS();
    AddComputeBuffer(ref gBDraw, nameof(gBDraw), 1);
    InitKernels();
    SetKernelValues(gBDraw, nameof(gBDraw), kernel_setDefaultTextInfo, kernel_getTextInfo, kernel_AppendBuff_GetIndexes, kernel_AppendBuff_IncSums, kernel_AppendBuff_IncFills1, kernel_AppendBuff_GetFills2, kernel_AppendBuff_GetFills1, kernel_AppendBuff_Get_Bits_Sums, kernel_AppendBuff_GetSums, kernel_AppendBuff_Get_Bits);
    AddComputeBuffer(ref tab_delimeted_text, nameof(tab_delimeted_text), textN);
    AddComputeBuffer(ref textInfos, nameof(textInfos), textN);
    AddComputeBuffer(ref fontInfos, nameof(fontInfos), fontInfoN);
    InitBuffers1_GS();
    isInitBuffers = true;
  }
  public virtual void InitBuffers0_GS()
  {
    AppendBuff_InitBuffers0_GS();
  }
  public virtual void InitBuffers1_GS()
  {
    AppendBuff_InitBuffers1_GS();
  }
  [HideInInspector] public uint[] AppendBuff_grp = new uint[1024];
  [HideInInspector] public uint[] AppendBuff_grp0 = new uint[1024];
  [Serializable]
  public struct GBDraw
  {
    public uint AppendBuff_IndexN, AppendBuff_BitN, AppendBuff_N, AppendBuff_BitN1, AppendBuff_BitN2, omitText, includeUnicode, fontInfoN, textN, textCharN, boxEdgeN;
    public float fontSize, boxThickness;
    public float4 boxColor;
  };
  public RWStructuredBuffer<GBDraw> gBDraw;
  public struct FontInfo { public float2 uvBottomLeft, uvBottomRight, uvTopLeft, uvTopRight; public int advance, bearing, minX, minY, maxX, maxY; };
  public struct TextInfo { public float3 p, right, up, p0, p1; public float2 size, uvSize; public float4 color, backColor; public uint justification, textI, quadType, axis; public float height; };
  public RWStructuredBuffer<uint> tab_delimeted_text, AppendBuff_Bits, AppendBuff_Sums, AppendBuff_Indexes, AppendBuff_Fills1, AppendBuff_Fills2;
  public RWStructuredBuffer<TextInfo> textInfos;
  public RWStructuredBuffer<FontInfo> fontInfos;
  public virtual void AllocData_tab_delimeted_text(uint n) => AddComputeBuffer(ref tab_delimeted_text, nameof(tab_delimeted_text), n);
  public virtual void AssignData_tab_delimeted_text(params uint[] data) => AddComputeBufferData(ref tab_delimeted_text, nameof(tab_delimeted_text), data);
  public virtual void AllocData_textInfos(uint n) => AddComputeBuffer(ref textInfos, nameof(textInfos), n);
  public virtual void AssignData_textInfos(params TextInfo[] data) => AddComputeBufferData(ref textInfos, nameof(textInfos), data);
  public virtual void AllocData_fontInfos(uint n) => AddComputeBuffer(ref fontInfos, nameof(fontInfos), n);
  public virtual void AssignData_fontInfos(params FontInfo[] data) => AddComputeBufferData(ref fontInfos, nameof(fontInfos), data);
  public virtual void AllocData_AppendBuff_Bits(uint n) => AddComputeBuffer(ref AppendBuff_Bits, nameof(AppendBuff_Bits), n);
  public virtual void AssignData_AppendBuff_Bits(params uint[] data) => AddComputeBufferData(ref AppendBuff_Bits, nameof(AppendBuff_Bits), data);
  public virtual void AllocData_AppendBuff_Sums(uint n) => AddComputeBuffer(ref AppendBuff_Sums, nameof(AppendBuff_Sums), n);
  public virtual void AssignData_AppendBuff_Sums(params uint[] data) => AddComputeBufferData(ref AppendBuff_Sums, nameof(AppendBuff_Sums), data);
  public virtual void AllocData_AppendBuff_Indexes(uint n) => AddComputeBuffer(ref AppendBuff_Indexes, nameof(AppendBuff_Indexes), n);
  public virtual void AssignData_AppendBuff_Indexes(params uint[] data) => AddComputeBufferData(ref AppendBuff_Indexes, nameof(AppendBuff_Indexes), data);
  public virtual void AllocData_AppendBuff_Fills1(uint n) => AddComputeBuffer(ref AppendBuff_Fills1, nameof(AppendBuff_Fills1), n);
  public virtual void AssignData_AppendBuff_Fills1(params uint[] data) => AddComputeBufferData(ref AppendBuff_Fills1, nameof(AppendBuff_Fills1), data);
  public virtual void AllocData_AppendBuff_Fills2(uint n) => AddComputeBuffer(ref AppendBuff_Fills2, nameof(AppendBuff_Fills2), n);
  public virtual void AssignData_AppendBuff_Fills2(params uint[] data) => AddComputeBufferData(ref AppendBuff_Fills2, nameof(AppendBuff_Fills2), data);
  public Texture2D _PaletteTex;
  public float4 palette(float v) => paletteColor(_PaletteTex, v);
  public float4 palette(float v, float w) => float4(palette(v).xyz, w);

  public Texture2D fontTexture;
  public virtual void onRenderObject_LIN(bool show, uint _itemN, ref uint i, ref uint index, ref uint3 LIN) { uint n = 0; if (show) { if (i < (n = _itemN)) LIN = uint3(index, i, 0); LIN.z += n; i -= n; } index++; }
  public virtual void onRenderObject_LIN(uint _itemN, ref uint i, ref uint index, ref uint3 LIN) { onRenderObject_LIN(true, _itemN, ref i, ref index, ref LIN); }
  public virtual uint3 onRenderObject_LIN(uint i) { uint3 LIN = u000; uint index = 0; onRenderObject_LIN(textN, ref i, ref index, ref LIN); onRenderObject_LIN(boxEdgeN, ref i, ref index, ref LIN); return LIN; }
  public virtual uint3 onRenderObject_LIN_Quads(uint i) { uint3 LIN = u000; uint index = 0; onRenderObject_LIN(textN, ref i, ref index, ref LIN); onRenderObject_LIN(boxEdgeN, ref i, ref index, ref LIN); return LIN; }
  public virtual uint3 onRenderObject_LIN_Points(uint i) { uint3 LIN = u000; return LIN; }
  [HideInInspector] public int kernel_setDefaultTextInfo; [numthreads(numthreads1, 1, 1)] protected void setDefaultTextInfo(uint3 id) { unchecked { if (id.x < textN) setDefaultTextInfo_GS(id); } }
  public virtual void setDefaultTextInfo_GS(uint3 id) { }
  [HideInInspector] public int kernel_getTextInfo; [numthreads(numthreads1, 1, 1)] protected void getTextInfo(uint3 id) { unchecked { if (id.x < textN) getTextInfo_GS(id); } }
  public virtual void getTextInfo_GS(uint3 id) { }
  [HideInInspector] public int kernel_AppendBuff_GetIndexes; [numthreads(numthreads1, 1, 1)] protected void AppendBuff_GetIndexes(uint3 id) { unchecked { if (id.x < AppendBuff_BitN) AppendBuff_GetIndexes_GS(id); } }
  [HideInInspector] public int kernel_AppendBuff_IncSums; [numthreads(numthreads1, 1, 1)] protected void AppendBuff_IncSums(uint3 id) { unchecked { if (id.x < AppendBuff_BitN) AppendBuff_IncSums_GS(id); } }
  [HideInInspector] public int kernel_AppendBuff_IncFills1; [numthreads(numthreads1, 1, 1)] protected void AppendBuff_IncFills1(uint3 id) { unchecked { if (id.x < AppendBuff_BitN1) AppendBuff_IncFills1_GS(id); } }
  [HideInInspector] public int kernel_AppendBuff_GetFills2; [numthreads(numthreads1, 1, 1)] protected IEnumerator AppendBuff_GetFills2(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI) { unchecked { if (id.x < AppendBuff_BitN2) yield return StartCoroutine(AppendBuff_GetFills2_GS(grp_tid, grp_id, id, grpI)); } }
  [HideInInspector] public int kernel_AppendBuff_GetFills1; [numthreads(numthreads1, 1, 1)] protected IEnumerator AppendBuff_GetFills1(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI) { unchecked { if (id.x < AppendBuff_BitN1) yield return StartCoroutine(AppendBuff_GetFills1_GS(grp_tid, grp_id, id, grpI)); } }
  [HideInInspector] public int kernel_AppendBuff_Get_Bits_Sums; [numthreads(numthreads1, 1, 1)] protected IEnumerator AppendBuff_Get_Bits_Sums(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI) { unchecked { if (id.x < AppendBuff_BitN) yield return StartCoroutine(AppendBuff_Get_Bits_Sums_GS(grp_tid, grp_id, id, grpI)); } }
  [HideInInspector] public int kernel_AppendBuff_GetSums; [numthreads(numthreads1, 1, 1)] protected IEnumerator AppendBuff_GetSums(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI) { unchecked { if (id.x < AppendBuff_BitN) yield return StartCoroutine(AppendBuff_GetSums_GS(grp_tid, grp_id, id, grpI)); } }
  [HideInInspector] public int kernel_AppendBuff_Get_Bits; [numthreads(numthreads1, 1, 1)] protected void AppendBuff_Get_Bits(uint3 id) { unchecked { if (id.x < AppendBuff_BitN) AppendBuff_Get_Bits_GS(id); } }
  public virtual v2f vert_GS(uint i, uint j, v2f o)
  {
    uint3 LIN = onRenderObject_LIN(i); int index = -1, level = ((int)LIN.x); i = LIN.y;
    if (level == ++index) { o = vert_Text(i, j, o); o.tj.x = 0; }
    else if (level == ++index) { o = vert_Box(i, j, o); o.tj.x = 0; }
    return o;
  }
  public v2f vert(uint i, uint j)
  {
    v2f o = default;
    o.pos = o.color = o.tj = o.tk = f0000; o.normal = o.p0 = o.p1 = o.wPos = f000; o.uv = f00; o.ti = float4(i, j, 0, 0);
    return vert_GS(i, j, o);
  }
  public virtual v2f vert_Text(uint i, uint j, v2f o) => o;
  public virtual v2f vert_Box(uint i, uint j, v2f o) => o;
  public virtual bool onRenderObject_GS_N(bool cpu)
  {
    RenderQuads(material, onRenderObject_LIN_Quads(0).z);
    RenderPoints(material, onRenderObject_LIN_Points(0).z);
    return true;
  }
  public override bool onRenderObject()
  {
    if (gBDraw == null) return false;
    bool render = true, cpu = false;
    onRenderObject_GS(ref render, ref cpu);
    g_SetData();
    if (!render) return false;
    if (cpu) Cpu(material, new { gBDraw }, new { tab_delimeted_text }, new { textInfos }, new { fontInfos }, new { AppendBuff_Bits }, new { AppendBuff_Sums }, new { AppendBuff_Indexes }, new { AppendBuff_grp }, new { AppendBuff_grp0 }, new { AppendBuff_Fills1 }, new { AppendBuff_Fills2 }, new { fontTexture }, new { _PaletteTex });
    else Gpu(material, new { gBDraw }, new { tab_delimeted_text }, new { textInfos }, new { fontInfos }, new { AppendBuff_Bits }, new { AppendBuff_Sums }, new { AppendBuff_Indexes }, new { AppendBuff_grp }, new { AppendBuff_grp0 }, new { AppendBuff_Fills1 }, new { AppendBuff_Fills2 }, new { fontTexture }, new { _PaletteTex });
    return onRenderObject_GS_N(cpu);
  }
  public virtual void onRenderObject_GS(ref bool render, ref bool cpu)
  {
    AppendBuff_onRenderObject_GS(ref render, ref cpu);
  }
  public virtual float4 frag_GS(v2f i, float4 color) => color;
  public virtual float4 frag(v2f i)
  {
    float4 color = i.color;
    color = frag_GS(i, color);
    if (color.a == 0) Discard(0);
    return color;
  }
  #region <AppendBuff>
  public void AppendBuff_print_Bits(string title) { StrBldr sb = StrBldr($"{title}, AppendBuff_Bits"); for (uint i = 0; i < AppendBuff_BitN; i++) sb.Add(" ", AppendBuff_Bits[i]); print(sb); }
  public void AppendBuff_print_Sums(string title) { StrBldr sb = StrBldr($"{title}, AppendBuff_Sums"); for (uint i = 0; i < AppendBuff_BitN; i++) sb.Add(" ", AppendBuff_Sums[i]); print(sb); }
  public void AppendBuff_print_Indexes(string title) { StrBldr sb = StrBldr($"{title}: AppendBuff_Indexes"); for (uint i = 0; i < AppendBuff_IndexN; i++) sb.Add(" ", AppendBuff_Indexes[i]); print(sb); }
  public virtual bool AppendBuff_IsBitOn(uint i) => i % 32 == 0;
  public void AppendBuff_SetN(uint n)
  {
    if (n > 2147450880) throw new Exception("gsAppendBuff: AppendBuff_N > 2,147,450,880");
    AppendBuff_N = n; AppendBuff_BitN = ceilu(AppendBuff_N, 32); AppendBuff_BitN1 = ceilu(AppendBuff_BitN, numthreads1); AppendBuff_BitN2 = ceilu(AppendBuff_BitN1, numthreads1);
    AllocData_AppendBuff_Bits(AppendBuff_BitN); AllocData_AppendBuff_Fills1(AppendBuff_BitN1); AllocData_AppendBuff_Fills2(AppendBuff_BitN2); AllocData_AppendBuff_Sums(AppendBuff_BitN);
  }
  public void AppendBuff_FillPrefixes() { Gpu_AppendBuff_GetFills1(); Gpu_AppendBuff_GetFills2(); Gpu_AppendBuff_IncFills1(); Gpu_AppendBuff_IncSums(); }
  public void AppendBuff_getIndexes() { AllocData_AppendBuff_Indexes(AppendBuff_IndexN); Gpu_AppendBuff_GetIndexes(); }
  public void AppendBuff_FillIndexes() { AppendBuff_FillPrefixes(); AppendBuff_getIndexes(); }
  public virtual uint AppendBuff_Run(uint n) { AppendBuff_SetN(n); Gpu_AppendBuff_GetSums(); AppendBuff_FillIndexes(); return AppendBuff_IndexN; }
  public uint AppendBuff_Run(int n) => AppendBuff_Run((uint)n);
  public uint AppendBuff_Run(uint2 n) => AppendBuff_Run(cproduct(n)); public uint AppendBuff_Run(uint3 n) => AppendBuff_Run(cproduct(n));
  public uint AppendBuff_Run(int2 n) => AppendBuff_Run(cproduct(n)); public uint AppendBuff_Run(int3 n) => AppendBuff_Run(cproduct(n));
  public virtual void AppendBuff_Init_Bits(uint n, GS parent, string _N, string _BitN, ref RWStructuredBuffer<uint> _Bits) { AppendBuff_SetN(n); parent.SetValue(_N, AppendBuff_N); parent.SetValue(_BitN, AppendBuff_BitN); }
  public virtual void AppendBuff_Prefix_Sums() { Gpu_AppendBuff_Get_Bits_Sums(); AppendBuff_FillPrefixes(); }
  public virtual void AppendBuff_Bit_Indexes(RWStructuredBuffer<uint> bits, GS _this, string _IndexN, ref RWStructuredBuffer<uint> _Indexes) { AppendBuff_Prefix_Sums(); AppendBuff_getIndexes(); _this.SetValue(_IndexN, AppendBuff_IndexN); }
  public uint AppendBuff_Assign_Bits(uint i, uint j, uint bits) => bits | (Is(i < AppendBuff_N && AppendBuff_IsBitOn(i)) << (int)j);
  public virtual void AppendBuff_Get_Bits_GS(uint3 id) { uint i = id.x, j, k, bits = 0; if (i < AppendBuff_BitN) { for (j = 0, k = i * 32; j < 32; j++) bits = AppendBuff_Assign_Bits(k + j, j, bits); AppendBuff_Bits[i] = bits; } }
  public virtual IEnumerator AppendBuff_GetSums_GS(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI)
  {
    uint i = id.x, c, s, j, k, bits = 0;
    if (i < AppendBuff_BitN) { for (j = 0, k = i * 32; j < 32; j++) bits = AppendBuff_Assign_Bits(k + j, j, bits); AppendBuff_Bits[i] = bits; c = countbits(bits); } else c = 0;
    AppendBuff_grp0[grpI] = c; AppendBuff_grp[grpI] = c; yield return GroupMemoryBarrierWithGroupSync();
    for (s = 1; s < numthreads1; s *= 2)
    {
      if (grpI >= s && i < AppendBuff_BitN) AppendBuff_grp[grpI] = AppendBuff_grp0[grpI] + AppendBuff_grp0[grpI - s]; yield return GroupMemoryBarrierWithGroupSync();
      AppendBuff_grp0[grpI] = AppendBuff_grp[grpI]; yield return GroupMemoryBarrierWithGroupSync();
    }
    if (i < AppendBuff_BitN) AppendBuff_Sums[i] = AppendBuff_grp[grpI];
  }
  public virtual IEnumerator AppendBuff_Get_Bits_Sums_GS(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI)
  {
    uint i = id.x, c = i < AppendBuff_BitN ? countbits(AppendBuff_Bits[i]) : 0, s;
    AppendBuff_grp0[grpI] = c; AppendBuff_grp[grpI] = c; yield return GroupMemoryBarrierWithGroupSync();
    for (s = 1; s < numthreads1; s *= 2)
    {
      if (grpI >= s && i < AppendBuff_BitN) AppendBuff_grp[grpI] = AppendBuff_grp0[grpI] + AppendBuff_grp0[grpI - s]; yield return GroupMemoryBarrierWithGroupSync();
      AppendBuff_grp0[grpI] = AppendBuff_grp[grpI]; yield return GroupMemoryBarrierWithGroupSync();
    }
    if (i < AppendBuff_BitN) AppendBuff_Sums[i] = AppendBuff_grp[grpI];
  }
  public virtual IEnumerator AppendBuff_GetFills1_GS(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI)
  {
    uint i = id.x, k = i + 1, j = k * numthreads1 - 1, c = i < AppendBuff_BitN1 - 1 ? AppendBuff_Sums[j] : i < AppendBuff_BitN1 ? AppendBuff_Sums[AppendBuff_BitN - 1] : 0, s;
    AppendBuff_grp0[grpI] = c; AppendBuff_grp[grpI] = c; yield return GroupMemoryBarrierWithGroupSync();
    for (s = 1; s < numthreads1; s *= 2)
    {
      if (grpI >= s && i < AppendBuff_BitN1) AppendBuff_grp[grpI] = AppendBuff_grp0[grpI] + AppendBuff_grp0[grpI - s]; yield return GroupMemoryBarrierWithGroupSync();
      AppendBuff_grp0[grpI] = AppendBuff_grp[grpI]; yield return GroupMemoryBarrierWithGroupSync();
    }
    if (i < AppendBuff_BitN1) AppendBuff_Fills1[i] = AppendBuff_grp[grpI];
  }
  public virtual IEnumerator AppendBuff_GetFills2_GS(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI)
  {
    uint i = id.x, k = i + 1, j = k * numthreads1 - 1, c = i < AppendBuff_BitN2 - 1 ? AppendBuff_Fills1[j] : i < AppendBuff_BitN2 ? AppendBuff_Fills1[AppendBuff_BitN1 - 1] : 0, s;
    AppendBuff_grp0[grpI] = c; AppendBuff_grp[grpI] = c; yield return GroupMemoryBarrierWithGroupSync();
    for (s = 1; s < numthreads1; s *= 2)
    {
      if (grpI >= s && i < AppendBuff_BitN2) AppendBuff_grp[grpI] = AppendBuff_grp0[grpI] + AppendBuff_grp0[grpI - s]; yield return GroupMemoryBarrierWithGroupSync();
      AppendBuff_grp0[grpI] = AppendBuff_grp[grpI]; yield return GroupMemoryBarrierWithGroupSync();
    }
    if (i < AppendBuff_BitN2) AppendBuff_Fills2[i] = AppendBuff_grp[grpI];
  }
  public virtual void AppendBuff_IncFills1_GS(uint3 id) { uint i = id.x; if (i >= numthreads1) AppendBuff_Fills1[i] += AppendBuff_Fills2[i / numthreads1 - 1]; }
  public virtual void AppendBuff_IncSums_GS(uint3 id) { uint i = id.x; if (i >= numthreads1) AppendBuff_Sums[i] += AppendBuff_Fills1[i / numthreads1 - 1]; if (i == AppendBuff_BitN - 1) AppendBuff_IndexN = AppendBuff_Sums[i]; }
  public virtual void AppendBuff_GetIndexes_GS(uint3 id) { uint i = id.x, j, sum = i == 0 ? 0 : AppendBuff_Sums[i - 1], b, i32 = i << 5, k; for (k = 0, b = AppendBuff_Bits[i]; b > 0; k++) { j = (uint)findLSB(b); AppendBuff_Indexes[sum + k] = i32 + j; b = SetBitu(b, j, 0); } }

  public virtual void base_AppendBuff_Start0_GS() { }
  public virtual void base_AppendBuff_Start1_GS() { }
  public virtual void base_AppendBuff_OnApplicationQuit() { Save_UI(); OnApplicationQuit_GS(); base.OnApplicationQuit(); already_quited = true; }
  public virtual void base_AppendBuff_LateUpdate0_GS() { }
  public virtual void base_AppendBuff_LateUpdate1_GS() { }
  public virtual void base_AppendBuff_Update0_GS() { }
  public virtual void base_AppendBuff_Update1_GS() { }
  public virtual void base_AppendBuff_OnValueChanged_GS() { }
  public virtual void base_AppendBuff_InitBuffers0_GS() { }
  public virtual void base_AppendBuff_InitBuffers1_GS() { }
  [HideInInspector]
  public virtual void base_AppendBuff_onRenderObject_GS(ref bool render, ref bool cpu) { }
  public virtual float4 base_frag_AppendBuff_GS(v2f i, float4 color) { return color; }
  public virtual void AppendBuff_InitBuffers0_GS() { }
  public virtual void AppendBuff_InitBuffers1_GS() { }
  public virtual void AppendBuff_LateUpdate0_GS() { }
  public virtual void AppendBuff_LateUpdate1_GS() { }
  public virtual void AppendBuff_Update0_GS() { }
  public virtual void AppendBuff_Update1_GS() { }
  public virtual void AppendBuff_Start0_GS() { }
  public virtual void AppendBuff_Start1_GS() { }
  public virtual void AppendBuff_OnValueChanged_GS() { }
  public virtual void AppendBuff_OnApplicationQuit_GS() { }
  public virtual void AppendBuff_onRenderObject_GS(ref bool render, ref bool cpu) { }
  public virtual float4 frag_AppendBuff_GS(v2f i, float4 color)
  {
    return color;
  }
  #endregion <AppendBuff>
}