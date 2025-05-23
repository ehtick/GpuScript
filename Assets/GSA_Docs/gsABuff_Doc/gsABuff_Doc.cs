using GpuScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class gsAppendBuff_Doc : gsAppendBuff_Doc_
{
  public virtual void AppendBuff_Run_On_Existing_Bits(uint n, Action a) { AppendBuff_SetN(n); a(); Gpu_AppendBuff_Get_Existing_Sums(); AppendBuff_FillIndexes(); }
  public override void AppendBuff_Get_Existing_Bits_GS(uint3 id) { uint i = id.x, j, k, bits = AppendBuff_Bits[i]; if (i < AppendBuff_BitN) { for (j = 0, k = i * 32; j < 32; j++) bits = AppendBuff_Assign_Bits(k + j, j, bits); AppendBuff_Bits[i] = bits; } }
  public override IEnumerator AppendBuff_Get_Existing_Sums_GS(uint3 grp_tid, uint3 grp_id, uint3 id, uint grpI)
  {
    uint i = id.x;
    if (i < AppendBuff_BitN)
    {
      uint s, bits = AppendBuff_Bits[i], c = countbits(bits);
      AppendBuff_grp0[grpI] = c; AppendBuff_grp[grpI] = c; yield return GroupMemoryBarrierWithGroupSync();
      for (s = 1; s < numthreads1; s *= 2)
      {
        if (grpI >= s && i < AppendBuff_BitN) AppendBuff_grp[grpI] = AppendBuff_grp0[grpI] + AppendBuff_grp0[grpI - s]; yield return GroupMemoryBarrierWithGroupSync();
        AppendBuff_grp0[grpI] = AppendBuff_grp[grpI]; yield return GroupMemoryBarrierWithGroupSync();
      }
      if (i < AppendBuff_BitN) AppendBuff_Sums[i] = AppendBuff_grp[grpI];
    }
  }



  public void AppendBuff_getIndexes_CPU() { AllocData_AppendBuff_Indexes(AppendBuff_IndexN); Cpu_AppendBuff_GetIndexes(); }
  public void AppendBuff_FillPrefixes_CPU() { Cpu_AppendBuff_GetFills1(); Cpu_AppendBuff_GetFills2(); Cpu_AppendBuff_IncFills1(); Cpu_AppendBuff_IncSums(); }
  public void AppendBuff_FillIndexes_CPU() { AppendBuff_FillPrefixes_CPU(); AppendBuff_getIndexes_CPU(); }

  public uint AppendBuff_Run_CPU(uint n)
  {
    AppendBuff_SetN(n);
    Cpu_AppendBuff_GetSums();
    AppendBuff_FillIndexes();
    return AppendBuff_IndexN;
  }
  //public uint AppendBuff_Run_CPU(uint n)
  //{
  //  //if (n == 0) return AppendBuff_IndexN = 0;
  //  //AppendBuff_SetN(n); Cpu_AppendBuff_Init_Bits_32(); Cpu_AppendBuff_Get_Bits_32();
  //  //useGpGpu = false;
  //  //AppendBuff_Sums[0] = AppendBuff_Bits[0];
  //  //for (int i = 1; i < AppendBuff_BitN; i++) AppendBuff_Sums[i] = AppendBuff_Sums[i - 1] + AppendBuff_Bits[i];
  //  //useGpGpu = true;
  //  //AllocData_AppendBuff_Indexes(AppendBuff_IndexN = AppendBuff_Sums[AppendBuff_BitN - 1]); Cpu_AppendBuff_CalcIndexes();

  //  if (n == 0) return AppendBuff_IndexN = 0;
  //  AppendBuff_SetN(n); if (n < 66_000_000) { Gpu_AppendBuff_Init_Bits_32(); Gpu_AppendBuff_Get_Bits_32(); } else Gpu_AppendBuff_Get_Bits();
  //  Gpu_AppendBuff_InitSums(); Gpu_AppendBuff_CalcSums(); Gpu_AppendBuff_Init_ColN_Sums(); Gpu_AppendBuff_Calc_ColN_Sums(); Gpu_AppendBuff_Add_ColN_Sums();
  //  AllocData_AppendBuff_Indexes(AppendBuff_IndexN = AppendBuff_Sums[AppendBuff_BitN - 1]); Gpu_AppendBuff_CalcIndexes();
  //  return AppendBuff_IndexN;

  //  return AppendBuff_IndexN;
  //}

  public override void Run_Append_Buffer()
  {
    AppendBuff_Run(AppendBuffTest_N);
    AppendBuffTest_IndexN = AppendBuff_IndexN;
    //if (processorType == ProcessorType.GPU) AppendBuffTest_Time_us = TimeAction_Str(AppendBuffTest_Runtime_N, () => AppendBuff_Run(AppendBuffTest_N), Unit.us);
    //else AppendBuffTest_Time_us = TimeAction_Str(AppendBuffTest_Runtime_N, () => AppendBuff_Run_CPU(AppendBuffTest_N), Unit.us);
    //if (processorType == ProcessorType.GPU) AppendBuffTest_Time_us = TimeAction(AppendBuffTest_Runtime_N, () => AppendBuff_Run(AppendBuffTest_N), Unit.s);
    //else AppendBuffTest_Time_us = TimeAction(AppendBuffTest_Runtime_N, () => AppendBuff_Run_CPU(AppendBuffTest_N), Unit.s);
    AppendBuffTest_Time_us = runOnGpu ? TimeAction(AppendBuffTest_Runtime_N, () => AppendBuff_Run(AppendBuffTest_N), Unit.s) : TimeAction(AppendBuffTest_Runtime_N, () => AppendBuff_Run_CPU(AppendBuffTest_N), Unit.s);
  }
  public override void OnValueChanged_GS()
  {
    base.OnValueChanged_GS();
    if (UI_runOnGpu.Changed) { processorType = runOnGpu ? ProcessorType.GPU : ProcessorType.CPU; UI_processorType.Changed = false; }
    if (UI_processorType.Changed) { runOnGpu = processorType == ProcessorType.GPU; UI_runOnGpu.Changed = false; }
  }



  public override void CalcPrimes()
  {
    piN = maxPrimeN;
    pjN = flooru(sqrt(piN));
    uint n = piN / 2;
    AppendBuff_SetN((n % 32) == 0 ? n : n + 32);
    Gpu_init_Primes(); Gpu_calc_primes();
    Gpu_AppendBuff_Get_Existing_Sums(); AppendBuff_FillIndexes(); Gpu_AppendBuff_Get_Existing_Bits();
    print($"There are {AppendBuff_IndexN:#,###} prime numbers < {piN:#,###}");
    print(string.Join("\t", AppendBuff_Indexes.Linq().Select(a => max(2, a * 2 + 1)).Take(25)));
    //float gpu_primes_runtime_us = (0, 1000).For().Select(a => Secs(() => Gpu_calc_primes())).Min() * 1e6f;
    //float gpu_list_runtime_us = (0, 100).For().Select(a => Secs(() => { Gpu_AppendBuff_Get_Existing_Sums(); AppendBuff_FillIndexes(); Gpu_AppendBuff_Get_Existing_Bits(); })).Min() * 1e6f;
    float gpu_primes_runtime_us = TimeAction(1000, () => Gpu_calc_primes(), Unit.us);
    float gpu_list_runtime_us = TimeAction(100, () => { Gpu_AppendBuff_Get_Existing_Sums(); AppendBuff_FillIndexes(); Gpu_AppendBuff_Get_Existing_Bits(); }, Unit.us);
    Calc_Cpu_Primes();
    print($"Gpu Prime Runtime = {gpu_primes_runtime_us} μs, Gpu List Runtime = {gpu_list_runtime_us} μs");
    print($"Gpu Prime Speedup = {cpu_calc_runtime / gpu_primes_runtime_us:#,###} X, Gpu List Speedup = {cpu_array_runtime / (gpu_primes_runtime_us + gpu_list_runtime_us):#,###} X");
  }
  public override void init_Primes_GS(uint3 id) => AppendBuff_Bits[id.x] = uint_max;
  public override void calc_primes_GS(uint3 id)
  {
    uint i = id.x, j = id.y + 2, i2 = i * 2 + 1, j2 = id.y * 2 + 3;
    if (i2 > j2 && (i2 % j2) == 0) InterlockedAnd(AppendBuff_Bits, i / 32, ~(1u << (int)(i % 32)));
  }

  public float cpu_calc_runtime = 0, cpu_array_runtime = 0;
  public void Calc_Cpu_Primes()
  {
    cpu_calc_runtime = 0;
    BitArray cpu_primes = null;
    var a = new List<int>();
    //cpu_array_runtime = Secs(() =>
    //{
    //	cpu_calc_runtime = Secs(() => { cpu_primes = cpu_computePrimes(); }) * 1e6f;
    //	for (int i = 0; i < cpu_primes.Length; i++) if (cpu_primes[i]) a.Add(i);
    //}) * 1e6f;
    cpu_array_runtime = TimeAction(1, () => { cpu_calc_runtime = TimeAction(1, () => { cpu_primes = cpu_computePrimes(); }, Unit.us); for (int i = 0; i < cpu_primes.Length; i++) if (cpu_primes[i]) a.Add(i); }, Unit.us);
    print($"CPU: There are {a.Count:#,###} prime numbers < {maxPrimeN:#,###}, calc = {cpu_calc_runtime:#,###} μs, convert to array = {cpu_array_runtime:#,###} μs");
  }
  public BitArray cpu_computePrimes()
  {
    BitArray primes = new((int)maxPrimeN);
    primes.SetAll(true);
    primes[0] = primes[1] = false;
    for (int i = 0, i2 = floori(sqrt(maxPrimeN)); i < i2; i++) if (primes[i]) for (int j = i * i; j < maxPrimeN; j += i) primes[j] = false;
    return primes;
  }

}