﻿// <copyright file="IFileSystemPropertyStore.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using FubarDev.WebDavServer.FileSystem;

namespace FubarDev.WebDavServer.Properties.Store
{
    public interface IFileSystemPropertyStore : IPropertyStore
    {
        string RootPath { get; set; }

        bool IgnoreEntry(IEntry entry);
    }
}
