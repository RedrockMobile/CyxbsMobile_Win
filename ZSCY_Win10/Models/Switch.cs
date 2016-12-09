using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Models
{
    class Switch
    {
        public static string[] MajorSwitch (string n)
        {
            if (n.Equals("光电工程学院"))
                return new string[] { "光电信息科学与工程", "电子科学与技术", "电磁场与无线技术", "电子信息科学与技术", "微电子科学与工程", "集成电路设计与集成系统" };
            else if (n.Equals("经济管理学院"))
                return new string[] {"工程管理", "工商管理","会计学","市场营销","经济学","信息管理与信息系统" };
            else if (n.Equals("国际学院"))
                return new string[] { "电子信息工程（中外合作办学）", "软件工程（中外合作办学）"};
            else if (n.Equals("法学院"))
                return new string[] { "法学","知识产权" };
            else if (n.Equals("传媒艺术学院"))
                return new string[] { "广播电视编导", "动画","数字媒体艺术", "视觉传达设计","环境设计","产品设计"};
            else if (n.Equals("通信与信息工程学院"))
                return new string[] { "广播电视工程","数字媒体技术", "通信工程","电子信息工程","信息工程" };
            else if (n.Equals("计算机科学与技术学院"))
                return new string[] { "计算机科学与技术","信息安全","网络工程","智能科学与技术","地理信息科学","空间信息与数字技术" };
            else if (n.Equals("软件工程学院"))
                return new string[] { "软件工程"};
            else if (n.Equals("体育学院"))
                return new string[] { "社会体育指导与管理"};
            else if (n.Equals("生物信息学院"))
                return new string[] { "生物信息学","生物医学工程"};
            else if (n.Equals("理学院"))
                return new string[] { "信息与计算科学","数学与应用数学","应用物理学" };
            else if (n.Equals("先进制造工程学院"))
                return new string[] { "机械设计制造及其自动化","机械电子工程"};
            else if (n.Equals("外国语学院"))
                return new string[] { "英语","翻译"};
            else if (n.Equals("自动化学院"))
                return new string[] { "自动化","电气工程及其自动化","物联网工程","智能电网信息工程","测控技术与仪器" };
            else
                return new string[] { };
        }
        public static SexRatio SexRatioSwitch(string n)
        {
            if (n.Equals("光电信息科学与工程") || n.Equals("电子科学与技术") || n.Equals("电磁场与无线技术") || n.Equals("电子信息科学与技术"))
                return new SexRatio(235, "77.80%", 67, "28.80%");
            else if (n.Equals("电子商务") || n.Equals("物流管理"))
                return new SexRatio(73, "61.68%", 45, "38.32%");
            else if (n.Equals("电子信息工程（中外合作办学）"))
                return new SexRatio(75, "75%", 25, "25%");
            else if (n.Equals("法学") || n.Equals("知识产权"))
                return new SexRatio(34, "33.01", 69, "66.99%");
            else if (n.Equals("工程管理"))
                return new SexRatio(49, "80.33%", 12, "19.67%");
            else if (n.Equals("工商管理") || n.Equals("会计学") || n.Equals("市场营销"))
                return new SexRatio(86, "42.16%", 118, "57.84%");
            else if (n.Equals("广播电视编导"))
                return new SexRatio(60, "25.21%", 178, "74.79%");
            else if (n.Equals("广播电视工程") || n.Equals("数字媒体技术"))
                return new SexRatio(79, "59.85%", 53, "40.15%");
            else if (n.Equals("微电子科学与工程") || n.Equals("集成电路设计与集成系统"))
                return new SexRatio(205, "88.74%", 26, "11.26%");
            else if (n.Equals("计算机科学与技术") || n.Equals("信息安全") || n.Equals("网络工程") || n.Equals("智能科学与技术") || n.Equals("地理信息科学") || n.Equals("空间信息与数字技术"))
                return new SexRatio(458, "79.79%", 116, "20.21%");
            else if (n.Equals("经济学"))
                return new SexRatio(42, "57.53%", 31, "42.47%");
            else if (n.Equals("软件工程"))
                return new SexRatio(443, "83.90%", 85, "16.10%");
            else if (n.Equals("软件工程（中外合作办学）"))
                return new SexRatio(85, "85.00%", 15, "15.00%");
            else if (n.Equals("社会体育指导与管理"))
                return new SexRatio(46, "75.41%", 15, "24.59%");
            else if (n.Equals("生物信息学"))
                return new SexRatio(48, "66.67%", 24, "33.33%");
            else if (n.Equals("生物医学工程"))
                return new SexRatio(47, "51.65%", 44, "48.35%");
            else if (n.Equals("信息与计算科学") || n.Equals("数学与应用数学") || n.Equals("应用物理学"))
                return new SexRatio(141, "72.31%", 54, "27.69%");
            else if (n.Equals("动画") || n.Equals("数字媒体艺术"))
                return new SexRatio(63, "37.28%", 106, "62.72%");
            else if (n.Equals("通信工程") || n.Equals("电子信息工程") || n.Equals("信息工程"))
                return new SexRatio(399, "76.00%", 126, "24.00%");
            else if (n.Equals("机械设计制造及其自动化") || n.Equals("机械电子工程"))
                return new SexRatio(251, "90.94%", 25, "9.06%");
            else if (n.Equals("信息管理与信息系统"))
                return new SexRatio(67, "62.62%", 40, "37.38%");
            else if (n.Equals("视觉传达设计") || n.Equals("环境设计") || n.Equals("产品设计"))
                return new SexRatio(55, "30.90%", 123, "69.10%");
            else if (n.Equals("英语") || n.Equals("翻译"))
                return new SexRatio(15, "18.29%", 67, "81.71%");
            else if (n.Equals("自动化") || n.Equals("电气工程及其自动化") || n.Equals("物联网工程") || n.Equals("智能电网信息工程") || n.Equals("测控技术与仪器"))
                return new SexRatio(472, "82.23%", 102, "17.77%");
            else
                return new SexRatio(0, "", 0, "");
        }
        public static SubjectRatio SubjectRatioSwitch(string n)
        {
            if (n.Equals("光电工程学院"))
                return new SubjectRatio("大学物理", "概率论", "工程图学", 54, 26, 20);
            else if (n.Equals("经济管理学院"))
                return new SubjectRatio("概率论", "高等数学", "C语言", 42, 38, 20);
            else if (n.Equals("国际学院"))
                return new SubjectRatio("软件设计基础", "线性代数", "大学物理", 52, 32, 16);
            else if (n.Equals("法学院"))
                return new SubjectRatio("刑法", "民法", "法理", 55, 24, 21);
            else if (n.Equals("传媒艺术学院"))
                return new SubjectRatio("视听说", "读写译", "美术史", 43, 32, 25);
            else if (n.Equals("通信与信息工程学院"))
                return new SubjectRatio("电子电路", "大学物理", "高等数学", 62, 20, 18);
            else if (n.Equals("计算机科学与技术学院"))
                return new SubjectRatio("大学物理", "高等数学", "线性代数", 40, 35, 25);
            else if (n.Equals("软件工程学院"))
                return new SubjectRatio("高等数学", "离散数学", "C++", 56, 23, 21);
            else if (n.Equals("体育学院"))
                return new SubjectRatio("运动解剖学", "体育概论", "健美操", 56, 22, 22);
            else if (n.Equals("生物信息学院"))
                return new SubjectRatio("高等数学", "视听说", "化学", 45, 31, 24);
            else if (n.Equals("理学院"))
                return new SubjectRatio("数学分析", "高等数学", "大学物理", 49, 28, 23);
            else if (n.Equals("先进制造工程学院"))
                return new SubjectRatio("工程图学", "大学物理", "高等数学", 55, 24, 21);
            else if (n.Equals("外国语学院"))
                return new SubjectRatio("基础英语", "英语语音", "英语阅读", 45, 28, 27);
            else if (n.Equals("自动化学院"))
                return new SubjectRatio("大学物理", "高等数学", "C语言", 45, 30, 25);
            else
                return new SubjectRatio("", "", "", 0, 0, 0);
        }
        public static CareerRatio CareerRatioSwitch(string n)
        {
            if (n.Equals("光电工程学院"))
                return new CareerRatio(71.06,23.40,5.53,0.01);
            else if (n.Equals("经济管理学院"))
                return new CareerRatio(85.61,9.30,5.09,0.00);
            else if (n.Equals("国际学院"))
                return new CareerRatio(0,0,0,0);
            else if (n.Equals("法学院"))
                return new CareerRatio(63.75,31.25,5.00,0.00);
            else if (n.Equals("传媒艺术学院"))
                return new CareerRatio(82.64,10.60,2.61,4.15);
            else if (n.Equals("通信与信息工程学院"))
                return new CareerRatio(75.60,22.23,2.08,0.09);
            else if (n.Equals("计算机科学与技术学院"))
                return new CareerRatio(73.02,21.50,4.67,0.81);
            else if (n.Equals("软件工程学院"))
                return new CareerRatio(86.97,7.04,4.58,1.41);
            else if (n.Equals("体育学院"))
                return new CareerRatio(86.67,11.67,1.67,0.00);
            else if (n.Equals("生物信息学院"))
                return new CareerRatio(71.64,23.51,4.48,0.37);
            else if (n.Equals("理学院"))
                return new CareerRatio(77.85,14.56,5.06,0.00);
            else if (n.Equals("先进制造工程学院"))
                return new CareerRatio(85.71,10.08,4.20,0.01);
            else if (n.Equals("外国语学院"))
                return new CareerRatio(84.38,15.62,0.00,0.00);
            else if (n.Equals("自动化学院"))
                return new CareerRatio(81.35,17.25,1.04,0.36);
            else
                return new CareerRatio(0,0,0,0);
        }
    }
}
