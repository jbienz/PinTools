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
using System.Windows;
using GalaSoft.MvvmLight;

namespace PinTools.Data
{
    /// <summary>
    /// Represents a table ROM image.
    /// </summary>
    public class ROM : ObservableObject, ISelectable
    {
        private Rect dmdLocation;
        /// <summary>
        /// Gets or sets the location of the DMD Window.
        /// </summary>
        /// <value>
        /// The location of the DMD Window.
        /// </value>
        public Rect DMDLocation
        {
            get { return dmdLocation; }
            set
            {
                if (dmdLocation != value)
                {
                    dmdLocation = value;
                    RaisePropertyChanged(() => DMDLocation);
                }
            }
        }

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
        /// Gets or sets the name of the <see cref="ROM"/>.
        /// </summary>
        /// <value>
        /// The name of the <c>ROM</c>.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the RotateLeft of the <see cref="ROM"/>.
        /// </summary>
        /// <value>
        /// The RotateLeft of the <c>ROM</c>.
        /// </value>
        public bool DMDRotateLeft { get; set; }

        /// <summary>
        /// Gets or sets the RotateRight of the <see cref="ROM"/>.
        /// </summary>
        /// <value>
        /// The RotateRight of the <c>ROM</c>.
        /// </value>
        public bool DMDRotateRight { get; set; }
    }
}
