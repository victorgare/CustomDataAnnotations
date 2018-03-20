using System;
using System.ComponentModel.DataAnnotations;

namespace CustomDataAnnotations.Attributes
{
    public sealed class CustomMethod : ValidationAttribute
    {
        private readonly string _className;
        private readonly string _methodName;

        public CustomMethod(string className, string methodName)
        {
            _className = className;
            _methodName = methodName;
        }

        public override bool IsValid(object value)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type typeObject = null;

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.Name == _className)
                    {
                        typeObject = type;
                    }
                }
            }

            if (typeObject != null)
            {
                var activator = Activator.CreateInstance(typeObject);

                object[] valor =
                {
                    value
                };

                var method = typeObject.GetMethod(_methodName);
                if (method != null)
                {
                    return (bool)method.Invoke(activator, valor);
                }

                ErrorMessage = "Method name invalid";
                return false;

            }

            ErrorMessage = "Class name invalid";
            return false;
        }
    }
}
