﻿using System;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using VSTS.DataBulkEditor.Core.ComponentContext;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace VSTS.DataBulkEditor.Core
{
    public class RegexFieldMap : IFieldMap
    {
        private string pattern;
        private string replacement;
        private string sourceField;
        private string targetField;

        public RegexFieldMap(string sourceField, string targetField, string pattern, string replacement)
        {
            this.sourceField = sourceField;
            this.targetField = targetField;
            this.pattern = pattern;
            this.replacement = replacement;
        }

        public RegexFieldMap(string sourceField, string targetField, string pattern) : this(sourceField, targetField, pattern, "$1")
        {
        }

        public void Execute(WorkItem source, WorkItem target)
        {
            if (source.Fields.Contains(sourceField) && !string.IsNullOrEmpty(source.Fields[sourceField].Value.ToString()))
            {
                if (Regex.IsMatch((string)source.Fields[sourceField].Value, pattern))
                {
                    target.Fields[targetField].Value = Regex.Replace((string)source.Fields[sourceField].Value, pattern, replacement);
                    Trace.WriteLine(string.Format("  [UPDATE] field tagged {0}:{1} to {2}:{3} with regex pattern of {4} resulting in {5}", source.Id, sourceField, target.Id, targetField, pattern, target.Fields[targetField].Value));
                }
            }
        }
    }
}