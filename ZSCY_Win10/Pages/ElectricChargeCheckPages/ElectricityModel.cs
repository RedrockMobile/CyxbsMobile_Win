using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    public class ElectricityByStuNum
    {
        public int status { get; set; }//网络状态
        public string version { get; set; }
        public string info { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string building { get; set; }//楼栋数
        public string room { get; set; }//房间号
        public Result result { get; set; }
    }

    public class Result
    {
        public Current current { get; set; }
        public Trend[] trend { get; set; }
    }

    public class Current
    {
        public string elec_spend { get; set; }//本月消耗电度数
        public string[] elec_cost { get; set; }//分别为本月电费消费整数和小数部分
        public string record_time { get; set; }//抄表时间
        public string elec_start { get; set; }//本月初表的度数
        public string elec_end { get; set; }//当前表的度数
        public string elec_free { get; set; }//每月免费额度
        public string elec_month { get; set; }//抄表月份

        private string _elec_perday = "";
        public string elec_perday//每日平均用量
        {
            get
            {
                return _elec_perday;
            }
            set
            {
                _elec_perday = Math.Round((double.Parse(value) / DateTime.UtcNow.Day), 2).ToString();
            }
        }

        private double _elec_percent;
        public double elec_percent//百分比
        {
            get
            {
                return _elec_percent;
            }
            set
            {
                _elec_percent = value;
            }
        }

        private string _elec_chargeBalance = "";
        public string elec_chargeBalance//电费余额
        {
            get
            {
                return _elec_chargeBalance;
            }
            set
            {
                _elec_chargeBalance = value;
            }
        }

        private string _elec_dumpEnergy = "";
        public string elec_dumpEnergy//电量剩余度数
        {
            get
            {
                return _elec_dumpEnergy;
            }
            set
            {
                _elec_dumpEnergy = value;
            }
        }
    }

    public class Trend
    {
        public string time { get; set; }
        public string spend { get; set; }
    }//半年内消耗电费的月份和度数


    public class ElectricityByRoomNum
    {
        public int status { get; set; }//网络状态
        public Elec_Inf elec_inf { get; set; }
    }

    public class Elec_Inf
    {
        public string elec_spend { get; set; }//本月消耗电度数
        public string[] elec_cost { get; set; }//分别为本月电费消费整数和小数部分
        public string record_time { get; set; }//抄表时间
        public string elec_start { get; set; }//本月初表的度数
        public string elec_end { get; set; }//当前表的度数
        public string elec_free { get; set; }//每月免费额度
        public string elec_month { get; set; }//抄表月份

        private string _elec_perday = "";
        public string elec_perday//每日平均用量
        {
            get
            {
                return _elec_perday;
            }
            set
            {
                _elec_perday = Math.Round((double.Parse(value) / DateTime.UtcNow.Day), 2).ToString();
            }
        }

        private double _elec_percent;
        public double elec_percent//百分比
        {
            get
            {
                return _elec_percent;
            }
            set
            {
                _elec_percent = value;
            }
        }

        private string _elec_chargeBalance = "";
        public string elec_chargeBalance//电费余额
        {
            get
            {
                return _elec_chargeBalance;
            }
            set
            {
                _elec_chargeBalance = value;
            }
        }

        private string _elec_dumpEnergy = "";
        public string elec_dumpEnergy//电量剩余度数
        {
            get
            {
                return _elec_dumpEnergy;
            }
            set
            {
                _elec_dumpEnergy = value;
            }
        }
    }
}
