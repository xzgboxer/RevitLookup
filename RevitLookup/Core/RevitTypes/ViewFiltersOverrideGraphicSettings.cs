﻿using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes
{
    public class ViewFiltersOverrideGraphicSettings : Data
    {
        private readonly View _view;

        public ViewFiltersOverrideGraphicSettings(string label, View view) : base(label)
        {
            _view = view;
        }

        public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.AreGraphicsOverridesAllowed() && _view.GetFilters().Count > 0;

        public override string StrValue()
        {
            return "< view filters override graphic settings >";
        }

        public override Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var filterOverrides = _view
                .GetFilters()
                .Select(x => new SnoopableObjectWrapper(_view.Document.GetElement(x).Name, _view.GetFilterOverrides(x)))
                .ToList();

            if (filterOverrides.Count == 0) return null;
            var form = new Objects(filterOverrides);
            return form;
        }
    }
}