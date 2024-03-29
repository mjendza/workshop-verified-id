using System;

namespace Portal.VerifiableCredentials.API.Services;

internal struct PushJsonComposedObjectAsValue<T> : IDisposable
{
    Action<T> _setValue;
    T _oldValue;

    internal PushJsonComposedObjectAsValue(T value, Func<T> getValue, Action<T> setValue)
    {
        if (getValue == null)
            throw new ArgumentNullException(nameof(getValue));
        this._setValue = setValue ?? throw new ArgumentNullException(nameof(setValue));
        this._oldValue = getValue();
        setValue(value);
    }

    #region IDisposable Members

    // By using a disposable struct we avoid the overhead of allocating and freeing an instance of a finalizable class.
    public void Dispose()
    {
        if (_setValue != null)
            _setValue(_oldValue);
    }

    #endregion
}