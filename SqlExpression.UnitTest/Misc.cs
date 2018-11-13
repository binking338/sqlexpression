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
            Expression.DefaultOption.Column2ParamContractHandler = DbContext.ToUpperCamalCase;

            main?.Invoke();

            Expression.DefaultOption.Column2ParamContractHandler = null;
        }

    }
    public enum TestEnum
    {
        Item1 = 1,
        Item2 = 2,
    }
}
