using System;
using System.Collections.Generic;

namespace GTA.PI.Models
{
    #region 保存&选股

    public class StockConditionVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InputConditionVM InputCondition { get; set; }
    }

    public class InputConditionVM
    {
        public List<IndexRangeConditionVM> IndexRangeConditions { get; set; }
        public List<CustomIndexCondition> CustomIndexConditions { get; set; }
    }

    public class IndexRangeConditionVM
    {
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string SelectTerm { get; set; }
        public string SelectTermDisplay { get; set; }
        public List<SelectTermList> SelectTermList { get; set; }
        public List<double> Value { get; set; }
        public List<SelectTermList> SelectLeftList { get; set; }
        public string SelectLeft { get; set; }
        public List<SelectTermList> SelectRightList { get; set; }
        public string SelectRightDisplay { get; set; }
        public string SelectRight { get; set; }

        /// <summary>
        /// 参数
        /// (技术指标用)
        /// </summary>
        public List<InputParams> ParamsValues { get; set; }
    }

    #endregion

    public class StockSolutionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 选股条件
    /// </summary>
    public class StockConditionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InputCondition InputCondition { get; set; }
    }

    public class InputCondition
    {
        public List<IndexRangeCondition> IndexRangeConditions { get; set; }
        public List<CustomIndexCondition> CustomIndexConditions { get; set; }
    }

    public class IndexRangeCondition
    {
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        //public string Desc { get; set; }
        public string SelectTerm { get; set; }
        public string SelectTermDisplay { get; set; }
        //public List<SelectTermList> SelectTermList { get; set; }
        //public List<double> Value { get; set; }
        //public List<SelectTermList> SelectLeftList { get; set; }
        public string SelectLeft { get; set; }
        //public List<SelectTermList> SelectRightList { get; set; }
        public string SelectRightDisplay { get; set; }
        public string SelectRight { get; set; }

        /// <summary>
        /// 参数
        /// (技术指标用)
        /// </summary>
        public List<InputParams> ParamsValues { get; set; }
    }

    public class CustomIndexCondition
    {
        public CustomIndex first { get; set; }
        public double? firstInputVal { get; set; }
        public string operator1 { get; set; }
        public CustomIndex second { get; set; }
        public double? secondInputVal { get; set; }
        public string operator2 { get; set; }
        public CustomIndex third { get; set; }
        public double? thirdInputVal { get; set; }
        public string operator1Name { get; set; }
        public string operator2Name { get; set; }
        public string isFirstRo { get; set; }
        public string isSecondRo { get; set; }
        public string isThirdRo { get; set; }
    }

    public class SelectTermList
    {
        public string SelectItem { get; set; }
        public string SelectDisplay { get; set; }
    }

    public class CustomIndex
    {
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        //public string Desc { get; set; }
        public string SelectTerm { get; set; }
        public string SelectTermDisplay { get; set; }
        //public List<SelectTermList> SelectTermList { get; set; }
        //public List<double> Value { get; set; }
        //public List<SelectTermList> SelectLeftList { get; set; }
        public string SelectLeft { get; set; }
        //public List<SelectTermList> SelectRightList { get; set; }
        public string SelectRightDisplay { get; set; }
        public string SelectRight { get; set; }

        /// <summary>
        /// 参数
        /// (技术指标用)
        /// </summary>
        public List<InputParams> ParamsValues { get; set; }
    }

    //public class Second
    //{
    //    public double? MinValue { get; set; }
    //    public double? MaxValue { get; set; }
    //    public string Code { get; set; }
    //    public string Name { get; set; }
    //    public string Desc { get; set; }
    //    public string SelectTerm { get; set; }
    //    public string SelectTermDisplay { get; set; }
    //    public List<SelectTermList> SelectTermList { get; set; }
    //    public List<double> Value { get; set; }
    //    public string SelectLeftList { get; set; }
    //    public string SelectLeft { get; set; }
    //    public string SelectRightList { get; set; }
    //    public string SelectRightDisplay { get; set; }
    //    public string SelectRight { get; set; }
    //    public string ParamsValues { get; set; }
    //}

    //public class Third
    //{
    //    public double? MinValue { get; set; }
    //    public double? MaxValue { get; set; }
    //    public string Code { get; set; }
    //    public string Name { get; set; }
    //    public string Desc { get; set; }
    //    public string SelectTerm { get; set; }
    //    public string SelectTermDisplay { get; set; }
    //    public List<SelectTermList> SelectTermList { get; set; }
    //    public List<double> Value { get; set; }
    //    public string SelectLeftList { get; set; }
    //    public string SelectLeft { get; set; }
    //    public string SelectRightList { get; set; }
    //    public string SelectRightDisplay { get; set; }
    //    public string SelectRight { get; set; }
    //    public string ParamsValues { get; set; }
    //}
}
