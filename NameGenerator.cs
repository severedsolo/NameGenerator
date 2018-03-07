using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace NameGenerator
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class NameGenerator : MonoBehaviour
    {
        List<string> surnames = new List<string>();
        List<string> females = new List<string>();
        List<string> males = new List<string>();           
        System.Random r = new System.Random();

        void Awake()
        {
            DontDestroyOnLoad(this);
            GameEvents.onKerbalAdded.Add(onKerbalAdded);
            StreamReader sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/surnames.txt");
            string reader;
            while ((reader = sr.ReadLine()) != null)
            {
                surnames.Add(reader);
            }
            sr.Close();
            sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/male.txt");
            while ((reader = sr.ReadLine()) != null)
            {
                males.Add(reader);
            }
            sr.Close();
            sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/female.txt");
            while ((reader = sr.ReadLine()) != null)
            {
                females.Add(reader);
            }
            sr.Close();
        }

        private void onKerbalAdded(ProtoCrewMember kerbal)
        {
            if (kerbal.name == "Jebediah Kerman" || kerbal.name == "Bob Kerman" || kerbal.name == "Bill Kerman" || kerbal.name == "Valentina Kerman") return;
            IEnumerable<ProtoCrewMember> crew = HighLogic.CurrentGame.CrewRoster.Crew;
            bool nameFound = false;
            string surname = surnames.ElementAt(r.Next(0, surnames.Count()));
            string forename = "";
            while (!nameFound)
            {
                nameFound = true;
                if(kerbal.gender == ProtoCrewMember.Gender.Male)
                {
                    forename = males.ElementAt(r.Next(0, males.Count()));
                }
                else
                {
                    forename = females.ElementAt(r.Next(0, females.Count()));
                }
                name = forename + " " + surname;
                for(int i = 0; i<crew.Count();i++)
                {
                    ProtoCrewMember p = crew.ElementAt(i);
                    if (p.name == name)
                    {
                        nameFound = false;
                        break;
                    }
                }
            }
            kerbal.ChangeName(name);
        }
    }
}
