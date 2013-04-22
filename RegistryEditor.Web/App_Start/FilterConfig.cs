using System.Web.Mvc;

namespace RegistryEditor.Web.App_Start {
  public class FilterConfig {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
      filters.Add(new HandleErrorAttribute());
    }
  }
}