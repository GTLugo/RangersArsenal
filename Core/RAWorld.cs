﻿using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RangersArsenal.Core
{
    internal class RAWorld : ModWorld
    {
        public static bool downedSunGod;

        public override void Initialize()
        {
            downedSunGod = false;
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedSunGod) downed.Add("sungod");
            return new TagCompound {
                ["downed"] = downed
            };
        }

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedSunGod = downed.Contains("sungod");
        }

        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();

            if (loadVersion == 0) {
                BitsByte flags = reader.ReadByte();
                downedSunGod = flags[0];
            } else {
                mod.Logger.WarnFormat("Extra Gunslinger Gear: Unknown loadVersion: {0}", loadVersion);
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = downedSunGod;
            writer.Write(flags);

            /*
			Remember that Bytes/BitsByte only have 8 entries. If you have more than 8 flags you want to sync, use multiple BitsByte:
				This is wrong:
			flags[8] = downed9thBoss; // an index of 8 is nonsense. 
				This is correct:
			flags[7] = downed8thBoss;
			writer.Write(flags);
			BitsByte flags2 = new BitsByte(); // create another BitsByte
			flags2[0] = downed9thBoss; // start again from 0
			// up to 7 more flags here
			writer.Write(flags2); // write this byte
			*/

            //If you prefer, you can use the BitsByte constructor approach as well.
            //writer.Write(saveVersion);
            //BitsByte flags = new BitsByte(downedAbomination, downedPuritySpirit);
            //writer.Write(flags);

            // This is another way to do the same thing, but with bitmasks and the bitwise OR assignment operator (the |=)
            // Note that 1 and 2 here are bit masks. The next values in the pattern are 4,8,16,32,64,128. If you require more than 8 flags, make another byte.
            //writer.Write(saveVersion);
            //byte flags = 0;
            //if (downedAbomination)
            //{
            //	flags |= 1;
            //}
            //if (downedPuritySpirit)
            //{
            //	flags |= 2;
            //}
            //writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedSunGod = flags[0];
            // As mentioned in NetSend, BitBytes can contain 8 values. If you have more, be sure to read the additional data:
            // BitsByte flags2 = reader.ReadByte();
            // downed9thBoss = flags[0];
        }
    }
}