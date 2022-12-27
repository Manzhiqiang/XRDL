using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XRDL.DDL.K3.BOS.Tools
{
    public class WriteLog
    {

        /// <summary>
        /// 创建对象
        /// </summary>
        private static object objLock = new object();
        private static object wmsobjLock = new object();
        public static void WriteLogAndTime(string Title,string Msg, string Path = @"ErpLog\Log")
        {
            //锁定县城
            lock (objLock)
            {
                //创建写入流
                StreamWriter sw = null;
                //监控代码段
                try
                {
                    //组装路径
                    Path = AppDomain.CurrentDomain.BaseDirectory + Path;
                    //判断文件是否存在
                    if (!Directory.Exists(Path))
                    {
                        //根据路径创建文件
                        Directory.CreateDirectory(Path);
                    }
                    //设置写入文件路径
                    sw = new StreamWriter(Path + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd-HH") + "Log.txt", true);
                    //写入记录信息                                                                                                                                                  
                    sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]------") + "【" + Title + "】" + Msg);
                }
                catch (Exception ex)
                {
                    //抛出异常信息
                    throw new Exception("写日志失败" + ex.Message + ",安装地址为：" + AppDomain.CurrentDomain.BaseDirectory);
                }
                finally
                {
                    //判断写入流非空
                    if (sw != null)
                    {
                        //释放资源
                        sw.Close();
                    }
                }
            }
        }

        public static void WriteWmsLog(string Title,string Msg, string Path = @"ErpLog\WmsLog")
        {
            //锁定县城
            lock (wmsobjLock)
            {
                //创建写入流
                StreamWriter sw = null;
                //监控代码段
                try
                {
                    //组装路径
                    Path = AppDomain.CurrentDomain.BaseDirectory + Path;
                    //判断文件是否存在
                    if (!Directory.Exists(Path))
                    {
                        //根据路径创建文件
                        Directory.CreateDirectory(Path);
                    }
                    //设置写入文件路径
                    sw = new StreamWriter(Path + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd-HH") + "Log.txt", true);
                    //写入记录信息
                    sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]------") + "【"+Title+"】"+ Msg);
                }
                catch (Exception ex)
                {
                    //抛出异常信息
                    throw new Exception("写日志失败" + ex.Message + ",安装地址为：" + AppDomain.CurrentDomain.BaseDirectory);
                }
                finally
                {
                    //判断写入流非空
                    if (sw != null)
                    {
                        //释放资源
                        sw.Close();
                    }
                }
            }
        }
    }
}
