using GridLevelEditor.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Media.Imaging;

namespace GridLevelEditorTest
{
    [TestClass]
    public class SaveLoadLevelTest
    {
        [TestMethod]
        public void TestMethod()
        {
            DateTime dt = DateTime.Now;
            string levelName = "tempname-" + dt.ToString("dd'_'MM'_'yyyy'_'fffffff");

            Level level = GetTempLevel(levelName);
            FileIO.SaveLevelData(level);

            Level loadedLevel = FileIO.GetLevelData(levelName);

            Assert.AreEqual(level.Name, loadedLevel.Name);
            Assert.AreEqual(level.Height, loadedLevel.Height);
            Assert.AreEqual(level.Width, loadedLevel.Width);

            if (level.Elems.Count == loadedLevel.Elems.Count)
            {
                for(int i = 0; i < level.Elems.Count; ++i)
                {
                    if(level.Elems[i].Id != null)
                    {
                        Assert.AreEqual(level.Elems[i].Id, loadedLevel.Elems[i].Id);
                    }
                    else
                    {
                        Assert.AreEqual("", loadedLevel.Elems[i].Id);
                    }
                    Assert.AreEqual(level.Elems[i].Image.UriSource.LocalPath, loadedLevel.Elems[i].Image.UriSource.LocalPath);
                }

                if(level.Data.Length == loadedLevel.Data.Length)
                {
                    for(int i = 0; i < level.Data.Length; ++i)
                    {
                        if(level.Data[i].Length == loadedLevel.Data[i].Length)
                        {
                            for(int j = 0; j < level.Data[i].Length; ++j)
                            {
                                Assert.AreEqual(level.Data[i][j], loadedLevel.Data[i][j]);
                            }
                        }
                        else
                        {
                            Assert.Fail("Wrong length: level.Data[i].Length, loadedLevel.Data[i].Length");
                        }
                    }
                }
                else
                {
                    Assert.Fail("Wrong length: level.Data.Length, loadedLevel.Data.Length");
                }
            }
            else 
            {
                Assert.Fail("Wrong length: level.Elems.Count, loadedLevel.Elems.Count");
            }
        }

        private Level GetTempLevel(string name)
        {
            Level level = new Level();
            level.Name = name;
            level.Height = 10;
            level.Width = 10;
            level.Elems.Add(new MgElem() { Id = "1_id", Image = new BitmapImage(new Uri(@"D:\Projects\GridLevelEditor\GridLevelEditorTest\TestResources\back.png")) });
            level.Elems.Add(new MgElem() { Id = "100", Image = new BitmapImage(new Uri(@"D:\Projects\GridLevelEditor\GridLevelEditorTest\TestResources\border.png")) });
            level.Elems.Add(new MgElem() { Id = "", Image = new BitmapImage(new Uri(@"D:\Projects\GridLevelEditor\GridLevelEditorTest\TestResources\border.png")) });
            level.Elems.Add(new MgElem() { Id = null, Image = new BitmapImage(new Uri(@"D:\Projects\GridLevelEditor\GridLevelEditorTest\TestResources\border.png")) });
            level.Data = new string[10][];
            Random rand = new Random();

            for (int i = 0; i < 10; ++i)
            {
                level.Data[i] = new string[10];
                for (int j = 0; j < 10; ++j)
                {
                    int next = rand.Next(0, 2);
                    switch (next)
                    {
                        default: level.Data[i][j] = "void"; break;
                        case 1: level.Data[i][j] = "1_id"; break;
                        case 2: level.Data[i][j] = "100"; break;
                    }
                }
            }

            return level;
        }
    }
}
