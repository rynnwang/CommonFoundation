﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Beyova.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonNetValueProviderFactory : ValueProviderFactory
    {
        /// <summary>
        /// Gets the value provider.
        /// </summary>
        /// <param name="ctlContext">The control context.</param>
        /// <returns></returns>
        public override IValueProvider GetValueProvider(ControllerContext ctlContext)
        {
            //if (!controllerContext.HttpContext.Request.ContentType.
            //    StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            //{
            //    return null;
            //}

            var reader = new StreamReader(ctlContext.HttpContext.Request.InputStream);
            reader.BaseStream.Position = 0;
            var json = reader.ReadToEnd()?.TrimStart(StringConstants.TrimmedCharacters);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return new JsonNetValueProvider(JToken.Parse(json));
        }
    }
}
