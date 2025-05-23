#pragma warning disable 0649
using GpuScript;

public class gsAGroupShared_GS : _GS
{
  [GS_UI, AttGS(GS_Buffer.GroupShared_Size, 1024)] uint[] grp, grp0;
  [GS_UI, AttGS(GS_Buffer.GroupShared_Size, 1024)] float2[] grpf2;

  uint IndexN, BitN, N, BitRowN, BitColN;
  uint[] Bits, Sums, Indexes, ColN_Sums;
  void Init_Bits_32() { Size(BitN); }
  void Get_Bits_32() { Size(BitN, 32); }
  void Get_Bits() { Size(BitN); }
  void InitSums() { Size(BitN); }
  void CalcSums() { Size(BitRowN, BitColN * (BitColN - 1) / 2); }
  void Init_ColN_Sums() { Size(BitRowN); }
  void Calc_ColN_Sums() { Size(BitRowN * (BitRowN - 1) / 2); }
  void Add_ColN_Sums() { Size(BitRowN - 1, BitColN); }
  void CalcIndexes() { Size(BitN); }

  //void Get_Bits_Init() { Size(BitN); }

}