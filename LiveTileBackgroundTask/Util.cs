using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace LiveTileBackgroundTask
{
    class Util
    {
        public static void UpdateTile(List<ClassList> tempList, int nowWeek, string weekDay)
        {
            //通过这个方法，我们就可以为动态磁贴的添加做基础
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            //这里设置的是所以磁贴都可以为动态
            updater.EnableNotificationQueue(true);
            updater.Clear();
            int itemCount = 0;
            for (int i = 0; i < tempList.Count; i++)
            {
                string course = tempList[i].Course.ToString();
                string teacher = tempList[i].Teacher.ToString();
                string lesson = tempList[i].Lesson.ToString() + "  ";
                string classroom = tempList[i].Classroom.ToString();
                int[] weeks = tempList[i].Week;
                for (int j = 0; j < weeks.Length; j++)//判断课程是否为本周课程
                {
                    if (weeks[j] == nowWeek && tempList[i].Day.Equals(weekDay))
                    {
                        //1：创建XML对象_宽磁贴
                        string xmlWide = "<tile>" +
                                "<visual>" +
                                    "<binding template=\"TileWide\">" +
                                        "<text hint-style=\"subtitle\"></text>" +
                                        "<text></text>" +
                                        "<text></text>" +
                                    "</binding>" +
                                "</visual>" +
                             "</tile>";
                        //1：创建XML对象_中磁贴
                        string xmlMedium = "<tile>" +
                                "<visual>" +
                                    "<binding template=\"TileMedium\">" +
                                        "<text hint-wrap=\"true\"></text>" +
                                        "<text></text>" +
                                        "<text hint-wrap=\"true\"></text>" +
                                    "</binding>" +
                                "</visual>" +
                             "</tile>";
                        //2.接着给这个XML对象赋值
                        XmlDocument docWide = new XmlDocument();
                        docWide.LoadXml(xmlWide);
                        XmlNodeList elementsWide = docWide.GetElementsByTagName("text");
                        elementsWide[0].AppendChild(docWide.CreateTextNode(course));
                        elementsWide[1].AppendChild(docWide.CreateTextNode(teacher));
                        elementsWide[2].AppendChild(docWide.CreateTextNode(lesson));
                        elementsWide[2].AppendChild(docWide.CreateTextNode(classroom));

                        XmlDocument docMedium = new XmlDocument();
                        docMedium.LoadXml(xmlMedium);
                        XmlNodeList elementsMedium = docMedium.GetElementsByTagName("text");
                        elementsMedium[0].AppendChild(docMedium.CreateTextNode(course));
                        elementsMedium[1].AppendChild(docMedium.CreateTextNode(teacher));
                        elementsMedium[2].AppendChild(docMedium.CreateTextNode(lesson));
                        elementsMedium[2].AppendChild(docMedium.CreateTextNode(classroom));

                        //3.然后用Update方法来更新这个磁贴
                        updater.Update(new TileNotification(docWide));
                        updater.Update(new TileNotification(docMedium));
                        //4.最后这里需要注意的是微软规定动态磁贴的队列数目小于5个，所以这里做出判断
                        if (itemCount++ > 5)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
