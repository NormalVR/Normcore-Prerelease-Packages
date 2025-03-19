using System;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Normcore.Services
{
    /// <summary>
    /// The interface for an authenticated client session. 
    /// </summary>
    public interface INormcoreServicesSession : IDisposable
    {
        /// <summary>
        /// Whether the session has been terminated.
        /// </summary>
        bool Disposed { get; }

        /// <summary>
        /// The base URL where this session sends API requests.
        /// </summary>
        string BaseUrl { get; }
        
        /// <summary>
        /// The ID of the Normcore App this session is connected to.
        /// </summary>
        string AppId { get; }
        
        /// <summary>
        /// The client used to interact with the Normcore Services API.
        /// </summary>
        ServicesApi Client { get; }
    }

    internal interface INormcoreServicesSessionInternal : INormcoreServicesSession
    {
        void CheckDisposed();
    }

    /// <summary>
    /// A class that manages an authenticated client session.
    /// </summary>
    /// <remarks>
    /// The session should be disposed once it is no longer required, or else the session will be kept alive,
    /// introducing unnesseccary overhead on the client.
    /// </remarks>
    /// <typeparam name="TAuth">The type of authentication used by the session.</typeparam>
    public sealed class NormcoreServicesSession<TAuth> : INormcoreServicesSessionInternal where TAuth : IAuthentication
    {
        private readonly Action _onDispose;
        private readonly CancellationTokenSource _heartbeatCancellation;
        private readonly Task _heartbeatTask;
        
        /// <inheritdoc />
        public bool Disposed { get; private set; }

        /// <inheritdoc />
        public string BaseUrl { get; }
        
        /// <inheritdoc />
        public string AppId { get; }

        /// <summary>
        /// The authentication used by the session.
        /// </summary>
        public TAuth Auth { get; }

        /// <inheritdoc />
        public ServicesApi Client { get; }

        internal NormcoreServicesSession(string baseUrl, string appId, TAuth auth, Action onDispose = null)
        {
            BaseUrl = baseUrl;
            AppId = appId;
            Auth = auth;

            Client = new ServicesApi(this, auth);

            _onDispose = onDispose;
            
            if (auth is IUserAuthentication)
            {
                _heartbeatCancellation = new CancellationTokenSource();
                _heartbeatTask = DoHeartbeatAsync();
            }
        }
        
        ~NormcoreServicesSession()
        {
            OnDispose(false);
            Debug.LogWarning($"An instance of {GetType().Name} was not disposed!");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            OnDispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnDispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            
            _heartbeatCancellation.Cancel();
            _onDispose?.Invoke();

            Disposed = true;
        }

        private async Task DoHeartbeatAsync()
        {
            while (!_heartbeatCancellation.IsCancellationRequested)
            {
                // default to a five second heartbeat interval if the services API cannot be reached
                var interval = 5 * 1000;

                try
                {
                    var heartbeat = await Client.Users.Heartbeat();
                    interval = heartbeat.Interval;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                try
                {
                    await Task.Delay(interval, _heartbeatCancellation.Token);
                }
                catch (Exception e) when (e is not (TaskCanceledException or ObjectDisposedException))
                {
                    Debug.LogException(e);
                }
            }
            
            _heartbeatCancellation.Dispose();
        }

        void INormcoreServicesSessionInternal.CheckDisposed()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(ServicesApi));
            }
        }
    }
}
