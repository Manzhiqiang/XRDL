using Kingdee.BOS;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.Const;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.Operation;
using Kingdee.BOS.Core.Interaction;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Metadata.ConvertElement;
using Kingdee.BOS.Core.Metadata.ConvertElement.ServiceArgs;
using Kingdee.BOS.Orm;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.K3.MFG.App;
using CYSD.DDL.K3.BOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CYSD.DDL.K3.BOS.Tools
{
    public class BillConvertHelper
    {
        /// <summary>
        /// 后台调用单据转换生成目标单
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public IEnumerable<DynamicObject> ConvertBills(Context ctx, ConvertOption option, IOperationResult pushResult = null)
        {
            List<DynamicObject> list = new List<DynamicObject>();
            IConvertService convertService = AppServiceContext.ConvertService;
            ConvertRuleElement rule = convertService.GetConvertRules(ctx, option.SourceFormId, option.TargetFormId)
                .FirstOrDefault(w => w.Id == option.ConvertRuleKey);

            FormMetadata metaData = (FormMetadata)AppServiceContext.MetadataService.Load(ctx, option.TargetFormId, true);
            if ((rule != null) && option.BizSelectRows != null && option.BizSelectRows.Count() > 0)
            {
                PushArgs serviceArgs = new PushArgs(rule, option.BizSelectRows);
                if (!option.SourceBillTypeId.IsNullOrEmptyOrWhiteSpace())
                {
                    //根据单据转换的单据类型的映射配置，根据源单的单据类型 获取目标单的单据类型
                    option.TargetBillTypeId = GetTargetBillTypeIdByBillTypeMap(rule, option.SourceBillTypeId);
                }

                if (option.TargetBillTypeId != null && option.TargetBillTypeId.Trim() != "")
                {
                    serviceArgs.TargetBillTypeId = option.TargetBillTypeId; // 请设定目标单据单据类型，内码ID。如无单据类型，可以空字符
                }
                if (option.TargetOrgId > 0)
                {
                    serviceArgs.TargetOrgId = Convert.ToInt64(option.TargetOrgId); // 请设定目标单据主业务组织。如无主业务组织，可以为0
                }
                serviceArgs.CustomParams.Add("SA_CustomConvertOption", option);//可以传递额外附加的参数给单据转换插件，如无此需求，可以忽略
                OperateOption operateOption = OperateOption.Create();
                operateOption.SetVariableValue("ValidatePermission", true);
                operateOption.SetVariableValue("autoPush", true);
                operateOption.SetVariableValue("IsAuto", true);
                operateOption.SetIgnoreWarning(true);
                operateOption.SetIgnoreInteractionFlag(true);
                operateOption.SetVariableValue(ConvertConst.SelectByBillId, false);
                operateOption.SetVariableValue(ConvertConst.SpecPush, true);
                ConvertOperationResult convertOperationResult = convertService.Push(ctx, serviceArgs, operateOption);
                if (!convertOperationResult.IsSuccess)
                {
                    pushResult = convertOperationResult as IOperationResult;
                    return null;
                }
                DynamicObject[] collection = convertOperationResult.TargetDataEntities
                    .Select(s => s.DataEntity).ToArray();
                if (collection != null)
                {
                    list.AddRange(collection);
                }
            }
            if (list.Count > 0)
            {
                try
                {
                    AppServiceContext.DBService.LoadReferenceObject(ctx, list.ToArray(), metaData.BusinessInfo.GetDynamicObjectType(), false);
                }
                catch
                {

                }

            }
            return list;
        }

        /// <summary>
        /// 后台调用单据转换生成目标单
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public IOperationResult ConvertBills2(Context ctx, ConvertOption option)
        {
            IOperationResult result = new OperationResult();
            List<DynamicObject> list = new List<DynamicObject>();
            IConvertService convertService = AppServiceContext.ConvertService;
            ConvertRuleElement rule = convertService.GetConvertRules(ctx, option.SourceFormId, option.TargetFormId)
               .FirstOrDefault(w => w.Id == option.ConvertRuleKey);
            FormMetadata metaData = (FormMetadata)AppServiceContext.MetadataService.Load(ctx, option.TargetFormId, true);
            if ((rule != null) && !option.BizSelectRows.IsEmpty())
            {
                PushArgs serviceArgs = new PushArgs(rule, option.BizSelectRows);
                if (!option.SourceBillTypeId.IsNullOrEmptyOrWhiteSpace())
                {
                    //根据单据转换的单据类型的映射配置，根据源单的单据类型 获取目标单的单据类型
                    option.TargetBillTypeId = GetTargetBillTypeIdByBillTypeMap(rule, option.SourceBillTypeId);
                }

                if (!option.TargetBillTypeId.IsNullOrEmptyOrWhiteSpace())
                {
                    serviceArgs.TargetBillTypeId = option.TargetBillTypeId;
                }
                if (option.TargetOrgId > 0)
                {
                    serviceArgs.TargetOrgId = Convert.ToInt64(option.TargetOrgId);
                }
                serviceArgs.CustomParams.Add("SA_CustomConvertOption", option);
                OperateOption operateOption = OperateOption.Create();
                operateOption.SetVariableValue("ValidatePermission", true);
                operateOption.SetVariableValue("IsAuto", true);
                ConvertOperationResult convertOperationResult = convertService.Push(ctx, serviceArgs, operateOption);
                if (!convertOperationResult.IsSuccess)
                {
                    result = convertOperationResult as IOperationResult;
                    return result;
                }
                DynamicObject[] collection = convertOperationResult.TargetDataEntities
                    .Select(s => s.DataEntity).ToArray();
                list.AddRange(collection);
            }
            if (list.Count > 0)
            {
                AppServiceContext.DBService.LoadReferenceObject(ctx, list.ToArray(), metaData.BusinessInfo.GetDynamicObjectType(), false);
            }

            if (option.IsDraft)
            {
                result = Kingdee.BOS.Contracts.ServiceFactory.GetService<IDraftService>(ctx).Draft(ctx, metaData.BusinessInfo, list.ToArray());
            }

            if (!result.IsSuccess)
                return result;

            if (option.IsSave)
            {
                result = AppServiceContext.SaveService.Save(ctx, metaData.BusinessInfo, list.ToArray());
            }

            if (!result.IsSuccess)
                return result;
            if (option.IsSubmit || option.IsAudit)
            {
                result = AppServiceContext.SubmitService.Submit(ctx, metaData.BusinessInfo,
                                   list.Select(item => ((Object)(Convert.ToInt64(item["Id"])))).ToArray(), "Submit");
            }
            if (!result.IsSuccess)
                return result;
            if (option.IsAudit)
            {
                List<KeyValuePair<object, object>> keyValuePairs = new List<KeyValuePair<object, object>>();
                list.ForEach(item =>
                {
                    keyValuePairs.Add(new KeyValuePair<object, object>(item.GetPrimaryKeyValue(), item));
                }
                );
                List<object> auditObjs = new List<object>();
                auditObjs.Add("1");
                auditObjs.Add("");
                OperateOption saveOption = OperateOption.Create();
                saveOption.SetIgnoreInteractionFlag(true);
                saveOption.AddInteractionFlag(Kingdee.K3.Core.SCM.SCMConst.MinusCheckSensor);
                result = AppServiceContext.SetStatusService.SetBillStatus(ctx, metaData.BusinessInfo,
                   keyValuePairs, auditObjs, "Audit", saveOption);

                if (!result.IsSuccess)
                    return result;
            }
            return result;
        }

        private string GetTargetBillTypeIdByBillTypeMap(ConvertRuleElement convertRuleElement, string sourceBillTypeId)
        {

            string result = string.Empty;
            ConvertPolicyElement val = ((IEnumerable<ConvertPolicyElement>)convertRuleElement.Policies).FirstOrDefault<ConvertPolicyElement>((Func<ConvertPolicyElement, bool>)((ConvertPolicyElement w) => w.ConvertPolicyTypeName == "Kingdee.BOS.App.Core.Convertible.BillTypeMapPolicy,Kingdee.BOS.App.Core"));
            Collection<BillTypeMapElement> billTypeMaps = (val as BillTypeMapPolicyElement).BillTypeMaps;
            if (!ListUtils.IsEmpty<BillTypeMapElement>((IEnumerable<BillTypeMapElement>)billTypeMaps))
            {
                BillTypeMapElement val2 = ((IEnumerable<BillTypeMapElement>)billTypeMaps).FirstOrDefault<BillTypeMapElement>((Func<BillTypeMapElement, bool>)((BillTypeMapElement f) => f.SourceBillTypeId == sourceBillTypeId));
                if (val2 != null)
                {
                    result = val2.TargetBillTypeId;
                }
                else
                {
                    val2 = ((IEnumerable<BillTypeMapElement>)billTypeMaps).FirstOrDefault<BillTypeMapElement>((Func<BillTypeMapElement, bool>)((BillTypeMapElement f) => f.SourceBillTypeId == "(All)"));
                    if (val2 != null)
                    {
                        result = val2.TargetBillTypeId;
                    }
                }
            }
            return result;
        }
    }
       
}
