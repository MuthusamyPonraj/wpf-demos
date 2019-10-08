#region Copyright Syncfusion Inc. 2001 - 2019
// Copyright Syncfusion Inc. 2001 - 2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

namespace OLAPClientConfiguration.ViewModel
{
    using Syncfusion.Olap.Manager;
    using System;
    using System.Collections.Generic;
    using Syncfusion.Windows.Shared;

    /// <summary>
    /// Interaction logic for OlapClient view.
    /// </summary>
    public class ViewModel : NotificationObject, IDisposable
    {
        #region Members

        /// <summary>
        /// Shared connection string.
        /// </summary>
        public static string ConnectionString;
        private OlapDataManager olapDataManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            this.olapDataManager = new OlapDataManager(ConnectionString);
        }

        #endregion

        #region Properties

        public OlapDataManager ClientDataManager
        {
            get { return this.olapDataManager; }
            set { this.olapDataManager = value; }
        }

        public IEnumerable<string> ClientDispModes
        {
            get
            {
                return Enum<Syncfusion.Windows.Client.Olap.DisplayModes>.GetNames();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && this.olapDataManager != null)
                this.olapDataManager.Dispose();
        }

        #endregion
    }
}