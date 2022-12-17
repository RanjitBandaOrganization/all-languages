// This file was auto-generated by ML.NET Model Builder. 

using Microsoft.ML.Data;

namespace RestaurantViolationsML.Model
{
    public class ModelInput
    {
        [ColumnName("InspectionType"), LoadColumn(0)]
        public string InspectionType { get; set; }


        [ColumnName("ViolationDescription"), LoadColumn(1)]
        public string ViolationDescription { get; set; }


        [ColumnName("RiskCategory"), LoadColumn(2)]
        public string RiskCategory { get; set; }


    }
}
