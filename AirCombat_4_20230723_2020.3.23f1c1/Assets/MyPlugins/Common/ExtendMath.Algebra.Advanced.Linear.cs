/****************************************************
    文件：ExtendMath.Algebra.Advanced.Linear.cs
	作者：lenovo
    邮箱: 
    日期：2024/5/21 13:49:26
	功能：线性代数
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExtendMath_Unit;
using Random = UnityEngine.Random;

public static partial class ExtendLinearAlgebra
{
    //高斯-赛德尔迭代（Gauss–Seidel method）,Liebmann方法 ,连续位移方法
    //数值线性代数中的一个迭代法，可用来求出线性方程组解的近似值
    //该方法以卡尔·弗里德里希·高斯和路德维希·赛德尔命名。
    //
    //同雅可比法一样，高斯-赛德尔迭代是基于矩阵分解原理。
    //在数值线性代数中，Gauss-Seidel方法也称为Liebmann方法或连续位移方法，是用于求解线性方程组的迭代方法。
        //虽然它可以应用于对角线上具有非零元素的任何矩阵，但只能在矩阵是对角线主导的或对称的和正定的情况下，保证收敛。 
        //在1823年，只在高斯给他的学生Gerling的私人信中提到。1874年之前由塞德尔自行出版。
}
public static partial class ExtendLinearAlgebra
{

        //线性代数是数学的一个分支
        // 研究对象是向量，向量空间（或称线性空间），线性变换和有限维的线性方程组。
        // 向量空间是现代数学的一个重要课题
        //
        // 抽象代数
        // 泛函分析中；通过解析几何，线性代数得以被具体表示。
        // 线性代数的理论已被泛化为算子理论。
        // 由于科学研究中的非线性模型通常可以被近似为线性模型，使得线性代数被广泛地应用于自然科学和社会科学中。

        //研究对象向量、矩阵、行列式
        //抽象代数、泛函分析

}

public static partial class ExtendLinearAlgebra//线性方程组
{
    public class LinearEquations
    {

    }
}
public static partial class ExtendLinearAlgebra//行列式
{

    public static void Example_Det()
    {
        Det2 det2 = new Det2(new Vector2(1, 2), new Vector2(4, 3));
        Det3 det3 = new Det3(new Vector3(1, 2, 3), new Vector3(4, 3, 2), new Vector3(5, 7, 8));
        //
        //13,4,5,6,1
        //13,5,5,78,6
        //13,2,5,6,1
        //13,4,8,6,1
        //1,4,5,6,7
        //Det det = new Det(new List<float> { 13,4,5,6,1});
        //det.AddRow(new List<float> { 13,5,5,78,6});
        //det.AddRow(new List<float> { 13,2,5,6,1});
        //det.AddRow(new List<float> { 13,4,8,6,1});
        //det.AddRow(new List<float> { 1,4,5,6,7});
        Det det =
           new Det(new List<float> { 12, 3, 3, 1 });
        det.AddRow(new List<float> { 1, 2, 4, 3 });
        det.AddRow(new List<float> { 6, 9, 5, 4 });
        det.AddRow(new List<float> { 2, 3, 4, 5 });
        Debug.Log(det2.Value());
        Debug.Log(det3.Value());
        Debug.Log(det.ToString());
        Debug.Log(det[4, 4]);
    }



    #region Det2
    public class Det2
    {
        float a;
        float b;
        float c;
        float d;

        public Det2(Vector2 v1, Vector2 v2)
        {
            a = v1.x;
            b = v1.y;
            c = v2.x;
            d = v2.y;
        }

        public float Value()
        {
            return a * d - b * c;
        }
    }

    #endregion


    #region Det3
    public class Det3
    {
        float a11;
        float a21;
        float a31;
        float a12;
        float a22;
        float a32;
        float a13;
        float a23;
        float a33;

        public Det3(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            a11 = v1.x;
            a21 = v1.y;
            a31 = v1.z;
            a12 = v2.x;
            a22 = v2.y;
            a32 = v2.z;
            a13 = v3.x;
            a23 = v3.y;
            a33 = v3.z;
        }

        /// <summary>对角线</summary>
        public float Value()
        {
            float value = 0;
            value += a11 * a22 * a33;
            value += a12 * a23 * a31;
            value += a13 * a21 * a32;
            value -= a13 * a22 * a31;
            value -= a11 * a23 * a32;
            value -= a12 * a21 * a33;

            return value;
        }
    }

    #endregion


    #region Det
    /// <summary>
    /// 行列式 四阶开始有问题.四阶不是一样的计算方式
    /// <br/>内部是先行后列,方便索引
    /// <br/>二维行列式由三角形面积引出(已知点的坐标)
    /// </summary>
    public class Det
    {


        #region 前置知识
        //行列式可以看做是有向面积或体积的概念在一般的欧几里得空间中的推广。
        //或者说，在 n 维欧几里得空间中，行列式描述的是一个线性变换对“体积”所造成的影响。
        //
        //上三角,下三角,对角,反对角
        //转置
        //按行展开,按列展开
        //
        //性质1　行列式与它的转置行列式相等。
        //性质2 互换行列式的两行(列)，行列式变号。
        //推论 如果行列式有两行(列)完全相同，则此行列式为零。
        //性质3 行列式的某一行(列)中所有的元素都乘以同一数k，等于用数k乘此行列式。
        //推论 行列式中某一行(列)的所有元素的公因子可以提到行列式符号的外面。
        //性质4 行列式中如果有两行(列)元素成比例，则此行列式等于零。
        //性质5 把行列式的某一列(行)的各元素乘以同一数然后加到另一列(行)对应的元素上去，行列式不变
        // 行列互换，行列式不变；
        // 一行的公因子可以提出去，或者以一数乘行列式的一行就相当于用这个数乘此行列式；
        // 如果行列式中一行为0，那么行列式为0；
        // 某一行是两组数的和，则此行列式等于两个行列式的和，而这两个行列式除此行外与原来的行列式对应的行相同；
        // 如果行列式中有两行相同，那么行列式为0；
        // 行列式中有成比例的两行，则行列式为0；
        // 把一行的倍数加到另一行，行列式值不变；
        // 对换行列式中两行的位置，行列式反号。
        //
        // a11  a12  a13  a14
        // a21  a22  a23  a24
        // a31  a32  a33  a34
        // a41  a42  a43  a44
        //对角线
        // left=a11*a22*a33*a44     right=1
        // left=a21*a32*a43         right=a14
        // left=a31*a42             right=a13*a24
        // left=a41                 right=a12*a23*a34
        //  a[k,j]                  a[k+j,rightStart+1]
        //反对角线
        // left=a11                 right=a24*a33*a42
        // left=a12*a21             right=a34*a43
        // left=a13*a22*a31         right=a44
        // left=a14*a23*a32*a41     right=1
        #endregion


        #region 字属
        List<List<float>> _det;


        int _cols { get { return _det[0].Count; } }//去掉索引0
        int _rows { get { return _det.Count; } }
        public float this[int row, int col]
        {
            get
            {
                return _det[row - 1][col - 1];
            }
            set
            {
                _det[row - 1][col - 1] = value;
            }
        }


        public Det Copy()
        {
            Det det = new Det(_det[0]);
            for (int i = 0; i < _rows; i++)
            {
                List<float> colLst = new List<float>();
                for (int j = 0; j < _cols; j++)
                {
                    colLst.Add(_det[i][j]);
                }
                det.AddRow(colLst);
            }

            return det;
        }
        public Det Transposition()
        {
            float tmp = 0;
            Det det = Copy();
            for (int i = 1; i < _rows; i++)
            {
                for (int j = 0; j < i + 1; j++) //到对角线就行
                {
                    tmp = det[i, j];
                    det[i, j] = det[j, i];
                    det[j, i] = tmp;
                }
            }
            return det;
        }
        public Det()
        {

        }
        public Det(List<float> row)
        {
            _det = new List<List<float>>();
            _det.Add(row);
        }
        public Det(int r, int c)
        {
            _det = new List<List<float>>();
            return;
            for (int i = 0; i < r; i++)
            {
                List<float> row = new List<float>();
                for (int j = 0; j < c; j++)
                {
                    row.Add(0);
                }
                _det.Add(row);
            }
        }
        #endregion




        public void AddRow(List<float> row)
        {

            if (row.Count == _cols)
            {
                _det.Add(row);
            }
            else if (row.Count < _cols) //补全
            {

                int sub = _cols - row.Count;
                for (int i = 1; i < sub + 1; i++)
                {
                    row.Add(0);
                }
                _det.Add(row);
            }
            else if (row.Count > _cols)
            {
                int sub = row.Count - _cols;
                int curCols = _cols;
                int afterCols = row.Count;
                for (int i = 0; i < _cols; i++)
                {
                    for (int j = 0; j < sub + 1; j++)
                    {
                        _det[i].Add(0);
                    }
                }
                _det.Add(row);

            }

        }


        public void AddCol(List<float> col)
        {

            throw new System.Exception("异常");
            if (col.Count == _det[1].Count)
            {
                for (int i = 1; i < _rows; i++)
                {
                    _det[i].Add(col[i + 1]);
                }
            }
            else if (col.Count < _cols) //补全
            {

                int sub = _cols - col.Count;
                int curCol = _cols;
                for (int i = 1; i < _cols + 1; i++) //原行
                {
                    for (int j = curCol + 1; j < 1; j++)
                    {

                    }

                }

            }
            else if (col.Count > _det.Count)
            {
                int sub = col.Count - _det.Count;
                _det.Add(col);
                for (int j = 1; j < sub; j++)
                {
                    _det[_rows][j] = 0;
                }
            }

            throw new System.Exception("异常");
        }
        public float Mult(float para)
        {
            return para * Value();
        }
        public float Value()
        {
            if (_rows != _cols)
            {
                throw new System.Exception("异常");
            }
            if (_rows == 1 && _cols == 1)
            {
                return _det[0][0];
            }
            if (_rows == 2 && _cols == 2)
            {
                return _det[0][0] * _det[1][1] - _det[1][0] * _det[0][1];   //ad-bc
            }
            if (_rows > 3) //4阶及以上
            {

                throw new System.Exception("异常,未开发");
            }

            int rows = 0;
            int cols = 0;
            float sum = 0;
            float tmp;
            float leftTmp = 1;
            float rightTmp = 1;
            int r = 0; int c = 0;
            float diagonal = 0;
            float reverseDiagonal = 0;
            int leftCnt = 0;
            int rightCnt = 0;
            int rightStart = 0;
            float tmpDet = 0;

            #region 对角线
            //对角线
            // left=a11*a22*a33*a44     right=1
            // left=a21*a32*a43         right=a14
            // left=a31*a42             right=a13*a24
            // left=a41                 right=a12*a23*a34
            for (int i = 0; i < _rows; i++)
            {
                leftCnt = _cols - i;
                rightCnt = i;
                leftTmp = 1;
                rightTmp = 1;
                for (int j = 0; j < leftCnt; j++)
                {

                    r = i + 1 + j;
                    c = j + 1;
                    leftTmp *= _det[r - 1][c - 1];
                }
                rightStart = c + 1;
                for (int j = 0; j < rightCnt; j++)
                {
                    r = 1 + j;
                    c = rightStart + j;
                    rightTmp *= _det[r - 1][c - 1];
                }
                tmp = leftTmp * rightTmp;
                diagonal += tmp;
            }
            #endregion


            #region 反对角线
            //反对角线
            // left=a11                 right=a24*a33*a42
            // left=a12*a21             right=a34*a43
            // left=a13*a22*a31         right=a44
            // left=a14*a23*a32*a41     right=1
            for (int i = 0; i < _rows; i++)
            {
                leftCnt = i + 1;
                rightCnt = _cols - leftCnt;
                leftTmp = 1;
                rightTmp = 1;
                for (int j = 0; j < leftCnt; j++)
                {
                    r = j + 1;
                    c = leftCnt - j;
                    leftTmp *= _det[r - 1][c - 1];
                }
                rightStart = r + 1;
                for (int j = 0; j < rightCnt; j++)
                {
                    r = rightStart + j;
                    c = _cols - j;
                    rightTmp *= _det[r - 1][c - 1];
                }
                tmp = leftTmp * rightTmp;
                reverseDiagonal -= tmp;
            }
            #endregion

            return diagonal + reverseDiagonal;
        }


        #region 打印
        public override string ToString()
        {
            string str = "";
            str += $"Det[{_rows},{_cols}]={Value()}\n";
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    str += _det[i][j] + "\t";
                }
                str += "\n";
            }

            return str;
        }

        public static string Show()
        {
            return "Determinant |A| det(A)";
        }
        #endregion



        #region 内部类
        public class DetUnit
        {
            public int RowIdx;
            public int ColIdx;
            /// <summary>浮点</summary>
            public float value;
            /// <summary>复数</summary>
            public ComplexNumber complexNumbersVal;

            public DetUnit(int rowIdx, int colIdx, float value)
            {
                RowIdx = rowIdx;
                ColIdx = colIdx;
                this.value = value;
            }

            public DetUnit(int rowIdx, int colIdx, ComplexNumber complexNumbers)
            {
                RowIdx = rowIdx;
                ColIdx = colIdx;
                this.complexNumbersVal = complexNumbers;
            }
        }

        #endregion  
    }
    #endregion



}
public static partial class ExtendLinearAlgebra  //矩阵
{

    /// <summary>
    /// matrix矩阵：矩阵=>方阵=>对角矩阵=>单位矩阵
    /// 行向量（转置）列向量
    /// 向量a * 矩阵 = 向量b（矩阵就a变为b）=>矩阵是对向量的一种变换
    /// 关键词，将矩阵拆成基向量，再乘以变换矩阵 ,具体看Common_Matrix.Rotate_RoundX，Matrix.Rotate_RoundY，Matrix.Rotate_RoundZ
    /// </summary>
    public class Matrix
    {


        #region TODO
        //逆矩阵
        //      转置矩阵
        //      单位矩阵
        //增广矩阵
        //符号矩阵
        //伴随矩阵
        //满秩矩阵
        //      对角矩阵
        //初等矩阵
        //雅可比矩阵
        //共轭矩阵
        //过渡矩阵
        #endregion
        #region 字属构造
        public int rowCnt;
        public int colCnt;
        public MatrixUnit[,] matrix;//[,] 矩阵  [][]拼图阵（缺这缺那）



        public Matrix(int rowCnt, int colCnt, float p = 0)
        {
            this.rowCnt = rowCnt;
            this.colCnt = colCnt;
            matrix = new MatrixUnit[rowCnt, colCnt];
            for (int i = 0; i < rowCnt; i++)
            {
                for (int j = 0; j < colCnt; j++)
                {
                    MatrixUnit unit = new MatrixUnit(i, j, p);
                    matrix[i, j] = unit;
                }
            }
        }

        public Matrix(int rowCnt, int colCnt, float[,] arr)
        {
            this.rowCnt = rowCnt;
            this.colCnt = colCnt;
            matrix = new MatrixUnit[rowCnt, colCnt];
            for (int i = 0; i < rowCnt; i++)
            {
                for (int j = 0; j < colCnt; j++)
                {
                    MatrixUnit unit = new MatrixUnit(i, j, arr[i, j]);
                    matrix[i, j] = unit;
                }
            }
        }
        #endregion


        #region pub
        /// <summary>
        /// 设置行
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="row"></param>
        public void SetRow(int idx, float[] row)
        {
            MatrixUnit unit;
            for (int i = 0; i < row.Length; i++)
            {
                unit = new MatrixUnit(idx, i, row[i]);
                matrix[idx, i] = unit;
            }
        }

        public void SetRow(int idx, ComplexNumber[] row)
        {
            MatrixUnit unit;
            for (int i = 0; i < row.Length; i++)
            {
                unit = new MatrixUnit(idx, i, row[i]);
                matrix[idx, i] = unit;
            }
        }

        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="col"></param>
        public void SetCol(int idx, float[] col)
        {
            MatrixUnit unit;
            for (int i = 0; i < col.Length; i++)
            {
                unit = new MatrixUnit(i, idx, col[i]);
                matrix[i, idx] = unit;
            }
        }


        public void SetVal(int rowIdx, int colIdx, float value)
        {
            MatrixUnit unit = new MatrixUnit(rowIdx, colIdx, value);
            matrix[rowIdx, colIdx] = unit;
        }


        public void SetUnit(MatrixUnit unit)
        {
            matrix[unit.RowIdx, unit.ColIdx] = unit;
        }


        public MatrixUnit[] GetRow(int idx)
        {
            if (idx >= rowCnt)
            {
                return null;
            }

            MatrixUnit[] row = new MatrixUnit[colCnt];
            for (int i = 0; i < colCnt; i++)
            {
                MatrixUnit unit = new MatrixUnit(idx, i, matrix[idx, i].value);
                row[i] = unit;
            }
            return row;
        }
        public MatrixUnit[] GetCol(int idx)
        {
            if (idx >= colCnt)
            {
                return null;
            }

            MatrixUnit[] col = new MatrixUnit[rowCnt];
            for (int i = 0; i < rowCnt; i++)
            {
                MatrixUnit unit = new MatrixUnit(i, idx, matrix[i, idx].value);
                col[i] = unit;
            }
            return col;
        }
        #endregion


        #region pub	static
        public static Matrix Add(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.rowCnt != matrix2.rowCnt || matrix1.colCnt != matrix2.colCnt)
            {
                Debug.LogErrorFormat("矩阵相加吗，行列不一致：matrix1：matrix2=（{0},{1}）：（{2},{3}）",
                    matrix1.rowCnt, matrix2.rowCnt,
                    matrix1.colCnt, matrix2.colCnt);
                return null;
            }

            int rowCnt = matrix1.rowCnt;
            int colCnt = matrix1.colCnt;
            Matrix matrix = new Matrix(rowCnt, colCnt);
            for (int i = 0; i < rowCnt; i++)
            {
                for (int j = 0; j < colCnt; j++)
                {
                    float value = matrix1.matrix[i, j].value + matrix2.matrix[i, j].value;
                    MatrixUnit unit = new MatrixUnit(i, j, value);
                    matrix.matrix[i, j] = unit;
                }
            }

            return matrix;
        }



        public static Matrix Multiply(Matrix matrix, float p)
        {

            for (int i = 0; i < matrix.rowCnt; i++)
            {
                for (int j = 0; j < matrix.colCnt; j++)
                {
                    matrix.matrix[i, j].value *= p;
                }
            }
            return matrix;
        }




        /// <summary>
        /// 求单位矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix UnitMatrix(Matrix matrix)
        {

            return UnitMatrix(matrix.rowCnt, matrix.colCnt);
        }

        public static Matrix UnitMatrix(int rowCnt, int colCnt)
        {

            Matrix matrix = new Matrix(rowCnt, colCnt, 0);
            SetDiagonal(matrix);

            return matrix;
        }


        /// <summary>
        /// 转置
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix Transpose(Matrix matrix)
        {
            Matrix tmp = new Matrix(matrix.colCnt, matrix.rowCnt);
            for (int i = 0; i < matrix.rowCnt; i++)
            {
                for (int j = 0; j < matrix.colCnt; j++)
                {
                    tmp.matrix[j, i] = new MatrixUnit(i, j, matrix.matrix[i, j].value);
                }
            }
            // matrix = leftTmp;
            return tmp;
        }


        /// <summary>
        /// 设置对角线
        /// </summary>
        /// <returns></returns>
        public static void SetDiagonal(Matrix matrix, float value = 1)
        {
            for (int i = 0; i < matrix.rowCnt; i++)
            {
                for (int j = 0; j < matrix.colCnt; j++)
                {
                    if (i == j)
                    {
                        matrix.SetVal(i, j, value);
                    }
                }
            }
        }
        public static Matrix CrossProduct(Matrix matrix1, Matrix matrix2)
        {
            return Multiply(matrix1, matrix2);
        }

        public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
        {
            // if (matrix1.列数 != matrix2.行数)
            if (matrix1.colCnt != matrix2.rowCnt)
            {
                Debug.LogErrorFormat("矩阵1的行数{0}!=矩阵2的列数{1}", matrix1.colCnt, matrix2.rowCnt);
                return null;
            }
            int cnt = matrix1.colCnt;//或 matrix2.rowCnt
            Matrix matrix = new Matrix(matrix1.rowCnt, matrix2.colCnt);



            /**
               int cnt = 6;
               Matrix matrix1 = new Matrix(6, cnt, 9);
               Matrix matrix2 = new Matrix(cnt ,3, 7);
            //**/
            for (int i = 0; i < matrix.rowCnt; i++)
            {
                for (int j = 0; j < matrix.colCnt; j++)
                {
                    MatrixUnit[] row = matrix1.GetRow(i);
                    MatrixUnit[] col = matrix2.GetCol(j);
                    float value = 0;
                    for (int m = 0; m < cnt; m++)
                    {
                        value += row[m].value * col[m].value;
                    }

                    MatrixUnit unit = new MatrixUnit(i, j, value);
                    matrix.matrix[i, j] = unit;
                }
            }

            return matrix;
        }
        #endregion


        #region 内部类
        public class MatrixUnit
        {
            public int RowIdx;
            public int ColIdx;
            /// <summary>浮点</summary>
            public float value;
            /// <summary>复数</summary>
            public ComplexNumber complexNumbersVal;

            public MatrixUnit(int rowIdx, int colIdx, float value)
            {
                RowIdx = rowIdx;
                ColIdx = colIdx;
                this.value = value;
            }

            public MatrixUnit(int rowIdx, int colIdx, ComplexNumber complexNumbers)
            {
                RowIdx = rowIdx;
                ColIdx = colIdx;
                this.complexNumbersVal = complexNumbers;
            }
        }

        #endregion


        #region ToString
        public override string ToString()
        {
            string str = string.Format("矩阵[{0},{1}]\n", rowCnt, colCnt);
            for (int i = 0; i < rowCnt; i++)
            {
                for (int j = 0; j < colCnt; j++)
                {
                    str += matrix[i, j].value + "\t";
                }
                str += "\n";
            }

            return str;
        }
        #endregion

    }

}



