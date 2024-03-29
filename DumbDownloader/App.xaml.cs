﻿using DumbDownloader.Models;
using DumbDownloader.ViewModels;
using DumbStockAPIService.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace DumbDownloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DbContextFactory? _dbContextFactory;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 데이터베이스 초기화
            _dbContextFactory = new DbContextFactory(DumbDownloader.Properties.Settings.Default.db_connection);
            using(MyDBContext? dBContext = _dbContextFactory.CreateDbContext())
            {
                dBContext?.Database.Migrate();
            }

            // MainWindow, MainViewModel 설정
            MainWindow window = new MainWindow();
            var viewModel = new MainViewModel("MainViewModel", _dbContextFactory);

            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler? handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                window.Close();
            };
            viewModel.RequestClose += handler;

            window.DataContext = viewModel;

            window.Show();
        }
    }
}
