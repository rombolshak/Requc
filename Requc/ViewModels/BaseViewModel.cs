using System;
using Requc.Helpers;

namespace Requc.ViewModels
{
    public abstract class BaseViewModel : NotificationObject, IDisposable
    {
        public abstract void Dispose();
    }
}