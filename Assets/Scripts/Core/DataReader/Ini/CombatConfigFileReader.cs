// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2023, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Core.DataReader.Ini
{
    using System;
    using System.IO;
    using System.Text;
    using IniParser;
    using IniParser.Model;
    using UnityEngine;

    public sealed class CombatConfigFileReader : IFileReader<CombatConfigFile>
    {
        private const string ROLE_POSITION_SECTION_HEADER_PREFIX = "RolePos";
        private const string FIVE_ELEMENTS_FORMATION_SECTION_HEADER_SUFFIX = "FiveLineup";
        private const string LEVEL_SECTION_HEADER = "Level";

        public CombatConfigFile Read(IBinaryReader reader, int codepage)
        {
            throw new NotImplementedException();
        }

        public CombatConfigFile Read(byte[] data, int codepage)
        {
            var parser = new FileIniDataParser();
            using var stream = new MemoryStream(data);
            using var reader = new StreamReader(stream, Encoding.GetEncoding(codepage));

            IniData iniData = parser.ReadData(reader);

            var actorGameBoxPositions = new Vector3[10];
            FiveElementsFormationConfig playerFormationConfig = default;
            FiveElementsFormationConfig enemyFormationConfig = default;
            int[] levelExperienceTable = new int[100];

            foreach (SectionData section in iniData.Sections)
            {
                if (section.SectionName.Equals(ROLE_POSITION_SECTION_HEADER_PREFIX,
                        StringComparison.OrdinalIgnoreCase))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        string x = iniData[section.SectionName][$"Role{i}-x"];
                        string z = iniData[section.SectionName][$"Role{i}-z"];

                        if (x.Contains(";")) x = x[..x.IndexOf(';')].Trim();
                        if (z.Contains(";")) z = z[..z.IndexOf(';')].Trim();

                        actorGameBoxPositions[i] = new Vector3(float.Parse(x), 0f, float.Parse(z));
                    }
                }

                if (section.SectionName.EndsWith(FIVE_ELEMENTS_FORMATION_SECTION_HEADER_SUFFIX))
                {
                    string x = iniData[section.SectionName]["x"];
                    string y = iniData[section.SectionName]["y"];
                    string z = iniData[section.SectionName]["z"];
                    string radius = iniData[section.SectionName]["radius"];

                    if (radius.Contains(";")) radius = radius[..radius.IndexOf(';')].Trim();

                    var config = new FiveElementsFormationConfig()
                    {
                        CenterGameBoxPosition = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)),
                        GameBoxRadius = float.Parse(radius),
                    };

                    if (section.SectionName.StartsWith("Enemy"))
                    {
                        enemyFormationConfig = config;
                    }
                    else
                    {
                        playerFormationConfig = config;
                    }
                }

                if (section.SectionName.Equals(LEVEL_SECTION_HEADER, StringComparison.OrdinalIgnoreCase))
                {
                    for (int i = 1; i <= 99; i++)
                    {
                        string exp = iniData[section.SectionName][i.ToString()];
                        levelExperienceTable[i] = int.Parse(exp);
                    }
                }
            }

            return new CombatConfigFile(actorGameBoxPositions,
                enemyFormationConfig,
                playerFormationConfig,
                levelExperienceTable);
        }
    }
}