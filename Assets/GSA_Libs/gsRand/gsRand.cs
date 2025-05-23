using GpuScript;

public class gsRand : gsRand_
{
  public uint Random_uint(uint minu = 0, uint maxu = uint_max) => roundu(UnityEngine.Random.Range((float)minu, (float)maxu));
  public uint4 Random_uint4(uint a, uint b, uint c, uint d) => uint4(Random_uint(0, a), Random_uint(0, b), Random_uint(0, c), Random_uint(0, d));
  public uint4 Random_uint4() => Random_uint4(330382100u, 1073741822u, 252645134u, 1971u);

  public virtual void Init(uint _n, uint seed = 0)
  {
    N = _n;
    if (seed > 0) UnityEngine.Random.InitState((int)seed);
    seed4 = Random_u4();
    AddComputeBuffer(ref rs, nameof(rs), N);
    Gpu_initSeed();
    for (I = 1; I < N; I *= 2) for (J = 0; J < 4; J++) Gpu_initState();
  }
  public override void initSeed_GS(uint3 id) { uint i = id.x; rs[i] = i == 0 ? seed4 : u0000; }
  public override void initState_GS(uint3 id) { uint i = id.x + I; if (i < N) rs[i] = index(rs[i], J, UInt(id.x, 0, uint_max)); }

  protected uint u(uint a, int b, int c, int d, uint e) => ((a & e) << d) ^ (((a << b) ^ a) >> c);
  protected uint4 U4(uint4 r) => uint4(u(r.x, 13, 19, 12, 4294967294u), u(r.y, 2, 25, 4, 4294967288u), u(r.z, 3, 11, 17, 4294967280u), r.w * 1664525 + 1013904223u);
  protected uint UV(uint4 r) => cxor(r);
  protected float FV(uint4 r) => 2.3283064365387e-10f * UV(r);
  public uint4 rUInt4(uint i) => U4(rs[i]);
  public uint4 UInt4(uint i) => rs[i] = rUInt4(i);
  public float rFloat(uint i) => FV(rUInt4(i));
  public float rFloat(uint i, float a, float b) => lerp(a, b, rFloat(i));
  public float Float(uint i) => FV(UInt4(i));
  public float Float(uint i, float A, float B) => lerp(A, B, Float(i));
  public int Int(uint i, int A, int B) => floori(Float(i, A, B));
  public int Int(uint i) => Int(i, int_min, int_max);
  public uint UInt(uint i, uint A, uint B) => flooru(Float(i, A, B));
  public uint UInt(uint i) => UV(UInt4(i));
  protected float3 onSphere_(float a, float b) => rotateX(rotateZ(f100, acos(a * 2 - 1)), b * TwoPI);
  protected float3 onSphere_(uint i) { uint j = i * 2; return onSphere_(Float(j), Float(j + 1)); }
  protected float3 onCircle_(float a) => rotateZ(f100, a * TwoPI);
  public float3 onSphere(uint i) { uint j = i * 2; return onSphere_(Float(j), Float(j + 1)); }
  public float3 inSphere(uint i) { uint j = i * 3; return pow(Float(j + 2), 0.3333333f) * onSphere_(Float(j), Float(j + 1)); }
  public float3 onCircle(uint i) => onCircle_(Float(i));
  public float3 inCircle(uint i) { uint j = i * 2; return onCircle_(Float(j)) * sqrt(Float(j + 1)); }
  public float3 inCube(uint i) { uint j = i * 3; return float3(Float(j), Float(j + 1), Float(j + 2)); }
  public float gauss(uint i) { uint j = i * 2; return sqrt(-2 * ln(1 - Float(j))) * cos(TwoPI * (1 - Float(j + 1))); }
  public float gauss(uint i, float mean, float standardDeviation) => standardDeviation * gauss(i) + mean;
  public float exponential(uint i) => -log(Float(i));
  public float exponential(uint i, float mean) => mean * exponential(i);
}