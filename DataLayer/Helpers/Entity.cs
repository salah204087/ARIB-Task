namespace DataLayer.Helpers;

public abstract class Entity : IDisposable
{
    bool disposed = false;

    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);
        // Suppress finalization.
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            // free other managed objects that implement
            // IDisposable only
        }
        disposed = true;
    }

}