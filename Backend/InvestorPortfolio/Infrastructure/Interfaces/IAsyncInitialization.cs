namespace Infrastructure.Interfaces;

public interface IAsyncInitialization
{
    Task Initialization { get; }
}