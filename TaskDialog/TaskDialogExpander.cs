﻿using System;
using System.ComponentModel;

namespace KPreisser.UI
{
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class TaskDialogExpander : TaskDialogControl
    {
        private string text;

        private string expandedButtonText;

        private string collapsedButtonText;

        private bool expandFooterArea;

        private bool expanded;


        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<TaskDialogBooleanStatusEventArgs> ExpandoButtonClicked;


        /// <summary>
        /// 
        /// </summary>
        public TaskDialogExpander()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public TaskDialogExpander(string text)
            : base()
        {
            this.text = text;
        }


        /// <summary>
        /// Gets or sets the text to be displayed in the dialog's expanded area.
        /// </summary>
        /// <remarks>
        /// This text can be changed while the dialog is shown.
        /// </remarks>
        public string Text
        {
            get => this.text;

            set {
                this.text = value;

                // Update the text if we are bound.
                this.boundTaskDialogContents?.BoundTaskDialog.UpdateTextElement(
                        TaskDialogTextElement.ExpandedInformation,
                        this.text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ExpandedButtonText
        {
            get => this.expandedButtonText;

            set
            {
                this.DenyIfBound();

                this.expandedButtonText = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CollapsedButtonText
        {
            get => this.collapsedButtonText;

            set
            {
                this.DenyIfBound();

                this.collapsedButtonText = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Expanded
        {
            get => this.expanded;

            set
            {
                // The Task Dialog doesn't provide a message type to click the expando
                // button, so we don't allow to change this property (it will however
                // be updated when we receive an ExpandoButtonClicked notification).
                // TODO: Should we throw only if the new value is different than the
                // old one?
                this.DenyIfBound();

                this.expanded = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ExpandFooterArea
        {
            get => this.expandFooterArea;

            set
            {
                this.DenyIfBound();

                this.expandFooterArea = value;
            }
        }


        internal override bool IsCreatable
        {
            get => base.IsCreatable && !TaskDialogContents.IsNativeStringNullOrEmpty(this.text);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.text ?? base.ToString();
        }


        internal override TaskDialogFlags GetFlags()
        {
            var flags = base.GetFlags();

            if (this.IsCreatable)
            {
                if (this.expanded)
                    flags |= TaskDialogFlags.ExpandedByDefault;
                if (this.expandFooterArea)
                    flags |= TaskDialogFlags.ExpandFooterArea;
            }

            return flags;
        }

        internal void HandleExpandoButtonClicked(bool expanded)
        {
            this.expanded = expanded;
            this.OnExpandoButtonClicked(
                    new TaskDialogBooleanStatusEventArgs(expanded));
        }


        private void OnExpandoButtonClicked(
                TaskDialogBooleanStatusEventArgs e)
        {
            this.ExpandoButtonClicked?.Invoke(this, e);
        }
    }
}
