namespace TablesAutomation.E2EFramework.Interfaces

{
    public interface IConfigurationService
    {
        T InitConfiguration<T>();

        public void Validate(string pathToShema);
    }
}