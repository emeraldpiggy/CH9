﻿using System;
using Telerik.Windows.Controls;
using Caliburn.Micro;

namespace CH9.MVVM
{
    public static class WindowManagerExt
    {
        /// <summary>
        /// Opens an Alert modal window
        /// </summary>
        public static void Alert(this IWindowManager windowManager, string title, string message)
        {
            WindowManagerBase.Alert(title, message);
        }

        /// <summary>
        /// Opens an Alert modal window
        /// </summary>
        public static void Alert(this IWindowManager windowManager, DialogParameters dialogParameters)
        {
            WindowManagerBase.Alert(dialogParameters);
        }

        /// <summary>
        /// Opens a Confirm modal window
        /// </summary>
        public static void Confirm(this IWindowManager windowManager, string title, string message, System.Action onOK, System.Action onCancel = null)
        {
            WindowManagerBase.Confirm(title, message, onOK, onCancel);
        }

        /// <summary>
        /// Opens a Confirm modal window
        /// </summary>
        public static void Confirm(this IWindowManager windowManager, DialogParameters dialogParameters)
        {
            WindowManagerBase.Confirm(dialogParameters);
        }

        /// <summary>
        /// Opens a Prompt modal window
        /// </summary>
        public static void Prompt(this IWindowManager windowManager, string title, string message, string defaultPromptResultValue, Action<string> onOK)
        {
            WindowManagerBase.Prompt(title, message, defaultPromptResultValue, onOK);
        }

        /// <summary>
        /// Opens a Prompt modal window
        /// </summary>
        public static void Prompt(this IWindowManager windowManager, DialogParameters dialogParameters)
        {
            WindowManagerBase.Prompt(dialogParameters);
        }
    }
}
