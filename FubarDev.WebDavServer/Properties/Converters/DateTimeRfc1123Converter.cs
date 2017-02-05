﻿// <copyright file="DateTimeRfc1123Converter.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Xml.Linq;

namespace FubarDev.WebDavServer.Properties.Converters
{
    public class DateTimeRfc1123Converter : IPropertyConverter<DateTime>
    {
        public DateTime FromElement(XElement element)
        {
            return DateTime.ParseExact(element.Value, "R", CultureInfo.InvariantCulture);
        }

        public XElement ToElement(XName name, DateTime value)
        {
            return new XElement(name, value.ToString("R"));
        }
    }
}
