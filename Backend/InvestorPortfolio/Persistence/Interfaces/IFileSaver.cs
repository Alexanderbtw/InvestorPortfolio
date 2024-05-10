namespace Persistence.Interfaces;

public interface IAsyncFileSaver<T> : IFileSaver<T>
{
    public abstract Task<bool> SaveAsync(T objectToSave, string path);

    public abstract Task<T> RecoveryAsync(string path);
}

public interface IFileSaver<T>
{
    public abstract bool Save(T objectToSave, string path);

    public abstract T Recovery(string path);
}