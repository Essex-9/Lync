﻿/* Copyright (C) 2012 Modality Systems - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the Microsoft Public License, a copy of which 
 * can be seen at: http://www.microsoft.com/en-us/openness/licenses.aspx
 * 
 * http://www.LyncAutoAnswer.com
*/

using System.Windows.Input;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;

namespace SuperSimpleLyncKiosk.ViewModels
{
    public class IncomingCallViewModel
    {
        private Command _endCallCommand;
        private LyncClient _lync;
        private Conversation _conversation;

        public IncomingCallViewModel()
        {
            _lync = LyncClient.GetClient();
            _lync.ConversationManager.ConversationAdded += ConversationManager_ConversationAdded;
        }

        void ConversationManager_ConversationAdded(object sender, Microsoft.Lync.Model.Conversation.ConversationManagerEventArgs e)
        {
            _conversation = e.Conversation;
        }

        public ICommand EndCallCommand
        {
            get
            {
                if (this._endCallCommand == null)
                    this._endCallCommand = new Command { Execute = ExecuteEndCall };
                return this._endCallCommand;
            }
        }

        private void ExecuteEndCall(object param)
        {
            if (_conversation == null)
                return;

            _conversation.End();

        }


    }
}
