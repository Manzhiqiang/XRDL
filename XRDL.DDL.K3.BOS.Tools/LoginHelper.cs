using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingdee.BOS.Authentication;
using Newtonsoft.Json;
using Kingdee.BOS.ServiceFacade.KDServiceFx;
using Kingdee.BOS.WebApi.ServicesStub;
using XRDL.DDL.K3.BOS.Models;
using Kingdee.BOS;

namespace XRDL.DDL.K3.BOS.Tools
{
   public class LoginHelper
    {
        public static bool LogIn(KDServiceContext context, string DBId, string UserName, string AppId,string AppSecret, ref string Msg, ref Context Ctx)
        {
            bool result = false;
            try
            {
               
                AuthService Auth = new AuthService(context);
                LoginResult re = Auth.LoginByAppSecret(DBId, UserName, AppId, AppSecret);
                //LoginResult re = Auth.LoginByAppSecret("5f50632403c31c", "徐思强", "213152_T74pxcjH0vm5W80OX15KT+TKRh6cXAst", "4a4e463d39734e0893eaf30c46ef6b88");
                if (re.LoginResultType == LoginResultType.Success || re.LoginResultType == LoginResultType.DealWithForm)
                {
                    Ctx = re.Context;
                    result = true;
                }
                else
                {
                   Msg = JsonConvert.SerializeObject(new KDResult() { IsSuccess = false, Msg = "授权登陆失败："+re.Message });
                }
            }
            catch (Exception e)
            {
                Msg = JsonConvert.SerializeObject(new KDResult() { IsSuccess = false, Msg = "授权登陆验证异常：" + e.Message });
            }
            return result;
        }
    }
}
