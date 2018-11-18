using System;
using System.Collections.Generic;
using System.Text;

namespace SqlExpression.UnitTest
{
    public class Misc
    {
        public static void UsingQuotationMark(Action main, string open="`", string close="`")
        {
            Expression.DefaultOption.OpenQuotationMark = open;
            Expression.DefaultOption.CloseQuotationMark = close;

            main?.Invoke();

            Expression.DefaultOption.OpenQuotationMark = string.Empty;
            Expression.DefaultOption.CloseQuotationMark = string.Empty;
        }

        public static void UsingParamName2UpperCamalCase(Action main)
        {
            Expression.DefaultOption.Column2ParamContractHandler = SqlExpression.Extension.DapperRepository.DbContext.ToUpperCamalCase;

            main?.Invoke();

            Expression.DefaultOption.Column2ParamContractHandler = null;
        }

        public static void UsingParamNameAsColumnName(Action main)
        {
            Expression.DefaultOption.Column2ParamContractHandler = null;

            main?.Invoke();
        }

        public static void UsingParamMark(Action main, string mark = "@")
        {
            Expression.DefaultOption.ParamMark = mark;

            main?.Invoke();

            Expression.DefaultOption.ParamMark = "@";
        }

    }
    public enum TestEnum
    {
        Item1 = 1,
        Item2 = 2,
    }
}
