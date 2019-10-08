#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;

namespace ComboBoxColumnsDemo
{
    class ItemsSourceSelectorBehavior: Behavior<SfDataGrid>
    {
        /// <summary>
        /// DataGrid defined thats defined in xaml.
        /// </summary>
        SfDataGrid dataGrid = null;
        protected override void OnAttached()
        {
            dataGrid = this.AssociatedObject as SfDataGrid;
            dataGrid.CurrentCellEndEdit += DataGrid_CurrentCellEndEdit;
        }

        private void DataGrid_CurrentCellEndEdit(object sender, CurrentCellEndEditEventArgs e)
        {
            var sfDataGrid = sender as SfDataGrid;
            if (sfDataGrid.CurrentColumn.MappingName != "ShipCountry")
                return;
            var datarow = dataGrid.RowGenerator.Items.FirstOrDefault(dr => dr.RowIndex == e.RowColumnIndex.RowIndex);
            datarow.Element.DataContext = null;
            dataGrid.UpdateDataRow(e.RowColumnIndex.RowIndex);
        }

        protected override void OnDetaching()
        {
            dataGrid.CurrentCellEndEdit -= DataGrid_CurrentCellEndEdit;
        }
    }
    /// <summary>
    /// Implementation class for ItemsSourceSelector interface
    /// </summary>
    public class CustomSelector : IItemsSourceSelector
    {
        public IEnumerable GetItemsSource(object record, object dataContext)
        {
            if (record == null)
                return null;

            var orderinfo = record as OrderDetails;
            var countryName = orderinfo.ShipCountry;

            var viewModel = dataContext as ViewModel;

            //Returns ShipCity collection based on ShipCountry.
            if (viewModel.ShipCities.ContainsKey(countryName))
            {
                ObservableCollection<ShipCityDetails> shipcities = null;
                viewModel.ShipCities.TryGetValue(countryName, out shipcities);
                return shipcities.ToList();
            }
            return null;
        }
    }
}
