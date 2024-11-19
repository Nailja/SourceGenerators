namespace TestGenerators.Contract
{
    public interface ITestRepositoryOperations
    {
        void GetAllOpertation();

        void TestGetElementOperation(int id);

        void InsertOperation();

        void UpdateOperation(int id);

        void DeleteOperation(int id);
    }
}
