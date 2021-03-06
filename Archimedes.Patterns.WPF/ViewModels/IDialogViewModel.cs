﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.WPF.ViewModels
{
    public interface IDialogViewModel : IWorkspaceViewModel
    {
        /// <summary>
        /// Dialoge Result
        /// </summary>
        DialogResultType DialogeResult { get; }

        IEnumerable<DialogCommand> DialogCommands { get; }
    }
}
