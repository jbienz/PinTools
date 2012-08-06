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
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace PinTools.Data
{
    /// <summary>
    /// The platform that a table belongs to.
    /// </summary>
    public enum Platform
    {
        VisualPinball,
        FuturePinball
    }

    /// <summary>
    /// Represents a table.
    /// </summary>
    public class Table : ObservableObject, ISelectable
    {
        /// <summary>
        /// Gets or sets the BackglassImageFound of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The BackglassImageFound of the <c>Table</c>.
        /// </value>
        public bool BackglassImageFound { get; set; }

        /// <summary>
        /// Gets or sets the BackglassImagePath of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The BackglassImagePath of the <c>Table</c>.
        /// </value>
        public string BackglassImagePath { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the table is considered "complete".
        /// </summary>
        /// <value>
        /// <c>True</c> if the table is considered "complete"; otherwise <c>False</c>.
        /// </value>
        public bool IsComplete { get; set; }

        private bool isSelected;
        /// <summary>
        /// Gets or sets the isSelected of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The isSelected of the <c>Table</c>.
        /// </value>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    RaisePropertyChanged(() => IsSelected);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Name of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The Name of the <c>Table</c>.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Platform of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The Emulator of the <c>Table</c>.
        /// </value>
        public Platform Platform { get; set; }

        /// <summary>
        /// Gets or sets the TablePath of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The TablePath of the <c>Table</c>.
        /// </value>
        public string TablePath { get; set; }

        /// <summary>
        /// Gets or sets the TableFound of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The TableFound of the <c>Table</c>.
        /// </value>
        public bool TableFound { get; set; }

        /// <summary>
        /// Gets or sets the TableImageFound of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The TableImageFound of the <c>Table</c>.
        /// </value>
        public bool TableImageFound { get; set; }

        /// <summary>
        /// Gets or sets the TableImagePath of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The TableImagePath of the <c>Table</c>.
        /// </value>
        public string TableImagePath { get; set; }

        /// <summary>
        /// Gets or sets the TableVideoFound of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The TableVideoFound of the <c>Table</c>.
        /// </value>
        public bool TableVideoFound { get; set; }

        /// <summary>
        /// Gets or sets the TableVideoPath of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The TableVideoPath of the <c>Table</c>.
        /// </value>
        public string TableVideoPath { get; set; }

        /// <summary>
        /// Gets or sets the UltraVPCompatible of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The UltraVPCompatible of the <c>Table</c>.
        /// </value>
        public bool UltraVPCompatible { get; set; }

        /// <summary>
        /// Gets or sets the UltraVPFound of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The UltraVPFound of the <c>Table</c>.
        /// </value>
        public bool UltraVPFound { get; set; }

        /// <summary>
        /// Gets or sets the UltraVPPath of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The UltraVPPath of the <c>Table</c>.
        /// </value>
        public string UltraVPPath { get; set; }

        /// <summary>
        /// Gets or sets the WheelImageFound of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The WheelImageFound of the <c>Table</c>.
        /// </value>
        public bool WheelImageFound { get; set; }

        /// <summary>
        /// Gets or sets the WheelImagePath of the <see cref="Table"/>.
        /// </summary>
        /// <value>
        /// The WheelImagePath of the <c>Table</c>.
        /// </value>
        public string WheelImagePath { get; set; }
    }
}
