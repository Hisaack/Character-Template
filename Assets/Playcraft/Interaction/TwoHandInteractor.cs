﻿using System;
using UnityEngine;

namespace Playcraft.VR
{
    public class TwoHandInteractor : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] MessageLink messenger;
        [SerializeField] TwoHandInteractor otherInteractor;
        [SerializeField] TagSO enter;
        [SerializeField] TagSO exit;
        [SerializeField] Interaction[] interactions;
        #pragma warning restore 0649
        
        public void Enter(Collider other)
        {
            var link = other.GetComponent<MessageLink>();
            if (link) Enter(link);
        }
        
        void Enter(MessageLink other)
        {
            foreach (var interaction in interactions)
                interaction.potentialLink = other;
            
            messenger.SetLink(other);
            messenger.Output(enter, other);
        }
        
        public void Exit(Collider other)
        {
            var link = other.GetComponent<MessageLink>();
            if (link) Exit(link);        
        }
        
        void Exit(MessageLink other)
        {
            foreach (var interaction in interactions)
                interaction.potentialLink = null;
            
            messenger.Output(exit, other);
            messenger.ClearLink();
        }
        
        public void Activate(TagSO message)
        {
            var interaction = GetInteraction(message, true);
            //Debug.Log("Found interaction for " + message + ": " + (interaction != null));    // OK
            if (interaction == null) return;
            
            var otherInteraction = otherInteractor.GetInteraction(message, true);

            if (otherInteraction != null && 
                otherInteraction.activeLink == interaction.potentialLink)
                return;
            
            interaction.activeLink = interaction.potentialLink;
            //messenger.SetLink(interaction.potentialLink);
            messenger.Output(message);
        }
        
        Interaction GetInteraction(TagSO message, bool activeSearch)
        {
            foreach (var interaction in interactions)
                if (interaction.IsMe(message, activeSearch))
                    return interaction;
            
            return null;
        }
        
        public void Deactivate(TagSO message)
        {
            var interaction = GetInteraction(message, false);
            //Debug.Log("Found interaction for " + message + ": " + (interaction != null));    // OK
            if (interaction == null || interaction.activeLink == null) return;
                
            messenger.Output(message);
            //messenger.ClearLink();    // ERROR
            interaction.activeLink = null;
        }
        
        [Serializable] public class Interaction
        {
            public TagSO activate;
            public TagSO deactivate;
            [NonSerialized] public MessageLink potentialLink;
            [NonSerialized] public MessageLink activeLink;
            
            public bool IsMe(TagSO query, bool isActive)
            {
                var message = isActive ? activate : deactivate;
                return message == query;
            }
        }
    }
}
