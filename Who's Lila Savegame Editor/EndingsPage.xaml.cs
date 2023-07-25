/****************************************************************************************
 *   EndingsPage.xaml.cs  --  This file is part of whos-lila-savegame-editor.           *
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

using System.Collections.Generic;
using System.Windows.Controls;

namespace Who_s_Lila_Savegame_Editor
{
    /// <summary>
    /// Interaction logic for EndingsPage.xaml
    /// </summary>

    public class Ending
    {
        private readonly EndingIndex index;
        private readonly PersistentSaveData saveData;

        public Ending(EndingIndex index, ref PersistentSaveData saveData) 
        {
            this.index = index;
            this.saveData = saveData;
        }

        public bool IsObtained
        {
            get
            {
                return this.saveData.Endings.endings.Contains(this.index);
            }

            set
            {
                if(value && !this.saveData.Endings.endings.Contains(this.index))
                {
                    this.saveData.Endings.endings.Add(this.index);
                    MainWindow.MarkChanges();
                }
                else if(!value && this.saveData.Endings.endings.Contains(this.index))
                {
                    this.saveData.Endings.endings.Remove(this.index);
                    this.saveData.DiscussedEndings.endings.Remove(this.index);
                    MainWindow.MarkChanges();
                }
            }
        }

        public bool IsDiscussed
        {
            get
            {
                return this.saveData.DiscussedEndings.endings.Contains(this.index);
            }

            set
            {
                if (value && !this.saveData.DiscussedEndings.endings.Contains(this.index))
                {
                    this.saveData.DiscussedEndings.endings.Add(this.index);
                    MainWindow.MarkChanges();
                }
                else if (!value && this.saveData.DiscussedEndings.endings.Contains(this.index))
                {
                    this.saveData.DiscussedEndings.endings.Remove(this.index);
                    MainWindow.MarkChanges();
                }
            }
        }

        public string Name
        {
            get
            {
                switch (this.index)
                {
                    case EndingIndex.FoolTarot:
                        return "0 - The Fool";
                    case EndingIndex.PriestessTarot:
                        return "II - The High Priestess";
                    case EndingIndex.EmpressReversedTarot:
                        return "III - The Empress (Reversed)";
                    case EndingIndex.EmperorReversedTarot:
                        return "IV - The Emperor (Reversed)";
                    case EndingIndex.HighPriestTarot:
                        return "V - The Hierophant";
                    case EndingIndex.LoversTarot:
                        return "VI - The Lovers";
                    case EndingIndex.StrengthTarot:
                        return "VIII - Strength";
                    case EndingIndex.Wheel:
                        return "X - The Wheel Of Fortune";
                    case EndingIndex.JusticeTarot:
                        return "XI - Justice";
                    case EndingIndex.HangedManTarot:
                        return "XII - The Hanged Man";
                    case EndingIndex.DeathTarot:
                        return "XIII - Death";
                    case EndingIndex.TowerTarot:
                        return "XVI - The Tower";
                    case EndingIndex.MoonTarot:
                        return "XVIII - The Moon";
                    case EndingIndex.SunTarot:
                        return "XIX - The Sun";
                    case EndingIndex.JudgementTarot:
                        return "XX - Judgement";
                    default:
                        break;
                }
                return "UNKNOWN ENDING";
            }
        }
    }
    public partial class EndingsPage : UserControl
    {
        public EndingsPage(ref PersistentSaveData saveData)
        {
            InitializeComponent();
            List<Ending> items = new List<Ending>
            {
                new Ending(EndingIndex.FoolTarot, ref saveData),
                new Ending(EndingIndex.PriestessTarot, ref saveData),
                new Ending(EndingIndex.EmpressReversedTarot, ref saveData),
                new Ending(EndingIndex.EmperorReversedTarot, ref saveData),
                new Ending(EndingIndex.HighPriestTarot, ref saveData),
                new Ending(EndingIndex.LoversTarot, ref saveData),
                new Ending(EndingIndex.StrengthTarot, ref saveData),
                new Ending(EndingIndex.Wheel, ref saveData),
                new Ending(EndingIndex.JusticeTarot, ref saveData),
                new Ending(EndingIndex.HangedManTarot, ref saveData),
                new Ending(EndingIndex.DeathTarot, ref saveData),
                new Ending(EndingIndex.TowerTarot, ref saveData),
                new Ending(EndingIndex.MoonTarot, ref saveData),
                new Ending(EndingIndex.SunTarot, ref saveData),
                new Ending(EndingIndex.JudgementTarot, ref saveData)
            };
            this.endingsList.ItemsSource = items;
        }
    }
}
