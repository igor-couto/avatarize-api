namespace avatarize.Services
{
    public class HashService
    {
        public virtual int GetHash(string input) => SDBMHash(input);
        
        private int SDBMHash(string input)
        {
            int hash = 0;

            for (var i = 0; i < input.Length; i++)
                hash = (hash << 6) + (hash << 16) - hash + ((byte)input[i]);

            return hash;
        }
    }
}
