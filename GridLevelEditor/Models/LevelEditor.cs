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
                {
                    return level.Name; 
                }
                return "";
            }
        }

        public LevelEditor()
        {
            string[] data = FileIO.GetLastData();
            if(data.Length > 0)
            {
                level = Level.GetLevel(data[0]);
            }
            else
            {
                level = new Level();
            }
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
    }
}
