#region Copyright Syncfusion Inc. 2001 - 2019
// <copyright file="ViewModel.cs" company="syncfusion">
//  Copyright (c) Syncfusion Inc. 2001 - 2019. All rights reserved.
//  Use of this code is subject to the terms of our license.
//  A copy of the current license can be obtained at any time by e-mailing
//  licensing@syncfusion.com. Re-distribution in any form is strictly
//  prohibited. Any infringement will be prosecuted under applicable laws. 
// </copyright> 
#endregion

namespace KPI.ViewModel
{
    using System;
    using Syncfusion.Olap.Manager;
    using Syncfusion.Olap.Reports;
    using Syncfusion.Windows.Shared;
    using System.Windows;

    /// <summary>
    /// ViewModel Class
    /// </summary>
    public class ViewModel : NotificationObject, IDisposable
    {
        #region Members

        /// <summary>
        /// Shared connection string.
        /// </summary>
        public static string ConnectionString;
        private OlapDataManager olapDataManager;
        private Visibility showLegend = Visibility.Visible;
        private DelegateCommand<object> legendDelegateCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            olapDataManager = new OlapDataManager(ConnectionString);
            olapDataManager.SetCurrentReport(SimpleDimensions());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the grid data manager.
        /// </summary>
        /// <value>The grid data manager.</value>
        public OlapDataManager DataManager
        {
            get { return olapDataManager; }
            set { olapDataManager = value; }
        }

        /// <summary>
        /// Gets or sets the legend visibility.
        /// </summary>
        /// <value>The legend visibility.</value>
        public Visibility ShowLegend
        {
            get { return showLegend; }
            set { showLegend = value; RaisePropertyChanged("ShowLegend"); }
        }
        
        /// <summary>
        /// Gets or sets the delegate command to show legend.
        /// </summary>
        /// <value>The delegate command to show legend.</value>
        public DelegateCommand<object> LegendDelegateCommand
        {
            get
            {
                return legendDelegateCommand ?? new DelegateCommand<object>(LegendOption);
            }
            set
            {
                legendDelegateCommand = value;
            }
        }

        #endregion

        #region Helper Methods

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && this.olapDataManager != null)
                this.olapDataManager.Dispose();
        }

        private OlapReport SimpleDimensions()
        {
            OlapReport olapReport = new OlapReport();
            olapReport.CurrentCubeName = "Adventure Works";
            //Getting KPI elements
            KpiElements kpiElement = new KpiElements();
            //Customizing KPI elements properties
            kpiElement.Elements.Add(new KpiElement { Name = "Revenue", ShowKPIGoal = true, ShowKPIStatus = true, ShowKPIValue = true, ShowKPITrend = true });

            //Specifying the Row Name for the Dimension element
            DimensionElement dimensionElementRow = new DimensionElement();
            dimensionElementRow.Name = "Date";
            dimensionElementRow.AddLevel("Fiscal", "Fiscal Year");
            //Adding categorical elements
            olapReport.CategoricalElements.Add(kpiElement);
            //Adding Series elements
            olapReport.SeriesElements.Add(dimensionElementRow);
            return olapReport;
        }

        /// <summary>
        /// Shows the legend of the control.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void LegendOption(object parameter)
        {
            if (parameter.ToString() == "True")
                this.ShowLegend = Visibility.Visible;
            else
                this.ShowLegend = Visibility.Collapsed;
        }

        #endregion
    }
}