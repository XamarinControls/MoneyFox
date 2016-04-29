﻿using MoneyFox.Shared;
using MoneyFox.Shared.Resources;
using System;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Windows.Converter
{
    public class PaymentHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            PaymentType pType = (PaymentType)value;
            switch (pType)
            {
                default:
                case PaymentType.Income:
                    return Strings.IncomeHeader;
                case PaymentType.Expense:
                    return Strings.ExpenseHeader;
                case PaymentType.Transfer:
                    return Strings.TransferHeader;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}