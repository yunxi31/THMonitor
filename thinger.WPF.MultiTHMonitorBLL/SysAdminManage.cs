using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.WPF.MultiTHMonitorDAL;
using thinger.WPF.MultiTHMonitorModels.SQL;

namespace thinger.WPF.MultiTHMonitorBLL
{
    public class SysAdminManage
    {
        private SysAdminService sysAdminService = new SysAdminService();

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="sysAdmin"></param>
        /// <returns></returns>
        public SysAdmin AdminLogin(SysAdmin sysAdmin)
        {
            return sysAdminService.AdminLogin(sysAdmin);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sysAdmin"></param>
        /// <returns></returns>
        public int AddSysAdmin(SysAdmin sysAdmin)
        {
            return sysAdminService.AddSysAdmin(sysAdmin);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public int DeleteSysAdmin(int loginId)
        {
            return sysAdminService.DeleteSysAdmin(loginId);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="sysAdmin"></param>
        /// <returns></returns>
        public int ModifySysAdmin(SysAdmin sysAdmin)
        {
            return sysAdminService.ModifySysAdmin(sysAdmin);
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        public List<SysAdmin> QuerySysAdmins()
        {
            return sysAdminService.QuerySysAdmins();
        }
    }
}
