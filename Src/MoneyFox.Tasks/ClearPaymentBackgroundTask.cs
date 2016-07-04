﻿using Windows.ApplicationModel.Background;
using Microsoft.HockeyApp;
using MoneyFox.Shared;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using MoneyFox.Windows.Services;
using MoneyFox.Windows.Shortcuts;

namespace MoneyFox.Tasks {
    public sealed class ClearPaymentBackgroundTask : IBackgroundTask {
        private readonly PaymentManager paymentManager;

        public ClearPaymentBackgroundTask() {

#if !DEBUG
            HockeyClient.Current.Configure(ServiceConstants.HOCKEY_APP_WINDOWS_ID,
                new TelemetryConfiguration {EnableDiagnostics = true});
#endif

            HockeyClient.Current.TrackEvent("Ctror Background Task");

            var sqliteConnectionCreator = new DatabaseManager(new WindowsSqliteConnectionFactory(),
                new MvxWindowsCommonFileStore());

            var sqliteConnection = new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWindowsCommonFileStore())
                .GetConnection();

            var accountRepository = new AccountRepository(new AccountDataAccess(sqliteConnection));

            var notificationService = new NotificationService();
            paymentManager = new PaymentManager(
                new PaymentRepository(new PaymentDataAccess(sqliteConnectionCreator),
                    new RecurringPaymentDataAccess(sqliteConnectionCreator),
                    accountRepository,
                    new CategoryRepository(new CategoryDataAccess(sqliteConnection)),
                    notificationService),
                accountRepository,
                null);
        }

        public void Run(IBackgroundTaskInstance taskInstance) {
            HockeyClient.Current.TrackEvent("Run BackgroundTask");

            paymentManager.ClearPayments();
            Tile.UpdateMainTile();
        }
    }
}