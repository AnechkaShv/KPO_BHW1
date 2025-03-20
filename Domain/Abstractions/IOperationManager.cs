using Domain.Entities;

namespace Domain.Abstractions;

public interface IOperationManager
{
    void AddOperation(Operation operation);
    List<Operation> GetAllOperations();
    void UpdateOperation(Operation operation);
    void RemoveOperation(Guid operationId);
}