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
using System.Xml.Linq;
using PinTools.Data;
using PinTools.Properties;

namespace PinTools.Services
{
    /// <summary>
    /// Loads table data and scans for dependencies.
    /// </summary>
    public class TableService
    {
        private Settings settings = Settings.Default;

        private void AddTables(Collection<Table> tables, Platform platform)
        {
            string imageExtension = settings.ImageExtension;
            string videoExtension = settings.VideoExtension;
            string ultraVPPath = settings.UltraVPPath;
            string ultraVPExtension = settings.UltraVPExtension;

            string backglassImagePath;
            string tableExtension;
            string tableImagePath;
            string tablePath;
            string tableVideoPath;
            bool ultraVPCompatible;
            string wheelImagePath;

            switch (platform)
            {
                case Platform.FuturePinball:
                    backglassImagePath = settings.FPBackglassImagePath;
                    tableExtension = settings.FPTableExtension;
                    tableImagePath = settings.FPTableImagePath;
                    tablePath = settings.FPTablePath;
                    tableVideoPath = settings.FPTableVideoPath;
                    ultraVPCompatible = false;
                    wheelImagePath = settings.FPWheelImagePath;
                    break;
                case Platform.VisualPinball:
                    backglassImagePath = settings.VPBackglassImagePath;
                    tableExtension = settings.VPTableExtension;
                    tableImagePath = settings.VPTableImagePath;
                    tablePath = settings.VPTablePath;
                    tableVideoPath = settings.VPTableVideoPath;
                    ultraVPCompatible = true;
                    wheelImagePath = settings.VPWheelImagePath;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            // Look for files
            foreach (var file in Directory.GetFiles(tablePath, "*" + tableExtension))
            {
                // Add to object instance and search for related files
                var table = new Table();
                table.Name = Path.GetFileNameWithoutExtension(file);
                table.BackglassImagePath = Path.Combine(backglassImagePath, table.Name + imageExtension);
                table.BackglassImageFound = File.Exists(table.BackglassImagePath);
                table.Platform = platform;
                table.TableFound = true;
                table.TableImagePath = Path.Combine(tableImagePath, table.Name + imageExtension);
                table.TableImageFound = File.Exists(table.TableImagePath);
                table.TableVideoPath = Path.Combine(tableVideoPath, table.Name + videoExtension);
                table.TableVideoFound = File.Exists(table.TableVideoPath);
                table.UltraVPCompatible = ultraVPCompatible;
                if (ultraVPCompatible)
                {
                    table.UltraVPPath = Path.Combine(ultraVPPath, table.Name + ultraVPExtension);
                    table.UltraVPFound = File.Exists(table.UltraVPPath);
                }
                table.WheelImagePath = Path.Combine(wheelImagePath, table.Name + imageExtension);
                table.WheelImageFound = File.Exists(table.WheelImagePath);

                // Check all fields required to be "complete"
                table.IsComplete = (table.TableFound && 
                    (settings.BackglassImageRequired ? table.BackglassImageFound : true) &&
                    (settings.TableImageRequired ? table.TableImageFound : true) &&
                    (settings.WheelImageRequired ? table.WheelImageFound : true) && 
                    (settings.TableVideoRequired ? table.TableVideoFound : true) && 
                    (settings.UltraVPRequired ? (table.UltraVPCompatible ? table.UltraVPFound : true) : true));

                // Add to the collection
                tables.Add(table);
            }
        }

        private void ExportDatabase(IEnumerable<Table> tables, string dbPath)
        {
            // If the file exists, back it up
            if (File.Exists(dbPath))
            {
                // If there was a previous backup, delete it.
                File.Delete(dbPath + ".backup");
                // Create the backup
                File.Move(dbPath, dbPath + ".backup");
            }

            // The declaration currently used by HyperPin
            var dec = new XDeclaration("1.0", "utf-8", "yes");

            // Create the document using the declaration
            XDocument doc = new XDocument(dec);
            
            // Create the menu node
            var menu = new XElement("menu");

            // Add games
            foreach (var table in tables)
            {
                // Create the game element
                var game = new XElement("game");
                
                // Set the name attribute
                game.SetAttributeValue("name", table.Name);

                // Set the description element
                game.SetElementValue("description", table.Name);

                // Extras
                game.SetElementValue("manufacturer", "");
                game.SetElementValue("year", "");
                game.SetElementValue("type", "");

                // Add the game to the menu
                menu.Add(game);
            }

            // Add the menu node to the document
            doc.Add(menu);

            // Save the document
            doc.Save(dbPath);
        }

        public void ExportDatabases(IEnumerable<Table> tables)
        {
            // Validate
            if (tables == null) throw new ArgumentNullException("tables");

            // Future Pinball
            var fpTables = from t in tables
                           where t.Platform == Platform.FuturePinball
                           select t;
            ExportDatabase(fpTables, settings.FPDBPath);

            // Visual Pinball
            var vpTables = from t in tables
                           where t.Platform == Platform.VisualPinball
                           select t;
            ExportDatabase(vpTables, settings.VPDBPath);
        }

        public Collection<Table> GetTables()
        {
            // Create the results class
            Collection<Table> results = new Collection<Table>();

            // Add each system
            AddTables(results, Platform.FuturePinball);
            AddTables(results, Platform.VisualPinball);

            // Return the results
            return results;
        }
    }
}
