using Microsoft.AspNetCore.Mvc;

namespace ImportShopApi.Extensions {
  public static class ControllerExtensions {
    public static ActionResult UnprocessableModelResult<T>(this T controller) where T : Controller
      => controller.UnprocessableEntity(controller.ModelState.GetErrors());

    public static T AddModelError<T>(this T controller, string error) where T : Controller {
      controller.ModelState.AddModelError("", error);

      return controller;
    }
  }
}