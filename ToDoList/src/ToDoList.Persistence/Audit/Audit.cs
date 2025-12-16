using System;

namespace ToDoList.Persistence.Audit;

public class Audit : IAudit
{
    public void StoreAudit(string message) => Console.WriteLine($"Audit: {message}");
}
