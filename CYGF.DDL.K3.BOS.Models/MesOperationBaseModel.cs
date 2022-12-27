using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
	/// <summary>
	/// 提交MES数据基础类
	/// </summary>
	public class MesOperationBaseModel<T> where T : class
	{
		/// <summary>
		/// 执行的操作,一般为Execute
		/// </summary>
		public string operation { get; set; }
		/// <summary>
		/// 操作的对象，如CreateUser、ReleaseProductionOrder
		/// </summary>
		public string target { get; set; }
		/// <summary>
		/// 提交的数据,以json数组格式提交
		/// </summary>
		public List<T> json { get; set; }
		/// <summary>
		/// 执行操作的用户，用于日志记录
		/// </summary>
		public string user { get; set; }
	}
}
