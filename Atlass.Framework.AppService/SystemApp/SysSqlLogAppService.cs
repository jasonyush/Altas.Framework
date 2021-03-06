﻿//------------------------------------------------------------------------------
// 作者：Generated by easycms
// 时间：2021/4/30 9:02:19
//------------------------------------------------------------------------------
using Atlass.Framework.Common;
using Atlass.Framework.Models;
using Atlass.Framework.ViewModels.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlass.Framework.AppService
{
    public class SysSqlLogAppService
    {
        private readonly IFreeSql Sqldb;
        public SysSqlLogAppService(IServiceProvider service)
        {
            Sqldb = service.GetRequiredService<IFreeSql>();
        }


        /// <summary>
        /// 数据列表
        ///</summary>
        public DataTableDto GetPageList(DataTableDto param,string tableName,DateTime stime)
        {

            var query = Sqldb.Select<sys_sql_log>()
                .Where(s=>s.excute_time>stime)
                .WhereIf(!tableName.IsEmpty(),s=>s.table_name==tableName)
                .OrderByDescending(s => s.excute_time)
                .Count(out long total)
                .Page(param.pageNumber, param.pageSize)
                .ToList();
            param.total = total;
            param.rows = query;
            return param;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="dto"></param>

        public void Save(sys_sql_log dto, LoginUserDto user)
        {

            if (dto.id == 0)
            {
                Sqldb.Insert(dto).ExecuteAffrows();
            }
            else
            {
                Sqldb.Update<sys_sql_log>().SetSource(dto).ExecuteAffrows();
            }

        }

        /// <summary>
        /// 获取sql执行日志
        ///</summary>
        public sys_sql_log GetModel(int id)
        {
            return Sqldb.Queryable<sys_sql_log>().Where(s => s.id == id).First();
        }

        /// <summary>
        /// 删除信息信息
        ///</summary>
        public void RemoveAll(string ids)
        {
            var idsArray = ids.SplitToArrayInt();
            Sqldb.Delete<sys_sql_log>().Where(s => idsArray.Contains(s.id)).ExecuteAffrows();
        }

        public void ClearTable()
        {
            Sqldb.Ado.ExecuteScalar("truncate table sys_sql_log");
        }
    }
}
