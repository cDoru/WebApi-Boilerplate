﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.Freight
{
    [Route("/freight/rcvy1", "Get")]
    [Route("/freight/rcvy1/{PortOfDischargeName}", "Get")]
    [Route("/freight/rcvy1/sps/{PortOfDischargeName}", "Get")]
    public class List_Rcvy1 : IReturn<CommonResponse>
    {
        public string PortOfDischargeName { get; set; }
    }
    public class List_Rcvy1_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public HashSet<string> GetList(List_Rcvy1 request)
        {
            HashSet<string> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (!string.IsNullOrEmpty(request.PortOfDischargeName))
                    {
                        Result = db.HashSet<string>(
								"Select PortOfDischargeName from rcvy1 where PortOfDischargeName is not null and PortOfDischargeName <> '' and PortOfDischargeName LIKE '" + request.PortOfDischargeName + "%' Order By PortOfDischargeName ASC"
                        );
                    }
                    else
                    {
                        Result = db.HashSet<string>(
								"Select PortOfDischargeName from rcvy1 where PortOfDischargeName is not null and PortOfDischargeName<>'' Order By PortofDischargeName ASC"
                        );
                    }
                }
            }
            catch { throw; }
            return Result;
        }
        public List<Rcvy1_sps> GetSpsList(List_Rcvy1 request)
        {
            List<Rcvy1_sps> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
						string strSQL = "SELECT VoyageID,VoyageNo,VesselCode,CloseDateTime,ETD,ETA,datediff(D,ETD,ETA) TranSit,PortofDischargeName," +
						"(select top 1 ShippinglineName from rcsl1 where shippinglinecode=rcvy1.shippinglinecode)  ShippinglineName " +
						"FROM rcvy1 Where StatusCode='USE' And ETD >= Convert(varchar(12),getdate(),112) Order By UpdateDateTime Desc";
						Result = db.Select<Rcvy1_sps>(strSQL);
					//string strSQL = "EXEC sps_Track_Rcvy1_List @intUserId=0,@PageSize=0,@PageCount=0,@RecordCount=0,@strWhere='And PortofDischargeName=''" + request.PortOfDischargeName + "'''";
                    //Result = db.SqlList<Rcvy1_sps>(strSQL);
                }
            }
            catch { throw; }
            return Result;
        }
    }
}