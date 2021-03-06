﻿using RealtimeNotifier.Feature.ItemActivities.Models;
using RealtimeNotifier.Foundation.SignalR.Services;
using Sitecore.Diagnostics;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtimeNotifier.Feature.ItemActivities
{
    public class ItemSavedNotification
    {
        private ISignalRService signalRService;
        public ItemSavedNotification(ISignalRService signalRService)
        {
            this.signalRService = signalRService;
        }

        protected void OnItemSaved(object sender, EventArgs args)
        {
            Sitecore.Data.Items.Item item = Event.ExtractParameter<Sitecore.Data.Items.Item>(args, 0);
            if (item.Paths.FullPath.ToLowerInvariant().StartsWith("/sitecore/content"))
            {
                signalRService.Signal(new ItemModel()
                {
                    ItemName = item.Name,
                    ItemID = item.ID.Guid.ToString("N"),
                    UserName = item.Statistics.UpdatedBy,
                    ItemPath = item.Paths.FullPath,
                    Message = $"{item.Name} has been udpated.",
                    DateTime = DateTime.Now.ToString()
                });
                Log.Info($"ItemSavedNotification: Triggered realtime notification for {item.ID}", this);
            }
        }
    }
}