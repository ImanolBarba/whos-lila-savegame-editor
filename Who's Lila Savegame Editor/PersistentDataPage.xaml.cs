/****************************************************************************************
 *   PersistentDataPage.xaml.cs  --  This file is part of whos-lila-savegame-editor.    *
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Who_s_Lila_Savegame_Editor
{
    /// <summary>
    /// Interaction logic for PersistentDataPage.xaml
    /// </summary>

    public class PersistentFlag
    {
        private readonly string flagID;
        private readonly PersistentSaveData saveData;

        public string Name
        {
            get
            {
                switch (this.flagID)
                {
                    case "HasReel":
                        return "Has reel";
                    case "SeenPhonePattern":
                        return "Seen phone pattern";
                    case "HadToShoot":
                        return "Had to shoot";
                    case "KilledWilliam":
                        return "Killed William";
                    case "TalkedAfterShot":
                        return "Talked after shot";
                    case "TalkedToYuInterrogation":
                        return "Talked to Yu interrogation";
                    case "ImLila1":
                        return "I'm Lila 1";
                    case "ImLila2":
                        return "I'm Lila 2";
                    case "FoundMe":
                        return "Found me";
                    case "TrueEnd":
                        return "True end";
                    case "TalkedAfterEnd":
                        return "Talked after end";
                    default:
                        break;
                }
                return "UNKNOWN FLAG";
            }
        }

        public bool Status
        {
            get
            {
                Type t = this.saveData.PersistentBucket.GetType();
                FieldInfo[] fields = t.GetFields();
                FieldInfo f = t.GetFields().First(m => m.Name == this.flagID);
                object? value = f.GetValue(this.saveData.PersistentBucket);

                return value != null && (bool)value;
            }
            set
            {
                Type t = this.saveData.PersistentBucket.GetType();
                FieldInfo f = t.GetFields().First(m => m.Name == this.flagID);
                f.SetValue(this.saveData.PersistentBucket, value);
                MainWindow.MarkChanges();
            }
        }

        public PersistentFlag(string flagID, ref PersistentSaveData saveData)
        {
            this.flagID = flagID;
            this.saveData = saveData;
        }
    }

    public class ComputerCode
    {
        private readonly PersistentSaveData saveData;

        public ComputerCode(ref PersistentSaveData saveData)
        {
            this.saveData = saveData;
        }
         
        public string Value
        {
            get
            {
                return this.saveData.PersistentBucket.ComputerCode.ToString();
            }

            set
            {
                try
                {
                    uint newCode = UInt32.Parse(value);
                    if (newCode > 999)
                    {
                        MessageBox.Show("Entered computer code exceeds 3 digits", "Unable to change computer code", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    this.saveData.PersistentBucket.ComputerCode = newCode;
                    

                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Entered computer code is not a valid number: " + ex.Message, "Unable to change computer code", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public partial class PersistentDataPage : UserControl
    {
        private readonly PersistentSaveData saveData;

        public PersistentDataPage(ref PersistentSaveData saveData)
        {
            InitializeComponent();

            this.saveData = saveData;

            List<PersistentFlag> items = new List<PersistentFlag>
            {
                new PersistentFlag("HasReel", ref saveData),
                new PersistentFlag("SeenPhonePattern", ref saveData),
                new PersistentFlag("HadToShoot", ref saveData),
                new PersistentFlag("KilledWilliam", ref saveData),
                new PersistentFlag("TalkedAfterShot", ref saveData),
                new PersistentFlag("TalkedToYuInterrogation", ref saveData),
                new PersistentFlag("ImLila1", ref saveData),
                new PersistentFlag("ImLila2", ref saveData),
                new PersistentFlag("FoundMe", ref saveData),
                new PersistentFlag("TrueEnd", ref saveData),
                new PersistentFlag("TalkedAfterEnd", ref saveData),
            };
            this.persistentDataList.ItemsSource = items;

            ComputerCode cc = new ComputerCode(ref this.saveData);
            this.computerCode.Text = this.saveData.PersistentBucket.ComputerCode.ToString();
            this.computerCode.DataContext = cc;
        }

        private void ComputerCodeEdited(object sender, RoutedEventArgs e)
        {
            this.computerCode.Background = new SolidColorBrush(Color.FromArgb(204, 159, 217, 248));
        }

        private void ComputerCodeChanged(object sender, RoutedEventArgs e)
        {
            string newCode = this.computerCode.Text;
            BindingExpression bE = this.computerCode.GetBindingExpression(TextBox.TextProperty);
            bE.UpdateSource();

            if(this.computerCode.Text != newCode)
            {
                this.computerCode.Background = new SolidColorBrush(Color.FromArgb(204, 214, 24, 24));
                return;
            }
            this.computerCode.Background = new SolidColorBrush(Color.FromArgb(204, 121, 248, 90));
            MainWindow.MarkChanges();
        }
    }
}
