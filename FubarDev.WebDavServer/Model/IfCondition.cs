﻿// <copyright file="IfCondition.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JetBrains.Annotations;

namespace FubarDev.WebDavServer.Model
{
    public class IfCondition : IIfMatcher
    {
        private IfCondition(bool not, Uri stateToken, EntityTag? etag)
        {
            Not = not;
            StateToken = stateToken;
            ETag = etag;
        }

        public bool Not { get; }

        [CanBeNull]
        public Uri StateToken { get; }

        public EntityTag? ETag { get; }

        public bool IsMatch(EntityTag etag, IReadOnlyCollection<Uri> stateTokens)
        {
            bool result;

            if (ETag.HasValue)
            {
                result = etag == ETag.Value;
            }
            else
            {
                Debug.Assert(StateToken != null, "StateToken != null");
                result = stateTokens.Any(x => x.Equals(StateToken));
            }

            return Not ? !result : result;
        }

        [NotNull]
        [ItemNotNull]
        internal static IEnumerable<IfCondition> Parse([NotNull] StringSource source)
        {
            while (!source.SkipWhiteSpace())
            {
                var isNot = false;
                Uri stateToken = null;
                EntityTag? etag = null;
                if (source.AdvanceIf("Not", StringComparison.OrdinalIgnoreCase))
                {
                    isNot = true;
                    source.SkipWhiteSpace();
                }

                var ch = source.Get();
                if (ch == '<')
                {
                    // Coded-URL found
                    var codedUrl = source.GetUntil('>');
                    if (codedUrl == null)
                        throw new ArgumentException($"{source.Remaining} is not a valid condition (Coded-URL not ending with '>')", nameof(source));
                    source.Advance(1);
                    stateToken = new Uri(codedUrl, UriKind.RelativeOrAbsolute);
                }
                else if (ch == '[')
                {
                    // Entity-tag found
                    etag = EntityTag.Parse(source).Single();
                    if (!source.AdvanceIf("]"))
                        throw new ArgumentException($"{source.Remaining} is not a valid condition (ETag not ending with ']')", nameof(source));
                }
                else
                {
                    source.Back();
                    break;
                }

                yield return new IfCondition(isNot, stateToken, etag);
            }
        }
    }
}