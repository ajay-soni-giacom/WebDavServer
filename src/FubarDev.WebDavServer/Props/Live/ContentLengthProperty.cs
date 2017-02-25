﻿// <copyright file="ContentLengthProperty.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using FubarDev.WebDavServer.Model;
using FubarDev.WebDavServer.Props.Converters;

namespace FubarDev.WebDavServer.Props.Live
{
    /// <summary>
    /// The <code>getcontentlength</code> property
    /// </summary>
    public class ContentLengthProperty : ITypedReadableProperty<long>, ILiveProperty
    {
        /// <summary>
        /// The XML property name
        /// </summary>
        public static readonly XName PropertyName = WebDavXml.Dav + "getcontentlength";

        private static readonly LongConverter _converter = new LongConverter();

        private readonly long _propValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentLengthProperty"/> class.
        /// </summary>
        /// <param name="propValue">The initial value</param>
        public ContentLengthProperty(long propValue)
        {
            _propValue = propValue;
            Cost = 0;
            Name = PropertyName;
        }

        /// <inheritdoc />
        public XName Name { get; }

        /// <inheritdoc />
        public IReadOnlyCollection<XName> AlternativeNames { get; } = new[] { WebDavXml.Dav + "contentlength" };

        /// <inheritdoc />
        public int Cost { get; }

        /// <inheritdoc />
        public async Task<XElement> GetXmlValueAsync(CancellationToken ct)
        {
            return _converter.ToElement(Name, await GetValueAsync(ct).ConfigureAwait(false));
        }

        /// <inheritdoc />
        public Task<long> GetValueAsync(CancellationToken ct)
        {
            return Task.FromResult(_propValue);
        }
    }
}
