using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ERP_System_Project.Services.Implementation.System_Security
{
    public class ReflectionService
    {
        public List<(string Module, string Controller, string Action)> GetAllControllersAndActions()
        {
            var asm = Assembly.GetExecutingAssembly();
            var controllers = asm.GetTypes()
                .Where(t => typeof(Controller).IsAssignableFrom(t) && !t.IsAbstract);

            var list = new List<(string, string, string)>();

            foreach (var controller in controllers)
            {
                string moduleName = controller.Namespace?.Split('.').LastOrDefault() ?? "Common";
                string controllerName = controller.Name.Replace("Controller", "");

                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m => typeof(IActionResult).IsAssignableFrom(m.ReturnType) ||
                               m.ReturnType == typeof(Task<IActionResult>))
                    .Where(m => !m.IsSpecialName &&
                               !m.GetCustomAttributes(typeof(NonActionAttribute), false).Any())
                    .Select(m => m.Name)
                    .Distinct();

                foreach (var action in actions)
                {
                    list.Add((moduleName, controllerName, action));
                }
            }

            return list;
        }
    }
}
