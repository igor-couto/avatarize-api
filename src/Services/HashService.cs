﻿namespace Avatarize.Services
{
    public class HashService
    {
        public virtual uint GetHash(string input) => Hash(input);

        private uint SDBMHash(string input)
        {
            uint hash = 0;

            for (uint i = 0; i < input.Length; i++)
                hash = (hash << 6) + (hash << 16) - hash + ((byte)input[(int)i]);

            return hash;
        }

        private uint Hash(string seed)
        {
            uint hash = 0;

            for (uint i = 0; i < seed.Length; i++)
            {
                hash = ((hash << 5) - hash + seed[(int) i]) | 0;
                hash = Xorshift(hash);
            }

            return hash;
        }

        private uint Xorshift(uint value)
        {
            value ^= value << 13;
            value ^= value >> 17;
            value ^= value << 5;

            return value;
        }
    }
}
