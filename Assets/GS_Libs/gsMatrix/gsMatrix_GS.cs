using GpuScript;
public class gsMatrix_GS : _GS
{
  uint IntsN, col_m, row_n, XN, AI0, XsI0, BsI0;
  int[] Ints;
  float[] A_matrix, Xs, Bs;
  void Get_A_matrix() { Size(col_m, row_n); }
  void Set_A_matrix() { Size(col_m, row_n); }
  void Set_Xs() { Size(col_m, XN); }
  void Get_Bs() { Size(col_m, XN); }
  void Zero_bs() { Size(col_m, XN); }
  void Calc_bs() { Size(col_m, row_n, XN); }
}