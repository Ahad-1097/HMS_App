using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using App.Models.DtoModel;

namespace App.Factory.Helper
{
    public class MyDataTable
    {
        public static DataTable tempTable = new DataTable();
        public static  int tempId = 1;
        static MyDataTable()
        {

            //data table 
            tempTable.Columns.Add("TempId", typeof(int));
            tempTable.Columns.Add("Id", typeof(int));
            tempTable.Columns.Add("PatientId", typeof(int));
            tempTable.Columns.Add("CBC", typeof(string));
            tempTable.Columns.Add("RFT", typeof(string));
            tempTable.Columns.Add("BloodSugar", typeof(string));
            tempTable.Columns.Add("SerumElectrolytes", typeof(string));
            tempTable.Columns.Add("LipidProfile", typeof(string));
            tempTable.Columns.Add("USG", typeof(string));
            tempTable.Columns.Add("CECT", typeof(string));
            tempTable.Columns.Add("MRI", typeof(string));
            tempTable.Columns.Add("FNAC", typeof(string));
            tempTable.Columns.Add("TrucutBiopsy", typeof(string));
            tempTable.Columns.Add("ReceptorStatus", typeof(string));
            tempTable.Columns.Add("MRCP", typeof(string));
            tempTable.Columns.Add("ERCP", typeof(string));
            tempTable.Columns.Add("EndoscopyUpperGI", typeof(string));
            tempTable.Columns.Add("EndoscopyLowerGI", typeof(string));
            tempTable.Columns.Add("PETCT", typeof(string));
            tempTable.Columns.Add("TumorMarkers", typeof(string));
            tempTable.Columns.Add("SONOMMOGRAPHY", typeof(string));
            tempTable.Columns.Add("LFT", typeof(string));
            tempTable.Columns.Add("TSPAG", typeof(string));
            tempTable.Columns.Add("TFT", typeof(string));
            tempTable.Columns.Add("OtherO", typeof(string));
            tempTable.Columns.Add("OtherT", typeof(string));
            tempTable.Columns.Add("OtherTh", typeof(string));
            tempTable.Columns.Add("Day", typeof(string));

            tempTable.PrimaryKey = new DataColumn[] { tempTable.Columns["TempId"] };
        }
        public DataTable GetTempTable(List<InvestigationModel> models)
        {
            
            foreach (var item in models)
            {
             
                //tempTable.Rows.Add(tempId,item.Id,item.PatientId, item.CBC, item.RFT, item.BloodSugar, item.SerumElectrolytes, item.LipidProfile, item.USG, item.CECT, item.MRI,
                //    item.FNAC, item.TrucutBiopsy,item.ReceptorStatus, item.MRCP, item.ERCP, item.EndoscopyUpperGI, item.EndoscopyLowerGI, item.PETCT, item.TumorMarkers, item.SONOMMOGRAPHY,
                //    item.LFT, item.TSPAG, item.TFT, item.OtherO, item.OtherT, item.OtherTh,item.Day);

                tempId++;
            }

            return tempTable;
        }

        
    }

}
