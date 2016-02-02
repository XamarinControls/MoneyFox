﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoneyManager.Core.StatisticProvider;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Moq;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Core.Tests.StatisticProvider
{
    public class CashFlowProviderTests
    {
        [Fact]
        public void Constructor_Null_NotNullObject()
        {
            new CashFlowProvider(null).ShouldNotBeNull();
        }

        [Fact]
        public void GetValues_SetupData_ListWithoutTransfer()
        {
            //Setup
            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.SetupAllProperties();

            var paymentRepository = paymentRepoSetup.Object;
            paymentRepository.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Type = (int) PaymentType.Income,
                    Date = DateTime.Today,
                    Amount = 60
                },
                new Payment
                {
                    Id = 2,
                    Type = (int) PaymentType.Spending,
                    Date = DateTime.Today,
                    Amount = 50
                },
                new Payment
                {
                    Id = 3,
                    Type = (int) PaymentType.Transfer,
                    Date = DateTime.Today,
                    Amount = 40
                }
            });

            //Excution
            var result = new CashFlowProvider(paymentRepository).GetValues(DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(3));

            //Assertion
            result.Income.Value.ShouldBe(60);
            result.Spending.Value.ShouldBe(50);
            result.Revenue.Value.ShouldBe(10);
        }

        [Fact]
        public void GetValues_SetupData_CalculatedCorrectTimeRange()
        {
            //Setup
            var paymentRepositorySetup = new Mock<IPaymentRepository>();
            paymentRepositorySetup.SetupAllProperties();

            var paymentRepository = paymentRepositorySetup.Object;
            paymentRepository.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Type = (int) PaymentType.Spending,
                    Date = DateTime.Today,
                    Amount = 60
                },
                new Payment
                {
                    Id = 2,
                    Type = (int) PaymentType.Spending,
                    Date = DateTime.Today.AddDays(5),
                    Amount = 50
                },
                new Payment
                {
                    Id = 3,
                    Type = (int) PaymentType.Spending,
                    Date = DateTime.Today.AddDays(-5),
                    Amount = 40
                }
            });

            //Excution
            var result = new CashFlowProvider(paymentRepository).GetValues(DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(3));

            //Assertion
            result.Income.Value.ShouldBe(0);
            result.Spending.Value.ShouldBe(60);
            result.Revenue.Value.ShouldBe(-60);
        }

        [Fact]
        public void GetValues_NullDependency_NullReferenceException()
        {
            Assert.Throws<NullReferenceException>(
                () => new CashFlowProvider(null).GetValues(DateTime.Today, DateTime.Today));
        }
    }
}