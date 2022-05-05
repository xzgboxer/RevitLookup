﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace RevitLookup.UI.Common;

/// <summary>
///     Helper class for Visual Studio designer.
/// </summary>
public static class Designer
{
    private static bool _validated;

    private static bool _isInDesignMode;

    /// <summary>
    ///     Indicates whether the project is currently in design mode.
    /// </summary>
    public static bool IsInDesignMode
    {
        get
        {
            if (_isInDesignMode)
                return true;

            if (_validated)
                _isInDesignMode = (bool) (DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject))
                    ?.DefaultValue ?? false);

            _validated = true;

            return _isInDesignMode;
        }
    }

    /// <summary>
    ///     Indicates whether the project is currently debugged.
    /// </summary>
    public static bool IsDebugging => Debugger.IsAttached;
}