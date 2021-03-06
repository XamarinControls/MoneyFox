﻿using System;
using System.Linq;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;

namespace MoneyFox.BusinessDbAccess.QueryObjects
{
    /// <summary>
    ///     Provides Extensions for payments queries.
    /// </summary>
    public static class PaymentQueryObjects
    {
        /// <summary>
        ///     Adds a filter to a query for cleared payments
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for cleared payments.</returns>
        public static IQueryable<Payment> AreCleared(this IQueryable<Payment> query)
        {
            return query.Where(payment => payment.IsCleared);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who are not cleared
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for not cleared payments.</returns>
        public static IQueryable<Payment> AreNotCleared(this IQueryable<Payment> query)
        {
            return query.Where(payment => !payment.IsCleared);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a date larger or equals to the passed date.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="date">Date to filter for.</param>
        /// <returns>Query filtered for not cleared payments.</returns>
        public static IQueryable<Payment> HasDateLargerEqualsThan(this IQueryable<Payment> query, DateTime date)
        {
            return query.Where(payment => payment.Date.Date >= date.Date);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a date smaller or equals to the passed date.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="date">Date to filter for.</param>
        /// <returns>Query filtered for the date.</returns>
        public static IQueryable<Payment> HasDateSmallerEqualsThan(this IQueryable<Payment> query, DateTime date)
        {
            return query.Where(payment => payment.Date.Date <= date.Date);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who are not Transfers
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for payments who are not transfers.</returns>
        public static IQueryable<Payment> WithoutTransfers(this IQueryable<Payment> query)
        {
            return query.Where(payment => payment.Type != PaymentType.Transfer);
        }
    }
}
