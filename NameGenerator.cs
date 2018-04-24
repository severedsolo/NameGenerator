using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Text;

namespace NameGenerator
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class NameGenerator : MonoBehaviour
    {
        List<string> americanSurnames = new List<string>();
        List<string> americanFemales = new List<string>();
        List<string> americanMales = new List<string>();
        List<string> russianMales = new List<string>();
        List<string> russianFemales = new List<string>();
        List<string> russianFemaleSurnames = new List<string>();
        List<string> russianMaleSurnames = new List<string>();
        int counter;
        bool validRussianData = false;

        System.Random r = new System.Random();

        void Awake()
        {
            DontDestroyOnLoad(this);
            GameEvents.onKerbalAddComplete.Add(onKerbalAdded);
            StreamReader sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/PluginData/americanSurnames.txt", Encoding.Unicode);
            string reader;
            while ((reader = sr.ReadLine()) != null)
            {
                americanSurnames.Add(reader);
            }
            sr.Close();
            sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/PluginData/americanMale.txt", Encoding.Unicode);
            while ((reader = sr.ReadLine()) != null)
            {
                americanMales.Add(reader);
            }
            sr.Close();
            sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/PluginData/americanFemale.txt", Encoding.Unicode);
            while ((reader = sr.ReadLine()) != null)
            {
                americanFemales.Add(reader);
            }
            sr.Close();
            sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/PluginData/russianMale.txt", Encoding.Unicode);
            while ((reader = sr.ReadLine()) != null)
            {
                russianMales.Add(reader);
            }
            sr.Close();
            sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/PluginData/russianFemale.txt", Encoding.Unicode);
            while ((reader = sr.ReadLine()) != null)
            {
                russianFemales.Add(reader);
            }
            sr.Close();
            sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/PluginData/russianFemaleSurnames.txt", Encoding.Unicode);
            while ((reader = sr.ReadLine()) != null)
            {
                russianFemaleSurnames.Add(reader);
            }
            sr.Close();
            sr = new StreamReader(KSPUtil.ApplicationRootPath + "/GameData/NameGenerator/PluginData/russianMaleSurnames.txt", Encoding.Unicode);
            while ((reader = sr.ReadLine()) != null)
            {
                russianMaleSurnames.Add(reader);
            }
            sr.Close();
            if (russianMales.Count > 0 && russianFemales.Count > 0) validRussianData = true;
            IEnumerable<ProtoCrewMember> crew = HighLogic.CurrentGame.CrewRoster.Crew;
            for (int i = 0; i < crew.Count(); i++)
            {
                ProtoCrewMember p = crew.ElementAt(i);
                if (p.type != ProtoCrewMember.KerbalType.Applicant) continue;
                onKerbalAdded(p);
            }
        }

        private void onKerbalAdded(ProtoCrewMember kerbal)
        {
            counter = 0;
            if (kerbal.isHero) return;
            bool nameFound = false;
            bool russian = (r.NextDouble() < 0.5 && validRussianData);
            string surname = "";
            if(!russian) surname = americanSurnames.ElementAt(r.Next(0, americanSurnames.Count()));
            else
            {
                if(kerbal.gender == ProtoCrewMember.Gender.Male) surname = russianMaleSurnames.ElementAt(r.Next(0, russianMaleSurnames.Count()));
                else surname = russianFemaleSurnames.ElementAt(r.Next(0, russianFemaleSurnames.Count()));
            }
            string forename = "";
            while (!nameFound)
            {
                nameFound = true;
                if(kerbal.gender == ProtoCrewMember.Gender.Male)
                {
                    if(!russian)forename = americanMales.ElementAt(r.Next(0, americanMales.Count()));
                    else forename = russianMales.ElementAt(r.Next(0, russianMales.Count()));
                }
                else
                {
                    if(!russian) forename = americanFemales.ElementAt(r.Next(0, americanFemales.Count()));
                    else forename = russianFemales.ElementAt(r.Next(0, russianFemales.Count()));
                }
                name = forename + " " + surname;
                IEnumerable<ProtoCrewMember> crew = HighLogic.CurrentGame.CrewRoster.Kerbals();
                for (int i = 0; i<crew.Count(); i++)
                {
                    ProtoCrewMember p = crew.ElementAt(i);
                    if (p.name == name)
                    {
                        nameFound = false;
                        counter++;
                        break;
                    }
                }
                if (counter > 50) nameFound = true;
            }
            kerbal.ChangeName(name);
        }
    }
}
