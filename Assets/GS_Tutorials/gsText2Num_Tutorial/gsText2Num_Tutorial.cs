using GpuScript;
using System;
using System.Linq;
public class gsText2Num_Tutorial : gsText2Num_Tutorial_
{
  public override bool ABuff_IsBitOn(uint i) => TextByte(Text, i) == ASCII_HT;
  public void Add(StrBldr sb, uint iterationN, params Action<uint>[] a) => a.ForEach(f => sb.Add($"\t{Secs(() => (0, iterationN).ForEach(i => f(i)))}"));
  public override void RunBenchmark() //https://cc.davelozinski.com/c-sharp/fastest-way-to-convert-a-string-to-an-int
  {
    int y;
    StrBldr sb = StrBldr().AddTabRow("N", "Convert.ToInt32", "int.TryParse", "int.Parse", "Custom", "GpuScript");
    (1000u, 100000000u, 10u).Decay().ForEach(iterationN =>
    {
      uint iN = min(iterationN, 50000000);
      string[] s = (0, iN).For().Select(i => i.ToString()).ToArray();
      sb.Add($"\n{iN:#,##0}");
      Add(sb, iN, (uint i) => Convert.ToInt32(s[i]), (uint i) => int.TryParse(s[i], out y), (uint i) => int.Parse(s[i]),
         (uint i) => { y = 0; (0, s[i].Length).ForEach(j => y = y * 10 + s[i][j] - '0'); });
      AllocData_ints(itemN = ABuff_Run(SetBytes(ref Text, s.Join("\t").ToBytes()) * 4));
      sb.Add($"\t{(0, 10).For().Select(t => Secs(() => Gpu_parseText())).Min()}");
    });
    print(sb);
  }
  public uint2 Get_tab_indexes(uint tabI) => uint2(tabI == 0 ? 0 : ABuff_Indexes[tabI - 1] + 1, ABuff_Indexes[tabI]);
  public override void parseText_GS(uint3 id) => ints[id.x] = ToInt(Text, Get_tab_indexes(id.x));

	//public static uint ToUInt(RWStructuredBuffer<uint> a, uint i0, uint i1)
	//{
	//	uint v = 0;
	//	for (uint i = i0; i < i1; i++) { uint c = TextByte(a, i); if (c >= ASCII_0 && c <= ASCII_9) v = v * 10 + c - ASCII_0; }
	//	return v;
	//}
	//public static uint ToUInt(string s)
	//{
	//	uint v = 0;
	//	for (int i = 0; i < s.Length; i++) { char c = s[i]; if (c >= '0' && c <= '9') v = v * 10 + c - '0'; }
	//	return v;
	//}

}
