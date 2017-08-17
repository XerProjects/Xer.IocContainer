using System;
using System.Collections.Generic;
using System.Text;
using Xer.IocContainer.Collections;

namespace Xer.IocContainer.LifetimeScopes
{
    internal class LifetimeScopeManager
    {
        private readonly XerContainer _contaner;
        private readonly LifetimeScopeCollection _lifetimeScopes = new LifetimeScopeCollection();

        public LifetimeScopeManager(XerContainer container)
        {
            _contaner = container;
        }
        
        public LifetimeScope CreateScope(string scopeName)
        {
            return new ControlledLifetimeScope(_contaner, scopeName);
        }

        public LifetimeScope GetScope(string scopeName)
        {
            return _lifetimeScopes[scopeName];
        }

        public LifetimeScope GetOrCreateScope(string scopeName)
        {
            LifetimeScope scope = _lifetimeScopes[scopeName];

            if(scope == null)
            {
                scope = CreateScope(scopeName);
            }

            return scope;
        }

        public void DisposeScope(string scopeName)
        {
            LifetimeScope scope = _lifetimeScopes[scopeName];
            if(scope != null)
            {
                scope.Dispose();
                
                // Unregister after disposing.
                _lifetimeScopes.Remove(scope.ScopeName);
            }
        }
    }
}
