using Catel.Data;
using GridLevelEditor.Objects;
using System;
using System.Collections.Generic;
using System.Xml;

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
                level = FileIO.GetLevelData(data[0]);
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

        public void ExportToXml(string path)
        {
            XmlDocument xml = new XmlDocument();
            XmlNode root = xml.CreateElement("level");
            XmlAttribute name = xml.CreateAttribute("name");
            name.Value = level.Name;
            root.Attributes.Append(name);
            xml.AppendChild(root);

            foreach(string[] row in level.Data)
            {
                XmlNode xmlRow = xml.CreateElement("row");
                foreach(string elem in row)
                {
                    XmlNode xmlElem = xml.CreateElement("element");
                    xmlElem.InnerText = elem;
                    xmlRow.AppendChild(xmlElem);
                }
                root.AppendChild(xmlRow);
            }

            xml.Save(path);
        }
    }
}
