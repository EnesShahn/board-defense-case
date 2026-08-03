namespace ESF.Core.DataRepository
{
    public abstract class BaseRepository
    {
        protected readonly string _profileName;

        protected BaseRepository(string profileName)
        {
            _profileName = profileName;
        }

        public abstract void Load();

        public abstract void Save();
    }
}