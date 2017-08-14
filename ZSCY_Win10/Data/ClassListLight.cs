using Newtonsoft.Json.Linq;
using System;

namespace ZSCY_Win10.Data
{
    internal class ClassListLight : EmptyClassDayLesson
    {
        public int Begin_lesson { get; set; }
        public int Period { get; set; } //2节连上，3节连上

        //public int[] LessonLast { get; set; }
        public int[] Week { get; set; } //上课的周次

        //public string Course { get; set; }
        public string[] Name { get; set; }

        public ClassListLight Clone()
        {
            ClassListLight cll = new ClassListLight();
            cll.Begin_lesson = this.Begin_lesson;
            cll.Period = this.Period;
            cll.Week = this.Week;
            //cll.Name = new string[this.Name.Length];
            cll.Hash_day = this.Hash_day;
            cll.Hash_lesson = this.Hash_lesson;
            //for (int i = 0; i < this.Name.Length; i++)
            //{
            //    cll.Name[i] = new string(this.Name[i].ToCharArray());//Name[i].ToString();
            //}
            return cll;
        }

        public ClassListLight getattribute(JObject classDetailJObject)
        {
            Hash_day = (int)classDetailJObject["hash_day"];
            Hash_lesson = (int)classDetailJObject["hash_lesson"];
            Begin_lesson = (int)classDetailJObject["begin_lesson"];
            Period = (int)classDetailJObject["period"];
            //int[] last = new int[Period];
            //for (int i = 0; i < Period; i++)
            //{
            //    last[i] = i + Begin_lesson;
            //}
            //LessonLast = last;
            var gradelimit = JArray.Parse(classDetailJObject["week"].ToString());
            int[] temp = new int[gradelimit.Count];
            for (int i = 0; i < gradelimit.Count; ++i)
            {
                temp[i] = Int32.Parse(gradelimit[i].ToString());
            }
            Week = temp;
            //Course = classDetailJObject["course"].ToString();
            if (Period == 3)
            {
                return new ClassListLight { Hash_day = this.Hash_day, Hash_lesson = this.Hash_lesson + 1, Begin_lesson = this.Begin_lesson, Period = 2, Week = this.Week, Name = this.Name };
            }
            else
                return null;
        }
    }

    public class People
    {
        public string name { get; set; }
        public string weekstostr { get; set; }
    }
}