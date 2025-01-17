﻿// Copyright 2003-2023 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Enums;
using RevitLookup.Core.Objects;
using Wpf.Ui.Common;

namespace RevitLookup.Views.Converters;

public abstract class DescriptorConverter : MarkupExtension, IValueConverter
{
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    protected static string CreateCombinedName(Descriptor descriptor)
    {
        if (string.IsNullOrEmpty(descriptor.Name)) return descriptor.Name;
        return string.IsNullOrEmpty(descriptor.Description) ? descriptor.Name : $"{descriptor.Description}: {descriptor.Name}";
    }

    protected static string CreateSingleName(Descriptor descriptor)
    {
        if (string.IsNullOrEmpty(descriptor.Name)) return descriptor.Name;
        return string.IsNullOrEmpty(descriptor.Description) ? descriptor.Name : descriptor.Description;
    }

    protected string ConvertInvalidNames(string text)
    {
        return text switch
        {
            null => "<null>",
            "" => "<empty>",
            _ => text
        };
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}

public sealed class SingleDescriptorConverter : DescriptorConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ConvertInvalidNames(CreateSingleName((Descriptor) value!));
    }
}

public sealed class CombinedDescriptorConverter : DescriptorConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ConvertInvalidNames(CreateCombinedName((Descriptor) value!));
    }
}

public sealed class HandledDescriptorConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var descriptor = (Descriptor) value!;
        if (descriptor is IDescriptorEnumerator enumerator) return !enumerator.IsEmpty;
        return descriptor is IDescriptorCollector;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}

public sealed class ExceptionDescriptorConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var descriptor = (Descriptor) value!;
        return descriptor is ExceptionDescriptor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}

public sealed class IconDescriptorConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var attributes = (MemberAttributes) value!;

        if ((attributes & MemberAttributes.Property) != 0 && (attributes & MemberAttributes.Private) != 0 && (attributes & MemberAttributes.Static) != 0) return SymbolRegular.CalendarLock16;
        if ((attributes & MemberAttributes.Property) != 0 && (attributes & MemberAttributes.Private) != 0) return SymbolRegular.DocumentLock16;
        if ((attributes & MemberAttributes.Property) != 0 && (attributes & MemberAttributes.Static) != 0) return SymbolRegular.ClipboardNote16;
        if ((attributes & MemberAttributes.Property) != 0) return SymbolRegular.ClipboardBulletListLtr16;

        if ((attributes & MemberAttributes.Method) != 0 && (attributes & MemberAttributes.Private) != 0  && (attributes & MemberAttributes.Static) != 0) return SymbolRegular.ShieldLock16;
        if ((attributes & MemberAttributes.Method) != 0 && (attributes & MemberAttributes.Private) != 0) return SymbolRegular.TableLock16;
        if ((attributes & MemberAttributes.Method) != 0 && (attributes & MemberAttributes.Static) != 0) return SymbolRegular.Box16;
        if ((attributes & MemberAttributes.Method) != 0) return SymbolRegular.Cube16;

        if ((attributes & MemberAttributes.Field) != 0 && (attributes & MemberAttributes.Private) != 0 && (attributes & MemberAttributes.Static) != 0) return SymbolRegular.TrophyLock16;
        if ((attributes & MemberAttributes.Field) != 0 && (attributes & MemberAttributes.Private) != 0) return SymbolRegular.TagLock16;
        if ((attributes & MemberAttributes.Field) != 0 && (attributes & MemberAttributes.Static) != 0) return SymbolRegular.TagMultiple16;
        if ((attributes & MemberAttributes.Field) != 0) return SymbolRegular.Tag16;

        if ((attributes & MemberAttributes.Event) != 0 && (attributes & MemberAttributes.Private) != 0) return SymbolRegular.FlashSettings20;
        if ((attributes & MemberAttributes.Event) != 0) return SymbolRegular.Flash16;

        if ((attributes & MemberAttributes.Extension) != 0) return SymbolRegular.CubeArrowCurveDown20;

        Debug.Assert(false, "Unsupported image");
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}