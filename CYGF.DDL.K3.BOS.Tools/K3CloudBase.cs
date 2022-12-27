using CYSD.DDL.K3.BOS.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYSD.DDL.K3.BOS.Tools
{
    public static class K3CloudBase
    {
        /// <summary>
        /// 云星空API转化消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static K3BaseMessageModel ExtConvertK3BaseModel(this string data)
        {
            var model = new K3BaseMessageModel();
            var tdata = JObject.Parse(Newtonsoft.Json.JsonConvert.DeserializeObject(data).ToString())["Result"]["ResponseStatus"];
            var success = Convert.ToBoolean(tdata["IsSuccess"].ToString());
            if (success)
            {
                model.FID = Convert.ToInt64(tdata["SuccessEntitys"][0]["Id"].ToString());
                model.FNumber = tdata["SuccessEntitys"][0]["Number"].ToString();
                model.Msg = string.Format("成功。FID:{0},FNumber:{1}", model.FID, model.FNumber);
            }
            else
            {
                var msg = string.Empty;
                var errorArr = tdata["Errors"];
                for (var a = 0; a < errorArr.Count(); a++)
                {
                    msg += tdata["Errors"][a]["Message"].ToString() + ",";
                }

                // var msg = tdata["Errors"][0]["Message"].ToString();
                model.Msg = msg;
            }
            model.isSuccess = success;
            return model;
        }
    }
}
