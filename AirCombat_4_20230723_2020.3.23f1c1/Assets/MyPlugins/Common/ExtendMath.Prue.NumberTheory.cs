/****************************************************
    文件：ExtendMath.NumberTheory.cs
	作者：lenovo
    邮箱: 
    日期：2024/5/21 14:22:13
	功能：数论
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExtendMath;
using Random = UnityEngine.Random;
 
public static partial class ExtendNumberTheory 
{
    public enum E数论
    {
        解析数论,
        代数数论,
        几何数论,
        计算数论,
        超越数论,
        组合数论,
        算术代数几何 ,
        //
        初等数论,//初等数论是用初等方法研究的数论，它的研究方法本质上说，就是利用整数环的整除性质，主要包括整除理论、同余理论、连分数理论。
        高等数论,//高等数论则包括了更为深刻的数学研究工具。它大致包括代数数论、解析数论、计算数论等等。

    }
    public enum E数论杂
    {
        //数论（number theory），是纯粹数学的分支之一，主要研究整数的性质。整数可以是方程式的解（丢番图方程）。有些解析函数（像黎曼ζ函数）中包括了一些整数、质数的性质，透过这些函数也可以了解一些数论的问题。透过数论也可以建立实数和有理数之间的关系，并且用有理数来逼近实数（丢番图逼近）。


        //数学理论或在较旧的使用中，叫做算术，是专门研究整数的纯数学的分支。它有时被称为“数学女王”，因为它在原理中的基础地位。数理论家研究质数以及由整数（例如有理数字）制成的对象的属性或定义为整数的概括（例如，代数整数）。
        //整数可以自己考虑或作为方程（Diophantine几何）的解决方案。通过研究以某种方式（分析数论）编码整数，素数或其他数论理论对象的分析对象（如Riemann zeta函数），通常最好地理解数论中的问题。人们还可以研究与有理数相关的实数，例如，由后者近似（Diophantine近似）。
        //数理论的较旧术语是算术。到二十世纪初，它被“数学理论”所取代（“算术”一词被普通大众用来表示“基本计算”，也在数学逻辑中获得了其他含义，如在数学理论中使用术语算术在二十世纪下半叶重新获得了一些地位，这可能部分是由于法国的影响力，特别是作为数理论的形容词，优选算术。
        //发展历史
        //播报
        //编辑
        //数论早期称为算术。到20世纪初，才开始使用数论的名称，而算术一词则表示“基本运算”，不过在20世纪的后半，有部份数学家仍会用“算术”一词来表示数论。1952年时数学家Harold Davenport仍用“高等算术”一词来表示数论，戈弗雷·哈罗德·哈代和爱德华·梅特兰·赖特在1938年写《数论介绍》简介时曾提到“我们曾考虑过将书名改为《算术介绍》，某方面而言是更合适的书名，但也容易让读者误会其中的内容”。
        //公元前300年，古希腊数学家欧几里德证明了有无穷多个素数，公元前250年古希腊数学家埃拉托塞尼发明了一种寻找素数的埃拉托斯特尼筛法。寻找一个表示所有素数的素数通项公式，或者叫素数普遍公式，是古典数论最主要的问题之一。
        //数论从早期到中期跨越了1000—2000年，在接近2000年时间，数论几乎是空白。中期主要指15-16世纪到19世纪，是由费马，梅森、欧拉、高斯、勒让德、黎曼、希尔伯特、Heegner等人发展的。
        //内容是寻找素数通项公式为主线的思想，开始由初等数论向解析数论和代数数论转变，产生了越来越多的猜想无法解决，遗留到20世纪，许许多多的困难还是依赖素数通项公式，例如黎曼猜想。如果找到一个素数通项公式，一些困难问题就可以由解析数论转回到初等数论范围。
        //到了十八世纪末，历代数学家积累的关于整数性质零散的知识已经十分丰富了，但是仍然没有找到素数产生的模式。德国数学家高斯集中前人的大成，写了一本书叫做《算术研究》，公元1800年寄给了法国科学院，但是法国科学院拒绝了高斯的这部杰作，高斯只好在1801年自己发表了这部著作。这部书开始了现代数论的新纪元。在《算术研究》中，高斯把过去研究整数性质所用的符号标准化了，把当时现存的定理系统化并进行了推广，把要研究的问题和已知的方法进行了分类，还引进了新的方法。高斯在这一著作中主要提出了同余理论， 并发现了著名的二次互反律， 被其誉之为“数论之酵母”。
        //黎曼在研究ζ函数时，发现了复变函数的解析性质和素数分布之间的深刻联系， 由此将数论领进了分析的领域。这方面主要的代表人物还有英国著名数论学家哈代、李特伍德、拉马努金等等。在国内，则有华罗庚、陈景润、王元等等。
        //另一方面， 由于此前人们一直关注费马大定理的证明， 所以又发展出了代数数论的研究课题。比如库默尔提出了理想数的概念--可惜他当时忽略了代数扩环的唯一分解定理不一定成立）。高斯研究了复整数环的理论--即高斯整数。他在3次情形的费马猜想中也用了扩环的代数数论性质。代数数论发展的一个里程碑，则是希尔伯特的《数论报告》。
        //随着数学工具的不断深化， 数论开始和代数几何深刻联系起来， 最终发展称为当今最深刻的数学理论，诸如算术代数几何， 它们将许多此前的研究方法和研究观点最终统一起来， 从更加高的观点出发，进行研究和探讨。
        //由于近代计算机科学和应用数学的发展，数论得到了广泛的应用。比如在计算方法、代数编码、组合论等方面都广泛使用了初等数论范围内的许多研究成果；又文献报道，有些国家应用“孙子定理”来进行测距，用原根和指数来计算离散傅立叶变换等。此外，数论的许多比较深刻的研究成果也在近似分析、差集合、快速变换等方面得到了应用。特别是由于计算机的发展，用离散量的计算去逼近连续量而达到所要求的精度已成为可能。
    }


    public abstract class A数论
    {
        //数论是纯粹数学的分支之一，主要研究整数的性质。
        //而整数的基本元素是素数（也称质数），所以数论的本质是对素数性质的研究。
        //数论被高斯誉为“数学中的皇冠”。
        //因此，数学家都喜欢把数论中一些悬而未决的疑难问题，叫做“皇冠上的明珠”，以鼓励人们去“摘取”。
    }
    public class E初等数论 :I研究内容,I研究结论
    {
        public void 研究内容()
        {    
            //本质上说，初等数论的研究手段局限在整除性质上。
            //整数环的整除理论及同余理论 ,
            //连分数理论,
            //少许不定方程,    
        }

        public void 研究结论()
        {
            //算术基本定理
            //欧几里得的质数无限证明
            //中国剩余定理
            //欧拉定理（其特例是费马小定理）
            //高斯的二次互反律
            //勾股方程的商高定理
            //佩尔方程的连分数求解法
            //等等
        }
    }

//解析数论
//借助微积分及复分析（即复变函数）来研究关于整数的问题，主要又可以分为乘性数论与加性数论两类。乘性数论藉由研究积性生成函数的性质来探讨素数分布的问题，其中质数定理与狄利克雷定理为这个领域中最著名的古典成果。加性数论则是研究整数的加法分解之可能性与表示的问题，华林问题是该领域最著名的课题。
//解析数论的创立当归功于黎曼。他发现了黎曼zeta函数之解析性质与数论中的素数分布问题存在深刻联系。确切的说， 黎曼ζ函数的非平凡零点的分布情况决定了素数的很多性质。黎曼猜测， 那些零点都落在复平面上实部为1/2的直线上。这就是著名的黎曼假设—千禧年大奖难题之一。值得注意的是， 欧拉实际上在处理素数无限问题时也用到了解析方法。
//解析数论方法除了圆法、筛法等等之外， 也包括和椭圆曲线相关的模形式理论等等。此后又发展到自守形式理论，从而和表示论联系起来。
//代数数论
//代数数论，将整数环的数论性质研究扩展到了更一般的整环上，特别是代数数域。一个主要课题就是关于代数整数的研究，目标是为了更一般地解决不定方程求解的问题。其中一个主要的历史动力来自于寻找费马大定理的证明。
//代数数论更倾向于从代数结构角度去研究各类整环的性质， 比如在给定整环上是否存在算术基本定理等等。
//这个领域与代数几何之间的关联尤其紧密， 它实际上也构成了交换代数理论的一部分。它也包括了其他深刻内容，比如表示论、p-adic理论等等。
//几何数论
//主要在于通过几何观点研究整数（在此即格点， 也称整点）的分布情形。最著名的定理为Minkowski定理。这门理论也是有闵科夫斯基所创。对于研究二次型理论有着重要作用。
//计算数论
//借助电脑的算法帮助研究数论的问题，例如素数测试和因数分解等和密码学息息相关的课题。
//超越数论
//研究数的超越性，其中对于欧拉常数与特定的riemann ζ函数值之研究尤其令人感到兴趣。此外它也探讨了数的丢番图逼近理论。
//组合数论
//利用组合和机率的技巧，非构造性地证明某些无法用初等方式处理的复杂结论。这是由保罗·艾狄胥开创的思路。比如兰伯特猜想的简化证明。
//算术代数几何
//这是数论发展到目前为止最深刻最前沿的领域， 可谓集大成者。它从代数几何的观点出发，通过深刻的数学工具去研究数论的性质。比如怀尔斯证明费马猜想就是这方面的经典实例。整个证明几乎用到了当时所有最深刻的理论工具。
//当代数论的一个重要的研究指导纲领，就是著名的郎兰兹纲领。
//研究方法
//除了上述传统方法之外，也有其他一些研究数论之法， 但是没有完全得到数学家的认可。
//比如有物理学家，通过量子力学方法声称证明了黎曼假设。


//●哥德巴赫猜想：是否每个大于2的偶数都可写成两个质数之和？
//●孪生素数猜想：孪生素数就是差为2的素数对，例如11和13。是否存在无穷多的孪生素数？
//●斐波那契数列内是否存在无穷多的素数？
//●是否存在无穷多的梅森素数？（指形如2p－1的正整数，其中指数p是素数，常记为Mp 。若Mp是素数，则称为梅森素数）
//●1995年怀尔斯和理查·泰勒证明了历时350年的费马猜想（费马大定理）。 [2]
//●黎曼猜想
//


//发现已知的最大素数
//美国中央密苏里大学数学家柯蒂斯·库珀领导的研究小组通过参加一个名为“互联网梅森素数大搜索”（GIMPS）的国际合作项目，于1月25日发现了目前已知的最大素数——257885161-1 （即2的57885161次方减1）。该素数是第48个梅森素数，有17425170位；如果用普通字号将它连续打印下来，其长度可超过65公里！美国数学学会发言人迈克·布林宣称：这是数论研究的一项重大突破。
//研究小组在大约1000台大学里的计算机上运行GIMPS的软件，每台计算机都不间断地用了39天时间证明257885161-1是个素数。之后其他研究者也独立验证了这一结果。近年来，库珀通过参加GIMPS项目一共发现了3个梅森素数。
//寻找梅森素数已成为发现已知最大素数的最有效途径。如今世界上有180多个国家和地区近28万人参加了GIMPS项目，并动用超过79万台计算机联网来寻找新的梅森素数。梅森素数是否有无穷多个？这是一个尚未破解的著名数学谜题。 [3]
//证明“弱孪生素数猜想”
//美国新罕布什尔大学数学家张益唐经过多年努力，在不依赖未经证明推论的前提下，率先证明了一个“弱孪生素数猜想”，即“存在无穷多个之差小于7000万的素数对”。4月17日，他将论文投稿给世界顶级期刊《数学年刊》。美国数学家、审稿人之一亨里克·艾温尼科评价说：“这是一流的数学工作。”他相信不久会有很多人把“7000万”这个数字“变小”。
//尽管从证明弱孪生素数猜想到证明孪生素数猜想还有相当的距离，英国《自然》杂志在线报道还是称张益唐的证明为一个“重要的里程碑”。由于孪生素数猜想与哥德巴赫猜想密切相关（姐妹问题），很多数学家希望通过解决这个猜想，进而攻克哥德巴赫猜想。
//值得一提的是，英国数学家戈弗雷·哈代和约翰·李特尔伍德曾提出一个“强孪生素数猜想”。这一猜想不仅提出孪生素数有无穷多对，而且还给出其渐近分布形式。中国数学家周海中指出：要证明强孪生素数猜想，人们仍要面对许多巨大的困难。 [3]
//解开“弱哥德巴赫猜想”
//5月13日，秘鲁数学家哈拉尔德·赫尔弗戈特在巴黎高等师范学院宣称：证明了一个“弱哥德巴赫猜想”，即“任何一个大于7的奇数都能被表示成3个奇素数之和”。
//他将论文投稿给全球最大的预印本网站（arXiv）；有专家认为这是哥德巴赫猜想研究的一项重大成果。不过，其证明是否成立，还有待进一步考证。
//赫尔弗戈特在论证技术上主要使用了哈代-李特尔伍德-维诺格拉多夫圆法。在这一圆法中，数学家创建了一个周期函数，其范围包括所有素数。1923年，哈代和李特尔伍德证明，假设广义黎曼猜想成立，三元哥德巴赫猜想对充分大的奇数是正确的；1937年，苏联数学家伊万·维诺格拉多夫更进一步，在无须广义黎曼猜想的情形下，直接证明了充分大的奇数可以表示为3个素数之和。
//英国数学家安德鲁·格兰维尔称，不幸的是，由于技术原因，赫尔弗戈特的方法很难证明“强哥德巴赫猜想”，即“关于偶数的哥德巴赫猜想”。如今数学界的主流意见认为：要证明强哥德巴赫猜想，还需要新的思路和工具，或者在现有的方法上进行重大的改进。（郑辉 作者系新加坡南洋理工大学教授） [3]
//
//我国的数论发展
//在我国近代，数论也是发展最早的数学分支之一。
//从二十世纪三十年代开始，在解析数论、丢番图方程、一致分布等方面都有过重要的贡献，出现了华罗庚、闵嗣鹤、柯召、陈景润、潘承洞等第一流的数论专家。
//其中华罗庚教授在三角和估值、堆砌素数论方面的研究是享有盛名的。
//1949年以后，数论的研究的得到了更大的发展。
//陈景润、王元等在“筛法”和“哥德巴赫猜想”方面的研究，已取得世界领先的优秀成绩；
//周海中在著名数论难题——梅森素数分布的研究中取得了世界领先的卓著成绩。 [4]

}



