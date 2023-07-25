/****************************************************************************************
 *   AboutWindow.xaml.cs  --  This file is part of whos-lila-savegame-editor.           *
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


using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace Who_s_Lila_Savegame_Editor
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = ((Hyperlink)sender).NavigateUri.ToString(),
                UseShellExecute = true
            });
        }
    }
}
