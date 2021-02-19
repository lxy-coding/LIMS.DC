using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.DC.Common.LOG;
using LIMS.DC.DAL;
using LIMS.DC.Model;
namespace LIMS.DC.Service
{
    class RealObjectManager
    {
        public List<RealObject> RealObjects { get; set; } = new List<RealObject>();

        /// <summary>
        /// 数据访问对象
        /// </summary>
        DC_Service dC_Service = new DC_Service();

        /// <summary>
        /// 日志对象
        /// </summary>
        SystemLog log = new SystemLog("实时数据");

        /// <summary>
        /// 入口
        /// </summary>
        public void Request()
        {
            try
            {
                LoadConfig();//加载配置
            }
            catch (Exception ex)
            {
                log.WriteLog( E_ProcessLogType.Error,"加载配置异常。"+ex.Message);
                return;
            }

            //更新数据到业务表
            TaskHelper.RegisterTask(()=> {
                try
                {
                    List<DC_REAL_DATA> datas = dC_Service.GetRealDatas();
                    foreach (var obj in RealObjects)
                    {
                        try
                        {
                            obj.Request(datas);
                        }
                        catch (Exception ex)
                        {
                            log.WriteLog(E_ProcessLogType.Error, "更新数据异常。" + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.WriteLog(E_ProcessLogType.Error, "查询已更新实时数据异常。" + ex.Message);
                }
            });
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        private void LoadConfig()
        {
            var groupsByUser= BaseData.Configs.Where(s=>s.SUBSCRIPTION==1 && !string.IsNullOrEmpty(s.TABLE_USER) && !string.IsNullOrEmpty(s.TABLE_NAME)
            && !string.IsNullOrEmpty(s.IDENTITY_VALUE) && !string.IsNullOrEmpty(s.FIELD_NAME)).GroupBy(s=>s.TABLE_USER);
            foreach (var groupUser in groupsByUser)
            {
                string user = groupUser.Key;
                var groupsByTable= groupUser.GroupBy(s=>s.TABLE_NAME);
                foreach (var groupTable in groupsByTable)
                {
                    string table = groupTable.Key;
                    var groupsByValue= groupTable.GroupBy(s=>s.IDENTITY_VALUE);
                    foreach (var groupValue in groupsByValue)
                    {
                        string identity = groupValue.Key;
                        LoadConfig(user,table,identity, groupValue.ToList());
                    }
                }
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="obj"></param>
        private void LoadConfig(string user,string table,string identity,List<DC_DATA_CONFIG> configs)
        {
            RealObject realObject = new RealObject();
            realObject.User = user;
            realObject.Table = table;
            realObject.IdentityValue = identity;
            realObject.Configs = configs;
            RealObjects.Add(realObject);
        }
    }
}
