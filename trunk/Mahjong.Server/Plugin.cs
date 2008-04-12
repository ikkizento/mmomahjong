using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Plugin;
using System.IO;
using System.Reflection;

namespace Mahjong.Server
{
    public class Plugin
    {
        List<IReferee> m_referees = new List<IReferee>();

        public List<IReferee> GetReferees()
        {
            return m_referees;
        }

        public Plugin(String Path)
        {
            string[] files = Directory.GetFiles(Path, "*.dll");

            foreach (string f in files)
            {
                try
                {
                    Assembly a = null;
                    a = Assembly.LoadFile(f);
                    System.Type[] types = a.GetTypes();
                    foreach (System.Type type in types)
                    {
                        if (type.BaseType.Name == "IReferee")
                        {
                            Console.WriteLine("Loading Referee : " + f);
                            IReferee ins = (IReferee)Activator.CreateInstance(type);
                            Console.WriteLine("Referee : " + ins.GetName());
                            m_referees.Add(ins);
                        }
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                }
            }
        }
    }
}
