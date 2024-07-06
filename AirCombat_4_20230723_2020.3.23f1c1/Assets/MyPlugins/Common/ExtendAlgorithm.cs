/****************************************************
    文件：ExtendAlgorithm.cs
	作者：lenovo
    邮箱: 
    日期：2023/9/10 16:10:36
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


#region 三大算法
public static partial class ExtendAlgorithm//排序
{
    static void Example_Algorithm_Sort()
    {
        InsertionSort();//插入排序
        RapidSort_Function();//快速排序
        SimpleSelectionSort();//简单选择排序

    }
    //插入排序
    //直接插入排序
    //折半插入排序
    //希尔排序
    //冒泡排序
    //快速排序
    //简单选择排序
    //堆排序
    //归并排序
    //基数排序

    static void InsertionSort()//插入排序
    {
        //数据
        int[] pokerArray = new int[] { 4, 3, 8, 6, 5, 7, 1, 10, 9, 7, 4 };
        //遍历
        for (int i = 1; i < pokerArray.Length; i++)//要被拿来比较的牌
        {
            if (pokerArray[i] < pokerArray[i - 1])//是小牌，拿起来插前面排好序的
            {
                int pokerInHand = pokerArray[i];//保存，因为后面后移会被覆盖

                for (int j = i - 1; j >= 0; j--)//跟前面排好序的牌比较
                {
                    if (pokerInHand < pokerArray[j])//比前面的牌小。下面进行后移，插入牌
                    {
                        pokerArray[j + 1] = pokerArray[j];//往后移留出空位
                        pokerArray[j] = pokerInHand;//空位插入前面保存的那张牌
                    }
                }
            }
        }
        //打印
        foreach (int i in pokerArray)
        {
            Console.Write(i + "\t");
        }
    }
    static void SimpleSelectionSort()//简单选择排序。大长腿比赛，逐渐淘汰掉小短身
    {
        int[] leggyContestantArray = new int[] { 162, 158, 163, 163, 160, 167, 170, 168, 168 };
        //排序
        for (int i = 0; i < leggyContestantArray.Length - 1; i++)//最后一名不用排，，所以i=1
        {
            int leader = leggyContestantArray[i];//第一名先先到先得
            int loser;
            for (int j = i + 1; j < leggyContestantArray.Length; j++)//i后面一堆竞选者
            {
                if (leggyContestantArray[j] > leader)//挑战成功的
                {
                    //比赛结果
                    loser = leader;//卫冕失败
                    leader = leggyContestantArray[j];//竞选对手上榜
                                                     //交换排位
                    leggyContestantArray[j] = loser;
                    leggyContestantArray[i] = leader;
                }
            }
        }

        //打印
        for (int i = 0; i < leggyContestantArray.Length; i++)
        {
            Console.WriteLine(leggyContestantArray[i]);

        }
    }
    static void RapidSort_Function()
    {
        int[] array = new int[] { 4, 3, 8, 6, 5, 7, 1, 10, 9, 7, 4 };
        int firstIndex = 0;
        int lastIndex = array.Length - 1;
        RapidSort(array, firstIndex, lastIndex);
        foreach (int i in array)
        {
            Console.Write(i + " ");
        }
    }
    static void RapidSort(int[] array, int left, int right)//快速排序
    {

        int i = left;
        int j = right;
        int pivot = array[i];

        while (i < j && true)
        {
            while (i < j && true)//右移
            {
                if (array[j] < pivot)//找到移值
                {
                    array[i] = array[j];
                    break;
                }
                else//没找到，移动另一边的索引
                {
                    j--;
                }
            }
            while (i < j && true)//左移
            {
                if (array[i] > pivot)
                {
                    array[j] = array[i];
                    break;
                }
                else
                {
                    i++;
                }
            }

            array[i] = pivot;

            RapidSort(array, left, i - 1);
            RapidSort(array, i + 1, right);
        }



    }
}
public static partial class ExtendAlgorithm//搜索/查找
{ 
    //顺序查找-有序表
    //顺序查找-无序表
    //折半查找
    //B树
    //B+树
    //红黑树.相对比AVL,优点是增删
    //哈希表-拉链法
    //哈希表-开放定址法
}
public static partial class ExtendAlgorithm //图结构
{ 
   //存储结构-邻接矩阵
   //存储结构-邻接链表
   //广度优先搜索-BFS
   //深度度优先搜索-DFS   
   //Prim(普里姆)算法
   //Kruskal(克鲁斯卡尔)算法
  
   //Dijkstra(迪杰斯特拉)算法
   //Floyd(弗洛伊德)算法
   //拓扑排序-栈
   //关键路径
}
#endregion

public static partial class ExtendAlgorithm
{
    //遗传算法 决策树 下棋 搜索算法
    //生命:初始化 杂交 切割 组合 旋转 变异 选择
    //解决问题:旅行商路线
}
public static partial class ExtendAlgorithm
{
    //动态规划 有向无环图DAG(上下游,但是有很多小河)/递推关系/初始条件
    //
    //走楼梯,只能走1步或2步,求走n步楼梯有多少种解法        
    //Fm=Fm_1+Fm_2(Fm=F(m-1)+F(m-2))
    //F1=1(1个台阶只有不走这一种),F2=1(2个台阶只有走一步一种),F3=2(2个台阶有两个走一步,一个走两步两种)
    //
    //最小单词距离(编辑距离)
    // cats=>cat(删除sub) cat=>cats(增加add) cat =>cut(替换replace)   距离都是1
    // 
    //九宫棋 马尔科夫决策问题MDP

    //线性规划


}
public static partial class ExtendAlgorithm 
{
    //快速傅立叶变换（Fast Fourier Transform，FFT）
    //卷积运算
    //伯恩斯坦多项
    //矩阵运算规则
    //伯努利时间序列
    //马尔科夫序列
    //排序

    //对比学习
    //强化学习
    //深度学习
}



