/****************************************************************************************
 *   SaveData.cs  --  This file is part of whos-lila-savegame-editor.                   *
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

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Who_s_Lila_Savegame_Editor
{
    internal class SaveData
    {
        static private void BackupSaveData(string srcPath)
        {
            string destPath = srcPath + ".bak";
            FileInfo src = new FileInfo(srcPath);
                
            src.CopyTo(destPath, true);

            var dest = new FileInfo(destPath);
            dest.CreationTime = src.CreationTime;
            dest.LastWriteTime = src.LastWriteTime;
            dest.LastAccessTime = src.LastAccessTime;
        }

        static public void SaveSaveData(PersistentSaveData saveData, string path)
        {
            if(File.Exists(path))
            {
                BackupSaveData(path);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);

#pragma warning disable SYSLIB0011
            binaryFormatter.Serialize(fileStream, saveData);

            fileStream.Close();

            MainWindow.ClearChanges();
        }

        static public PersistentSaveData LoadSaveData(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Save data not found in " + path);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

#pragma warning disable SYSLIB0011
            PersistentSaveData? data = binaryFormatter.Deserialize(fileStream) as PersistentSaveData ??
                throw new InvalidDataException("Save data could not be deserialised, possibly corrupt data.");
            fileStream.Close();
            return data;
        }
    }
}
