﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using Lync.Enum;
using Lync.Repository;
using Lync.Service;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Lync.Model
{
	public class LyncConversation : ObservableObject
	{
		private ILog _log = LogManager.GetLog(typeof(LyncConversation));

		protected ConversationService ConversationService { get; set; }
		protected ContactService ContactService { get; set; }

		public Conversation Conversation { get; set; }

		public ConversationType Type;

		public string ExternalUrl { get; set; }
		public Guid ExternalId { get; set; }

		private List<Participant> _participants;
		public List<Participant> Participants
		{
			get { return _participants; }
			set
			{
				Set("Participants", ref _participants, value);
			}

		}

		private bool _isCanAddParticipant;
		public bool IsCanAddParticipant
		{
			get
			{
				return _isCanAddParticipant;
			}
			set
			{
				Set("IsCanAddParticipant", ref _isCanAddParticipant, value);
			}
		}

		private bool _isCanRemoveParticipant;


		public bool IsCanRemoveParticipant
		{
			get
			{
				return _isCanRemoveParticipant;
			}
			set
			{
				Set("IsCanRemoveParticipant", ref _isCanRemoveParticipant, value);
			}
		}

		protected ConversationRepository Repository { get; set; }

		public Action<Action> RunAtUI = DispatcherHelper.CheckBeginInvokeOnUI;


		public LyncConversation()
		{
			ContactService = ContactService.Instance;
			ConversationService = ConversationService.Instance;
			Participants = new List<Participant>();
		}

		public void CreateConversation()
		{
			ConversationService.AddConversation(this);
		}

		public void HandleAdded()
		{
			Repository = ConversationRepository.Instance;
			Repository.Clear();

			//registers for participant events
			foreach (var participant in Conversation.Participants)
			{
				Participants.Add(participant);
			}
			Conversation.ParticipantAdded += OnConversationParticipantAdded;
			Conversation.ParticipantRemoved += OnConversationParticipantRemoved;
			Conversation.StateChanged += OnConversationStateChanged;

			//subscribes to the conversation action availability events (for the ability to add/remove participants)
			Conversation.ActionAvailabilityChanged += OnConversationActionAvailabilityChanged;
			Conversation.PropertyChanged += OnConversationPropertyChanged;


			HandleAddedInternal();
		}

		protected virtual void HandleAddedInternal()
		{

		}



		/// <summary>
		/// Ends the conversation if the user closes the window.
		/// </summary>
		public void Close()
		{
			if (Conversation == null)
			{
				return;
			}

			_log.Debug("Close");
			//need to remove event listeners otherwide events may be received after the form has been unloaded
			Conversation.StateChanged -= OnConversationStateChanged;
			Conversation.ParticipantAdded -= OnConversationParticipantAdded;
			Conversation.ParticipantRemoved -= OnConversationParticipantRemoved;
			Conversation.ActionAvailabilityChanged -= OnConversationActionAvailabilityChanged;

			CloseInternal();

			//if the conversation is active, will end it
			if (Conversation.State != ConversationState.Terminated)
			{
				//ends the conversation which will disconnect all modalities
				try
				{
					Conversation.End();
				}
				catch (LyncClientException lyncClientException)
				{
					_log.ErrorException("Close", lyncClientException);
				}
				catch (SystemException systemException)
				{
					if (LyncModelExceptionHelper.IsLyncException(systemException))
					{
						// Log the exception thrown by the Lync Model API.
						_log.ErrorException("Error: ", systemException);
					}
					else
					{
						// Rethrow the SystemException which did not come from the Lync Model API.
						throw;
					}
				}
			}

			Conversation = null;
		}

		protected virtual void CloseInternal()
		{

		}


		private void OnConversationPropertyChanged(object sender, ConversationPropertyChangedEventArgs e)
		{
			Conversation conference = (Conversation)sender;

			switch (e.Property)
			{
				case ConversationProperty.ConferenceAcceptingParticipant:
					Contact acceptingContact = (Contact)e.Value;
					break;
				case ConversationProperty.ConferencingUri:
					break;
				case ConversationProperty.ConferenceAccessInformation:

					try
					{

						var conferenceKey = CreateConferenceKey();
					}
					catch (NullReferenceException nr)
					{
						_log.ErrorException("Null ref Eception on ConferenceAccessInformation changed ", nr);
					}
					catch (LyncClientException lce)
					{
						_log.ErrorException("Exception on ConferenceAccessInformation changed ", lce);
					}
					break;
			}

		}


		private void OnConversationParticipantAdded(object sender, ParticipantCollectionChangedEventArgs e)
		{
			RunAtUI
				(() =>
					{
						var newPart = e.Participant;
						Participants.Add(newPart);
						_log.Debug("OnConversationParticipantAdded  {0}", newPart);
						ConversationParticipantAddedInternal(e.Participant);
					}
				);
		}

		protected virtual void ConversationParticipantAddedInternal(Participant participant)
		{

		}

		private void OnConversationParticipantRemoved(object sender, ParticipantCollectionChangedEventArgs e)
		{
			RunAtUI
				(() =>
					{

						var removePart = Participants.Where(p => p.Equals(e.Participant)).SingleOrDefault();
						if (removePart != null)
						{
							Participants.Remove(removePart);
						}
						ConversationParticipantRemovedInternal(removePart);

						_log.Debug("OnConversationParticipantRemoved  {0}", removePart);
					}
				);
		}



		protected virtual void ConversationParticipantRemovedInternal(Participant participant)
		{

		}

		private void OnConversationStateChanged(object sender, ConversationStateChangedEventArgs e)
		{
			_log.Debug("OnConversationStateChanged  NewState:{0}", e.NewState.ToString());
		}

		private void OnConversationActionAvailabilityChanged(object sender, ConversationActionAvailabilityEventArgs e)
		{
			//posts the execution into the UI thread
			RunAtUI
				(() =>
					{
						//each action is mapped to a button in the UI
						switch (e.Action)
						{
							case ConversationAction.AddParticipant:
								IsCanAddParticipant = e.IsAvailable;
								break;

							case ConversationAction.RemoveParticipant:
								IsCanRemoveParticipant = e.IsAvailable;
								break;
						}
						_log.Debug("OnConversationActionAvailabilityChanged  Action: {0}", e.Action.ToString());
					}
			);
		}



		/// <summary>
		/// Returns the meet-now meeting access key as a string
		/// </summary>
		/// <returns></returns>
		private string CreateConferenceKey()
		{
			string returnValue = string.Empty;
			try
			{

				//These properties are used to invite people by creating an email (or text message, or IM)
				//and adding the dial in number, external Url, internal Url, and conference Id
				ConferenceAccessInformation conferenceAccess =
					(ConferenceAccessInformation)Conversation.Properties[ConversationProperty.ConferenceAccessInformation];

				if (conferenceAccess == null)
				{
					if (!Conversation.CanSetProperty(ConversationProperty.ConferenceAccessInformation))
					{
						return string.Empty;
					}

					//Conversation.BeginSetProperty(
					//	ConversationProperty.ConferenceAccessInformation,
					//	 new ConferenceAccessInformation() , (ar) =>
					//	{
					//		Conversation.EndSetProperty(ar);
					//	},
					//	null);
				}

				StringBuilder MeetingKey = new StringBuilder();

				if (conferenceAccess.Id.Length > 0)
				{
					MeetingKey.Append("Meeting Id: " + conferenceAccess.Id);
					MeetingKey.Append(System.Environment.NewLine);
				}

				if (conferenceAccess.AdmissionKey.Length > 0)
				{
					MeetingKey.Append(conferenceAccess.AdmissionKey);
					MeetingKey.Append(System.Environment.NewLine);
				}

				string[] attendantNumbers = (string[])conferenceAccess.AutoAttendantNumbers;

				StringBuilder sb2 = new StringBuilder();
				sb2.Append(System.Environment.NewLine);
				foreach (string aNumber in attendantNumbers)
				{
					sb2.Append("\t\t" + aNumber);
					sb2.Append(System.Environment.NewLine);
				}
				if (sb2.ToString().Trim().Length > 0)
				{
					MeetingKey.Append("Auto attendant numbers:" + sb2.ToString());
					MeetingKey.Append(System.Environment.NewLine);
				}

				if (conferenceAccess.ExternalUrl.Length > 0)
				{
					MeetingKey.Append("External Url: " + conferenceAccess.ExternalUrl);
					MeetingKey.Append(System.Environment.NewLine);
				}

				if (conferenceAccess.InternalUrl.Length > 0)
				{
					MeetingKey.Append("Inner Url: " + conferenceAccess.InternalUrl);
					MeetingKey.Append(System.Environment.NewLine);
				}

				MeetingKey.Append("Meeting access type: " + (
					(ConferenceAccessType)Conversation.Properties[ConversationProperty.ConferencingAccessType])
					.ToString());

				MeetingKey.Append(System.Environment.NewLine);
				returnValue = MeetingKey.ToString();

			}
			catch (System.NullReferenceException nr)
			{
				System.Diagnostics.Debug.WriteLine(
					"Null ref Eception on ConferenceAccessInformation changed " + nr.Message);
			}
			catch (LyncClientException lce)
			{
				System.Diagnostics.Debug.WriteLine(
					"Exception on ConferenceAccessInformation changed " + lce.Message);
			}
			return returnValue;
		}



	}



}
