using GridLevelEditor.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace GridLevelEditor.Objects
{
    public static partial class FileIO
    {
        public static string[] GetLastData()
        {
            string datafromfile = ReadFromFile(".gridleveldata");
            return datafromfile.Split(new string[] { Splitter.Global }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        public static void SaveLastData(params string[] data)
        {
            WriteToFile(".gridleveldata", data);
        }

        public static Level GetLevelData(string levelName)
        {
            Directory.CreateDirectory("Levels");
            Directory.CreateDirectory("Levels/" + levelName);
            string datafromfile = ReadFromFile("Levels/" + levelName + "/.leveldata");
            if (datafromfile == "")
            {
                return new Level();
            }
            string[] loadedLeveldata = datafromfile.Split(new string[] { Splitter.Global }, System.StringSplitOptions.RemoveEmptyEntries);

            Level level = new Level();
            if(loadedLeveldata.Length > 0) 
                level.Name = loadedLeveldata[0];

            if(loadedLeveldata.Length > 1)
            {
                if(int.TryParse(loadedLeveldata[1], out int h))
                {
                    level.Height = h;
                }
            }

            if(loadedLeveldata.Length > 2)
            {
                if(int.TryParse(loadedLeveldata[2], out int w))
                {
                    level.Width = w;
                }
            }

            string levelElems = "";
            if (loadedLeveldata.Length > 3)
                levelElems = loadedLeveldata[3];

            string[] levelElemsArray = levelElems.Split(new string[] { Splitter.Level }, System.StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < levelElemsArray.Length - 1; i += 2)
            {
                string id = levelElemsArray[i];
                if(id == Splitter.Void)
                {
                    id = "";
                }
                level.Elems.Add(new MgElem() 
                {
                    Id = id,
                    Image = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(levelElemsArray[i + 1]))
                });
            }

            string levelData = "";
            if (loadedLeveldata.Length > 4)
                levelData = loadedLeveldata[4];

            string[] rows = levelData.Split(new string[] { Splitter.Column }, System.StringSplitOptions.RemoveEmptyEntries);
            level.Data = new string[rows.Length][];
            for(int i = 0; i < rows.Length; ++i)
            {
                string[] cols = rows[i].Split(new string[] { Splitter.Row }, System.StringSplitOptions.RemoveEmptyEntries);
                level.Data[i] = new string[cols.Length];
                for(int j = 0; j < cols.Length; ++j)
                {
                    if (cols[j] == Splitter.Void)
                        cols[j] = "";
                    level.Data[i][j] = cols[j];
                }
            }
            return level;
        }

        public static void SaveLevelData(Level level)
        {
            string levelElems = "";
            foreach(MgElem elem in level.Elems)
            {
                string elemId = elem.Id;
                if (elemId == "" || elemId == null)
                    elemId = Splitter.Void;
                levelElems += elemId + Splitter.Level + elem.Image.UriSource.LocalPath + Splitter.Level;
            }

            string levelData = "";
            foreach(string[] strdata in level.Data)
            {
                foreach(string str in strdata)
                {
                    string s = str;
                    if (s == "" || s == null)
                        s = Splitter.Void;
                    levelData += s + Splitter.Row;
                }
                levelData += Splitter.Column;
            }

            Directory.CreateDirectory("Levels");
            Directory.CreateDirectory("Levels/" + level.Name);
            WriteToFile("Levels/" + level.Name + "/.leveldata", level.Name, level.Height.ToString(), level.Width.ToString(), levelElems, levelData);
        }

        public static void GetAllLevels(ObservableCollection<Level> levels)
        {
            if(Directory.Exists("Levels"))
            {
                string[] paths = Directory.GetDirectories("Levels\\");
                foreach(string path in paths)
                {
                    string[] nodes = path.Split('\\');
                    if(nodes.Length > 0)
                    {
                        string levelName = nodes[nodes.Length - 1];
                        levels.Add(GetLevelData(levelName));
                    }
                }
            }
        }

        private static string ReadFromFile(string filename)
        {
            string res = "";

            using (FileStream fstream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (BinaryReader reader = new BinaryReader(fstream))
                {
                    if (fstream.Length != 0)
                    {
                        res = reader.ReadString();
                    }
                }
            }

            return res;
        }

        private static void WriteToFile(string filename, params string[] data)
        {
            string res = "";

            for (int i = 0; i < data.Length; ++i)
            {
                res += data[i] + Splitter.Global;
            }

            using (FileStream fstream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (BinaryWriter writer = new BinaryWriter(fstream))
                {
                    writer.Write(res);
                }
            }
        }
    }
}
