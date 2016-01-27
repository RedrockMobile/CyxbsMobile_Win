using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Data;

namespace ZSCY_Win10.Util
{
    class EmptyClass
    {
        /// <summary>
        /// 要查询的周次
        /// </summary>
        public int Weeknum { get; set; }
        /// <summary>
        /// 所有学号的课程，数据源
        /// </summary>
        public Dictionary<string,List<ClassListLight>> Searchlist { get; set; }

        public EmptyClass(int weeknum, Dictionary<string, List<ClassListLight>> searchlist)
        {
#if DEBUG
            this.Weeknum = 11;// weeknum;
            this.Searchlist = searchlist;
#else
            this.Weeknum=weeknum;
            this.Searchlist = searchlist;
#endif
        }
        public void getfreetime(ref List<ClassListLight> result)
        {
            //所有人的名字
            string[] names = (from n in Searchlist.Keys select n).ToArray();
            //星期，时间段，人名数组
            List<ClassListLight> clist = new List<ClassListLight>();
            if (Weeknum != 0)
            {
                foreach (var key in Searchlist.Keys)
                {
                    //找到该周的所有课程
                    clist.AddRange((from n in Searchlist[key] where n.Week.Contains(Weeknum) select n).ToList());
                }
                //添加都没课的时间
                //day和lesson都没有在clist里出现过就添加一个classlistlight对象
                for (int i = 0; i < 7; i++)//一周
                {
                    for (int j = 0; j < 6; j++)//一天
                    {
                        //查时间有没有在集合里出现过
                        ClassListLight ourfreetime = new ClassListLight() { Hash_day = i, Hash_lesson = j };
                        if (!clist.Contains(ourfreetime, new ClassListLigthCompare()))
                        {
                            ourfreetime.Name = names;
                            clist.Add(ourfreetime);
                        }
                    }
                }
                //clist = clist.OrderBy(x => x.Hash_day).ToList();
                //筛选出该周内所有不在同一时间上课的课    
                var diisclist = from n in clist group n by new { n.Hash_day, n.Hash_lesson } into g where g.Count() < names.Length select g;
                var ll = diisclist.ToList();
                for (int i = 0; i < ll.Count; i++)
                {
                    ClassListLight tobeadded = ll[i].ToList()[0];
                    if (tobeadded.Name != names)
                    {
                        tobeadded.Name = names.Except(tobeadded.Name).ToArray();
                    }
                    result.Add(tobeadded);
                }
                //大家都没课的时间
            }
            else
            {
                //todo学期空课表





            }
        }
        private class ClassListLigthCompare : IEqualityComparer<ClassListLight>
        {
            public bool Equals(ClassListLight x, ClassListLight y)
            {
                if (x.Hash_day == y.Hash_day && x.Hash_lesson == y.Hash_lesson)
                {
                    return true;
                }
                else
                    return false;
            }

            public int GetHashCode(ClassListLight obj)
            {
                return 0;
            }
        }
    }
}
