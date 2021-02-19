using LIMS.DC.DAL;
using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    class ActionObject
    {
        public IPLCOperate Operator { get; set; }
        /// <summary>
        /// 天车id
        /// </summary>
        public int Cra_ID { get; set; }
        /// <summary>
        /// 数据访问对象
        /// </summary>
        DC_Service dC_Service = new DC_Service();
        /// <summary>
        /// 动作时间
        /// </summary>
        public DC_DATA_CONFIG ACTION_TIME { get; set; }
        /// <summary>
        /// X轴坐标
        /// </summary>
        public DC_DATA_CONFIG ACTION_X { get; set; }
        /// <summary>
        /// Y轴坐标
        /// </summary>
        public DC_DATA_CONFIG ACTION_Y { get; set; }
        /// <summary>
        /// 副Y轴坐标
        /// </summary>
        public DC_DATA_CONFIG ACTION_LITTLE_Y { get; set; }
        /// <summary>
        /// Z轴坐标
        /// </summary>
        public DC_DATA_CONFIG ACTION_Z { get; set; }
        /// <summary>
        /// 副Z轴坐标
        /// </summary>
        public DC_DATA_CONFIG ACTION_LITTLE_Z { get; set; }
        /// <summary>
        /// 主钩重量
        /// </summary>
        public DC_DATA_CONFIG ACTION_WEIGHT { get; set; }
        /// <summary>
        /// 副钩重量
        /// </summary>
        public DC_DATA_CONFIG ACTION_LITTLE_WEIGHT { get; set; }
        /// <summary>
        /// 动作类型
        /// </summary>
        public DC_DATA_CONFIG ACTION_SYMBOL { get; set; }
        /// <summary>
        /// 吊物数量
        /// </summary>
        public DC_DATA_CONFIG ACTION_OBJ_COUNT { get; set; }
        /// <summary>
        /// 缓存序号
        /// </summary>
        public DC_DATA_CONFIG ACTION_CACHE_NUM { get; set; }
        /// <summary>
        /// 已读序号
        /// </summary>
        public DC_DATA_CONFIG ACTION_READED_NUM { get; set; }

        public List<DataRow> Columns { get; set; }

        private bool IsComplete = true;

        public async void Request()
        {
            if (!IsComplete) return;
            await Task.Run(()=> {
                IsComplete = false;
                try
                {
                    bool res = IsGenerateAction();
                    if (res)
                    {
                        Thread.Sleep(1000);
                        object time = null, x =null, y = null, l_y = null, z = null, l_z = null, wgt = null, l_wgt = null, symbol = null, 
                        count = null, cNum = null;

                        time = ACTION_TIME == null ? null : Operator.Read(ACTION_TIME);
                        x = ACTION_X==null?null: Operator.Read(ACTION_X);
                        y = ACTION_Y == null ? null : Operator.Read(ACTION_Y);
                        l_y = ACTION_LITTLE_Y == null ? null : Operator.Read(ACTION_LITTLE_Y);
                        z = ACTION_Z == null ? null : Operator.Read(ACTION_Z);
                        l_z = ACTION_LITTLE_Z == null ? null : Operator.Read(ACTION_LITTLE_Z);
                        wgt = ACTION_WEIGHT == null ? null : Operator.Read(ACTION_WEIGHT);
                        l_wgt = ACTION_LITTLE_WEIGHT == null ? null : Operator.Read(ACTION_LITTLE_WEIGHT);
                        symbol = ACTION_SYMBOL == null ? null : Operator.Read(ACTION_SYMBOL);
                        count = ACTION_OBJ_COUNT == null ? null : Operator.Read(ACTION_OBJ_COUNT);
                        cNum = ACTION_CACHE_NUM == null ? null : Operator.Read(ACTION_CACHE_NUM);

                        x = ValidateValueRange(Columns.First(s=>s["COLUMN_NAME"].ToString()== "COORD_X"),x);
                        y = ValidateValueRange(Columns.First(s=>s["COLUMN_NAME"].ToString()== "COORD_Y"), y);
                        l_y = ValidateValueRange(Columns.First(s=>s["COLUMN_NAME"].ToString()== "LIITLT_HOOK_Y"), l_y);
                        z = ValidateValueRange(Columns.First(s=>s["COLUMN_NAME"].ToString()== "COORD_Z"), z);
                        l_z = ValidateValueRange(Columns.First(s=>s["COLUMN_NAME"].ToString()== "LITTLE_HOOK_Z"), l_z);
                        wgt = ValidateValueRange(Columns.First(s=>s["COLUMN_NAME"].ToString()== "WEIGHT"), wgt);
                        symbol = ValidateValueRange(Columns.First(s=>s["COLUMN_NAME"].ToString()== "ACTION_SYMBOL"), symbol);
                        count = ValidateValueRange(Columns.First(s=>s["COLUMN_NAME"].ToString()== "HANGE_QUAN"), count);
                        cNum = ValidateValueRange(Columns.First(s=>s["COLUMN_NAME"].ToString()== "CACHE_NUM"), cNum);

                        int result= dC_Service.InsertAction(Cra_ID, time, x, y, l_y, z, l_z, wgt, l_wgt, symbol, count, cNum);
                        if(result==1)
                        {
                            Operator.Write(ACTION_READED_NUM, cNum);
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    IsComplete = true;
                }
            });
        }

        private bool IsGenerateAction()
        {
            if(ACTION_CACHE_NUM==null || ACTION_READED_NUM==null)
            {
                throw new ArgumentNullException("缓存序号或已读序号为空。");
            }
            int cNum = Convert.ToInt32( Operator.Read(ACTION_CACHE_NUM));
            int rNum = Convert.ToInt32( Operator.Read(ACTION_READED_NUM));
            if(cNum!= rNum)
            {
                return true;
            }
            return false;
        }

        private object ValidateValueRange(DataRow row, object obj)
        {
            string type = row["DATA_TYPE"].ToString().ToUpper();
            switch (type)
            {
                case "NUMBER":
                    double nValue = Convert.ToDouble(obj);
                    double max = Math.Pow(10, (double)(Convert.ToInt32(row["DATA_PRECISION"]) - Convert.ToInt32(row["DATA_SCALE"])));
                    double min = -max;
                    if (nValue < max && nValue > min)
                    {
                        return obj;
                    }
                    break;
                case "VARCHAR2":
                    string strValue = obj.ToString();
                    if (strValue.Length < Convert.ToInt32(row["DATA_LENGTH"]) + 1)
                    {
                        return obj;
                    }
                    break;
            }
            return null;
        }
    }
}
