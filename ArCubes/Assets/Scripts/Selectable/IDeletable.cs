using System;

namespace Selectable
{
    public interface IDeletable<out T>
    {
        public event Action<T> OnDeletionTriggered;
    }
}