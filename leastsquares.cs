internal class leastsquares
{
  public static void buildgeneralleastsquares(ref double[] y, ref double[] w, ref double[,] fmatrix, int n, int m, ref double[] c)
  {
    double[,] numArray1 = new double[0, 0];
    double[,] numArray2 = new double[0, 0];
    double[,] numArray3 = new double[0, 0];
    double[] numArray4 = new double[0];
    double[] tau = new double[0];
    double[,] numArray5 = new double[0, 0];
    double[] tauq = new double[0];
    double[] taup = new double[0];
    double[] d = new double[0];
    double[] e = new double[0];
    bool isupper = false;
    int val1 = n;
    int num1 = m;
    c = new double[num1 - 1 + 1];
    double[,] numArray6 = new double[num1 + 1, System.Math.Max(val1, num1) + 1];
    double[] numArray7 = new double[System.Math.Max(val1, num1) + 1];
    for (int index = 1; index <= val1; ++index)
      numArray7[index] = w[index - 1] * y[index - 1];
    for (int index = val1 + 1; index <= num1; ++index)
      numArray7[index] = 0.0;
    for (int index1 = 1; index1 <= num1; ++index1)
    {
      int num2 = -1;
      for (int index2 = 1; index2 <= val1; ++index2)
        numArray6[index1, index2] = fmatrix[index2 + num2, index1 - 1];
    }
    for (int index1 = 1; index1 <= num1; ++index1)
    {
      for (int index2 = val1 + 1; index2 <= num1; ++index2)
        numArray6[index1, index2] = 0.0;
    }
    for (int index1 = 1; index1 <= num1; ++index1)
    {
      for (int index2 = 1; index2 <= val1; ++index2)
        numArray6[index1, index2] = numArray6[index1, index2] * w[index2 - 1];
    }
    int n1 = System.Math.Max(val1, num1);
    lq.lqdecomposition(ref numArray6, num1, n1, ref tau);
    lq.unpackqfromlq(ref numArray6, num1, n1, ref tau, num1, ref numArray2);
    double[,] numArray8 = new double[2, num1 + 1];
    for (int index = 1; index <= num1; ++index)
      numArray8[1, index] = 0.0;
    for (int index1 = 1; index1 <= num1; ++index1)
    {
      double num2 = 0.0;
      for (int index2 = 1; index2 <= n1; ++index2)
        num2 += numArray7[index2] * numArray2[index1, index2];
      numArray8[1, index1] = num2;
    }
    for (int index1 = 1; index1 <= num1 - 1; ++index1)
    {
      for (int index2 = index1 + 1; index2 <= num1; ++index2)
        numArray6[index1, index2] = numArray6[index2, index1];
    }
    for (int index1 = 2; index1 <= num1; ++index1)
    {
      for (int index2 = 1; index2 <= index1 - 1; ++index2)
        numArray6[index1, index2] = 0.0;
    }
    bidiagonal.tobidiagonal(ref numArray6, num1, num1, ref tauq, ref taup);
    bidiagonal.multiplybyqfrombidiagonal(ref numArray6, num1, num1, ref tauq, ref numArray8, 1, num1, true, false);
    bidiagonal.unpackptfrombidiagonal(ref numArray6, num1, num1, ref taup, num1, ref numArray3);
    bidiagonal.unpackdiagonalsfrombidiagonal(ref numArray6, num1, num1, ref isupper, ref d, ref e);
    if (!bdsvd.bidiagonalsvddecomposition(ref d, e, num1, isupper, false, ref numArray8, 1, ref numArray2, 0, ref numArray3, num1))
    {
      for (int index = 0; index <= num1 - 1; ++index)
        c[index] = 0.0;
    }
    else
    {
      if (d[1] != 0.0)
      {
        for (int index = 1; index <= num1; ++index)
          numArray8[1, index] = d[index] <= 5E-15 * System.Math.Sqrt((double) num1) * d[1] ? 0.0 : numArray8[1, index] / d[index];
      }
      for (int index = 1; index <= num1; ++index)
        numArray7[index] = 0.0;
      for (int index1 = 1; index1 <= num1; ++index1)
      {
        double num2 = numArray8[1, index1];
        for (int index2 = 1; index2 <= num1; ++index2)
          numArray7[index2] = numArray7[index2] + num2 * numArray3[index1, index2];
      }
      for (int index = 1; index <= num1; ++index)
        c[index - 1] = numArray7[index];
    }
  }
}
