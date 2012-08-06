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
    /// The view model for a list of selectable items.
    /// </summary>
    public class SelectionVM<T> : ViewModelBase where T:ISelectable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="SelectionVM"/> instance.
        /// </summary>
        public SelectionVM()
        {
            // Create commands
            CreateCommands();
        }
        #endregion // Constructors

        #region Internal Methods
        protected virtual void CreateCommands()
        {
            SelectAllCommand = new RelayCommand(SelectAll, () => { return items != null && items.Count > 0; });
            SelectNoneCommand = new RelayCommand(SelectNone, () => { return items != null && items.Count > 0; });
        }

        protected virtual void SelectAll()
        {
            foreach (var item in Items)
            {
                item.IsSelected = true;
            }
        }

        protected virtual void SelectNone()
        {
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }
        }

        protected virtual void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        #endregion // Internal Methods

        #region Overridables / Event Triggers
        protected virtual void OnItemsChanged()
        {
            SelectAllCommand.RaiseCanExecuteChanged();
            SelectNoneCommand.RaiseCanExecuteChanged();
        }
        #endregion // Overridables / Event Triggers

        #region Public Properties
        /// <summary>
        /// Gets or sets the SelectAll of the <see cref="TablesVM"/>.
        /// </summary>
        /// <value>
        /// The SelectAll of the <c>TablesVM</c>.
        /// </value>
        public RelayCommand SelectAllCommand { get; set; }

        /// <summary>
        /// Gets or sets the SelectNone of the <see cref="TablesVM"/>.
        /// </summary>
        /// <value>
        /// The SelectNone of the <c>TablesVM</c>.
        /// </value>
        public RelayCommand SelectNoneCommand { get; set; }

        private Collection<T> items;
        /// <summary>
        /// Gets or sets the collection of items.
        /// </summary>
        /// <value>
        /// The collection of items.
        /// </value>
        public Collection<T> Items
        {
            get { return items; }
            set
            {
                if (items != value)
                {
                    items = value;
                    RaisePropertyChanged(() => Items);
                    OnItemsChanged();
                }
            }
        }
        #endregion // Public Properties
    }
}
