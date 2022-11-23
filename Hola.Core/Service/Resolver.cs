using System;
using System.Collections;
using System.Collections.Generic;

namespace Hola.Core.Service
{
    public class Resolver
    {
        private class Registration
        {
            public Type ServiceType { get; set; }
            public Func<object, object> Activator { get; set; }
        }

        private static readonly Hashtable registery;

        static Resolver()
        {
            registery = new Hashtable();
        }

        public static void Register<T>(Func<object, T> activator)
        {
            registery[typeof(T)] = new Registration
            {
                ServiceType = typeof(T),
                Activator = (state) => activator(state)
            };
        }

        public static void Register<T, S>() where S : T, new()
        {
            registery[typeof(T)] = new Registration
            {
                ServiceType = typeof(T),
                Activator = (state) => new S()
            };
        }

        public static void Register<T>() where T : new()
        {
            Register<T, T>();
        }

        public static void Register<T>(T instance) where T : class
        {
            Register((state) => instance);
        }

        public static T Resolve<T>(object state = null)
        {
            if (registery[typeof(T)] is Registration registration)
                return (T)registration.Activator(state);
            return default;
        }

        public static List<T> ResolveAll<T>(object state = null)
        {
            var result = new List<T>();

            if (registery.Values != null)
            {
                foreach (Registration registration in registery.Values)
                {
                    if (typeof(T).IsAssignableFrom(registration.ServiceType))
                        result.Add((T)registration.Activator(state));
                }
            }

            return result;
        }
    }
}