using Catel.Data;
using GridLevelEditor.Objects;
using System;
using System.Collections.Generic;

namespace GridLevelEditor.Models
{
    public class LevelEditor : ModelBase
    {
        private Level level;

        public string LevelName 
        { 
            get
            { 
                if(level != null)
                    return level.Name; 

                return "";
            }
            set
            {
                if (value != null)
                    level.Name = value;
            }
        }

        public bool IsLoaded { private set; get; }

        public LevelEditor()
        {
            string[] data = FileIO.GetLastData();
            IsLoaded = false;

            if(data.Length > 0 && FileIO.LevelExists(data[0]))
            {
                level = Level.GetLevel(data[0]);
                IsLoaded = true;
            }
            else
            {
                level = new Level();
            }
        }

        public void ReInit()
        {
            level = new Level();
            IsLoaded = false;
        }

        public void Save()
        {
            if (level.Name == "")
                level.Name = "tempsave_" + DateTime.Now.ToString("dd'_'MM'_'yyyy'_'fffffff");
            FileIO.SaveLastData(LevelName);
            FileIO.SaveLevelData(level);
        }

        public void AddMgElem(MgElem elem)
        {
            level.Elems.Add(elem);
        }

        public void RemoveMgElem(MgElem elem)
        {
            level.Elems.Remove(elem);
        }

        public List<MgElem> GetElems()
        {
            return level.Elems;
        }

        public void ClearElems()
        {
            level.Elems.Clear();
        }

        public void SetLevelSize(int height, int width)
        {
            level.Height = height;
            level.Width = width;
        }

        public KeyValuePair<int, int> GetLevelSize()
        {
            return new KeyValuePair<int, int>(level.Height, level.Width);
        }

        public void UpdateData(string[][] data)
        {
            level.Data = data;
        }

        public string[][] GetLevelData()
        {
            return level.Data;
        }

        public void Load(Level level)
        {
            this.level = level;
            IsLoaded = true;
        }

        public void DeleteLevel()
        {
            FileIO.DeleteLevel(level);
            IsLoaded = false;
        }
    }
}
