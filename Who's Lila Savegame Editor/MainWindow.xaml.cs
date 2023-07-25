/****************************************************************************************
 *   MainWindow.xaml.cs  --  This file is part of whos-lila-savegame-editor.            *
 *                                                                                      *
 *   Copyright (C) 2023 Imanol-Mikel Barba Sabariego                                    *
 *                                                                                      *
 *   whos-lila-savegame-editor is free software: you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by the Free      *
 *   Software Foundation, either version 3 of the License, or (at your option) any      *
 *   later version.                                                                     *
 *                                                                                      *
 *   whos-lila-savegame-editor is distributed in the hope that it will be useful, but   *
 *   WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or      *
 *   FITNESS FOR A PARTICULAR  PURPOSE. See the GNU General Public License for more     *
 *   details.                                                                           *
 *                                                                                      *
 *   You should have received a copy of the GNU General Public License along with this  *
 *   program. If not, see http://www.gnu.org/licenses/.                                 *
 *                                                                                      *
 ****************************************************************************************/

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace Who_s_Lila_Savegame_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string defaultSaveDataPath = "\\AppData\\LocalLow\\Garage_Heathen\\WhosLila\\important-data.pd";

        private string currentSavePath = "";
        private PersistentSaveData saveData;

        private static bool changes = false;

        public MainWindow()
        {
            InitializeComponent();
            this.saveData = new PersistentSaveData();
        }

        public static void MarkChanges()
        {
            changes = true;    
        }
        public static void ClearChanges()
        {
            changes = false;
        }

        private void AboutDialog(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Owner = this;
            about.ShowDialog();
        }

        private void NewSaveData(object sender, RoutedEventArgs e)
        {
            this.saveData = new PersistentSaveData();
            LoadSaveDataIntoUI();
        }

        private void LoadDefaultSaveData(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetEnvironmentVariable("USERPROFILE") + defaultSaveDataPath;
            this.currentSavePath = path;
            try
            {
                this.saveData = SaveData.LoadSaveData(path);
                LoadSaveDataIntoUI();
            }
            catch (Exception ex) when (ex is IOException ||
                               ex is FileNotFoundException ||
                               ex is InvalidDataException)
            {
                MessageBox.Show("Unable to load save data: " + ex.Message, "Load error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSaveDataFromFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    this.saveData = SaveData.LoadSaveData(openFileDialog.FileName);
                    this.currentSavePath = openFileDialog.FileName;
                    LoadSaveDataIntoUI();
                }
                catch (Exception ex) when (ex is IOException ||
                               ex is FileNotFoundException ||
                               ex is InvalidDataException)
                {
                    MessageBox.Show("Unable to load save data: " + ex.Message, "Load error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadSaveDataIntoUI()
        {
            if (this.saveData == null)
            {
                MessageBox.Show("No savegame data loaded!", "Load error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.endingsTab.Content = new EndingsPage(ref this.saveData);
            this.persistentTab.Content = new PersistentDataPage(ref this.saveData);
            this.addressTab.Content = new AddressesPage(ref this.saveData);
            this.pallettesTab.Content = new PalettesPage(ref this.saveData);
        }

        private void Save(string path)
        {
            if (path == "")
            {
                MessageBox.Show("Data was not saved", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                SaveData.SaveSaveData(this.saveData, path);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to save data: " + ex.Message, "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveIntoFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                this.Save(saveFileDialog.FileName);
            }
        }

        private void SaveIntoCurrentOpenFile(object sender, RoutedEventArgs e)
        {
            if (this.currentSavePath == "")
            {
                this.SaveIntoFile(sender, e);
                return;
            }
            this.Save(this.currentSavePath);
        }

        private void RequestExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ExitApplication(object sender, CancelEventArgs e)
        {
            if(changes)
            {
                if (
                    MessageBox.Show(
                        "Save changes before exiting?",
                        "Confirm unsaved changes",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    ) == MessageBoxResult.Yes
                )
                {
                    SaveIntoCurrentOpenFile(sender, new RoutedEventArgs());
                }
            }
            Application.Current.Shutdown();
        }
    }
}