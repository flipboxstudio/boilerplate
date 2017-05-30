using System;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    ///     <see cref="IUrlHelper" /> extension methods.
    /// </summary>
    public static class UrlHelperExtension
    {
        /// <summary>
        ///     Generates a fully qualified URL to the specified content by using the specified content path.
        ///     Converts a virtual (relative) path to an application absolute path.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="contentPath">The content path.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteContent(
            this IUrlHelper url,
            string contentPath)
        {
            var request = url.ActionContext.HttpContext.Request;

            return
                new Uri(new Uri(request.Scheme + "://" + request.Host.Value), url.Content(contentPath)).ToString()
                    .TrimEnd('/');
        }

        /// <summary>
        ///     Generates a fully qualified URL to the specified route by using the route name and route values.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteRouteUrl(
            this IUrlHelper url,
            string routeName,
            object routeValues = null)
        {
            return url.RouteUrl(routeName, routeValues, url.ActionContext.HttpContext.Request.Scheme).TrimEnd('/');
        }
    }
}