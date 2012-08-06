#region License
/****************************************************************************************
 * Licensed under the MIT License
 *
 * Copyright (c)2012
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *****************************************************************************************/
 #endregion // License
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PinTools.Data;
using PinTools.Services;

namespace PinTools.ViewModels
{
    /// <summary>
    /// The view model for the tables screen.
    /// </summary>
    public class TablesVM : SelectionVM<Table>
    {
        #region Member Variables
        private TableService tableService;
        #endregion // Member Variables

        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="TablesVM"/> instance.
        /// </summary>
        public TablesVM()
        {
            // Create services
            tableService = new TableService();
        }
        #endregion // Constructors

        #region Internal Methods
        protected override void CreateCommands()
        {
            base.CreateCommands();
            ExportCommand = new RelayCommand(Export, () => { return Items != null && Items.Count > 0; });
            RefreshCommand = new RelayCommand(Refresh);
            SelectCompleteCommand = new RelayCommand(SelectComplete, () => { return Items != null && Items.Count > 0; });
        }

        private void Export()
        {
            if ((Items == null) || (Items.Count < 1)) { return; }
            var toExport = from i in Items where i.IsSelected select i;
            int exportCount = toExport.Count();

            if (MessageBox.Show(string.Format("Export {0} Table(s)?", exportCount), "Export?", MessageBoxButton.YesNo) != MessageBoxResult.Yes) { return; }

            try
            {
                // Export
                tableService.ExportDatabases(toExport);

                // Notify success
                MessageBox.Show("Export complete.");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void Refresh()
        {
            try
            {
                Items = tableService.GetTables();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void SelectComplete()
        {
            foreach (var item in Items)
            {
                item.IsSelected = item.IsComplete;
            }
        }
        #endregion // Internal Methods

        #region Overridables / Event Triggers
        protected override void OnItemsChanged()
        {
            base.OnItemsChanged();
            ExportCommand.RaiseCanExecuteChanged();
            SelectCompleteCommand.RaiseCanExecuteChanged();
        }
        #endregion // Overridables / Event Triggers

        #region Public Properties
        /// <summary>
        /// Gets the command to export the table list to HyperPin database files.
        /// </summary>
        /// <value>
        /// The command to export the table list to HyperPin database files.
        /// </value>
        public RelayCommand ExportCommand { get; private set; }

        /// <summary>
        /// Gets the command to refresh the table list.
        /// </summary>
        /// <value>
        /// The command to refresh the table list.
        /// </value>
        public RelayCommand RefreshCommand { get; private set; }

        /// <summary>
        /// Gets the command to select only "complete" tables.
        /// </summary>
        /// <value>
        /// The command to select only "complete" tables.
        /// </value>
        public RelayCommand SelectCompleteCommand { get; private set; }
        #endregion // Public Properties
    }
}
