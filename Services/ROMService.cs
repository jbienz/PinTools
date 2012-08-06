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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Win32;
using PinTools.Data;
using PinTools.Properties;

namespace PinTools.Services
{
    /// <summary>
    /// Loads ROM data and can apply changes.
    /// </summary>
    public class ROMService
    {
        private const string dmd_pos_x = "dmd_pos_x";
        private const string dmd_pos_y = "dmd_pos_y";
        private const string dmd_width = "dmd_width";
        private const string dmd_height = "dmd_height";
        private const string rol = "rol";
        private const string ror = "ror";
        private const string PinMAMEKey = @"Software\Freeware\Visual PinMame";

        private Settings settings = Settings.Default;

        public Collection<ROM> GetROMs()
        {
            // Create the results class
            var results = new Collection<ROM>();

            // Open the registry key
            var pinMameKey = Registry.CurrentUser.OpenSubKey(PinMAMEKey);

            // Add each ROM
            foreach (var romKeyName in pinMameKey.GetSubKeyNames())
            {
                // Open the ROM key
                var romKey = pinMameKey.OpenSubKey(romKeyName);

                // Load values for location
                var x = (int)romKey.GetValue(dmd_pos_x, 0);
                var y = (int)romKey.GetValue(dmd_pos_y, 0);
                var width = (int)romKey.GetValue(dmd_width, 0);
                var height = (int)romKey.GetValue(dmd_height, 0);

                // Create the ROM
                var rom = new ROM()
                {
                    Name = romKeyName,
                    DMDLocation = new Rect(x, y, width, height),
                    DMDRotateLeft = Convert.ToBoolean((int)romKey.GetValue(rol, 0)),
                    DMDRotateRight = Convert.ToBoolean((int)romKey.GetValue(ror, 0)),
                };

                // Add the ROM to the collection
                results.Add(rom);

                // Close the ROM key
                romKey.Close();
            }

            // Close the PinMAME key
            pinMameKey.Close();

            // Return the results
            return results;
        }

        /// <summary>
        /// Sets the location of the DMD window for all of the specified ROMs.
        /// </summary>
        /// <param name="roms">
        /// The list of ROMs to set the location on.
        /// </param>
        /// <param name="location">
        /// The location to set.
        /// </param>
        public void SetDMD(IEnumerable<ROM> roms, Rect location, bool rotateLeft, bool rotateRight)
        {
            // Validate
            if (roms == null) throw new ArgumentNullException("roms");
            if (location.Width < 1 || location.Height < 1) throw new ArgumentException("Location must have a valid size", "location");
            if (rotateLeft && rotateRight) throw new InvalidOperationException("Can't rotate both left and right.");

            // Open the registry key
            var pinMameKey = Registry.CurrentUser.OpenSubKey(PinMAMEKey);

            // Update each ROM
            foreach (var rom in roms)
            {
                // Open the ROM key
                var romKey = pinMameKey.OpenSubKey(rom.Name, true);

                // Set values for location
                romKey.SetValue(dmd_pos_x, location.X, RegistryValueKind.DWord);
                romKey.SetValue(dmd_pos_y, location.Y, RegistryValueKind.DWord);
                romKey.SetValue(dmd_width, location.Width, RegistryValueKind.DWord);
                romKey.SetValue(dmd_height, location.Height, RegistryValueKind.DWord);
                romKey.SetValue(rol, Convert.ToInt32(rotateLeft), RegistryValueKind.DWord);
                romKey.SetValue(ror, Convert.ToInt32(rotateRight), RegistryValueKind.DWord);

                // Close the ROM key
                romKey.Close();
            }

            // Close the PinMAME key
            pinMameKey.Close();
        }
    }
}
