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
    /// The view model for the ROMS screen.
    /// </summary>
    public class ROMsVM : SelectionVM<ROM>
    {
        #region Member Variables
        private ROMService romsService;
        #endregion // Member Variables

        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="ROMsVM"/> instance.
        /// </summary>
        public ROMsVM()
        {
            // Create services
            romsService = new ROMService();
        }
        #endregion // Constructors

        #region Internal Methods
        protected override void CreateCommands()
        {
            base.CreateCommands();
            RefreshCommand = new RelayCommand(Refresh);
            SetDMDLocationCommand = new RelayCommand(SetDMDLocation, () => { return Items != null && Items.Count > 0; });
        }

        private void SetDMDLocation()
        {
            if ((Items == null) || (Items.Count < 1)) { return; }

            var toExport = from i in Items where i.IsSelected select i;
            int exportCount = toExport.Count();
            if (exportCount < 1) return;

            if (DMDWidth < 1 || DMDHeight < 1)
            {
                MessageBox.Show("DMD Location does not have a valid width or height.");
                return;
            }

            if (dmdRotateLeft && dmdRotateRight)
            {
                MessageBox.Show("Can't rotate both left and right.");
                return;
            }

            if (MessageBox.Show(string.Format("Update {0} DMD location(s)?", exportCount), "Update?", MessageBoxButton.YesNo) != MessageBoxResult.Yes) { return; }

            try
            {
                // Set the locations
                romsService.SetDMD(toExport, new Rect(dmdX, dmdY, dmdWidth, dmdHeight), dmdRotateLeft, dmdRotateRight);

                // Refresh the list
                Refresh();

                // Notify success
                MessageBox.Show("Update complete.");
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
                Items = romsService.GetROMs();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        #endregion // Internal Methods

        #region Overridables / Event Triggers
        protected override void OnItemsChanged()
        {
            base.OnItemsChanged();
            SetDMDLocationCommand.RaiseCanExecuteChanged();
        }
        #endregion // Overridables / Event Triggers

        #region Public Properties
        private int dmdX;
        /// <summary>
        /// Gets or sets the dmdX of the <see cref="ROMsVM"/>.
        /// </summary>
        /// <value>
        /// The dmdX of the <c>ROMsVM</c>.
        /// </value>
        public int DMDX
        {
            get { return dmdX; }
            set
            {
                if (dmdX != value)
                {
                    dmdX = value;
                    RaisePropertyChanged(() => DMDX);
                }
            }
        }

        private int dmdY;
        /// <summary>
        /// Gets or sets the dmdX of the <see cref="ROMsVM"/>.
        /// </summary>
        /// <value>
        /// The dmdX of the <c>ROMsVM</c>.
        /// </value>
        public int DMDY
        {
            get { return dmdY; }
            set
            {
                if (dmdY != value)
                {
                    dmdY = value;
                    RaisePropertyChanged(() => DMDY);
                }
            }
        }

        private int dmdWidth;
        /// <summary>
        /// Gets or sets the dmdWidth of the <see cref="ROMsVM"/>.
        /// </summary>
        /// <value>
        /// The dmdWidth of the <c>ROMsVM</c>.
        /// </value>
        public int DMDWidth
        {
            get { return dmdWidth; }
            set
            {
                if (dmdWidth != value)
                {
                    dmdWidth = value;
                    RaisePropertyChanged(() => DMDWidth);
                }
            }
        }

        private int dmdHeight;
        /// <summary>
        /// Gets or sets the dmdHeight of the <see cref="ROMsVM"/>.
        /// </summary>
        /// <value>
        /// The dmdHeight of the <c>ROMsVM</c>.
        /// </value>
        public int DMDHeight
        {
            get { return dmdHeight; }
            set
            {
                if (dmdHeight != value)
                {
                    dmdHeight = value;
                    RaisePropertyChanged(() => DMDHeight);
                }
            }
        }

        private bool dmdRotateLeft;
        /// <summary>
        /// Gets or sets the dmdRotateLeft of the <see cref="ROMsVM"/>.
        /// </summary>
        /// <value>
        /// The dmdRotateLeft of the <c>ROMsVM</c>.
        /// </value>
        public bool DMDRotateLeft
        {
            get { return dmdRotateLeft; }
            set
            {
                if (dmdRotateLeft != value)
                {
                    dmdRotateLeft = value;
                    RaisePropertyChanged(() => DMDRotateLeft);
                }
            }
        }

        private bool dmdRotateRight;
        /// <summary>
        /// Gets or sets the dmdRotateRight of the <see cref="ROMsVM"/>.
        /// </summary>
        /// <value>
        /// The dmdRotateRight of the <c>ROMsVM</c>.
        /// </value>
        public bool DMDRotateRight
        {
            get { return dmdRotateRight; }
            set
            {
                if (dmdRotateRight != value)
                {
                    dmdRotateRight = value;
                    RaisePropertyChanged(() => DMDRotateRight);
                }
            }
        }

        private ROM selectedROM;
        /// <summary>
        /// Gets or sets the selectedROM of the <see cref="ROMsVM"/>.
        /// </summary>
        /// <value>
        /// The selectedROM of the <c>ROMsVM</c>.
        /// </value>
        public ROM SelectedROM
        {
            get { return selectedROM; }
            set
            {
                if (selectedROM != value)
                {
                    selectedROM = value;
                    RaisePropertyChanged(() => SelectedROM);
                    if (selectedROM != null)
                    { 
                        DMDX = (int)selectedROM.DMDLocation.X;
                        DMDY = (int)selectedROM.DMDLocation.Y;
                        DMDWidth = (int)selectedROM.DMDLocation.Width;
                        DMDHeight = (int)selectedROM.DMDLocation.Height;
                        DMDRotateLeft = selectedROM.DMDRotateLeft;
                        DMDRotateRight = selectedROM.DMDRotateRight;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the command to refresh the ROM list.
        /// </summary>
        /// <value>
        /// The command to refresh the ROM list.
        /// </value>
        public RelayCommand RefreshCommand { get; private set; }

        /// <summary>
        /// Gets the command to set the DMD location.
        /// </summary>
        /// <value>
        /// The command to set the DMD location.
        /// </value>
        public RelayCommand SetDMDLocationCommand { get; private set; }
        #endregion // Public Properties
    }
}
